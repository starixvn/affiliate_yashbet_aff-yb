var AccountOverview = (function ($) {
    var DEFAULT_ACTION_OF_ACCOUNT_OVERVIEW = 'Summary';
    var allowedActionListOfAccountOverview = ["Summary", "AffiliateLink", "MediaGallery", "SubAffLink", "SubAffMediaGallery"];

    var _me = {
        initialize: function () {
            var hash = window.location.hash;
            var actionUrl = hash.split('#')[2];

            _me.initEvent();
            _me.renderView(actionUrl);
        },

        initEvent: function () {
            $('#account-overview-menu').on('click', '.account-overview-menu-item', function () {
                var name = $(this).data('name');
                history.pushState({
                    actionName: name
                }, null, '#AccountOverview#' + name);

                _me.renderView(name);
            });
        },

        activeMenu: function (actionName) {
            var activeClassName = "active";
            $(".account-overview-menu-item").removeClass(activeClassName);
            $("#menu" + actionName).addClass(activeClassName);
        },

        renderView: function (actionName) {
            if (allowedActionListOfAccountOverview.indexOf(actionName) === -1) {
                actionName = DEFAULT_ACTION_OF_ACCOUNT_OVERVIEW;
            }

            _me.activeMenu(actionName);
            //window.removeEventListener("scroll", handleScroll);
            //window.removeEventListener("resize", handleResize);

            $.ajax({
                type: "GET",
                dataType: "html",
                cache: false,
                url: Site.resolveRelativeUrl("UserPanel/" + actionName),
                success: function (response) {
                    $("#account-overview-content").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },
    };

    return _me;
})(jQuery);

AccountOverview.initialize();