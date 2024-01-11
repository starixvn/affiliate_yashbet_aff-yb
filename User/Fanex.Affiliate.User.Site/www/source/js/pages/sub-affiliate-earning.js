var SubAffEarning = (function ($) {

    var _me = {
        initialize: function () {
            SubAffEarning.initEvent();
        },

        initEvent: function () {
            $("#base-currency").change(function () {
                _me.renderContent();
            });
        },

        renderContent: function () {
            var params = {
                monthDate: $("#MonthDate").val(),
                baseCurrency: $("#base-currency").val(),
                IsOnlyContent: true
            };

            _me.renderSubAffEarning(params);
        },

        renderSubAffEarning: function (params) {
            $.ajax({
                type: "POST",
                data: JSON.stringify(params),
                dataType: "html",
                contentType: "application/json; charset=UTF-8",
                cache: false,
                url: Site.resolveRelativeUrl("UserPanel/SubAffEarning"),
                success: function (response) {
                    $("#subAffEarning").html(response);
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

SubAffEarning.initialize();