modules.component('mediumNews', {
  templateUrl: '/app/app-portal/components/medium-news/view.html',
  controller: [
    '$rootScope', 'CommonService', '$http',
    function ($rootScope, commonService,  $http) {
      var ctrl = this;
      ctrl.init = function () {
        var items = JSON.parse(_getMediumApiResult());
      }

      var _getMediumApiResult = async function (req) {
        return $http('https://api.rss2json.com/v1/api.json?rss_url=https://medium.com/feed/mixcore').then(function (resp) {
          return resp.items;
        },
          function (error) {
            return { isSucceed: false, errors: [error.statusText || error.status] };
          });
      };
    }
  ],
  bindings: {
    totalPriceStatus: '=',
    currencyCode: '=',
    totalPrice: '='
  }
});