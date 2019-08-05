
app.component('postParents', {
    templateUrl: '/app/app-portal/pages/post/components/parents/view.html',
    bindings: {
        post: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});