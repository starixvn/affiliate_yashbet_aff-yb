'use strict';

var TrackingStatistics = (function ($) {
    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },

        initEvent: function initEvent() {
            $('#btnStatisticsSubmit').on('click', function () {
                $("#statisticsDetailTable").html('');

                var params = {
                    trackingCode: $('#TrackingCode').val()
                };
                _me.getMainView(params);
            });

            $('#statisticsTable').on('click', '.statisticsItem', function () {
                var params = {
                    trackingCode: $('#TrackingCode').val(),
                    dateMonth: $(this).data('date')
                };
                _me.getDetailView(params);
            });
        },

        getMainView: function getMainView(params) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/TrackingStatistics/Index"),
                contentType: "application/json; charset=UTF-8",
                success: function success(response) {
                    $("#statisticsTable").html(response);
                },
                beforeSend: function beforeSend() {
                    Site.showLoader();
                },
                complete: function complete() {
                    Site.hideLoader();
                }
            });
        },

        getDetailView: function getDetailView(params) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/TrackingStatistics/TrackingStatisticsDetail"),
                contentType: "application/json; charset=UTF-8",

                success: function success(response) {
                    $("#statisticsDetailTable").html(response);
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

TrackingStatistics.initialize();

