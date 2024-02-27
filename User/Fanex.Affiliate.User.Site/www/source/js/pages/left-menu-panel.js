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

            $('a.left-menu-item').bind('click', function (e) {
                window.removeEventListener("scroll", _me.handleActiveLeftMenuByScroll);
                e.preventDefault();

                $(this).blur();
                var hrefTarget = $(this).attr("href");

                _me.handleOpenMenuContent(hrefTarget);
                _me.handelAnimate(hrefTarget);
                
                setTimeout(function () {
                    $('#leftmenu').modal('hide');
                }, 400);
            });
        },

        handleActiveLeftMenuByScroll: function () {
            const sections = $("section.info-question");

            if (sections.length) {
                let theFirstMenuId = sections[0].getAttribute("id");
                let activeMenuId = theFirstMenuId;
                let currentScreenTop = lastScreenTop;

                for (let section of sections) {
                    const sectionTop = section.offsetTop;

                    if (window.pageYOffset >= sectionTop - (window.innerHeight * 1 / 5)) {
                        activeMenuId = section.getAttribute("id");
                        currentScreenTop = sectionTop;
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
                $(`${preClasss}-left-panel-nav a.left-menu-item.active`).removeClass(activeClassName);
                activeLeftMenu.addClass(activeClassName);

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
            var btnToggleMenuContent = $(`${hrefTarget} .button-toggle`);

            if (!btnToggleMenuContent.hasClass('open')) {
                btnToggleMenuContent.trigger('click');
            }
        }
    };

    return _me;
})(jQuery);

LeftMenuPanel.initialize();