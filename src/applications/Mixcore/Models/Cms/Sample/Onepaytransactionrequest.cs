using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Onepaytransactionrequest
{
    public int Id { get; set; }

    public int VpcVersion { get; set; }

    public string VpcCurrency { get; set; }

    public string VpcCommand { get; set; }

    public string VpcAccessCode { get; set; }

    public string VpcMerchant { get; set; }

    public string VpcLocale { get; set; }

    public string VpcReturnUrl { get; set; }

    public string VpcMerchTxnRef { get; set; }

    public string VpcOrderInfo { get; set; }

    public string VpcAmount { get; set; }

    public string VpcTicketNo { get; set; }

    public string AgainLink { get; set; }

    public string Title { get; set; }

    public string VpcSecureHash { get; set; }

    public string VpcCustomerPhone { get; set; }

    public string VpcCustomerEmail { get; set; }

    public string VpcCustomerId { get; set; }

    public string OnepayStatus { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }
}
