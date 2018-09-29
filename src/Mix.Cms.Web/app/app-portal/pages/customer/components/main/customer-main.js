
app.component('customerMain', {
    templateUrl: '/app-portal/pages/customer/components/main/customer-main.html',
    bindings: {
        customer: '=',
        onDelete: '&',
        onUpdate: '&'
    }
});