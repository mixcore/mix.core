modules.component('attributeList', {
    templateUrl: '/app/app-portal/components/attribute-list/view.html',
    controller: ['$rootScope', '$scope','AttributeFieldService',
        function ($rootScope, $scope, service) {
            var ctrl = this;
            ctrl.selectedCol = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.$onInit = async function(){
                var getDefaultAttr = await service.getSingle([null, 'portal']);
                if (getDefaultAttr.isSucceed) {
                    ctrl.defaultAttr = getDefaultAttr.data;
                    ctrl.defaultAttr.options = [];
                }
            };
            ctrl.addAttr = function () {
                if (ctrl.attributes) {
                    var t = angular.copy(ctrl.defaultAttr);
                    t.priority = ctrl.attributes.length +1;
                    ctrl.attributes.push(t);
                }
            };
            ctrl.removeAttribute = function(attr, index){
                ctrl.attributes.splice(index,1);
                ctrl.removeAttributes.push(attr);
            };
            ctrl.addOption = function (col, index) {
                var val = $('#option_' + index).val();
                col.options = col.options || [];
                var opt = {
                    'value': val,
                    'dataType': 7
                };
                col.options.push(opt);
                $('#option_' + index).val('');
            };
            ctrl.generateForm = function(){
                var formHtml = document.createElement('module-form');
                formHtml.setAttribute('class','row');
                angular.forEach(ctrl.activedData.attributes, function(e,i){
                    var el;
                    var label = document.createElement('label');
                    label.setAttribute('class', 'control-label');
                    label.setAttribute('ng-bind', '{{data.title}}');
                    
                    switch(e.dataType){
                        case 1:
                        el = document.createElement('input');
                        el.setAttribute('type', 'datetime-local');                                 
                        break;
                        
                        case 2:
                        el = document.createElement('input');
                        el.setAttribute('type', 'date');                                 
                        break;
                        
                        case 3:
                        el = document.createElement('input');
                        el.setAttribute('type', 'time');                                 
                        break;
    
                        case 5:
                        el = document.createElement('input');
                        el.setAttribute('type', 'tel');                                 
                        break;
                       
                        case 6:
                        el = document.createElement('input');
                        el.setAttribute('type', 'number');                                 
                        break;
                       
                        case 8:
                        el = document.createElement('trumbowyg');
                        el.setAttribute('options', '{}');                                 
                        el.setAttribute('type', 'number');                                 
                        break;
                        
                        case 9:
                        el = document.createElement('textarea');
                        break;
    
                        default:
                        el = document.createElement('input');
                        el.setAttribute('type', 'text');
                        formHtml.appendChild(el);
                        break;
                    }
                    el.setAttribute('ng-model', 'data.jItem[' + e.name + '].value');
                    el.setAttribute( 'placeholder', '{{$ctrl.title}}');
                    formHtml.appendChild(label);      
                    formHtml.appendChild(el);      
                    
                });
                console.log(formHtml);
                ctrl.activedData.formView.content = formHtml.innerHTML;
            };
    
            ctrl.generateName = function (col) {
                col.name = $rootScope.generateKeyword(col.title, '_');
            };
            ctrl.removeAttr = function (index) {
                if (ctrl.attributes) {
                    ctrl.attributes.splice(index, 1);
                }
            };
            ctrl.updateOrders = function(index){
                if(index> ctrl.dragStartIndex){
                    ctrl.attributes.splice(ctrl.dragStartIndex, 1);
                }
                else{
                    ctrl.attributes.splice(ctrl.dragStartIndex+1, 1);
                }
                angular.forEach(ctrl.attributes, function(e,i){
                    e.priority = i;
                });
            };           
            ctrl.dragStart = function(index){
                ctrl.dragStartIndex = index;
            };
            ctrl.showReferences = function(col){
                ctrl.colRef = col;
                $('#modal-navs').modal('show');
            };
            ctrl.referenceCallback = function(selected){
                if (selected && selected.length) {
                    ctrl.colRef.reference = selected;
                    ctrl.colRef.referenceId = selected[0].id;
                }                
                $('#modal-navs').modal('hide');
            };
        }],
    bindings: {
        header: '=',
        attributes: '=',
        removeAttributes: '='
    }
});