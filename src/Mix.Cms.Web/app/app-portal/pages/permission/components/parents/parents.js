
app.component('permissionParents', {
    templateUrl: '/app-portal/pages/permission/components/parents/parents.html',    
    bindings: {
        page: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});