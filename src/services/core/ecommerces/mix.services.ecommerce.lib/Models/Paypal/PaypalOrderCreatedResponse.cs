﻿namespace Mix.Services.Ecommerce.Lib.Models.Paypal
{
    public class PaypalOrderCreatedResponse
    {
        public string id { get; set; }
        public string? intent { get; set; }
        public string? status { get; set; }
        public DateTime create_time { get; set; }
        public List<PaypalLink> links { get; set; }
        public List<ResponsePurchaseUnit> purchase_units { get; set; } = new();

        public PaypalOrderCreatedResponse()
        {
        }
    }

}
