'use strict';

var Home = (function ($) {
    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },
        initEvent: function initEvent() {
            $('#sportBanner').on('click', function (event) {
                event.preventDefault();
                var redirectLink = $(this).attr('href');
                PageTracking.addTrackingForSignUpPage();

                setTimeout(function () {
                    Site.redirect(redirectLink);
                }, 10);
            });

            $('#casinoBanner').on('click', function (event) {
                event.preventDefault();
                var $this = $(this);
                var isLoggedIn = !$this.hasClass('mediaLoginLink');

                if (isLoggedIn) {
                    PageTracking.addTrackingForMediaPage();

                    setTimeout(function () {
                        Site.redirect($this.attr('href'));
                    }, 10);
                }
            });
        }
    };

    return _me;
})(jQuery);

Home.initialize();

