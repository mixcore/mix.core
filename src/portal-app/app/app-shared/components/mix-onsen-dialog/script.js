
modules.component('mixOnsenDialog', {
  templateUrl: '/app/app-shared/components/mix-onsen-dialog/view.html',
  controller: ['$rootScope', '$scope', '$location', function ($rootScope, $scope, $location) {
    var ctrl = this;
    ctrl.showDialog = function () {
      if (ctrl.dialog) {
        ctrl.dialog.show();
      } else {
        ons.createElement('dialog.html', { parentScope: $scope, append: true })
          .then(function (dialog) {
            ctrl.dialog = dialog;
            dialog.show();
          }.bind(ctrl));
      }
    }.bind(ctrl);
    ctrl.alert = function (msg) {
      ons.notification.alert(msg);
    }
    ctrl.confirm = function (msg) {
      ons.notification.confirm({message: msg});
    }
    ctrl.prompt = function (msg) {
      ons.notification.prompt({message: msg});
    }
  }],
  bindings: {

  }
});