
modules.component('urlAlias', {
    templateUrl: '/app-shared/components/url-alias/url-alias.html',
    controller: ['$scope', function ($scope) {
        var ctrl = this;        
    }],
    bindings: {
        urlAlias: '=',
        callback: '&'
    }
});