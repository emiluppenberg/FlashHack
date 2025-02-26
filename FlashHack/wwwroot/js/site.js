
// site.js
$(document).ready(function () {
    $(".loading-screen").hide();

    $(document).ajaxStart(function () {
        $(".loading-screen").show();
    }).ajaxStop(function () {
        $(".loading-screen").hide();
    });

    $(window).on('load', function () {
        $(".loading-screen").hide();
    });

    $(".loading-screen").show();
});
