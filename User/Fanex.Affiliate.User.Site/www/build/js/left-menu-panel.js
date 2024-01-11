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

            $('a.left-menu-item').bind('click', function (e) {
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
            var sections = $("section.info-group");

            if (sections.length) {
                var theFirstMenuId = sections[0].getAttribute("id");
                var activeMenuId = theFirstMenuId;
                var currentScreenTop = lastScreenTop;

                var _iteratorNormalCompletion = true;
                var _didIteratorError = false;
                var _iteratorError = undefined;

                try {
                    for (var _iterator = sections[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                        var section = _step.value;

                        var sectionTop = section.offsetTop;

                        if (window.pageYOffset >= sectionTop - window.innerHeight * 1 / 5) {
                            activeMenuId = section.getAttribute("id");
                            currentScreenTop = sectionTop;
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
            var leftMenuList = $("a.left-menu-item[href*=\"#\"]");
            var activeLeftMenu = $("a.left-menu-item[href=\"" + hrefTarget + "\"]");
            var leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                $("a.left-menu-item.active").removeClass(activeClassName);
                activeLeftMenu.addClass(activeClassName);

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
            var leftMenuList = $("a.left-menu-item[href*=\"#\"]");
            var activeLeftMenu = $("a.left-menu-item.active");
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

