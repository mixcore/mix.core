modules.component('jsonEditor', {
    bindings: {
        data: '=?', // json obj (ex: { field1: 'some val' })
        folder: '=?', // filepath (ex: 'data/jsonfile.json')
        filename: '=?', // filepath (ex: 'data/jsonfile.json')
    },
    templateUrl: '/app/app-portal/components/json-editor/view.html',
    controller: ['$rootScope', '$scope','$element', 'ngAppSettings', 'FileServices',
        function ($rootScope, $scope, $element, ngAppSettings, fileService) {
            var ctrl = this;
            ctrl.previousId = null;
            ctrl.editor = null;
            ctrl.options = {
                theme: 'bootstrap4'
            };

            ctrl.init = async function () {
                ctrl.guid = $rootScope.generateUUID();
                await ctrl.loadFile();
                ctrl.options.schema = ctrl.data;
                ctrl.editor =  new JSONEditor(document.getElementById('je'),ctrl.options);                
            };

            ctrl.loadFile = async function () {
                $rootScope.isBusy = true;
                $scope.listUrl = '/portal/file/list?folder=' + ctrl.folder;
                $rootScope.isBusy = true;
                var response = await fileService.getFile(ctrl.folder, ctrl.filename);
                if (response.isSucceed) {
                    ctrl.file = response.data;
                    ctrl.data = $.parseJSON(response.data.content);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    $rootScope.showErrors(response.errors);
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.updateContent = function () {
                ctrl.content = ctrl.editor.getValue();
                console.log(ctrl.content);
            };
            
        }
    ]
});