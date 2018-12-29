modules.component('modalNavArticles', {
    templateUrl: '/app/app-portal/components/modal-nav-articles/modal-nav-articles.html',
    controller: [
        function(){
            var ctrl = this;
            ctrl.loadArticles = function(pageIndex){
                ctrl.callback({ pageIndex: pageIndex });
            }
        }
        
    ],
    bindings: {
        srcId: '=',
        data: '=',        
        request: '=',        
        onDelete: '&',
        callback: '&'        
    }
});