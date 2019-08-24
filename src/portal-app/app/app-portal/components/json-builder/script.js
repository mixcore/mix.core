
modules.component('jsonBuilder', {
    templateUrl: '/app/app-portal/components/json-builder/view.html',
    bindings: {
        'data': '=?', // json obj (ex: { field1: 'some val' })
        'allowedTypes': '=?' // string array ( ex: [ 'type1', 'type2' ] )
    },
    controller: ['$rootScope', '$scope', '$location', 'CommonService', 'ngAppSettings',
        function ($rootScope, $scope, $location, commonService, ngAppSettings) {
            var ctrl = this;
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
            ctrl.init = function () {

                ctrl.data = {
                    o: {
                        a: 'av',
                        b: 'bv'
                    },
                    c: 'cv',
                    d: 'dv'
                };
                var arr = [];
                ctrl.parseObjToList(ctrl.data, arr);
                ctrl.dropzones.root = arr;
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
                                obj = {
                                    name: key,
                                    type: "array",
                                    columns: [
                                        {
                                            allowedTypes: ['object'],
                                            items: []
                                        }
                                    ]
                                };
                                angular.forEach(item[key], element => {
                                    Object.keys(element).forEach(function (key, index) {
                                        ctrl.parseObjToList(element, obj.columns[0].items);
                                    });
                                });
                            } else {
                                obj = {
                                    name: key,
                                    type: "object",
                                    columns: [
                                        {
                                            allowedTypes: ['object', 'item'],
                                            items: []
                                        }
                                    ]
                                };
                                ctrl.parseObjToList(item[key], obj.columns[0].items);
                            }
                            break;
                        default:
                            obj = {
                                name: key,
                                type: "item",
                                dataType: 7,
                                value: item[key]
                            };
                            break;
                    }
                    items.push(obj);
                });
            };
            ctrl.parseObj = function (items, obj) {
                angular.forEach(items, item => {
                    switch (item.type) {
                        case 'array':
                            var arr = [];
                            var o = {};
                            angular.forEach(item.columns[0].items, sub => {
                                // array only allow obj => sub -> obj
                                ctrl.parseObj(sub.columns[0].items, o);
                                arr.push(o);
                            });
                            obj[item.name] = arr;
                            break;
                        case 'object':
                            var o = {};
                            ctrl.parseObj(item.columns[0].items, o);
                            obj[item.name] = o;
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
        }]
});