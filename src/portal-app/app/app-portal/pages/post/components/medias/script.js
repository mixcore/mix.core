
app.component('postMedias', {
    templateUrl: '/app/app-portal/pages/post/components/medias/view.html',
    controller: function () {
        var ctrl = this;
        ctrl.activeMedia = function (media) {
            var currentItem = null;
            if (ctrl.post.mediaNavs === null) {
                ctrl.post.mediaNavs = [];
            }
            $.each(ctrl.post.mediaNavs, function (i, e) {
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
                    post: ctrl.post.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.post.mediaNavs.length + 1,
                    isActived: true
                };
                media.isHidden = true;
                ctrl.post.mediaNavs.push(currentItem);
            }
        }        
    },
    bindings: {
        post: '=',
        medias: '=',
        loadMedia: '&',
        onDelete: '&',
        onUpdate: '&'
    }
});