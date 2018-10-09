modules.component('templateEditor', {
    templateUrl: '/app-portal/components/template-editor/templateEditor.html',
    controller: ['$scope', '$rootScope', '$routeParams', 'ngAppSettings', 'GlobalSettingsService', 'TemplateService',
        function ($scope, $rootScope, $routeParams, ngAppSettings, globalSettingsService, service) {
            BaseCtrl.call(this, $scope, $rootScope, $routeParams, ngAppSettings, service);
            var ctrl = this;
            globalSettingsService.fillGlobalSettings().then(function (response) {
                ctrl.settings = response;
                setTimeout(() => {
                    ctrl.loadTemplates();
                }, 200);
            });
            ctrl.loadParams = async function () {
                $rootScope.isBusy = true;
                ctrl.folderType = $routeParams.folderType ? $routeParams.folderType : 'Masters';
                ctrl.backUrl = '/portal/template/list/' + $routeParams.themeId;
                ctrl.themeId = $routeParams.themeId;
            }
            ctrl.loadTemplates = async function (activedId, activedName, folder) {

                if (folder) {
                    ctrl.folder = folder;
                    ctrl.activedId = activedId;
                    ctrl.activedName = activedName;
                }
                else if (ctrl.template) {
                    ctrl.folder = ctrl.template.folderType;
                    ctrl.activedId = ctrl.template.id;
                    ctrl.activedName = ctrl.template.fileName;
                }
                var request = {
                    pageSize: null,
                    pageIndex: 0,
                    orderBy: 'fileName',
                    direction: 0,
                    key: ctrl.template.folderType,
                    keyword: ''
                }
                var result = await service.getList(request, [ctrl.settings.themeId]);
                if (result.isSucceed) {
                    ctrl.initTemplate(result, ctrl.activedId, ctrl.activedName);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (result) { $rootScope.showErrors(result.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

            ctrl.initTemplate = function (response, activedId, activedName) {
                ctrl.templates = response.data.items;
                if (ctrl.templates != undefined && ctrl.templates.length > 0) {
                    var newTemplate = angular.copy(ctrl.templates[0]);
                    newTemplate.id = 0;
                    newTemplate.fileName = 'create new';
                    newTemplate.content = "<div></div>";
                    ctrl.templates.splice(0, 0, newTemplate);
                    var templates = ctrl.templates;
                    $.each(templates, function (i, e) {
                        if (e.id == activedId) {
                            ctrl.template = e;
                        }
                        if (e.fileName == activedName) {
                            ctrl.template = e;
                        }
                    });
                    ctrl.updateEditors();
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };

            ctrl.selectTemplate = function (template) {
                ctrl.template = template;
                ctrl.updateEditors();
            };
            ctrl.updateEditors = function () {
                setTimeout(function () {
                    $.each($('.code-editor'), function (i, e) {
                        var container = $(this);
                        var editor = ace.edit(e);
                        var val = $(this).next('input').val();
                        editor.setValue(val);
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
                            vm.template.content = editor.getValue();
                            $(container).parent().find('.code-content').val(editor.getValue());
                        });
                    });
                }, 200);
            }
        }],
    bindings: {
        template: '='
    }
});
