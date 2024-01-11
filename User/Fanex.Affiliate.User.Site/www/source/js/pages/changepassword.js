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