modules.component('mediumNews', {
  templateUrl: '/app/app-portal/components/medium-news/view.html',
  controller: [
    '$rootScope', '$http',
    function ($rootScope, $http) {
      var ctrl = this;
      ctrl.items = [];
      ctrl.init = function () {
        var req= {
          method: 'GET',
          url: 'https://api.rss2json.com/v1/api.json?rss_url=https://medium.com/feed/mixcore'
        };
        ctrl.getMediumApiResult(req);
      };

      ctrl.getMediumApiResult = async function (req) {
        return $http(req).then(function (resp) {
          if(resp.status == '200')
          {
              ctrl.items = resp.data.items;
          }
          else{
            console.log(resp);
 
          }
        },
          function (error) {
            return { isSucceed: false, errors: [error.statusText || error.status] };
          });
      };
    }
  ],
  bindings: {
  }
});