
modules.component('moduleForm', {
    templateUrl: '/app/app-shared/components/module-data/module-form.html',
    controller: ['$scope', '$rootScope','ngAppSettings', '$routeParams', '$timeout', '$location', 'AuthService', 'ModuleDataService',
        function ($scope, $rootScope, ngAppSettings, $routeParams, $timeout, $location, authService, moduleDataService) {
            var ctrl = this;
            $rootScope.isBusy = false;

            ctrl.initModuleForm = async function () {
                var resp = null;
                if (!ctrl.moduleId) {
                    resp = await moduleDataService.initModuleForm(ctrl.name);
                }
                else {
                    resp = await moduleDataService.getModuleData(ctrl.moduleId, ctrl.d, 'portal');
                }
                if (resp && resp.isSucceed) {
                    ctrl.data = resp.data;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                    //ctrl.initEditor();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $scope.$apply();
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
                    ctrl.initModuleForm();
                    $rootScope.showMessage('Thành công', 'success');
                    $rootScope.isBusy = false;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                    //$location.path('/portal/moduleData/details/' + resp.data.id);
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };


        }],
    bindings: {
        moduleId: '=',
        d: '=',
        title: '=',
        name: '=',
        backUrl: '='
    }
});


modules.component('moduleFormEditor', {
    templateUrl: '/app/app-shared/components/module-data/module-form-editor.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        this.dataTypes = {
            'string': 0,
            'int': 1,
            'image': 2,
            'icon': 3,
            'codeEditor': 4,
            'html': 5,
            'textArea': 6,
            'boolean': 7,
            'mdTextArea': 8
        };
        ctrl.initEditor = function () {
            setTimeout(function () {
                // Init Code editor
                $.each($('.code-editor'), function (i, e) {
                    var container = $(this);
                    var editor = ace.edit(e);
                    if (container.hasClass('json')) {
                        editor.session.setMode("ace/mode/json");
                    }
                    else {
                        editor.session.setMode("ace/mode/razor");
                    }
                    editor.setTheme("ace/theme/chrome");
                    //editor.setReadOnly(true);

                    editor.session.setUseWrapMode(true);
                    editor.setOptions({
                        maxLines: Infinity
                    });
                    editor.getSession().on('change', function (e) {
                        // e.type, etc
                        $(container).parent().find('.code-content').val(editor.getValue());
                    });
                })
                $.each($('.editor-content'), function (i, e) {
                    var $demoTextarea = $(e);
                    $demoTextarea.trumbowyg({
                        semantic: false
                    }).on('tbwblur', function () {
                        ctrl.data.value = $demoTextarea.val();
                    });
                });
            }, 200);
        };
    }
    ],
    bindings: {
        data: '=',
        inputClass: '='
    }
});

modules.component('moduleDataPreview', {
    templateUrl: '/app/app-shared/components/module-data/module-data-preview.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
    }
    ],
    bindings: {
        data: '=',
        width: '=',
    }
});