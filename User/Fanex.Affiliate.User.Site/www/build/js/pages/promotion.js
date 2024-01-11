'use strict';

var Promo = (function ($) {
    var endDate = new Date(window.dic.EndDatePromotion);
    var one_day = 60 * 60 * 24 * 1000;
    var one_hour = 60 * 60 * 1000;
    var one_min = 60 * 1000;
    var one_sec = 1000;
    var countDownInterval = undefined;

    function startCountDown() {
        countDownInterval = setInterval(countDown, 1000);
    }

    function countDown() {
        var now = new Date();
        if (now > endDate) {
            clearInterval(countDownInterval);
            $('.count-down-content').empty().html('<div class="count-down-text">' + window.dic.Finished + '</div>');
        } else {
            var diffDate = endDate - now;
            var day = Math.floor(diffDate / one_day);
            var hour = Math.floor((diffDate - day * one_day) / one_hour);
            var min = Math.floor((diffDate - day * one_day - hour * one_hour) / one_min);
            var sec = Math.floor((diffDate - day * one_day - hour * one_hour - min * one_min) / one_sec);
            $('.day .number').empty().html(day);
            $('.hour .number').empty().html(hour);
            $('.min .number').empty().html(min);
            $('.sec .number').empty().html(sec);
        }
    }

    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },
        initEvent: function initEvent() {
            startCountDown();
        }
    };

    return _me;
})(jQuery);

Promo.initialize();

