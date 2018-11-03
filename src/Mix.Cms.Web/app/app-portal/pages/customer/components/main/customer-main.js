
app.component('customerMain', {
    templateUrl: '/app/app-portal/pages/customer/components/main/customer-main.html',
    bindings: {
        customer: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});