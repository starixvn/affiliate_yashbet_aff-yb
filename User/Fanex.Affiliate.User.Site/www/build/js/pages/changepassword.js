var AuthHelper = (function ($) {
    function encrypt(plainText, key) {
        var encrypt = new JSEncrypt();
        encrypt.setPublicKey(key);

        return encrypt.encrypt(plainText);
    }

    function base64DecodeUnicode(str) {
        return decodeURIComponent(Array.prototype.map.call(atob(str), function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        }).join(''))
    }

    function getUtcUnixTimeStamp() {
        var now = new Date();
        return Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(),
              now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
    }

    var _me = {
        encrypt: encrypt,
        base64DecodeUnicode: base64DecodeUnicode,
        getUtcUnixTimeStamp: getUtcUnixTimeStamp
    }

    return _me;
})(jQuery);
var ChangePassword = (function ($) {
    var _me = {
        initialize: function () {
            _me.initEvent();
        },

        initEvent: function () {
            $("#btnCancel").on("click", function () {
                Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#MyAccount"));
            });

            $("#updatePasswordForm").on("submit", function (e) {
                e.preventDefault();
                _me.submit();
                return false;
            });

            $('#mainMenu .parent .submenu-container').hover(function () {
                $(this).addClass("hover");
            }, function () {
                $(this).removeClass("hover");
            });
        },

        submit: function () {
            var $form = $("#updatePasswordForm");
            var validator = $form.validate();
            var isValidForm = validator.form();

            if (isValidForm) {
                var $newPassword = $("#NewPassword");
                var $oldPassword = $("#OldPassword");
                var $confirmedNewPassword = $("#ConfirmNewPassword");

                if ($newPassword.val() !== $confirmedNewPassword.val()) {
                    Site.displayResult(false, resource.notMatchPassword);
                    return;
                }

                var publicKey = AuthHelper.base64DecodeUnicode($("#PublicKey").val());

                var encryptedOldPassword = AuthHelper.encrypt($oldPassword.val(), publicKey);
                var encryptedNewPassword = AuthHelper.encrypt($newPassword.val(), publicKey);
                var confirmedNewPassword = AuthHelper.encrypt($confirmedNewPassword.val(), publicKey);

                $oldPassword.val(encryptedOldPassword);
                $newPassword.val(encryptedNewPassword);
                $confirmedNewPassword.val(confirmedNewPassword);

                var postData = $form.serializeObject();

                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("Password/Update"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function (response) {
                        $form[0].reset();
                        validator.resetForm();
                        Site.displayResult(response.IsOk, response.Message);
                    },
                    beforeSend: function () {
                        Site.showLoader();
                    },
                    complete: function () {
                        Site.hideLoader();
                    }
                });
            }
        }
    };

    return _me;
})(jQuery);

ChangePassword.initialize();