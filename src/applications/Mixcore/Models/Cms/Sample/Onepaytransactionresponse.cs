using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Onepaytransactionresponse
{
    public int Id { get; set; }

    public string VpcCommand { get; set; }

    public string VpcLocale { get; set; }

    public string VpcCurrencyCode { get; set; }

    public string VpcMerchTxnRef { get; set; }

    public string VpcMerchant { get; set; }

    public string VpcOrderInfo { get; set; }

    public string VpcAmount { get; set; }

    public string VpcTxnResponseCode { get; set; }

    public string VpcTransactionNo { get; set; }

    public string VpcMessage { get; set; }

    public string VpcAdditionData { get; set; }

    public string VpcSecureHash { get; set; }

    public string OnepayStatus { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }
}
