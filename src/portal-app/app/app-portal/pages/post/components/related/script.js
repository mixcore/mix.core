
app.component('postRelated', {
    templateUrl: '/app/app-portal/pages/post/components/related/view.html',
    controller: function () {
        var ctrl = this;
        ctrl.activePost = function (model) {
            var currentItem = null;
            $.each(ctrl.post.postNavs, function (i, e) {
                if (e.relatedPostId === model.id) {
                    e.isActived = model.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    relatedPostId: model.id,
                    sourcePostId: ctrl.post.id,
                    specificulture: ctrl.post.specificulture,
                    priority: ctrl.post.postNavs.length + 1,
                    relatedPost: pr,
                    isActived: true
                };
                model.isHidden = true;
                ctrl.post.postNavs.push(currentItem);
            }
        }
    },
    bindings: {
        post: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});