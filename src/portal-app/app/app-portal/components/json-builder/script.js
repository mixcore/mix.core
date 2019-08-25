
modules.component('jsonBuilder', {
    templateUrl: '/app/app-portal/components/json-builder/view.html',
    bindings: {
        'data': '=?', // json obj (ex: { field1: 'some val' })
        'folder': '=?', // filepath (ex: 'data/jsonfile.json')
        'filename': '=?', // filepath (ex: 'data/jsonfile.json')
        'allowedTypes': '=?' // string array ( ex: [ 'type1', 'type2' ] )
    },
    controller: ['$rootScope', '$scope', '$location', 'FileServices', 'ngAppSettings',
        function ($rootScope, $scope, $location, fileService, ngAppSettings) {
            var ctrl = this;
            ctrl.file = null;
            ctrl.translate = $rootScope.translate;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.templates = [
                { type: 'item', name: 'i1', dataType: 7, value: '' },
                { type: 'object', name: 'o1', columns: [{ allowedTypes: ['array', 'object', 'item'], items: [] }] },
                { type: 'array', name: 'a1', columns: [{ allowedTypes: ['object'], items: [] }] }
            ];
            ctrl.draft = [];
            ctrl.model = {};
            ctrl.dropzones = {
                'root': []
            };
            ctrl.selected = null;
            ctrl.init = async function () {
                var arr = [];
                if (!ctrl.data && ctrl.filename) {
                    await ctrl.loadFile();
                    ctrl.parseObjToList(ctrl.data, arr);
                    ctrl.dropzones.root = arr;
                } else {
                    ctrl.parseObjToList(ctrl.data, arr);
                    ctrl.dropzones.root = arr;
                }

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
            ctrl.saveFile = async function () {            
                $rootScope.isBusy = true;
                ctrl.model = {};
                ctrl.parseObj(ctrl.dropzones.root, ctrl.model);
                ctrl.file.content = JSON.stringify(ctrl.model);
                var resp = await fileService.saveFile(ctrl.file);
                if (resp && resp.isSucceed) {
                    $scope.activedFile = resp.data;
                    $rootScope.showMessage('Thành công', 'success');
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.update = function () {
                ctrl.model = {};
                ctrl.parseObj(ctrl.dropzones.root, ctrl.model);
            };
            ctrl.parseObjToList = function (item, items) {
                // key: the name of the object key
                // index: the ordinal position of the key within the object 
                Object.keys(item).forEach(function (key, index) {
                    var obj = {};
                    var objType = typeof (item[key]);
                    switch (objType) {
                        case 'object':
                            if (Array.isArray(item[key])) {
                                obj = angular.copy(ctrl.templates[2]);
                                obj.name = key;
                                ctrl.parseObjToList(item[key], obj.columns[0].items);
                                items.push(obj);
                            } else {
                                obj = angular.copy(ctrl.templates[1]);
                                obj.name = key;
                                ctrl.parseObjToList(item[key], obj.columns[0].items);
                                items.push(obj);
                            }
                            break;
                        default:
                            obj = angular.copy(ctrl.templates[0]);
                            obj.name = key;
                            obj.value = item[key];
                            items.push(obj);
                            break;
                    }

                });
            };
            ctrl.parseObj = function (items, obj, name) {
                angular.forEach(items, item => {
                    switch (item.type) {
                        case 'array':
                            obj[item.name] = [];
                            angular.forEach(item.columns[0].items, sub => {
                                var o = {};
                                ctrl.parseObj(sub.columns[0].items, o);
                                obj[item.name].push(o);
                            });
                            break;
                        case 'object':
                            var o = {};
                            angular.forEach(item.columns[0].items, sub => {                                
                                ctrl.parseObj(sub.columns[0].items, o, item.name);
                                if (name) {
                                    obj[name] = o;
                                } else {
                                    obj[item.name] = o;
                                }
                            });
                            break;
                        case 'item':
                            obj[item.name] = item.value;
                            break;
                    }
                });
            };
            ctrl.newItem = function (item) {
                item.name = $rootScope.generateUUID();
                ctrl.update();
            }
            ctrl.addProperty = function (item, name, val) {
                item[name] = val;
            };
            ctrl.deleteProperty = function (item, name) {
                delete item[name];
            };
            ctrl.addField = function (item) {
                var i = 0;
                var field = ctrl.templates[0];
                field.name = 'f' + (item.columns[0].items.length + 1);
                item.columns[0].items.push(field);
                item.showMenu = false;
            };
            ctrl.addObj = function (item) {
                var i = 0;
                var obj = ctrl.templates[1];
                obj.name = 'o' + (item.columns[0].items.length + 1);
                item.columns[0].items.push(obj);
                item.showMenu = false;
            };
            ctrl.clone = function (item, list) {
                var obj = angular.copy(item);
                obj.name = item.name + '_copy';
                list.items.push(obj);
                item.showMenu = false;
            };
            
            ctrl.remove = function (index, list) {
                if(confirm('Remove this')){
                    list.items.splice(index,1);
                }
            };
            
        }]
});