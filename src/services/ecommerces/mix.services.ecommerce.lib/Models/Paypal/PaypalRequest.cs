using Mix.Services.Ecommerce.Lib.ViewModels;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public sealed class PaypalOrderRequest
    {
        public string? intent { get; set; }
        public RedirecUrls application_context { get; set; }
        public List<PurchaseUnit> purchase_units { get; set; } = new();

        public PaypalOrderRequest()
        {
        }

        public PaypalOrderRequest(string intent, OrderViewModel order, string returnUrl, string cancelUrl)
        {
            this.intent = intent;
            purchase_units.Add(new(order));
            application_context = new RedirecUrls(returnUrl, cancelUrl);
        }
    }

    public class PurchaseUnit
    {

        public PurchaseUnit(OrderViewModel order)
        {
            foreach (var item in order.OrderItems)
            {
                items.Add(new(item));
            }
            amount = new(order);
        }
        public string reference_id { get; set; }
        public Payee payee { get; set; }
        public List<PurchaseItem> items { get; set; } = new();
        public AmountV2 amount { get; set; }

    }
    public class PaypalLink
    {
        public string rel { get; set; }
        public string method { get; set; }
        public string href { get; set; }
    }

    public class PurchaseItem
    {
        public PurchaseItem(OrderItemViewModel item)
        {
            name = item.Title;
            description = item.Description;
            quantity = item.Quantity;
            unit_amount = new(item);
        }

        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public UnitAmount unit_amount { get; set; }
    }

    public class AmountV2
    {
        public AmountV2(OrderViewModel order)
        {
            currency = order.Currency;
            value = $"{order.Total}.00";
            breakdown = new(order);
        }

        public string currency { get; set; }
        public string value { get; set; }
        public Breakdown breakdown { get; set; }
    }
    public class Payee
    {
        public string merchant_id { get; set; }
        public string email_address { get; set; }
    }

    public class Breakdown
    {
        public Breakdown(OrderViewModel order)
        {
            item_total = new(order.Currency, order.Total);
        }

        public UnitAmount item_total { get; set; }
        public UnitAmount shipping { get; set; }
        public UnitAmount handling { get; set; }
        public UnitAmount tax_total { get; set; }
        public UnitAmount insurance { get; set; }
        public UnitAmount shipping_discount { get; set; }
        public UnitAmount discount { get; set; }
    }

    public class UnitAmount
    {
        public UnitAmount(OrderItemViewModel item)
        {
            currency = item.Currency;
            value = $"{item.Price}.00";
        }

        public UnitAmount(string? currency, double? total)
        {
            this.currency = currency;
            value = $"{total}.00";
        }

        public string currency { get; set; }
        public string value { get; set; }
    }

    public class RedirecUrls
    {
        public RedirecUrls(string returnUrl, string cancelUrl)
        {
            return_url = returnUrl;
            cancel_url = cancelUrl;
        }

        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
}
