
app.component('articleRelated', {
    templateUrl: '/app-portal/pages/article/components/related/articleRelated.html',
    controller: function () {
        var ctrl = this;
        ctrl.activeArticle = function (pr) {
            var currentItem = null;
            $.each(ctrl.article.articleNavs, function (i, e) {
                if (e.relatedArticleId == pr.id) {
                    e.isActived = pr.isActived;
                    currentItem = e;
                    return false;
                }
            });
            if (currentItem == null) {
                currentItem = {
                    relatedArticleId: pr.id,
                    sourceArticleId: ctrl.article.id,
                    specificulture: ctrl.article.specificulture,
                    priority: ctrl.article.articleNavs.length + 1,
                    relatedArticle: pr,
                    isActived: true
                };
                pr.isHidden = true;
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