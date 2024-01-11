var Login = (function ($) {
    var _refreshCaptchaTimeoutId = null;
    var REFRESH_CAPTCHA_INTERVAL = 600000;

    function registerRefreshLoginCaptcha(isShowingModal) {
        function refreshCaptcha() {
            if (isShowingModal) {
                _me.generateDialogCaptcha();
            }
            else {
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
        initialize: function () {
            _me.registerLoginDialog();
            _me.initEvent();
            _me.initLoginDialogEvent();
            registerRefreshLoginCaptcha(false);
        },
        registerLoginDialog: function () {
            $("#loginDialog").on("show.bs.modal", function (e) {
                _me.generateDialogCaptcha();
                registerRefreshLoginCaptcha(true);
            })

            $("#loginDialog").on("hide.bs.modal", function (e) {
                $("#loginDialogcaptchaImg").attr("src", "");
                _me.generateCaptcha();
                registerRefreshLoginCaptcha(false);
            })
        },
        initLoginDialogEvent: function () {
            $('#mobileLogin').on('click', function () {
                // $("#loginDialog").modal({ backdrop: "static" });
                $("#loginDialog").modal();  // remove backdrop: "static" to close popup when tap out
            });

            $(".mediaLoginLink").on("click", function () {
                $('#isCLickedBannerLink').val("1");
                // $("#loginDialog").modal({ backdrop: "static" });
                $("#loginDialog").modal();  // remove backdrop: "static" to close popup when tap out
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
        initEvent: function () {
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

        requestSignin: function (onDialog) {
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
                var publicKey = AuthHelper.base64DecodeUnicode($("#PublicKey").val());
                var timeStamp = AuthHelper.getUtcUnixTimeStamp();

                var queryString = 'u=' + encodeURIComponent(username.trim()) + '&p=' + encodeURIComponent(password) + '&t=' + timeStamp;
                var encryptedQueryString = AuthHelper.encrypt(queryString, publicKey);
                $password.val(encryptedQueryString);

                var postData = {
                    __RequestVerificationToken: token,
                    LoginInfo: encodeURIComponent(encryptedQueryString),
                    ValidationKey: validation
                };

                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("Auth/SignIn"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function (response) {
                        if (response.IsOk) {
                            var isRedirectToMediaPage = $('#isCLickedBannerLink').val() === "1";
                            if (onDialog && isRedirectToMediaPage) {
                                PageTracking.addTrackingForMediaPage();

                                setTimeout(function () {
                                    Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#AccountOverview#MediaGallery"));
                                }, 10);
                            }
                            else {
                                Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#AccountOverview"));
                            }
                        }
                        else {
                            if (response.ShouldVerifyAccount) {
                                Site.redirect(Site.resolveRelativeUrl('VerificationAccount'));
                            }
                            else {
                                alert(response.Message);
                                if (onDialog) {
                                    $username.val("");
                                    $password.val("");
                                    $validation.val("");
                                    _me.generateDialogCaptcha();
                                    registerRefreshLoginCaptcha(true);
                                }
                                else {
                                    Site.reload();
                                }
                                toggleLoginButton(true, onDialog);
                            }
                        }
                    },
                    beforeSend: function () {
                        Site.showLoader();
                        toggleLoginButton(false, onDialog);
                    },
                    complete: function (jqXHR, textStatus) {
                        Site.hideLoader();
                        if (textStatus === 'error') {
                            toggleLoginButton(true, onDialog);
                        }
                    }
                });
            }
            else {
                alert($("<div/>").html(validationResult.message).text());
            }
        },

        validate: function (username, password, validationCode, prefix) {
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

        generateDialogCaptcha: function () {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?_v=" + new Date().getTime());
            $("#loginDialogcaptchaImg").attr("src", captchaUrl);
        },
        generateCaptcha: function () {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?_v=" + new Date().getTime());
            $("#captchaImg").attr("src", captchaUrl);
        }
    };

    return _me;
})(jQuery);

Login.initialize();