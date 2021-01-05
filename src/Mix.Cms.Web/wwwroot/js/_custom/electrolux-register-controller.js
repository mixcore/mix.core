app.controller('ElectroluxRegisterController',
  ["$scope", "$controller", "ngAppSettings",
    "RestRelatedAttributeDataPortalService",
    "RestAttributeSetDataPortalService", function ($scope, $controller, ngAppSettings, navService, dataService) {
      $controller('AttributeSetFormController', { $scope: $scope }); //This works
      $scope.receipt = null;
      $scope.initNestedData = async function () {
        var getDefaultReceipt = await dataService.initData('thong_tin_hoa_don');
        var getDefaultMedia = await dataService.initData('media');
        if (getDefaultReceipt.isSucceed) {
          $scope.receipt = getDefaultReceipt.data;
        }
        if (getDefaultMedia.isSucceed) {
          $scope.defaultMedia = getDefaultMedia.data;
        }
      }
      $scope.selectFiles = function () {
        
      }
    }]);
