"use strict";

var ContactUs = (function ($) {
    var _me = {
        initialize: function initialize() {
            _me.initEvent();
        },

        initEvent: function initEvent() {
            $("#contactForm").on("submit", function () {
                _me.submit($(this));
                return false;
            });

            $("#btnReset").on("click", function () {
                _me.reset();
            });

            $("#captchaContactUs").on("click", function () {
                _me.refreshCaptcha();
            });
        },

        reset: function reset() {
            var requiredInputList = $("input,textarea", $("#contactForm"));

            var _iteratorNormalCompletion = true;
            var _didIteratorError = false;
            var _iteratorError = undefined;

            try {
                for (var _iterator = requiredInputList[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                    var element = _step.value;

                    var $element = $(element);
                    $element.val("");
                    var $container = $element.closest(".group");
                    Site.toggleErrorMessage($container, true);
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
        },

        submit: function submit($form) {
            var isValid = _me.checkRequiredInput() && _me.checkValidEmail();

            if (isValid) {
                var postData = $form.serializeObject();
                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("ContactUs/Index"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function success(response) {
                        _me.handleResponseResult(response);
                    },
                    beforeSend: function beforeSend() {
                        Site.showLoader();
                    },
                    complete: function complete() {
                        Site.hideLoader();
                    }
                });
            }
        },

        handleResponseResult: function handleResponseResult(response) {
            _me.refreshCaptcha();
            $("#messageSuccess").toggle(response.IsOk);
            $("#messageError").toggle(!response.IsOk).html(response.Message || resourceLanguage.generalError);
            if (response.IsOk) {
                $("#mainContent").remove();
            }
        },

        checkRequiredInput: function checkRequiredInput() {
            var requiredInputList = $("input[type=text],textarea", $("#contactForm"));
            var isValid = true;

            var _iteratorNormalCompletion2 = true;
            var _didIteratorError2 = false;
            var _iteratorError2 = undefined;

            try {
                for (var _iterator2 = requiredInputList[Symbol.iterator](), _step2; !(_iteratorNormalCompletion2 = (_step2 = _iterator2.next()).done); _iteratorNormalCompletion2 = true) {
                    var element = _step2.value;

                    if (element.name !== "Affiliate") {
                        var $element = $(element);
                        var hasValue = !!$element.val();

                        var $container = $element.closest(".group");
                        Site.toggleErrorMessage($container, hasValue);

                        if (isValid && !hasValue) {
                            isValid = false;
                        }
                    }
                }
            } catch (err) {
                _didIteratorError2 = true;
                _iteratorError2 = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion2 && _iterator2["return"]) {
                        _iterator2["return"]();
                    }
                } finally {
                    if (_didIteratorError2) {
                        throw _iteratorError2;
                    }
                }
            }

            return isValid;
        },

        checkValidEmail: function checkValidEmail() {
            var $email = $("#Email");
            var isValid = Site.emailValidationPattern.test($email.val());

            var $container = $email.closest(".group");
            Site.toggleErrorMessage($container, isValid);

            return isValid;
        },

        refreshCaptcha: function refreshCaptcha() {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?type=1&_v=" + new Date().getTime());
            $("#captchaContactUs").attr("src", captchaUrl);
        }
    };

    return _me;
})(jQuery);

ContactUs.initialize();

