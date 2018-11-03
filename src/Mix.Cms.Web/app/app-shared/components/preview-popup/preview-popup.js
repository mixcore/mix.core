modules.component('previewPopup', {
    templateUrl: '/app/app-shared/components/preview-popup/preview-popup.html',
    controller: ['$location', function ($location) {
        var ctrl = this;
        ctrl.goToLink = async function (link) {
            $('#dlg-preview-popup').modal('hide');
            $location.path(link);
        };
    }],
    bindings: {
        previewObject: '='
    }
});