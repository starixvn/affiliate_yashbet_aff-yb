"use strict";

var SubAffEarning = (function ($) {

    var _me = {
        initialize: function initialize() {
            SubAffEarning.initEvent();
        },

        initEvent: function initEvent() {
            $("#base-currency").change(function () {
                _me.renderContent();
            });
        },

        renderContent: function renderContent() {
            var params = {
                monthDate: $("#MonthDate").val(),
                baseCurrency: $("#base-currency").val(),
                IsOnlyContent: true
            };

            _me.renderSubAffEarning(params);
        },

        renderSubAffEarning: function renderSubAffEarning(params) {
            $.ajax({
                type: "POST",
                data: JSON.stringify(params),
                dataType: "html",
                contentType: "application/json; charset=UTF-8",
                cache: false,
                url: Site.resolveRelativeUrl("UserPanel/SubAffEarning"),
                success: function success(response) {
                    $("#subAffEarning").html(response);
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

SubAffEarning.initialize();

