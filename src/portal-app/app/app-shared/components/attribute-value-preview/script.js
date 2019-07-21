
modules.component('attributeValuePreview', {
    templateUrl: '/app/app-shared/components/attribute-value-preview/view.html',
    controller: ['$rootScope', function ($rootScope) {
        var ctrl = this;
    }
    ],
    bindings: {
        data: '=',
        width: '=',
    }
});