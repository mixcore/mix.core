using Mix.Services.Ecommerce.Lib.Interfaces;
using Mix.Services.Ecommerce.Lib.ViewModels;
using Newtonsoft.Json.Linq;

namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public class PaypalOrderRequest
    {
        public string? intent { get; set; }
        public RedirecUrls application_context { get; set; }
        public List<PaypalRequestPurchaseUnit> purchase_units { get; set; } = new();

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

    public class PaypalRequestPurchaseUnit
    {
        public PaypalRequestPurchaseUnit()
        {
            
        }
        public PaypalRequestPurchaseUnit(OrderViewModel order)
        {
            foreach (var item in order.OrderItems)
            {
                items.Add(new(item));
            }
            amount = new(order);
            //if (order.Address != null && !string.IsNullOrEmpty(order.Address.CountryCode) && !string.IsNullOrEmpty(order.Address.PostalCode))
            //{
            //    shipping = new()
            //    {
            //        address = new PaypalAddress()
            //        {
            //            address_line_1 = order.Address?.ToString(),
            //            address_line_2 = order.Address?.ToString(),
            //            country_code = order.Address?.CountryCode ?? "US",
            //            postal_code = order.Address?.PostalCode
            //        }
            //    };
            //}
        }

        public string reference_id { get; set; }
        public Payee payee { get; set; }
        public PaypalShipping shipping { get; set; }
        public List<PurchaseItem> items { get; set; } = new();
        public PaypalAmount amount { get; set; }

    }

    public class ResponsePurchaseUnit
    {
        public ResponsePurchaseUnit()
        {
            
        }
        public string reference_id { get; set; }
        public PaypalShipping shipping { get; set; }
        public List<PurchaseItem> items { get; set; } = new();
        public JObject seller_protection { get; set; }
        public SellerReceivableBreakdown seller_receivable_breakdown { get; set; }
        public List<PaypalLink> links { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }

    }

    public class SellerReceivableBreakdown
    {
        public SellerReceivableBreakdown()
        {
            
        }
        public PaypalAmount gross_amount { get; set; }
        public PaypalAmount paypal_fee { get; set; }
        public PaypalAmount net_amount { get; set; }
    }

    public class PaypalShipping
    {
        public string type { get; set; } = "SHIPPING";
        public PaypalName name { get; set; }
        public PaypalAddress address { get; set; }
        public PaypalShipping()
        {

        }
    }

    public class PaypalLink
    {
        public PaypalLink()
        {
            
        }
        public string rel { get; set; }
        public string method { get; set; }
        public string href { get; set; }
    }

    public class PurchaseItem
    {
        public PurchaseItem()
        {
            
        }
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

    public class PaypalAmount
    {
        public PaypalAmount()
        {
            
        }
        public PaypalAmount(OrderViewModel order)
        {
            currency_code = order.Currency;
            value = $"{order.Total}";
            breakdown = new(order);
        }

        public string currency_code { get; set; }
        public string value { get; set; }
        public Breakdown breakdown { get; set; }
    }
    public class Payee
    {
        public Payee()
        {
            
        }
        public string merchant_id { get; set; }
        public string email_address { get; set; }
    }

    public class Breakdown
    {
        public Breakdown()
        {
            
        }
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
            currency_code = item.Currency;
            value = $"{item.Price}";
        }
        public UnitAmount()
        {
            
        }
        public UnitAmount(string? currency, double? total)
        {
            currency_code = currency;
            value = $"{total}";
        }

        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class RedirecUrls
    {
        public RedirecUrls()
        {
            
        }
        public RedirecUrls(string returnUrl, string cancelUrl)
        {
            return_url = returnUrl;
            cancel_url = cancelUrl;
        }

        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }
}
