"use strict";

var LeftMenuPanel = (function ($) {
    var activeClassName = "active";
    var isOpenLeftMenuMobile = false;
    var lastScreenTop = 0;

    var _me = {
        initialize: function initialize() {
            _me.initEvent();
            window.addEventListener("load", _me.handleActiveLeftMenuByScroll);
        },

        initEvent: function initEvent() {
            $('.mobile-left-panel').click(function (e) {
                $("#leftmenu").modal();
            });

            $('a.parent-menu').click(function (e) {
                e.preventDefault();
                $(this).blur();

                $(this).next("ul.sub-menu").stop().animate({
                    height: "toggle", opacity: "toggle"
                }, 600);
            });

            $('ul.sub-menu a.left-menu-item').click(function (e) {
                window.removeEventListener("scroll", _me.handleActiveLeftMenuByScroll);
                e.preventDefault();

                var hrefTarget = $(this).attr("href");

                _me.handelAnimate(hrefTarget);
                setTimeout(function () {
                    $('#leftmenu').modal('hide');
                }, 400);
            });
        },

        handleCloseLeftMenu: function handleCloseLeftMenu() {
            isOpenLeftMenuMobile = false;
            $('body').unbind("click");

            $('.left-panel.active').removeClass('active');
            $('body.overflow-left-panel').removeClass('overflow-left-panel');
        },

        handleActiveLeftMenuByScroll: function handleActiveLeftMenuByScroll() {
            var leftMenus = $(".left-menu-title");

            if (leftMenus.length) {
                var theFirstMenuId = leftMenus[0].getAttribute("id");
                var activeMenuId = theFirstMenuId;
                var currentScreenTop = lastScreenTop;

                var _iteratorNormalCompletion = true;
                var _didIteratorError = false;
                var _iteratorError = undefined;

                try {
                    for (var _iterator = leftMenus[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                        var menu = _step.value;

                        var offsetMenuTop = menu.offsetParent.offsetTop + menu.offsetTop;

                        if (window.pageYOffset >= offsetMenuTop - 20) {
                            activeMenuId = menu.getAttribute("id");
                            currentScreenTop = offsetMenuTop;
                        }
                    }
                } catch (err) {
                    _didIteratorError = true;
                    _iteratorError = err;
                } finally {
                    try {
                        if (!_iteratorNormalCompletion && _iterator["return"]) {
                            _iterator["return"]();
                        }
                    } finally {
                        if (_didIteratorError) {
                            throw _iteratorError;
                        }
                    }
                }

                _me.setActiveLeftMenu("#" + activeMenuId, currentScreenTop);
                window.addEventListener("scroll", _me.handleActiveLeftMenuByScroll);
            }
        },

        handelAnimate: function handelAnimate(hrefTarget) {
            _me.setActiveLeftMenu(hrefTarget, lastScreenTop);

            $('html, body').stop().animate({
                scrollTop: $(hrefTarget).offset().top
            }, 600, function () {
                window.addEventListener("scroll", _me.handleActiveLeftMenuByScroll);
            });
        },

        setActiveLeftMenu: function setActiveLeftMenu(hrefTarget, currentScreenTop) {
            var leftMenuList = $(".sub-menu a.left-menu-item[href*=\"#\"]");
            var activeLeftMenu = $(".sub-menu a.left-menu-item[href=\"" + hrefTarget + "\"]");
            var leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                $(".sub-menu a.left-menu-item.active").removeClass(activeClassName);
                activeLeftMenu.addClass(activeClassName);
                $("a.parent-menu.active").removeClass("active");
                activeLeftMenu.parents("ul.sub-menu").prev("a.parent-menu").addClass("active");

                var activeIndex = leftMenuList.index(activeLeftMenu);
                var maxScrollTop = $('#left-panel-nav').outerHeight() + 24 - $('#left-panel-menu').outerHeight(); //24px of margin
                var currentScrollTop = $('#left-panel-menu').scrollTop();
                var newScrollTop = activeIndex / leftMenuNumbers * maxScrollTop;

                if (lastScreenTop < currentScreenTop && currentScrollTop < newScrollTop || lastScreenTop > currentScreenTop && currentScrollTop > newScrollTop) {
                    $('#left-panel-menu').stop().animate({
                        scrollTop: newScrollTop
                    }, 400);
                }

                lastScreenTop = currentScreenTop;
            }
        },

        handleScrollLeftMenuInMobile: function handleScrollLeftMenuInMobile() {
            var leftMenuList = $(".sub-menu a.left-menu-item[href*=\"#\"]");
            var activeLeftMenu = $(".sub-menu a.left-menu-item.active");
            var leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                var activeIndex = leftMenuList.index(activeLeftMenu);
                var maxScrollTop = $('#left-panel-nav').outerHeight() + 24 - $('#left-panel-menu').outerHeight();
                var newScrollTop = activeIndex / leftMenuNumbers * maxScrollTop;

                $('#left-panel-menu').stop().animate({
                    scrollTop: newScrollTop
                }, 400);
            }
        }
    };

    return _me;
})(jQuery);

LeftMenuPanel.initialize();

