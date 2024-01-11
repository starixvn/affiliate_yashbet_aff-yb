var DailyEarning = (function ($) {
    var _me = {
        initialize: function () {
            DailyEarning.initEvent();
        },

        initEvent: function () {
            $("#ProductId").on("change", function () {
                _me.renderContent();
            });

            $("#base-currency").change(function () {
                _me.renderContent();
            });
        },

        renderContent: function () {
            var params = {
                monthDate: $("#MonthDate").val(),
                productId: $("#ProductId").val(),
                baseCurrency: $("#base-currency").val(),
                IsOnlyContent: true
            };

            _me.renderDailyEarning(params);
        },

        renderDailyEarning: function (params) {
            $.ajax({
                type: "POST",
                data: JSON.stringify(params),
                dataType: "html",
                contentType: "application/json; charset=UTF-8",
                cache: false,
                url: Site.resolveRelativeUrl("UserPanel/DailyEarning"),
                success: function (response) {
                    $("#dailyEarningContent").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        }
    };

    return _me;
})(jQuery);

DailyEarning.initialize();