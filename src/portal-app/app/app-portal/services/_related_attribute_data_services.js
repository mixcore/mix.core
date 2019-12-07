
'use strict';
app.factory('RelatedAttributeSetDataService', ['$rootScope', 'CommonService', 'BaseService',
    function ($rootScope, commonService, baseService) {
        var serviceFactory = angular.copy(baseService);
        serviceFactory.init('related-attribute-data/portal');

        var _getList = async function (viewType, request,attributeSetId,parentType, parentId) {
            request.query  = '';        
            request.key = viewType;    
            if(parentType){
                if(request.query){
                    request.query += '&';
                }
                request.query += 'parentType=' + parentType;
            }
            if(parentId){
                if(request.query){
                    request.query += '&';
                }
                request.query += "parentId='" + parentId+ "'";
            }        
            if(attributeSetId){
                if(request.query){
                    request.query += '&';
                }
                request.query += "attributeSetId=" + attributeSetId;
            }        
            var url = this.prefixUrl + '/list';
           
            var req = {
                method: 'POST',
                url: url,
                data: JSON.stringify(request)
            };
            return await commonService.getApiResult(req);
        };
        
        serviceFactory.getList = _getList;
        return serviceFactory;

    }]);
