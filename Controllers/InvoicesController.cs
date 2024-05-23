using AccountingProgram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AccountingProgram
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvoicesController> _logger;
        private readonly UserManager<User> _userManager;

        public InvoicesController(ApplicationDbContext context, ILogger<InvoicesController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        private void PopulateCustomersDropDownList(object? selectedCustomer = null)
        {
            var customersQuery = from c in _context.Customers
                                 orderby c.Name
                                 select c;
            ViewBag.Customers = new SelectList(customersQuery.AsNoTracking(), "CustomerId", "Name", selectedCustomer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateCustomersDropDownList();
            var model = new InvoiceCreateViewModel
            {
                Items = new List<InvoiceItemViewModel>(),
                IssueDate = DateOnly.FromDateTime(DateTime.Now),
                SaleDate = DateOnly.FromDateTime(DateTime.Now)
            };
            ViewBag.PaymentMethods = GetPaymentMethods();
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceCreateViewModel model)
        {

            if (model == null)
            {
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var invoice = new Invoice
                {
                    IssueDate = model.IssueDate,
                    SaleDate = model.SaleDate,
                    TotalAmount = model.TotalAmount,
                    Notes = model.Notes ?? string.Empty,
                    PaymentMethod = model.PaymentMethod,
                    InvoiceItems = new List<InvoiceItem>(),
                    UserId = userId,
                    CustomerId = model.CustomerId
                };

                try
                {
                    foreach (var itemViewModel in model.Items ?? new List<InvoiceItemViewModel>())
                    {
                        var invoiceItem = new InvoiceItem
                        {
                            Description = itemViewModel.Description,
                            Quantity = itemViewModel.Quantity,
                            UnitPrice = itemViewModel.UnitPrice
                        };

                        invoice.InvoiceItems.Add(invoiceItem);
                    }

                    _context.Invoices.Add(invoice);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Faktura została dodana";
                    return RedirectToAction("Index", "Invoices");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Wystąpił wyjątek podczas dodawania faktury.");
                    TempData["ErrorMessage"] = "Wystąpił błąd podczas dodawania faktury";
                }
            }
            else
            {
                _logger.LogWarning("Model nie jest prawidłowy.");
                PopulateCustomersDropDownList();
                ViewBag.PaymentMethods = GetPaymentMethods();
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Główna księgowa")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Faktura została usunięta.";
            }
            else
            {
                TempData["ErrorMessage"] = "Nie znaleziono faktury.";
            }
            return RedirectToAction("Index", "Invoices");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(u => u.User)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }


        [HttpGet]
        [Authorize(Roles = "Samodzielna księgowa, Główna księgowa")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Pobieranie danych faktury o ID: {id} do edycji.");

            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (invoice == null)
            {
                _logger.LogWarning($"Faktura o ID: {id} nie została znaleziona.");
                return NotFound();
            }

            var model = new InvoiceCreateViewModel
            {
                InvoiceId = invoice.InvoiceId,
                IssueDate = invoice.IssueDate,
                SaleDate = invoice.SaleDate,
                TotalAmount = invoice.TotalAmount,
                Notes = invoice.Notes,
                PaymentMethod = invoice.PaymentMethod,
                CustomerId = invoice.CustomerId ?? 0,
                Items = invoice.InvoiceItems!.Select(i => new InvoiceItemViewModel
                {
                    InvoiceItemId = i.InvoiceItemId,
                    Description = i.Description,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };
            PopulateCustomersDropDownList(invoice.CustomerId);
            ViewBag.PaymentMethods = GetPaymentMethods();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Samodzielna księgowa, Główna księgowa")]
        public async Task<IActionResult> Edit(int id, InvoiceCreateViewModel model)
        {
            if (id != model.InvoiceId)
            {
                _logger.LogError("ID faktury nie zgadza się.");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Przetwarzanie edycji faktury.");

                var invoiceToUpdate = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .FirstOrDefaultAsync(i => i.InvoiceId == id);

                if (invoiceToUpdate == null)
                {
                    _logger.LogWarning($"Faktura o ID: {id} nie została znaleziona podczas próby edycji.");
                    return NotFound();
                }

                invoiceToUpdate.IssueDate = model.IssueDate;
                invoiceToUpdate.SaleDate = model.SaleDate;
                invoiceToUpdate.TotalAmount = model.TotalAmount;
                invoiceToUpdate.Notes = model.Notes ?? string.Empty;
                invoiceToUpdate.PaymentMethod = model.PaymentMethod;

                invoiceToUpdate.InvoiceItems!.Clear();
                foreach (var itemViewModel in model.Items)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        InvoiceItemId = itemViewModel.InvoiceItemId,
                        Description = itemViewModel.Description,
                        Quantity = itemViewModel.Quantity,
                        UnitPrice = itemViewModel.UnitPrice,
                        InvoiceId = invoiceToUpdate.InvoiceId
                    };
                    invoiceToUpdate.InvoiceItems.Add(invoiceItem);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Faktura została zaktualizowana";
                    return RedirectToAction("Details", new { id = invoiceToUpdate.InvoiceId });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Wystąpił wyjątek podczas aktualizacji faktury.");
                    TempData["ErrorMessage"] = "Wystąpił błąd podczas aktualizacji faktury";
                }
            }
            else
            {
                _logger.LogWarning("Model nie jest prawidłowy.");
                PopulateCustomersDropDownList(model.CustomerId);
                ViewBag.PaymentMethods = GetPaymentMethods();
            }

            return View(model);
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            IEnumerable<Invoice> invoices;

            if (userRoles.Contains("Główna księgowa"))
            {
                // Główna księgowa może zobaczyć wszystkie faktury
                invoices = await _context.Invoices
                    .Include(i => i.InvoiceItems)
                    .Include(c => c.Customer)
                    .ToListAsync();
            }
            else
            {
                // Inni użytkownicy widzą tylko swoje faktury
                invoices = await _context.Invoices
                    .Where(i => i.UserId == user.Id)
                    .Include(c => c.Customer)
                    .Include(i => i.InvoiceItems)
                    .ToListAsync();
            }

            return View(invoices);
        }


        private static List<SelectListItem> GetPaymentMethods()
        {
            return
                [
                    new SelectListItem { Value = "Gotówka", Text = "Gotówka"},
                    new SelectListItem { Value = "Karta płatnicza", Text = "Karta płatnicza"},
                    new SelectListItem { Value = "Przelew bankowy", Text = "Przelew bankowy"},
                    new SelectListItem { Value = "Blik", Text = "Blik"}
                ];
        }
    }
}