
app.component('themeExportModules', {
    templateUrl: '/app/app-portal/pages/theme/components/theme-export-modules/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.updateModuleExport = function(module){
            ctrl.selectedExport.modules = angular.copy($rootScope.filterArray(ctrl.exportData.modules, 'isActived', true)); 
            angular.forEach(ctrl.selectedExport.modules,function(e){
                e.data.items = angular.copy($rootScope.filterArray(e.data.items, 'isActived', true));
            });
            
            
        };    
    }],
    bindings: {
        exportData: '=',
        selectedExport: '='
    }
});