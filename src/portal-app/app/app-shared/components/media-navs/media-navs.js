
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
            if (ctrl.data === null || ctrl.isSingle) {
                ctrl.data = [];
            }
            $.each(ctrl.data, function (i, e) {
                if (e.mediaId === media.id) {
                    e.isActived = media.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    description: media.description !== 'undefined' ? media.description : '',
                    image: media.filePath,
                    mediaId: media.id,
                    product: ctrl.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.data.length + 1,
                    isActived: true
                };
                ctrl.data.push(currentItem);
            }
            if(ctrl.isSingle){
                if(!media.isActived){
                    ctrl.output = '';
                    ctrl.data = [];
                }
                else{
                    ctrl.output = ctrl.data[0].image;
                    //ctrl.loadMedias(ctrl.request.pageIndex);
                }
            }
        };
        ctrl.loadMedias = async function (pageIndex) {
            if(!ctrl.prefix){
                ctrl.prefix = 'media_navs';
            }
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
                var resp = await mediaService.getList(ctrl.request);
                if (resp && resp.isSucceed) {
                    ctrl.medias = resp.data;
                    if (ctrl.data) {
                        angular.forEach(ctrl.medias.items, function (value, key) {
                            var temp = ctrl.data.filter(function (item) {
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
        data: '=',
        prefix: '=',
        sourceFieldName:'=',        
        isSingle: '=',
        output: '=',
        loadMedia: '&',
        onDelete: '&',
        onUpdate: '&'
    }
});