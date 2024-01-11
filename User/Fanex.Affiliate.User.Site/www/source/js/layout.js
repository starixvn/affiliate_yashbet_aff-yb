var Layout = (function ($) {
    var DEFAULT_SPEED = 20;

    function initSlider() {
        //var $mainBanner = $('#mainBanner');
        //$mainBanner.carousel();
    }

    var _me = {
        initialize: function () {
            initSlider();
            _me.registerAjaxHandlerError();
            _me.initEvent();
            _me.renderHotNews();
        },
        initEvent: function () {
            $("#selectedLanguage").on("click", function (e) {
                if ($(this).hasClass('active')) {
                    $(this).blur();
                } else {
                    $(this).addClass("active");
                }
            });

            $("#selectedLanguage").on("blur", function (e) {
                $(this).removeClass("active");
            });

            $("#languageList").on("click", ".language-item", function () {
                var $selectedItem = $(this);
                Site.changeLanguage($selectedItem.data('option'));
            });

            $('li.parent').on('mouseover', function () {
                var $menuItem = $(this),
                    $submenuWrapper = $('> .submenu-container', $menuItem);

                // grab the menu item's position relative to its positioned parent
                var menuItemPos = $menuItem.position();
                var withMenu = $menuItem.outerWidth();
                var heightMenu = $menuItem.outerHeight();
                var subWidth = $submenuWrapper.outerWidth();
                // place the submenu in the correct position relevant to the menu item
                $submenuWrapper.css({
                    top: menuItemPos.top + heightMenu,
                    left: menuItemPos.left + withMenu - subWidth
                });
            });

            $('#btn-menu').click(function (e) {
                if (isTriggerClick) {
                    _me.handleCloseMenu();
                } else {
                    _me.handleBindingClickBodyEvent();

                    $('body').toggleClass('menu-open');
                    $('.navbar-header').toggleClass('menu-open');
                }
            });

            $(window).scroll(function () {
                var aTop = 111;
                if ($(this).scrollTop() >= aTop) {
                    $(".mobile-left-panel").addClass("fixed");
                }
                else {
                    $(".mobile-left-panel").removeClass("fixed");
                }

                if ($(this).scrollTop() > 200) {
                    $('#moveToTop').fadeIn(200);
                } else {
                    $('#moveToTop').fadeOut(200);
                }
            });
        },

        handleBindingClickBodyEvent: function () {
            const hasClass = $('body').hasClass("menu-open");

            if (hasClass) {
                $('body').unbind("click");
                isOpenMenuMobile = false;
            } else {
                $('.overlay').click(function () {
                    if (isOpenMenuMobile) {
                        _me.handleClickOutAfterOpenMenu();
                    } else {
                        isOpenMenuMobile = true;
                    }
                });
            }
        },

        handleClickOutAfterOpenMenu: function () {
            isTriggerClick = true;
            $("#btn-menu").trigger("click");
        },

        handleCloseMenu: function () {
            isTriggerClick = false;
            isOpenMenuMobile = false;
            $('body').unbind("click");
            $('body.menu-open').removeClass('menu-open');
            $('.navbar-header.menu-open').removeClass('menu-open');
        },

        registerAjaxHandlerError: function () {
            $(document).ajaxError(function (event, jqXHR) {
                var status = jqXHR.status;

                switch (status) {
                    case 401:
                    case 403:
                        window.location.href = Site.resolveRelativeUrl("");
                        break;
                    case 404:
                        window.location.href = Site.resolveRelativeUrl("Notify/PageNotFound");
                        break;
                    case 500:
                        alert($("<div/>").html(window.Lang.ServerError).text());
                        break;
                    default:
                        break;
                }
            });
        },
        renderHotNews: function () {
            $.ajax({
                type: "GET",
                url: Site.resolveRelativeUrl("News/LatestHotNews"),
                dataType: "html",
                cache: false,
                success: function (response) {
                    var $marquee = $("#marquee");
                    $marquee.html(response);

                    const containerWidth = $("#marqueeContainer").outerWidth();
                    const hotNewsWidth = $marquee.outerWidth();
                    const speedRate = hotNewsWidth / containerWidth;
                    const speed = speedRate < 1 ? DEFAULT_SPEED : (speedRate + 0.25) * DEFAULT_SPEED;

                    $marquee.css("animation-duration", speed + "s");
                }
            });
        }
    }

    return _me;
})(jQuery);

Layout.initialize();