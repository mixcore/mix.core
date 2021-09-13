using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Payment.Domain.Enums
{
    public enum PayPalResponseStatusEnum
    {
        CREATED, // The order was created with the specified context.
        SAVED, // The order was saved and persisted. The order status continues to be in progress until a capture is made with final_capture = true for all purchase units within the order.
        APPROVED, // The customer approved the payment through the PayPal wallet or another form of guest or unbranded payment. For example, a card, bank account, or so on.
        VOIDED, // All purchase units in the order are voided.
        COMPLETED, // The payment was authorized or the authorized payment was captured for the order.
        PAYER_ACTION_REQUIRED, //  The order requires an action from the payer (e.g. 3DS authentication). Redirect the payer to the "rel":"payer-action" HATEOAS link returned as part of the response prior to authorizing or capturing the order.
    }
}
