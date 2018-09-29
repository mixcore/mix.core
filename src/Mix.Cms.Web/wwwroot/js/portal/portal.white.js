/*Portal JS*/
$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    $('[data-toggle="tooltip"]').tooltip({
        delay: { "show": 500, "hide": 100 }
    });

    $("#sw-docs-nav li a").click(function () {
        $(this).children("span.oi").toggleClass('oi-chevron-right').toggleClass('oi-chevron-bottom');
    });

    $(".draggable").draggable({
        cursor: "move",
        cursorAt: { top: 56, left: 56 },
        connectToSortable: ".sortable",
        helper: "clone",
        revert: "invalid"
    });
    $(".draggable-header").draggable({
        cursor: "move",
        cursorAt: { top: 56, left: 56 },
        connectToSortable: ".sortable",
        handle: ".card-header",
        helper: "clone",
        revert: "invalid"
    });
    $("ul, li").disableSelection();

    // $(window).on("scroll",function(){
    //     var wn = $(window).scrollTop();
    //   if(wn > 120){
    //       $("header").css("background","rgba(0, 0, 0, 0.88)");
    //   }
    //   else{
    //       $("header").css("background","rgba(0, 0, 0, 0.5)");
    //   }
    // });
});