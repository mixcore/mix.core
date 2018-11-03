
app.component('articleMedias', {
    templateUrl: '/app/app-portal/pages/article/components/medias/articleMedias.html',
    controller: function () {
        var ctrl = this;
        ctrl.activeMedia = function (media) {
            var currentItem = null;
            if (ctrl.article.mediaNavs === null) {
                ctrl.article.mediaNavs = [];
            }
            $.each(ctrl.article.mediaNavs, function (i, e) {
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
                    article: ctrl.article.id,
                    specificulture: media.specificulture,
                    position: 0,
                    priority: ctrl.article.mediaNavs.length + 1,
                    isActived: true
                };
                media.isHidden = true;
                ctrl.article.mediaNavs.push(currentItem);
            }
        }
    },
    bindings: {
        article: '=',
        medias: '=',
        loadMedia: '&',
        onDelete: '&',
        onUpdate: '&'
    }
});