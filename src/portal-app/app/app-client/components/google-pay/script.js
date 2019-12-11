modules.component('googlePay', {
    templateUrl: '/app/app-client/components/google-pay/view.html',
    controller: [
        '$rootScope', 'CommonService',
        function ($rootScope, commonService) {
            var ctrl = this;
            ctrl.baseRequest = {
                apiVersion: 2,
                apiVersionMinor: 0
            };
            ctrl.tokenizationSpecification = {
                type: 'PAYMENT_GATEWAY',
                parameters: {
                    'gateway': 'example',
                    'gatewayMerchantId': 'exampleGatewayMerchantId'
                }
            };
            ctrl.allowedCardNetworks = ["AMEX", "DISCOVER", "JCB", "MASTERCARD", "VISA"];
            ctrl.allowedCardAuthMethods = ["PAN_ONLY", "CRYPTOGRAM_3DS"];
            ctrl.baseCardPaymentMethod = {
                type: 'CARD',
                parameters: {
                    allowedAuthMethods: ctrl.allowedCardAuthMethods,
                    allowedCardNetworks: ctrl.allowedCardNetworks
                }
            };
            ctrl.cardPaymentMethod = Object.assign(
                { tokenizationSpecification: ctrl.tokenizationSpecification },
                ctrl.baseCardPaymentMethod
            );
            ctrl.paymentsClient = null;
            ctrl.paymentDataRequest = null;
            ctrl.getGoogleIsReadyToPayRequest = function () {
                return Object.assign(
                    {},
                    ctrl.baseRequest,
                    {
                        allowedPaymentMethods: [ctrl.baseCardPaymentMethod]
                    }
                );
            };
            ctrl.getGooglePaymentDataRequest = function() {
                ctrl.paymentDataRequest = Object.assign({}, ctrl.baseRequest);
                ctrl.paymentDataRequest.allowedPaymentMethods = [ctrl.cardPaymentMethod];
                ctrl.paymentDataRequest.transactionInfo = ctrl.getGoogleTransactionInfo();
                ctrl.paymentDataRequest.merchantInfo = {
                  // @todo a merchant ID is available for a production environment after approval by Google
                  // See {@link https://developers.google.com/pay/api/web/guides/test-and-deploy/integration-checklist|Integration checklist}
                  merchantId: '01234567890123456789',
                  merchantName: 'Example Merchant'
                };
                return ctrl.paymentDataRequest;
              };
              ctrl.getGooglePaymentsClient = function() {
                if ( ctrl.paymentsClient === null ) {
                    ctrl.paymentsClient = new google.payments.api.PaymentsClient({environment: 'TEST'});
                }
                return ctrl.paymentsClient;
              }
              ctrl.onGooglePayLoaded = function() {
                ctrl.paymentsClient = ctrl.getGooglePaymentsClient();
                ctrl.paymentsClient.isReadyToPay(ctrl.getGoogleIsReadyToPayRequest())
                    .then(function(response) {
                      if (response.result) {
                        ctrl.addGooglePayButton();
                        // @todo prefetch payment data to improve performance after confirming site functionality
                        // prefetchGooglePaymentData();
                      }
                    })
                    .catch(function(err) {
                      // show error in developer console for debugging
                      console.error(err);
                    });
              };
              ctrl.addGooglePayButton = function() {
                ctrl.paymentsClient = ctrl.getGooglePaymentsClient();
                const button =
                    ctrl.paymentsClient.createButton({onClick: ctrl.onGooglePaymentButtonClicked});
                document.getElementById('container').appendChild(button);
              };
              ctrl.getGoogleTransactionInfo = function() {
                return {
                  currencyCode: ctrl.currencyCode || 'USD',
                  totalPriceStatus: ctrl.totalPriceStatus || 'FINAL',
                  // set to cart total
                  totalPrice: ctrl.totalPrice || '0.00'
                };
              };
              ctrl.prefetchGooglePaymentData = function() {
                ctrl.paymentDataRequest = ctrl.getGooglePaymentDataRequest();
                // transactionInfo must be set but does not affect cache
                ctrl.paymentDataRequest.transactionInfo = {
                  totalPriceStatus: 'NOT_CURRENTLY_KNOWN',
                  currencyCode: 'USD'
                };
                ctrl.paymentsClient = ctrl.getGooglePaymentsClient();
                ctrl.paymentsClient.prefetchPaymentData(ctrl.paymentDataRequest);
              };
              ctrl.onGooglePaymentButtonClicked = function() {
                ctrl.paymentDataRequest = ctrl.getGooglePaymentDataRequest();
                ctrl.paymentDataRequest.transactionInfo = ctrl.getGoogleTransactionInfo();
              
                ctrl.paymentsClient = ctrl.getGooglePaymentsClient();
                ctrl.paymentsClient.loadPaymentData(ctrl.paymentDataRequest)
                    .then(function(paymentData) {
                      // handle the response
                      ctrl.processPayment(paymentData);
                    })
                    .catch(function(err) {
                      // show error in developer console for debugging
                      console.error(err);
                    });
              };
              ctrl.processPayment = function(paymentData) {
                // show returned data in developer console for debugging
                  // console.log(paymentData);
                // @todo pass payment token to your gateway to process payment
                paymentToken = paymentData.paymentMethodData.tokenizationData.token;
              };
              ctrl.init = function(){
                  setTimeout(() => {
                    ctrl.onGooglePayLoaded();
                  }, 1000);
              }
        }
    ],
    bindings: {
        totalPriceStatus: '=',
        currencyCode: '=',
        totalPrice: '='
    }
});