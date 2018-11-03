
app.component('mediaNavs', {
    templateUrl: '/app/app-shared/components/media-navs/media-navs.html',
    controller: ['$rootScope', '$scope', 'ngAppSettings', 'MediaService', 
        function ($rootScope, $scope, ngAppSettings, mediaService) {
        var ctrl = this;
        mediaService.init('media');
        ctrl.request = {
            pageSize: '10',
            pageIndex: 0,
            status: '2',
            orderBy: 'CreatedDateTime',
            direction: '1',
            fromDate: null,
            toDate: null,
            keyword: ''
        };
        ctrl.medias = [];
        ctrl.activeMedia = function (media) {
            var currentItem = null;
            if (ctrl.mediaNavs === null) {
                ctrl.mediaNavs = [];
            }
            $.each(ctrl.mediaNavs, function (i, e) {
                if (e.mediaId === media.id) {
                    e.isActived = media.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    description: media.description !== 'undefined' ? media.description : '',
                    image: media.fullPath,
                    mediaId: media.id,
                    product: ctrl.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.mediaNavs.length + 1,
                    isActived: true
                };
                ctrl.mediaNavs.push(currentItem);
            }
        };
        ctrl.loadMedias = async function (pageIndex) {
            if (pageIndex !== undefined) {
                ctrl.request.pageIndex = pageIndex;
            }
            if (ctrl.request.fromDate !== null) {
                var d = new Date(ctrl.request.fromDate);
                ctrl.request.fromDate = d.toISOString();
            }
            if (ctrl.request.toDate !== null) {
                ctrl.request.toDate = ctrl.request.toDate.toISOString();
            }
            if ($rootScope.globalSettings) {
                $rootScope.isBusy = true;
                console.log(mediaService);
                var resp = await mediaService.getList(ctrl.request);
                if (resp && resp.isSucceed) {
                    ctrl.medias = resp.data;
                    if (ctrl.mediaNavs) {
                        angular.forEach(ctrl.medias.items, function (value, key) {
                            var temp = ctrl.mediaNavs.filter(function (item) {
                                return item.mediaId === value.id;
                            })[0];
                            value.isActived = temp !== undefined;
                        });
                    }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
                else {
                    if (resp) { $rootScope.showErrors(resp.errors); }
                    $rootScope.isBusy = false;
                    $scope.$apply();
                }
            }
        };
    }],
    bindings: {
        mediaNavs: '=',
        loadMedia: '&',
        onDelete: '&',
        onUpdate: '&'
    }
});