var ChannelLinkStatistic = (function ($) {
    function toggleChildrenElem(children) {
        $(children).toggleClass("display-none");
    }

    var _me = {
        initialize: function () {
            _me.initEvent();
        },

        initEvent: function () {
            $("#channel-table").on("click", ".btn.btn-transparent", function () {
                let parentRow = $(this).parents("tr");
                let id = parentRow.data("id");
                let children = parentRow.siblings(`tr[data-parent='${id}']`);
                toggleCollapseButton(this);
                toggleChildrenElem(children);
            });
        },
    };

    return _me;
})(jQuery);

ChannelLinkStatistic.initialize();