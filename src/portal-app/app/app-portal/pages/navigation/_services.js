'use strict';
app.factory('NavigationService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('attribute-set-data');

        var _getList = async function (viewType, objData, attributeSetId, attributeSetName, parentType, parentId) {
            objData.filter  = '';
            if(attributeSetId){
                objData.filter += 'attributeSetId eq ' + attributeSetId;
            }
            if(attributeSetName){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += "attributeSetName eq '" + attributeSetName + "'";;
            }
            if(parentType){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += 'parentType eq ' + parentType;
            }
            if(parentId){
                if(objData.filter){
                    objData.filter += ' and ';
                }
                objData.filter += "parentId eq '" + parentId + "'";
            }        
            var data = serviceFactory.parseODataQuery(objData);           
            var url = this.prefixUrl + '/' + viewType;
            if(data){
                url = url.concat(data);
            }
            var req = {
                method: 'GET',
                url: url
            };
            return await commonService.getApiResult(req);
        };

        serviceFactory.getList = _getList;
        return serviceFactory;

    }]);
