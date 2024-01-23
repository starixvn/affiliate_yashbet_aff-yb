var SUPPORT_MIN_WITDH = 716;
var SUB_MENU_HEIGHT = 41;
var DETAILS_SECTION_MENU_HEIGHT = 33;

function toggleCollapseButton(btn) {
    $(btn).toggleClass("active-btn");
}

var UserPanel = (function ($) {
    var DEFAULT_ACTION = 'AccountOverview';
    var allowedActionList = ["AccountOverview", "StatementHistory", "MyAccount", "PaymentRequest",
        "ReferrersStatistics", "LandingPageStatistics", "SubAffEarning",
        "DailyEarning", "MessageHistory", "RevenueReport", "MarketingLinkStatistics",
        "PlayerTransferBalance", "PlayerTransferBalanceHistory"];

    var _ajaxRequest = null;

    var _me = {
        datepicker: [],
        initialize: function () {
            _me.renderView();
            UserPanel.initEvent();
        },

        initEvent: function () {
            $(window).on('hashchange', function (event) {
                event.stopPropagation();
                _me.renderView();
            });

            $('#mainMenu .parent .submenu-container').hover(function () {
                $(this).addClass("hover");
            }, function () {
                $(this).removeClass("hover");
            });
        },

        activeMenu: function (actionName) {
            var activeClassName = "active";
            $("#mainMenu .menu-item").removeClass(activeClassName);
            $("#menu" + actionName).addClass(activeClassName);

            //fix issue hover after click on ipad
            $('#mainMenu .parent .submenu-container').removeClass('hover');

            if ($('.content-menu-mobile').length) {
                $(".content-menu-mobile .menu-item").removeClass(activeClassName);
                $("#menu" + actionName + "Mobile").addClass(activeClassName);
                $('.content-menu-mobile .show-name-menu').html($("#menu" + actionName + "Mobile").html());
            }
        },

        renderView: function () {
            _me.clearEventListener();
            $("#mainPanel").html("");
            var actionUrl = _me.getActionUrl(false);
            var actionName = actionUrl.split('?')[0];
            _me.activeMenu(actionName);

            var url = Site.resolveRelativeUrl("UserPanel/" + actionUrl);

            if (_ajaxRequest !== null) {
                _ajaxRequest.abort();
            }

            _ajaxRequest = $.ajax({
                type: "GET",
                dataType: "html",
                cache: false,
                url: url,
                success: function (response) {
                    $("#mainPanel").html(response);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        clearEventListener: function () {
            $("#mainPanel *").off();
            if (_me.datepicker) {
                for (var i = 0; i < _me.datepicker.length; i++) {
                    if (_me.datepicker[i]) {
                        _me.datepicker[i].destroy();
                    }
                }
                _me.datepicker = [];
            }
        },

        getActionUrl: function (onlyAction) {
            var hash = window.location.hash;
            var actionUrl = hash.split('#')[1];
            if (!actionUrl) {
                return DEFAULT_ACTION;
            }

            var urlPartList = actionUrl.split('?');
            var actionName = urlPartList[0];

            if (allowedActionList.indexOf(actionName) === -1) {
                actionName = DEFAULT_ACTION;
            }

            var queryString = (!onlyAction && urlPartList[1]) ? '?' + urlPartList[1] : '';

            return actionName + queryString;
        },
    };

    return _me;
})(jQuery);

UserPanel.initialize();