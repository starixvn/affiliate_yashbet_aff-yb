var LeftMenuPanel = (function ($) {
    const activeClassName = "active";
    const maxWidth = 1024;

    let lastScreenTop = 0;

    var _me = {
        initialize: function () {
            _me.initEvent();
            window.addEventListener("load", _me.handleActiveLeftMenuByScroll);
        },

        initEvent: function () {
            $('.mobile-left-panel').click(function (e) {
                $("#leftmenu").modal();
            })

            $('#leftmenu').on('shown.bs.modal', function (e) {
                _me.handleActiveLeftMenuByScroll();
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

                _me.handleOpenMenuContent(hrefTarget);
                _me.handelAnimate(hrefTarget);

                setTimeout(function () {
                    $('#leftmenu').modal('hide');
                }, 400);
                
            });
        },

        handleActiveLeftMenuByScroll: function () {
            const leftMenus = $(".left-menu-title");

            if (leftMenus.length) {
                let theFirstMenuId = leftMenus[0].getAttribute("id");
                let activeMenuId = theFirstMenuId;
                let currentScreenTop = lastScreenTop;

                for (let menu of leftMenus) {
                    const offsetMenuTop = menu.offsetParent.offsetTop + menu.offsetTop;

                    if (window.pageYOffset >= offsetMenuTop - 20) {
                        activeMenuId = menu.getAttribute("id");
                        currentScreenTop = offsetMenuTop;
                    }
                }

                _me.setActiveLeftMenu(`#${activeMenuId}`, currentScreenTop);
                window.addEventListener("scroll", _me.handleActiveLeftMenuByScroll);
            }
        },

        handelAnimate: function (hrefTarget) {
            _me.setActiveLeftMenu(hrefTarget, lastScreenTop);

            $('html, body').stop().animate({
                scrollTop: $(hrefTarget).offset().top
            }, 600, function () {
                window.addEventListener("scroll", _me.handleActiveLeftMenuByScroll);
            });
        },

        setActiveLeftMenu: function (hrefTarget, currentScreenTop) {
            let preClasss = screen.width > maxWidth ? ".desktop" : ".mobile";

            const leftMenuList = $(`${preClasss}-left-panel-nav a.left-menu-item[href*="#"]`);
            const activeLeftMenu = $(`${preClasss}-left-panel-nav a.left-menu-item[href="${hrefTarget}"]`);
            const leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                $(`${preClasss}-left-panel-nav a.left-menu-item.active`).removeClass("active");

                activeLeftMenu.addClass(activeClassName);
                activeLeftMenu.parents("ul.sub-menu").prev("a.parent-menu").addClass("active");

                const activeIndex = leftMenuList.index(activeLeftMenu);
                const maxScrollTop = ($(`${preClasss}-left-panel-nav`).outerHeight() + $(`${preClasss}-left-panel-menu .heading-left-panel`).outerHeight()) - $(`${preClasss}-left-panel-menu`).outerHeight();
                const currentScrollTop = $(`${preClasss}-left-panel-menu`).scrollTop();
                const newScrollTop = (activeIndex / leftMenuNumbers) * maxScrollTop;

                if ((lastScreenTop <= currentScreenTop && currentScrollTop < newScrollTop) || (lastScreenTop >= currentScreenTop && currentScrollTop > newScrollTop)) {
                    $(`${preClasss}-left-panel-menu`).stop().animate({
                        scrollTop: activeIndex > (leftMenuNumbers / 2) ? newScrollTop + activeLeftMenu.outerHeight() : newScrollTop
                    }, 400);
                }

                lastScreenTop = currentScreenTop;
            }
        },

        handleOpenMenuContent: function (hrefTarget) {
            var btnToggleMenuContent = $(hrefTarget).parents(".body-result").prev(".left-menu-title").children(".button-toggle");

            if (!btnToggleMenuContent.hasClass('open')) {
                btnToggleMenuContent.trigger('click');
            }
        }
    };

    return _me;
})(jQuery);

LeftMenuPanel.initialize();