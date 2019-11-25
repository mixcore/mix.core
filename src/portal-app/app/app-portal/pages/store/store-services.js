'use strict';
app.factory('StoreService', ['BaseService','CommonService', function (baseService, commonService) {

    var serviceFactory = angular.copy(baseService);
    
    return serviceFactory;

}]);
