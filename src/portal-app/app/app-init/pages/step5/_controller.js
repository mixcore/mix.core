'use strict';
app.controller('Step5Controller', ['$scope', '$rootScope', 
    'CommonService', 'Step5Services',
    function ($scope, $rootScope, commonService, service) {
        var rand = Math.random();
        $scope.data = {
            isCreateDefault: true,
            theme:null,
        };
        $scope.init = async function () {
           
        };
        
        $scope.submit = async function () {
            $rootScope.isBusy = true;            
            var form = document.getElementById('frm-theme');
            var frm = new FormData();
            var url = '/init/init-cms/step-5';

            $rootScope.isBusy = true;
            // Looping over all files and add it to FormData object
            frm.append('theme', form['theme'].files[0]);
            // Adding one more key to FormData object
            frm.append('model', angular.toJson($scope.data));

            var response = await service.ajaxSubmitForm(frm, url);
            if (response.isSucceed) {
                $scope.activedData = response.data;
                $rootScope.isBusy = false;
                window.top.location="/";
                $scope.$apply();
            }
            else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }]);