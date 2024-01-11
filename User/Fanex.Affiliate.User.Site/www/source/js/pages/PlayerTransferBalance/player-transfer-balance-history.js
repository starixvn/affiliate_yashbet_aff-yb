var PlayerTransferBalanceHistory = (function ($) {
    var _pageSize = 20;
    var _pageIndex = 0;

    function initPager() {
        $("#pager").pager({
            totalItems: parseInt($("#TotalItem").val()),
            pageSize: _pageSize,
            pageIndex: _pageIndex,
            languageCode: Site.getCurrentLanguage(),
            onChange: function (pageIndex, pageSize) {
                _pageIndex = pageIndex;
                _pageSize = pageSize;
                let param = _me.getParam();
                _me.getDetailView(param);
            }
        });
    }

    function formatDateUnit(value) {
        return value < 10 ? '0' + value : value;
    }

    function initDatePicker(id, triggerId) {
        let firstDate = new Date('11/01/2021');
        let lastDate = new Date($('#ToDate').val());
        UserPanel.datepicker.push(new Pikaday({
            field: document.getElementById(id),
            trigger: document.getElementById(triggerId),
            yearRange: [2020, new Date().getFullYear()],
            minDate: firstDate,
            maxDate: lastDate,
            toString: function (date, format) {
                var day = date.getDate();
                var month = date.getMonth() + 1;
                var year = date.getFullYear();
                return formatDateUnit(month) + "/" + formatDateUnit(day) + "/" + year;
            }
        }));
    }

    var _me = {
        initialize: function () {
            initDatePicker('FromDate', 'fromPicker');
            initDatePicker('ToDate', 'toPicker');
            _me.initEvent();
        },

        initEvent: function () {
            $('#btnSubmit').on('click', function () {
                _pageIndex = 0;
                var params = _me.getParam();

                $('#pager').show();
                _me.getDetailView(params, initPager);

            });
        },


        getDetailView: function (params, onSuccess) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/PlayerTransferBalanceHistory/GetHistory"),
                contentType: "application/json; charset=UTF-8",

                success: function (response) {
                    $("#table").html(response);
                    onSuccess && onSuccess();
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        getParam: function () {
            return {
                playerName: $('#PlayerName').val(),
                fromDate: $('#FromDate').val(),
                toDate: $('#ToDate').val(),
                pageSize: _pageSize,
                pageIndex: _pageIndex + 1
            };
        }
    };

    return _me;
})(jQuery)

PlayerTransferBalanceHistory.initialize();