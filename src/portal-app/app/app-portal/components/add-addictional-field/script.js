modules.component('addAddictionalField', {
    templateUrl: '/app/app-portal/components/add-addictional-field/view.html',
    bindings: {
        parentId: '=',
        parentType: '=',
        values: '='
    },
    controller: ['$rootScope', '$scope',
        function ($rootScope, $scope, ) {
            var ctrl = this;
            ctrl.value = {};
            ctrl.field = { dataType: 7 };
            ctrl.selectedCol = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = async function () {
            };
            ctrl.addAttr = function () {
                
                if (ctrl.field.name) {
                    var current = $rootScope.findObjectByKey(ctrl.values, 'attributeFieldName', ctrl.field.name);
                    if (current) { 
                        $rootScope.showErrors(['Field ' + ctrl.field.name + ' existed!']);
                    }
                    else {
                        var t = angular.copy(ctrl.field);
                        t.priority = ctrl.values.length + 1;
                        ctrl.value.attributeFieldName = ctrl.field.name;
                        ctrl.value.dataType = ctrl.field.dataType;
                        ctrl.value.priority = ctrl.values.length + 1;
                        ctrl.value.field = t;
                        ctrl.values.push(ctrl.value);

                        //reset field option
                        ctrl.field.title = '';
                        ctrl.field.name = '';
                        ctrl.field.dataType = 0;
                    }
                }
                else {
                    $rootScope.showErrors(['Please Add Field Name']);
                }
            };

            ctrl.removeAttribute = function (attr, index) {
                if (attr) {
                    $rootScope.showConfirm(ctrl, 'removeAttributeConfirmed', [attr, index], null, 'Remove Field', 'Are you sure');
                }
            }
            ctrl.removeAttributeConfirmed = function (attr, index) {
                ctrl.fields.splice(index, 1);
                ctrl.removeAttributes.push(attr);
            };

            ctrl.generateName = function (col) {
                col.name = $rootScope.generateKeyword(col.title, '_');
            };

            ctrl.removeAttr = function (index) {
                if (ctrl.fields) {
                    ctrl.fields.splice(index, 1);
                }
            };
            ctrl.updateOrders = function (index) {
                if (index > ctrl.dragStartIndex) {
                    ctrl.fields.splice(ctrl.dragStartIndex, 1);
                }
                else {
                    ctrl.fields.splice(ctrl.dragStartIndex + 1, 1);
                }
                angular.forEach(ctrl.fields, function (e, i) {
                    e.priority = i;
                });
            };
            ctrl.dragStart = function (index) {
                ctrl.dragStartIndex = index;
            };

        }]

});