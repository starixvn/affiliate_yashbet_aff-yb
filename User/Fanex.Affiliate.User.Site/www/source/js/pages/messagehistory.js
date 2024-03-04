var MessageHistory = (function ($) {
    var _pageSize = 10;
    var _pageIndex = 0;

    function initPager() {
        $("#messagePager").pager({
            totalItems: parseInt($("#TotalItem").val()),
            pageSize: _pageSize,
            pageIndex: _pageIndex,
            languageCode: Site.getCurrentLanguage(),
            onChange: function (pageIndex, pageSize) {
                _pageIndex = pageIndex;
                _pageSize = pageSize;

                var params = _me.getParams();
                _me.submit(params);
            }
        });
    }

    function formatDateUnit(value) {
        return value < 10 ? '0' + value : value;
    }

    function initDatePicker(id, triggerId) {
        new Pikaday({
            field: document.getElementById(id),
            trigger: document.getElementById(triggerId),
            yearRange: [2020, new Date().getFullYear()],
            toString: function (date, format) {
                var day = date.getDate();
                var month = date.getMonth() + 1;
                var year = date.getFullYear();
                return formatDateUnit(month) + "/" + formatDateUnit(day) + "/" + year;
            },
            onOpen: function () {
                if ($("#chkSearchAll").prop("checked")) {
                    this.hide();
                }
            }
        });
    }

    var _me = {
        initialize: function () {
            initDatePicker('FromDate', 'fromPicker');
            initDatePicker('ToDate', 'toPicker');
            initPager();
            _me.initEvent();
        },

        initEvent: function () {
            $("#btnSubmit").on("click", function () {
                var params = _me.getParams();
                _me.submit(params, _me.updatePager);
            });

            $("#chkSearchAll").on("click", function () {
                var isChecked = $(this).prop("checked");
                _me.toggleDate(isChecked);
            });

            $('#mainMenu .parent .submenu-container').hover(function () {
                $(this).addClass("hover");
            }, function () {
                $(this).removeClass("hover");
            });
        },

        getParams: function () {
            var hasDateRange = !$("#chkSearchAll").prop("checked");
            var seachBy = $("#Search").val();

            return {
                FromDate: hasDateRange && $("#FromDate").val(),
                ToDate: hasDateRange && $("#ToDate").val(),
                Important: !seachBy ? "" : (seachBy === "1" ? true : false),
                PageSize: _pageSize,
                PageIndex: _pageIndex + 1
            }
        },

        submit: function (params, onSuccess) {
            $.ajax({
                type: "GET",
                dataType: "html",
                cache: false,
                data: params,
                url: Site.resolveRelativeUrl("News/GetMessageHistory"),
                success: function (response) {
                    $("#messageDataList").html(response);
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

        updatePager: function () {
            $("#messagePager").pager("update", {
                pageIndex: _pageIndex,
                totalItems: parseInt($("#TotalItem").val())
            });
        },

        toggleDate: function (isDisabled) {
            $(".date-wrapper > *").toggleClass("disabled", isDisabled);
        }
    }

    return _me;
})(jQuery)

MessageHistory.initialize();