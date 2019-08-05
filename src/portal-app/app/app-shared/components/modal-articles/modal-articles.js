modules.component('modalPosts', {
    templateUrl: '/app/app-shared/components/modal-posts/modal-posts.html',
    controller: 'ModalPostController',
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