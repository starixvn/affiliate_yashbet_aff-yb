var Codition = (function ($) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },
        initEvent: function () {

            $('.info-question:not(:first-of-type) .body-result').css("display", "none");
            $('.info-question:first-of-type .button-toggle').addClass("open");
            $('.button-toggle').click(function () {
                $('.open').not(this).removeClass('open').parent().next().slideUp(300);
                $(this).toggleClass('open').parent().next().slideToggle(300);
            });
        }
    };

    return _me;
})(jQuery);

Codition.initialize();