modules.component('mixDataPreview', {
    templateUrl: '/app/app-shared/components/mix-data-preview/mix-data-preview.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
    }
    ],
    bindings: {
        type: '=',
        value: '=',
        width: '=',
    }
});