'use strict';
app.controller('Step5Controller', ['$scope', '$rootScope',
    'CommonService', 'Step5Services',
    function ($scope, $rootScope, commonService, service) {
        var rand = Math.random();
        $scope.data = {
            isCreateDefault: true,
            theme: null,
        };
        $scope.init = async function () {
            $('.preventUncheck').on('change', function(e) {
                if ($('.preventUncheck:checked').length == 0 && !this.checked)
                    this.checked = true;
            });
            $(".option").click(function () {
                $(".option").removeClass("active");
                $(this).addClass("active");
            });
            $("#theme-1").change(function() {                
                $('.bg-register-image')[0].style.backgroundImage = "url('../assets/img/bgs/r_theme1.png')";                
            });            
            $("#theme-2").change(function() {                
                $('.bg-register-image')[0].style.backgroundImage = "url('../assets/img/bgs/r_theme2.png')";                
            });
            $("#theme-3").change(function() {                
                $('.bg-register-image')[0].style.backgroundImage = "url('../assets/img/bgs/right-bg.png')";                
            });
            $("input:checkbox").click(function() {
                if ($(this).is(":checked")) {
                    var group = "input:checkbox[name='" + $(this).attr("name") + "']";
                    $(group).prop("checked", false);
                    $(this).prop("checked", true);
                } else {
                    $(this).prop("checked", false);
                }
            });
        };
        $scope.loadProgress = async function (percent) {
            var elem = document.getElementsByClassName("progress-bar")[0];
            elem.style.width = percent + '%';
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
                window.top.location = "/";
                $scope.$apply();
            } else {
                $rootScope.showErrors(response.errors);
                $rootScope.isBusy = false;
                $scope.$apply();
            }
        }
    }
]);