
app.component('themeExportAttributeSets', {
    templateUrl: '/app/app-portal/pages/theme/components/theme-export-attribute-sets/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.updateAttributeSetExport = function(){
            ctrl.selectedExport.attributeSets = angular.copy($rootScope.filterArray(ctrl.exportData.attributeSets, ['isActived'], [true])); 
            angular.forEach(ctrl.selectedExport.attributeSets,function(e){
                e.data = angular.copy($rootScope.filterArray(e.data, ['isActived'], [true]));
            });
        };    
        ctrl.selectAll = function (isSelectAll, arr) {
            ctrl.selectedList.data = [];
            angular.forEach(arr, function (e) {
                e.isActived = isSelectAll;
                ctrl.updateAttributeSetExport();
            });
        };        
    }],
    bindings: {
        exportData: '=',
        selectedExport: '='
    }
});