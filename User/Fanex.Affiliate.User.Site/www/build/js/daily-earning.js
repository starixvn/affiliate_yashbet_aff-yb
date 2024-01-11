"use strict";

var DailyEarning = (function ($) {
    var _me = {
        initialize: function initialize() {
            DailyEarning.initEvent();
        },

        initEvent: function initEvent() {
            $("#ProductId").on("change", function () {
                _me.renderContent();
            });

            $("#base-currency").change(function () {
                _me.renderContent();
            });
        },

        renderContent: function renderContent() {
            var params = {
                monthDate: $("#MonthDate").val(),
                productId: $("#ProductId").val(),
                baseCurrency: $("#base-currency").val(),
                IsOnlyContent: true
            };

            _me.renderDailyEarning(params);
        },

        renderDailyEarning: function renderDailyEarning(params) {
            $.ajax({
                type: "POST",
                data: JSON.stringify(params),
                dataType: "html",
                contentType: "application/json; charset=UTF-8",
                cache: false,
                url: Site.resolveRelativeUrl("UserPanel/DailyEarning"),
                success: function success(response) {
                    $("#dailyEarningContent").html(response);
                },
                beforeSend: function beforeSend() {
                    Site.showLoader();
                },
                complete: function complete() {
                    Site.hideLoader();
                }
            });
        }
    };

    return _me;
})(jQuery);

DailyEarning.initialize();

