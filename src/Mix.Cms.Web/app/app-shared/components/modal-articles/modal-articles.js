modules.component('modalArticles', {
    templateUrl: '/app/app-shared/components/modal-articles/modal-articles.html',
    controller: 'ModalArticleController',
    bindings: {
        data: '=',
        childName: '=',
        canDrag: '=',
        editUrl: '=',
        columns: '=',
        onDelete: '&',
        onUpdateInfos: '&',
        onUpdateChildInfos: '&',
    }
});