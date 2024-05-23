$(document).ready(function () {
    function recalculateIndexes() {
        $('.invoiceItem').each(function (index) {
            $(this).find('input, select').each(function () {
                var name = $(this).attr('name').replace(/\[\d+\]/, '[' + index + ']');
                $(this).attr('name', name);
            });
        });
    }

    $('.priceInput').on('input', function () {
        var value = $(this).val().replace(',', '.');
        $(this).val(value);
        calculateTotal();  // Możesz też odświeżać total gdy cena się zmienia
    });

    function calculateTotal() {
        let totalAmount = 0;
        $('.invoiceItem').each(function () {
            const quantity = parseFloat($(this).find('.quantityInput').val()) || 0;
            const unitPrice = parseFloat($(this).find('.priceInput').val()) || 0;
            const totalPrice = quantity * unitPrice;
            $(this).find('.totalPriceInput').val(totalPrice.toFixed(2));
            totalAmount += totalPrice;
        });
        $('#totalAmount').val(totalAmount.toFixed(2));
    }

    function attachEventListeners() {
        $('.invoiceItem').on('change', '.quantityInput, .priceInput', calculateTotal);
        $('body').on('click', '.removeItem', function () {
            $(this).closest('.invoiceItem').remove();
            recalculateIndexes();
            calculateTotal();
        });
    }

    function generateInvoiceItem(index) {
        var html = `<div class="invoiceItem my-3">
                <div class="row mb-2">
                    <div class="col-5">
                        <label>Opis</label>
                        <input name="Items[${index}].Description" class="form-control" />
                    </div>
                    <div class="col-2">
                        <label>Ilość</label>
                        <input name="Items[${index}].Quantity" type="number" class="form-control quantityInput" />
                    </div>
                    <div class="col-2">
                        <label>Cena</label>
                        <input name="Items[${index}].UnitPrice" type="number" id="priceInput" class="form-control priceInput" />
                    </div>
                    <div class="col-2">
                        <label>Suma</label>
                        <input readonly="readonly" class="form-control totalPriceInput" />
                    </div>
                    <div class="col-1">
                        <label class="text-white">Usuń</label>
                        <button type="button" class="btn btn-danger btn-sm removeItem"><i class="bi bi-trash"></i></button>
                    </div>
                </div>
            </div>`;
        var element = $(html);
        attachEventListenersToItem(element);
        return element;
    }

    $('#addNewItem').click(function () {
        const newItemIndex = $("#invoiceItems .invoiceItem").length;
        var newItem = generateInvoiceItem(newItemIndex);
        $('#invoiceItems').append(newItem);
        calculateTotal();  // Oblicz total po dodaniu nowego elementu
    });

    function attachEventListenersToItem(item) {
        item.find('.quantityInput, .priceInput').change(calculateTotal);
        item.find('.removeItem').click(function () {
            $(this).closest('.invoiceItem').remove();
            recalculateIndexes();
            calculateTotal();
        });
    }

    attachEventListeners(); // Attach to existing items on page load
    calculateTotal(); // Initial total calculation for loaded data
});
