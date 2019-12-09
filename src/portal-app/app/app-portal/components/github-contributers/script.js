modules.component('githubContributers', {
  templateUrl: '/app/app-portal/components/github-contributers/view.html',
  controller: [
    '$rootScope', '$http',
    function ($rootScope, $http) {
      var ctrl = this;
      ctrl.items = [];
      ctrl.init = function () {
        var req= {
          method: 'GET',
          url: 'https://api.github.com/repos/mixcore/mix.core/contributors'
        };
        ctrl.getGithubApiResult(req);
      };

      ctrl.getGithubApiResult = async function (req) {
        return $http(req).then(function (resp) {
          if(resp.status == '200')
          {
              ctrl.items = resp.data;
          }
          else{
 
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