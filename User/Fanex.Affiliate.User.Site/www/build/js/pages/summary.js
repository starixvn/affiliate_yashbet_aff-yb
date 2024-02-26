(function ($) {
    var defaultOption = {
        monthNames: [
            ['Jan', 'Feb', 'Mar'],
            ['Apr', 'May', 'Jun'],
            ['Jul', 'Aug', 'Sep'],
            ['Oct', 'Nov', 'Dec']
        ],
        range: [new Date(2020, 1, 1), new Date()],
        disableFutureMonth: true,
        closeOnSelected: true,
        triggerSelector: undefined,
        onSelect: function (month, year) { },
        format: function (month, year) {
            var monthArray = this.monthNames.reduce(function (prev, current) {
                return prev.concat(current);
            }, []);

            return monthArray[month - 1] + "-" + year;
        }
    };

    function buildHeader(minYear, maxYear) {
        var header = ['<div class="mp-header"><select class="mp-select-year">'];
        for (var i = maxYear; i >= minYear; i--) {
            header.push('<option value="' + i + '">' + i + '</option>');
        }

        header.push('</select></div>');

        return header.join("");
    }

    function buildMonth(monthNames, selectedYear, range, isDisableFutureMonth) {
        var month = ['<table class="mp-month-table"><tbody>'];
        for (var i = 0; i < monthNames.length; i++) {
            var monthRow = monthNames[i];

            var minRange = range[0];
            var maxRange = range[1];

            month.push('<tr class="mp-row">');
            for (var j = 0; j < monthRow.length; j++) {
                var currentMonth = i * monthRow.length + j;
                var isDisable = (selectedYear <= minRange.getFullYear() && currentMonth < minRange.getMonth()) ||
                    (selectedYear >= maxRange.getFullYear() && currentMonth > maxRange.getMonth());

                var disableClass = isDisableFutureMonth && isDisable ? "mp-disabled" : "";
                month.push('<td><button class="mp-btn-month ' + disableClass + '" data-month="' + currentMonth + '">' + monthRow[j] + '</button></td>');
            }
            month.push('</tr>');
        }
        month.push('</tbody></table>');

        return month.join("");
    }

    var _methods = {
        init: function (option) {
            return this.each(function () {
                var mainOption = $.extend(defaultOption, option || {});

                var $self = $(this);
                var $wrapper = $self.parent();
                $wrapper.find(".mp-container").off();
                $self.find(".mp-container").remove();

                var isDisableFutureMonth = mainOption.disableFutureMonth;
                var range = mainOption.range;
                var minYear = range[0].getFullYear();
                var maxYear = range[1].getFullYear();

                var monthNames = mainOption.monthNames;

                var pickerHtml = '<div class="mp-container" style="display: none;">' + buildHeader(minYear, maxYear) +
                    buildMonth(monthNames, maxYear, range, isDisableFutureMonth) + '</div>';
                $wrapper.append(pickerHtml);

                var $container = $wrapper.find(".mp-container");
                $container.on("click", ".mp-btn-month", function () {
                    $container.find(".mp-btn-month").removeClass("mp-active");
                    $(this).addClass("mp-active");

                    var month = $(this).data("month") + 1;
                    var year = parseInt($container.find(".mp-select-year").val());

                    var displayText = mainOption.format(month, year);
                    $self.val(displayText);
                    mainOption.onSelect(month, year);

                    if (mainOption.closeOnSelected) {
                        $container.hide();
                    }
                });

                $container.on("change", ".mp-select-year", function () {
                    var selectedYear = parseInt($(this).val());
                    var newMonthHtml = buildMonth(monthNames, selectedYear, mainOption.range, mainOption.disableFutureMonth);
                    $container.find(".mp-month-table").remove();
                    $container.append(newMonthHtml);
                });

                $self.on("click", function () {
                    $container.show();
                });

                $("html").on("click", function (e) {
                    var $target = $(e.target);

                    if (!$.contains($wrapper[0], $target[0])) {
                        $container.hide();
                    }
                });

                var triggerSelector = mainOption.triggerSelector;
                if (triggerSelector) {
                    $(triggerSelector).on("click", function () {
                        $container.show();
                    })
                }
            });
        }
    };

    $.fn.monthPicker = function (methodOrOptions) {
        if (_methods[methodOrOptions]) {
            return _methods[methodOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof methodOrOptions === 'object' || !methodOrOptions) {
            // Default to "init"
            return _methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + methodOrOptions + ' does not exist on jQuery.monthPicker');
        }
    };
})(jQuery);
var Summary = (function ($) {
    function formatDateUnit(value) {
        return value < 10 ? '0' + value : value;
    }

    function initMonthPicker() {
        let today = $("#ToDay").val();
        const sixMonthAgo = new Date(today);
        sixMonthAgo.setMonth(sixMonthAgo.getMonth() - 6);

        $("#MonthDate").monthPicker({
            range: [new Date(sixMonthAgo.getFullYear(), sixMonthAgo.getMonth() + 1, 1), new Date(today)],
            triggerSelector: "#monthPickerTrigger",
            onSelect: function (month, year) {
                var date = formatDateUnit(month) + "/01/" + year;
                $("#MonthDate").data("original-date", date);
            }
        });
    }

    var _me = {
        initialize: function () {
            var params = {
                monthDate: $("#MonthDate").data("original-date")
            };

            initMonthPicker();
            _me.initEvent();
            _me.renderDashBoardView(params);
            _me.renderDetailView(params, "Overview");
        },

        initEvent: function () {
            $("#statisticsSubmit").on("click", function () {
                var params = {
                    monthDate: $("#MonthDate").data("original-date")
                }

                var actionMenu = $('.details-section-menu-item.active').data('name');

                _me.renderDashBoardView(params);
                _me.renderDetailView(params, actionMenu);
            });

            $('#details-section-menu').on('click', '.details-section-menu-item', function () {
                var name = $(this).data('name');
                _me.renderDetailView({ monthDate: $("#MonthDate").data("original-date") }, name, true);
            });
        },

        renderDashBoardView: function (params) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/Summary/DashBoard"),
                contentType: "application/json; charset=UTF-8",
                success: function (response) {
                    $("#dashboard-section").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        activeMenu: function (actionName) {
            var activeClassName = "active";
            $(".details-section-menu-item").removeClass(activeClassName);
            $("#menu" + actionName).addClass(activeClassName);
        },

        renderDetailView: function (params, actionName, isClickSubMenu) {
            _me.activeMenu(actionName);

            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                url: Site.resolveRelativeUrl("UserPanel/Summary/" + actionName),
                contentType: "application/json; charset=UTF-8",
                success: function (response) {
                    $("#details-section-content").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    isClickSubMenu && Site.hideLoader();
                }
            });
        },
    };

    return _me;
})(jQuery);

Summary.initialize();