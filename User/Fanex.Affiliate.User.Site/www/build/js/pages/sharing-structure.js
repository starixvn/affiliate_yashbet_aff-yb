var Sharing = (function ($) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },
        initEvent: function () {
            $('.info-question .button-toggle').addClass("open");
            $('.button-toggle').click(function () {
                $(this).toggleClass('open').parent().next().slideToggle(300);
            });

        }
    };

    return _me;
})(jQuery);

Sharing.initialize();