using AccountingProgram.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingProgram.Controllers
{
    public class CustomersController: Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Name = model.Name,
                    NIPNumber = model.NIPNumber,
                    City = model.City,
                    Street = model.Street,
                    Address = model.Address,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                };

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Główna księgowa")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerCreateViewModel
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                NIPNumber = customer.NIPNumber,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                City = customer.City,
                Street = customer.Street,
                Address = customer.Address,
                PostalCode = customer.PostalCode
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = await _context.Customers.FindAsync(model.CustomerId);
                if (customer == null)
                {
                    return NotFound();
                }

                customer.Name = model.Name;
                customer.NIPNumber = model.NIPNumber;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Email = model.Email;
                customer.City = model.City;
                customer.Street = model.Street;
                customer.Address = model.Address;
                customer.PostalCode = model.PostalCode;

                _context.Update(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}