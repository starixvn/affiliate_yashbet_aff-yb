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