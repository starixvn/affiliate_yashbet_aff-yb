var Promo = (function ($) {
    const endDate = new Date(window.dic.EndDatePromotion);
    const one_day = 60 * 60 * 24 * 1000;
    const one_hour = 60 * 60 * 1000;
    const one_min = 60 * 1000;
    const one_sec = 1000;
    let countDownInterval;

    function startCountDown() {
        countDownInterval = setInterval(countDown, 1000);
    }

    function countDown() {
        let now = new Date();
        if (now > endDate) {
            clearInterval(countDownInterval);
            $('.count-down-content').empty().html(`<div class="count-down-text">${window.dic.Finished}</div>`)
        }
        else {
            let diffDate = endDate - now;
            let day = Math.floor(diffDate / one_day);
            let hour = Math.floor((diffDate - (day * one_day)) / one_hour);
            let min = Math.floor((diffDate - (day * one_day) - (hour * one_hour)) / one_min);
            let sec = Math.floor((diffDate - (day * one_day) - (hour * one_hour) - (min * one_min)) / one_sec);
            $('.day .number').empty().html(day);
            $('.hour .number').empty().html(hour);
            $('.min .number').empty().html(min);
            $('.sec .number').empty().html(sec);
        }
    }

    var _me = {
        initialize: function () {
            _me.initEvent();
        },
        initEvent: function () {
            startCountDown();
        },
    };

    return _me;
})(jQuery);

Promo.initialize();