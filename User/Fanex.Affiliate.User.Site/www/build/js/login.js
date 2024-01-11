'use strict';

var Login = (function ($) {
    var _refreshCaptchaTimeoutId = null;
    var REFRESH_CAPTCHA_INTERVAL = 600000;

    function encrypt(plainText, key) {
        var encrypt = new JSEncrypt();
        encrypt.setPublicKey(key);

        return encrypt.encrypt(plainText);
    }

    function base64DecodeUnicode(str) {
        return decodeURIComponent(Array.prototype.map.call(atob(str), function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
    }

    function getUtcUnixTimeStamp() {
        var now = new Date();
        return Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
    }

    function registerRefreshLoginCaptcha(isShowingModal) {
        function refreshCaptcha() {
            if (isShowingModal) {
                _me.generateDialogCaptcha();
            } else {
                _me.generateCaptcha();
            }
            clearTimeout(_refreshCaptchaTimeoutId);
            _refreshCaptchaTimeoutId = setTimeout(refreshCaptcha, REFRESH_CAPTCHA_INTERVAL);
        }

        clearTimeout(_refreshCaptchaTimeoutId);
        _refreshCaptchaTimeoutId = setTimeout(function () {
            refreshCaptcha();
        }, REFRESH_CAPTCHA_INTERVAL);
    }

    function toggleLoginButton(isEnable, onDialog) {
        var btnLoginselector = onDialog ? '#btnDialogLogin' : '#btnLogin';
        $(btnLoginselector).prop('disabled', !isEnable);
    }

    var _me = {
        initialize: function initialize() {
            _me.registerLoginDialog();
            _me.initEvent();
            _me.initLoginDialogEvent();
            registerRefreshLoginCaptcha(false);
        },
        registerLoginDialog: function registerLoginDialog() {
            $("#loginDialog").on("show.bs.modal", function (e) {
                _me.generateDialogCaptcha();
                registerRefreshLoginCaptcha(true);
            });

            $("#loginDialog").on("hide.bs.modal", function (e) {
                $("#loginDialogcaptchaImg").attr("src", "");
                _me.generateCaptcha();
                registerRefreshLoginCaptcha(false);
            });
        },
        initLoginDialogEvent: function initLoginDialogEvent() {
            $('#mobileLogin').on('click', function () {
                $("#loginDialog").modal({ backdrop: "static" });
            });

            $(".mediaLoginLink").on("click", function () {
                $('#isCLickedBannerLink').val("1");
                $("#loginDialog").modal({ backdrop: "static" });
            });

            $("#btnDialogLogin").on("click", function () {
                _me.requestSignin(true);
            });

            $("#dialogUserInput input").on("keypress", function (e) {
                if (e.keyCode === 13 && !$('#btnDialogLogin').prop('disabled')) {
                    _me.requestSignin(true);
                }
            });

            $("#loginDialogcaptchaImg").on("click", function () {
                _me.generateDialogCaptcha();
                registerRefreshLoginCaptcha(true);
            });
        },
        initEvent: function initEvent() {
            $("#btnLogin").on("click", function () {
                _me.requestSignin();
            });

            $("#userLogin input").on("keypress", function (e) {
                if (e.keyCode === 13 && !$('#btnLogin').prop('disabled')) {
                    _me.requestSignin();
                }
            });

            $("#captchaImg").on("click", function () {
                _me.generateCaptcha();
                registerRefreshLoginCaptcha(false);
            });
        },

        requestSignin: function requestSignin(onDialog) {
            var prefix = onDialog ? "dialog" : "";

            var $username = $("#" + prefix + "txtUsername");
            var $password = $("#" + prefix + "txtPassword");
            var $validation = $("#" + prefix + "txtValidation");

            var username = $username.val();
            var password = $password.val();
            var validation = $validation.val();

            var validationResult = _me.validate(username, password, validation, prefix);
            if (validationResult.isValid) {
                var token = $('input[name="__RequestVerificationToken"]', $("#userLogin")).val();
                var publicKey = base64DecodeUnicode($("#PublicKey").val());

                var queryString = 'u=' + encodeURIComponent(username.trim()) + '&p=' + encodeURIComponent(password) + '&t=' + getUtcUnixTimeStamp();
                var encryptedQueryString = encrypt(queryString, publicKey);
                $password.val(encryptedQueryString);

                var postData = {
                    __RequestVerificationToken: token,
                    LoginInfo: encryptedQueryString,
                    ValidationKey: validation
                };

                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("Auth/SignIn"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function success(response) {
                        if (response.IsOk) {
                            var isRedirectToMediaPage = $('#isCLickedBannerLink').val() === "1";
                            if (onDialog && isRedirectToMediaPage) {
                                PageTracking.addTrackingForMediaPage();

                                setTimeout(function () {
                                    Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#MediaGallery"));
                                }, 10);
                            } else {
                                Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#AccountOverview"));
                            }
                        } else {
                            alert($("<div/>").html(response.Message).text());
                            if (onDialog) {
                                $username.val("");
                                $password.val("");
                                $validation.val("");
                            } else {
                                Site.reload();
                            }
                            toggleLoginButton(true, onDialog);
                        }
                    },
                    beforeSend: function beforeSend() {
                        Site.showLoader();
                        toggleLoginButton(false, onDialog);
                    },
                    complete: function complete(jqXHR, textStatus) {
                        Site.hideLoader();
                        if (textStatus === 'error') {
                            toggleLoginButton(true, onDialog);
                        }
                    }
                });
            } else {
                alert($("<div/>").html(validationResult.message).text());
            }
        },

        validate: function validate(username, password, validationCode, prefix) {
            var empty = "";
            var result = {
                isValid: false,
                message: empty
            };

            if (username === empty) {
                $("#" + prefix + "txtUsername").focus();
                result.message = validationMessage.enterUserName;
                return result;
            }

            if (password === empty) {
                $("#" + prefix + "txtPassword").focus();
                result.message = validationMessage.enterPassword;
                return result;
            }

            if (validationCode === empty) {
                $("#" + prefix + "txtValidation").focus();
                result.message = validationMessage.enterValidationCode;
                return result;
            }

            result.isValid = true;
            return result;
        },

        generateDialogCaptcha: function generateDialogCaptcha() {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?_v=" + new Date().getTime());
            $("#loginDialogcaptchaImg").attr("src", captchaUrl);
        },
        generateCaptcha: function generateCaptcha() {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?_v=" + new Date().getTime());
            $("#captchaImg").attr("src", captchaUrl);
        }
    };

    return _me;
})(jQuery);

Login.initialize();

