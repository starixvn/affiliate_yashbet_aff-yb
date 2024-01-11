"use strict";

var ChannelLinkStatistic = (function ($) {
    function toggleChildrenElem(children) {
        $(children).toggleClass("display-none");
    }

    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },

        initEvent: function initEvent() {
            $("#channel-table").on("click", ".btn.btn-transparent", function () {
                var parentRow = $(this).parents("tr");
                var id = parentRow.data("id");
                var children = parentRow.siblings("tr[data-parent='" + id + "']");
                toggleCollapseButton(this);
                toggleChildrenElem(children);
            });
        }
    };

    return _me;
})(jQuery);

ChannelLinkStatistic.initialize();

