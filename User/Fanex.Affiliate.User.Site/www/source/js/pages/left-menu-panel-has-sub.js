var LeftMenuPanel = (function ($) {
    const activeClassName = "active";
    let isOpenLeftMenuMobile = false;
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

        handleCloseLeftMenu: function () {
            isOpenLeftMenuMobile = false;
            $('body').unbind("click");

            $('.left-panel.active').removeClass('active');
            $('body.overflow-left-panel').removeClass('overflow-left-panel');
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
            const leftMenuList = $(`.sub-menu a.left-menu-item[href*="#"]`);
            const activeLeftMenu = $(`.sub-menu a.left-menu-item[href="${hrefTarget}"]`);
            const leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                $(".sub-menu a.left-menu-item.active").removeClass(activeClassName);
                activeLeftMenu.addClass(activeClassName);
                $("a.parent-menu.active").removeClass("active");
                activeLeftMenu.parents("ul.sub-menu").prev("a.parent-menu").addClass("active");

                const activeIndex = leftMenuList.index(activeLeftMenu);
                const maxScrollTop = ($('#left-panel-nav').outerHeight() + 24) - $('#left-panel-menu').outerHeight(); //24px of margin
                const currentScrollTop = $('#left-panel-menu').scrollTop();
                const newScrollTop = (activeIndex / leftMenuNumbers) * maxScrollTop;

                if ((lastScreenTop < currentScreenTop && currentScrollTop < newScrollTop) || (lastScreenTop > currentScreenTop && currentScrollTop > newScrollTop)) {
                    $('#left-panel-menu').stop().animate({
                        scrollTop: newScrollTop
                    }, 400);
                }

                lastScreenTop = currentScreenTop;
            }
        },

        handleScrollLeftMenuInMobile: function () {
            const leftMenuList = $(`.sub-menu a.left-menu-item[href*="#"]`);
            const activeLeftMenu = $(`.sub-menu a.left-menu-item.active`);
            const leftMenuNumbers = leftMenuList.length;

            if (leftMenuNumbers) {
                const activeIndex = leftMenuList.index(activeLeftMenu);
                const maxScrollTop = ($('#left-panel-nav').outerHeight() + 24) - $('#left-panel-menu').outerHeight();
                const newScrollTop = (activeIndex / leftMenuNumbers) * maxScrollTop;

                $('#left-panel-menu').stop().animate({
                    scrollTop: newScrollTop
                }, 400);
            }
        }
    };

    return _me;
})(jQuery);

LeftMenuPanel.initialize();