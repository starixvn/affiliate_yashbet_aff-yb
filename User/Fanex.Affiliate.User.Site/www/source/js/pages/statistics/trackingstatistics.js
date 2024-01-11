var TrackingStatistics = (function ($) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },

        initEvent: function () {
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

        getMainView: function (params) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/TrackingStatistics/Index"),
                contentType: "application/json; charset=UTF-8",
                success: function (response) {
                    $("#statisticsTable").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        getDetailView: function (params) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/TrackingStatistics/TrackingStatisticsDetail"),
                contentType: "application/json; charset=UTF-8",

                success: function (response) {
                    $("#statisticsDetailTable").html(response);
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
})(jQuery)

TrackingStatistics.initialize();