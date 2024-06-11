"use strict";

var LeftMenuPanel = (function ($) {
    var activeClassName = "active";
    var maxWidth = 1024;

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

            $('#leftmenu').on('shown.bs.modal', function (e) {
                _me.handleActiveLeftMenuByScroll();
            });

            $('a.parent-menu').click(function (e) {
                e.preventDefault();
                $(this).blur();

                $(this).next("ul.sub-menu-group").stop().animate({
                    height: "toggle", opacity: "toggle"
                }, 600);
            });

            $('a.sub-menu').click(function (e) {
                window.removeEventListener("scroll", _me.handleActiveLeftMenuByScroll);
                e.preventDefault();

                var hrefTarget = $(this).attr("href");

                _me.handleOpenMenuContent(hrefTarget);
                _me.handelAnimate(hrefTarget);

                setTimeout(function () {
                    $('#leftmenu').modal('hide');
                }, 400);
            });
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

                        var offsetMenuTop = menu.offsetTop + (menu.offsetParent && menu.offsetParent.offsetTop);

                        if (window.scrollY >= offsetMenuTop - 20) {
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
            var preClasss = screen.width > maxWidth ? ".desktop" : ".mobile";

            var leftMenuList = $(preClasss + "-left-panel-nav a.left-menu-item[href*=\"#\"]");
            var activeLeftMenu = $(preClasss + "-left-panel-nav a.left-menu-item[href=\"" + hrefTarget + "\"]");
            var leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                $(preClasss + "-left-panel-nav a.left-menu-item.active").removeClass(activeClassName);

                activeLeftMenu.addClass(activeClassName);
                activeLeftMenu.parents("ul.sub-menu-group").prev("a.parent-menu").addClass(activeClassName);

                var activeIndex = leftMenuList.index(activeLeftMenu);
                var maxScrollTop = $(preClasss + "-left-panel-nav").outerHeight() + $(preClasss + "-left-panel-menu .heading-left-panel").outerHeight() - $(preClasss + "-left-panel-menu").outerHeight();
                var currentScrollTop = $(preClasss + "-left-panel-menu").scrollTop();
                var newScrollTop = activeIndex / leftMenuNumbers * maxScrollTop;

                if (lastScreenTop <= currentScreenTop && currentScrollTop < newScrollTop || lastScreenTop >= currentScreenTop && currentScrollTop > newScrollTop) {
                    $(preClasss + "-left-panel-menu").stop().animate({
                        scrollTop: activeIndex > leftMenuNumbers / 2 ? newScrollTop + activeLeftMenu.outerHeight() : newScrollTop
                    }, 400);
                }

                lastScreenTop = currentScreenTop;
            }
        },

        handleOpenMenuContent: function handleOpenMenuContent(hrefTarget) {
            var btnToggleMenuContent = $(hrefTarget).parents(".body-result").prev(".left-menu-title").children(".button-toggle");

            if (!btnToggleMenuContent.hasClass('open')) {
                btnToggleMenuContent.trigger('click');
            }
        }
    };

    return _me;
})(jQuery);

LeftMenuPanel.initialize();

