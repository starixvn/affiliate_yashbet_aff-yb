"use strict";

var AfterLoginYB = (function ($) {

    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },

        initEvent: function initEvent() {
            //menu pc
            $('.content-menu-pc .parent .menu-item').click(function () {
                var isOpenMenu = $(this).hasClass("open-menu");

                if (isOpenMenu) {
                    $(this).blur();
                } else {
                    $(this).addClass("open-menu");
                }
            });

            $('.content-menu-pc .parent .menu-item').blur(function () {
                $(this).removeClass("open-menu");
            });

            //menu mobile
            $('.show-name-menu').click(function () {
                var isActive = $('.dropdown-content-menu').hasClass('active');

                if (isActive) {
                    $('ul.sub-menu-content').removeClass('active');
                    $('.dropdown-content-menu').removeClass('active');
                } else {
                    $('.dropdown-content-menu').addClass('active');
                    $('.dropdown-content-menu .menu-item').first().focus();
                }
            });

            $('.dropdown-content-menu > li > .menu-item, .sub-menu-content .menu-item').click(function () {
                var menuTitle = $(this).text();

                $('.show-name-menu').html(menuTitle);
                $('.dropdown-content-menu').removeClass('active');
                $('.dropdown-content-menu .sub-menu-content').removeClass('active');
            });

            $('.dropdown-content-menu .parent-item').click(function () {
                var isActive = $(this).next('.sub-menu-content').hasClass('active');

                if (isActive) {
                    $(this).next('.sub-menu-content').removeClass('active');
                } else {
                    $('.dropdown-content-menu .sub-menu-content').removeClass('active');

                    $(this).next('.sub-menu-content').addClass('active');
                    $(".sub-menu-content.active .menu-item").first().focus();
                }
            });

            $('.dropdown-content-menu').focusout(function () {
                var isHoverMenu = $(".dropdown-content-menu").is(':hover');

                if (!isHoverMenu) {
                    $('ul.dropdown-content-menu').removeClass("active");
                    $('ul.sub-menu-content').removeClass('active');
                }
            });
        }
    };

    return _me;
})(jQuery);

AfterLoginYB.initialize();

