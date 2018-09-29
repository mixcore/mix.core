
app.component('customerOrders', {
    templateUrl: '/app-portal/pages/customer/components/orders/customer-orders.html',
    controller: ['$rootScope', 'OrderServices', function($rootScope, orderServices){
        var ctrl = this;
        ctrl.removeOrder = function (id) {
            $rootScope.showConfirm(ctrl, 'removeOrderConfirmed', [id], null, 'Remove Order', 'Are you sure');
        }

        ctrl.removeOrderConfirmed = async function (id) {
            var result = await orderServices.removeOrder(id);
            if (result.isSucceed) {
                $rootScope.showMessage('success', 'success');
                window.top.location = window.top.location.href;
            }
            else {
                $rootScope.showMessage('failed');
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }],
    bindings: {
        customer: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});