
modules.component('urlAlias', {
    templateUrl: '/app-portal/components/url-alias/url-alias.html',
    controller: ['$scope', function ($scope) {
        var ctrl = this;        
    }],
    bindings: {
        urlAlias: '=',
        callback: '&'
    }
});