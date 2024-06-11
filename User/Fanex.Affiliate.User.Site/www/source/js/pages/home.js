var Home = (function ($) {
    function topBanner() {
        var swiper = new Swiper(".top-banner", {
            loop: true,
            speed: 900,
            autoplay: {
                delay: 3500,
                disableOnInteraction: false,
            },
            pagination: {
                el: ".swiper-pagination",
                clickable: true
            }
        });
    }
    var _me = {
        initialize: function () {
           /* topBanner();*/
            _me.initEvent();
        },
        initEvent: function () {
            
        }
    };

    return _me;
})(jQuery);

Home.initialize();