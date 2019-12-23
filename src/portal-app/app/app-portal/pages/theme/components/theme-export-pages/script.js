
app.component('themeExportPages', {
    templateUrl: '/app/app-portal/pages/theme/components/theme-export-pages/view.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', function ($rootScope, $scope) {
        var ctrl = this;
        ctrl.updatePageExport = function(page){
            ctrl.selectedExport.pages = angular.copy($rootScope.filterArray(ctrl.exportData.pages, ['isActived'], [true])); 
            angular.forEach(ctrl.selectedExport.pages,function(e){
                e.moduleNavs = angular.copy($rootScope.filterArray(e.moduleNavs, ['isActived'], [true]));
                angular.forEach(e.moduleNavs,function(n){
                    n.module.data.items = angular.copy($rootScope.filterArray(n.module.data.items, ['isActived'], [true]));
                    $rootScope.removeObjectByKey(ctrl.exportData.modules, 'id', n.moduleId);
                    $rootScope.removeObjectByKey(ctrl.selectedExport.modules, 'id', n.moduleId);
                });
            });
            
            
        };    
    }],
    bindings: {
        exportData: '=',
        selectedExport: '='
    }
});