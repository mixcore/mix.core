'use strict';
var app = angular.module('MixPortal',
    ['ngRoute', 'ngFileUpload', 'LocalStorageModule']);
    var modules = angular.module('components', []);

app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
}).directive('file', function () {
    return {
        scope: {
            file: '='
        },
        link: function (scope, el, attrs) {
            el.bind('change', function (event) {
                var files = event.target.files;
                var file = files[0];
                scope.file = file;
                scope.$apply();
            });
        }
    };
}).directive('imageonload', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('load', function () {
            });
            element.bind('error', function () {
            });
        }
    };
}).filter('utcToLocal', Filter)
    .constant('ngAppSettings', {
        apiServiceBaseUri: '/',
        serviceBase: '',
        clientId: 'ngAuthApp',
        facebookAppId: '464285300363325'
    });

app.run(['$rootScope', '$location', 'CommonService',
    function ($rootScope, $location, commonService) {
        commonService.removeSettings();
        commonService.removeTranslator();
        $rootScope.currentContext = $rootScope;
        $rootScope.errors = [];
        $rootScope.isBusy = false;
        $rootScope.message = {
            title: 'test',
            content: '',
            errors: [],
            okFuncName: null,
            okArgs: [],
            cancelFuncName: null,
            cancelArgs: [],
            lblOK: 'OK',
            lblCancel: 'Cancel',
            context: $rootScope
        };

        $rootScope.$watch('isBusy', function (newValue, oldValue) {
            if (newValue) {
                $rootScope.message.content = '';
                $rootScope.errors = [];
            }
        });

        $rootScope.showErrors = function (errors) {
            $.each(errors, function (i, e) {
                $rootScope.showMessage(e, 'danger');
            });
        }

        $rootScope.showMessage = function (content, type) {
            var from = 'bottom';
            var align = 'right';
            $.notify({
                icon: "now-ui-icons ui-1_bell-53",
                message: content,

            }, {
                    type: type,
                    timer: 2000,
                    placement: {
                        from: from,
                        align: align
                    }
                });
        }

    }]);

function Filter($filter) {
    return function (utcDateString, format) {
        // return if input date is null or undefined
        if (!utcDateString) {
            return;
        }

        // append 'Z' to the date string to indicate UTC time if the timezone isn't already specified
        if (utcDateString.indexOf('Z') === -1 && utcDateString.indexOf('+') === -1) {
            utcDateString += 'Z';
        }

        // convert and format date using the built in angularjs date filter
        return $filter('date')(utcDateString, format);
    };
}