modules.component('employeeForm', {
    templateUrl: '/app/app-client/components/employee-form/view.html',
    bindings: {
        attrSetId: '=',
        attrSetName: '=',
        attrDataId: '=?',
        attrData: '=?',
        parentType: '=?', // attribute set = 1 | post = 2 | page = 3 | module = 4
        parentId: '=?',
        defaultId: '=',
        saveData: '&?'
    },
    controller: ['$rootScope', '$scope', 'BaseService', 'AttributeSetDataService',
        function ($rootScope, $scope, baseService, service) {
            var ctrl = this;
            ctrl.isBusy = false;
            ctrl.attributes = [];
            ctrl.defaultData = null;
            ctrl.selectedProp = null;
            ctrl.settings = $rootScope.globalSettings;
            ctrl.mediaFile = {
                file: null,
                fullPath: '',
                folder: 'module-data',
                title: '',
                description: ''
            };
            ctrl.$onInit = async function () {
                ctrl.loadData();
            };
            ctrl.loadData = async function () {

                /*
                    If input is data id => load ctrl.attrData from service and handle it independently
                    Else modify input ctrl.attrData
                */
                $rootScope.isBusy = true;
                ctrl.defaultData = await service.getSingle('portal', [ctrl.defaultId, ctrl.attrSetId, ctrl.attrSetName]);
                if (ctrl.attrDataId) {
                    ctrl.attrData = await service.getSingle('portal', [ctrl.attrDataId, ctrl.attrSetId, ctrl.attrSetName]);
                    if (ctrl.attrData) {
                        ctrl.defaultData.attributeSetId = ctrl.attrData.attributeSetId;
                        ctrl.defaultData.attributeSetName = ctrl.attrData.attributeSetName;
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    } else {
                        if (ctrl.attrData) {
                            $rootScope.showErrors('Failed');
                        }
                        $rootScope.isBusy = false;
                        $scope.$apply();
                    }

                }
                else {
                    if (!ctrl.attrData) {
                        ctrl.attrData = angular.copy(ctrl.defaultData);
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            };
            ctrl.reload = async function () {
                ctrl.attrData = angular.copy(ctrl.defaultData);
            };
            ctrl.submit = async function () {
                angular.forEach(ctrl.attrData.values, function (e) {
                    //Encrypt field before send
                    if (e.field && e.field.isEncrypt) {
                        var encryptData = $rootScope.encrypt(e.stringValue);
                        e.encryptKey = encryptData.key;
                        e.encryptValue = encryptData.data;
                        e.stringValue = null;
                    }
                });

                ctrl.isBusy = true;
                var saveResult = await service.save('portal', ctrl.attrData);
                if (saveResult.isSucceed) {
                    ctrl.isBusy = false;
                    $rootScope.isBusy = false;
                    $scope.$apply();
                } else {
                    ctrl.isBusy = false;
                    $rootScope.isBusy = false;
                    if (saveResult) {
                        $rootScope.showErrors(saveResult.errors);
                    }
                    $scope.$apply();
                }
            };

            ctrl.filterData = function (attributeName) {
                if (ctrl.attrData) {
                    var attr = $rootScope.findObjectByKey(ctrl.attrData.values, 'attributeFieldName', attributeName);
                    if (!attr) {
                        attr = angular.copy($rootScope.findObjectByKey(ctrl.defaultData.values, 'attributeFieldName', attributeName));
                        ctrl.attrData.data.push(attr);
                    }
                    return attr;
                }
            };

            ctrl.saveEmployee = function () {
                var msg = $rootScope.settings.data['employee_confirm_msg'] || 'Are you sure you want to submit this form? Please be noted that after submission, all information cannot be changed or adjusted.';
                $rootScope.showConfirm(ctrl, 'saveEmployeeConfirmed', [], null, '', msg);
            };
            ctrl.saveEmployeeConfirmed = async function () {
                if (ctrl.validateEmployee(ctrl.attrData)) {
                    var mediaService = angular.copy(baseService);
                    mediaService.init('media');
                    $rootScope.isBusy = true;
                    var getMedia = await mediaService.getSingle(['portal']);
                    if (getMedia.isSucceed) {
                        
                        var media = getMedia.data;
                        media.title = '';
                        media.description = '';
                        media.mediaFile = ctrl.mediaFile;

                        var url = mediaService.prefixUrl + '/save';
                        var fd = new FormData();
                        // var file = media.mediaFile.file;
                        media.mediaFile.file = null;
                        fd.append('model', angular.toJson(media));
                        fd.append('file', null);                        
                        
                        var resp = await mediaService.ajaxSubmitForm(fd, url);
                        if (resp && resp.isSucceed) {
                            var avatar = ctrl.filterData('avatar');
                            if (avatar) {
                                avatar.stringValue = resp.data.fullPath;
                            }

                            return ctrl.submit();
                        }
                        else {
                            if (resp) { $rootScope.showErrors(resp.errors); }
                            $rootScope.isBusy = false;
                            $scope.$apply();
                        }
                    }

                }
            };
            ctrl.validateEmployee = function (data) {
                if (!ctrl.mediaFile.fileName) {
                    $rootScope.showErrors([$rootScope.translate('avatar_required')]);
                    return false;
                } else {
                    return true;
                }
            };
        }]
});