
app.component('articleRelated', {
    templateUrl: '/app/app-portal/pages/article/components/related/articleRelated.html',
    controller: function () {
        var ctrl = this;
        ctrl.activeArticle = function (model) {
            var currentItem = null;
            $.each(ctrl.article.articleNavs, function (i, e) {
                if (e.relatedArticleId === model.id) {
                    e.isActived = model.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem === null) {
                currentItem = {
                    relatedArticleId: model.id,
                    sourceArticleId: ctrl.article.id,
                    specificulture: ctrl.article.specificulture,
                    priority: ctrl.article.articleNavs.length + 1,
                    relatedArticle: pr,
                    isActived: true
                };
                model.isHidden = true;
                ctrl.article.articleNavs.push(currentItem);
            }
        }
    },
    bindings: {
        article: '=',
        list: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});