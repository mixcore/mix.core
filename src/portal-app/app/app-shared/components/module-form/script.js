modules.component('moduleForm', {
    templateUrl: '/app/app-shared/components/module-form/view.html',
    controller: ['$scope', '$rootScope', 'ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'SharedModuleDataService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, moduleDataService) {
            var ctrl = this;
            $rootScope.isBusy = false;

            ctrl.initModuleForm = async function () {
                var resp = null;
                if (!$rootScope.isInit) {
                    setTimeout(function () { ctrl.initModuleForm(); }, 500);
                } else {
                    if (!ctrl.moduleId) {
                        resp = await moduleDataService.initModuleForm(ctrl.name);
                    }
                    else {
                        resp = await moduleDataService.getModuleData(ctrl.moduleId, ctrl.d, 'portal');
                    }
                    if (resp && resp.isSucceed) {
                        ctrl.data = resp.data;
                        ctrl.data.postId = ctrl.postId;
                        ctrl.data.productId = ctrl.productId;
                        ctrl.data.categoryId = ctrl.categoryId;
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                    else {
                        if (resp) { $rootScope.showErrors(resp.errors); }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
            };

            ctrl.loadModuleData = async function () {
                $rootScope.isBusy = true;
                var id = $routeParams.id;
                var response = await moduleDataService.getModuleData(ctrl.moduleId, ctrl.d, 'portal');
                if (response.isSucceed) {
                    ctrl.data = response.data;
                    //$rootScope.initEditor();
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.submitFormData = async function(){
                if($('.g-recaptcha').length>0)
                {
                    
                }
                else{
                    ctrl.saveModuleData();
                }
            }
            ctrl.saveModuleData = async function () {
                var form = $('#module-' + ctrl.data.moduleId);
                
                $.each(ctrl.data.dataProperties, function (i, e) {
                    switch (e.dataType) {
                        case 5:
                            e.value = $(form).find('.' + e.name).val();
                            break;
                        default:
                            e.value = e.value ? e.value.toString() : null;
                            break;
                    }
                });
                var resp = await moduleDataService.saveModuleData(ctrl.data);
                if (resp && resp.isSucceed) {
                    ctrl.data = resp.data;                    
                    if (ctrl.saveSuccessCallback) {
                        ctrl.saveSuccessCallback({ data: ctrl.data });
                    }
                    else {
                        var msg = $rootScope.translate('success');
                        $rootScope.showMessage(msg, 'success');
                        ctrl.initModuleForm();
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }
                }
                else {
                    if (resp) { 
                        // if(ctrl.failedCallback){
                        //     ctrl.failedCallback({ response: resp });
                        // }
                        // else{
                        //     $rootScope.showErrors(resp.errors);     
                        // }                        
                        $rootScope.showErrors(resp.errors);     
                    }
                    
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

        }],
    bindings: {
        moduleId: '=',
        categoryId: '=',
        productId: '=',
        postId: '=',
        d: '=',
        title: '=',
        name: '=',
        submitText: '=',
        isShowTitle: '=',
        backUrl: '=',
        saveCallback: '&?',
        failedCallback: '&?',
    }
});
