modules.component('googleAnalytic', {
  templateUrl: '/app/app-portal/components/google-analytic/view.html',
  controller: [
    '$rootScope', 'CommonService',
    function ($rootScope, commonService) {
      var ctrl = this;
      ctrl.init = function () {
        gapi.analytics.ready(function () {

          /**
           * Authorize the user immediately if the user has already granted access.
           * If no access has been created, render an authorize button inside the
           * element with the ID "embed-api-auth-container".
           */
          gapi.analytics.auth.authorize({
            container: 'embed-api-auth-container',
            //REPLACE WITH YOUR CLIENT ID
            clientid: $rootScope.settings.data.Google_Analytic_Key
            //secret: 'C1XJeorJGwTE5tLzgmSQP2_D'
          });


          /**
           * Create a ViewSelector for the first view to be rendered inside of an
           * element with the id "view-selector-1-container".
           */
          var viewSelector1 = new gapi.analytics.ViewSelector({
            container: 'view-selector-1-container'
          });
          /**
           * Create a new ActiveUsers instance to be rendered inside of an
           * element with the id "active-users-container" and poll for changes every
           * five seconds.
           */
          var activeUsers = new gapi.analytics.ext.ActiveUsers({
            container: 'active-users-container',
            pollingInterval: 5
          });
          /**
           * Add CSS animation to visually show the when users come and go.
           */
          activeUsers.once('success', function () {
            var element = this.container.firstChild;
            var timeout;

            this.on('change', function (data) {
              var element = this.container.firstChild;
              var animationClass = data.delta > 0 ? 'is-increasing' : 'is-decreasing';
              element.className += (' ' + animationClass);

              clearTimeout(timeout);
              timeout = setTimeout(function () {
                element.className =
                  element.className.replace(/ is-(increasing|decreasing)/g, '');
              }, 3000);
            });
          });

          /**
           * Create a ViewSelector for the second view to be rendered inside of an
           * element with the id "view-selector-2-container".
           */
          var viewSelector2 = new gapi.analytics.ViewSelector({
            container: 'view-selector-2-container'
          });

          // Render both view selectors to the page.
          // viewSelector1.execute();
          //viewSelector2.execute();


          /**
           * Create the first DataChart for top countries over the past 30 days.
           * It will be rendered inside an element with the id "chart-1-container".
           */
          var dataChart1 = new gapi.analytics.googleCharts.DataChart({
            query: {
              // ids: $rootScope.settings.data.Google_Analytic_Ids,
              metrics: 'ga:sessions',
              dimensions: 'ga:date',
              'start-date': '30daysAgo',
              'end-date': 'yesterday'
            },
            chart: {
              container: 'chart-1-container',
              type: 'LINE',
              options: {
                width: '100%',
                legendTextStyle: { color: '#FFF' },
                titleTextStyle: { color: '#FFF' },
                backgroundColor: { fill: 'transparent' },
                hAxis: {
                  textStyle: { color: '#FFF' }
                },
                vAxis: {
                  textStyle: { color: '#FFF' }
                }
              }
            }
          });


          dataChart1.set({ query: { ids: $rootScope.settings.data.Google_Analytic_Ids } }).execute();
          activeUsers.set({ ids: $rootScope.settings.data.Google_Analytic_Ids }).execute();


          /**
           * Create the second DataChart for top countries over the past 30 days.
           * It will be rendered inside an element with the id "chart-2-container".
           */
          var dataChart2 = new gapi.analytics.googleCharts.DataChart({
            query: {
              // ids: $rootScope.settings.data.Google_Analytic_Ids,
              metrics: 'ga:sessions',
              dimensions: 'ga:country',
              'start-date': '30daysAgo',
              'end-date': 'yesterday',
              'max-results': 6,
              sort: '-ga:sessions'
            },
            chart: {
              container: 'chart-2-container',
              type: 'PIE',
              options: {
                width: '100%',
                pieHole: 4 / 9
              }
            }
          });

          /**
           * Update the first dataChart when the first view selecter is changed.
           */
          viewSelector1.on('change', function (ids) {
            dataChart1.set({ query: { ids: ids } }).execute();
            // Start tracking active users for this view.
            activeUsers.set({ ids: ids }).execute();
          });

          /**
           * Update the second dataChart when the second view selecter is changed.
           */
          viewSelector2.on('change', function (ids) {
            dataChart2.set({ query: { ids: ids } }).execute();
          });

        });
      }
    }
  ],
  bindings: {
    totalPriceStatus: '=',
    currencyCode: '=',
    totalPrice: '='
  }
});