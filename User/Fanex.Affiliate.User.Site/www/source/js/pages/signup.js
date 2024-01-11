var SignUp = (function ($) {
    var _isReadTermAndCondition = false;
    var _isClickSubmit = false;

    var _me = {
        initialize: function () {
            _me.initEvent();
            _me.refreshCaptcha();
        },

        initEvent: function () {
            $("#accountForm").on("submit", function (e) {
                e.preventDefault();
                return _me.submit();
            });

            $("#btnReset").on("click", function () {
                _me.reset();
            });

            $("#chkAgree").on("click", function () {
                $("#btnSubmit").prop("disabled", !this.checked);
                if (this.checked && !_isReadTermAndCondition) {
                    _isReadTermAndCondition = true;
                    window.open(Site.resolveRelativeUrl("TermAndCondition/Popup"), '', 'scrollbars=yes,resizable=yes,width=1280,height=800');
                }
            });

            $("#btnAddUrl").on("click", function () {
                _me.addUrl();
            });

            $("#websiteInfo").on("click", ".btnDeleteUrl", function () {
                var $this = $(this);
                WebSite.deleteWebsiteUrl($this);
                WebSite.reoderInputUrl(resourceLanguage.webSiteUrl);
            });

            $("#accountForm").on("change", "input[name=HasWebSite][type=radio]", function () {
                var hasWebSite = this.value === "True";
                WebSite.changeWebsiteOption(hasWebSite, _isClickSubmit);
            });

            $('select[data-val="true"]').on("change", function () {
                var validator = $("#accountForm").validate();

                validator.element($(this));
            });

            $("#signupCaptcha").on("click", function () {
                _me.refreshCaptcha();
            })
        },

        submit: function () {
            _isClickSubmit = true;
            var agree = $("#chkAgree").prop("checked");
            var $form = $("#accountForm");
            var validator = $form.validate();

            var isValidForm = agree && validator.form();
            if (isValidForm) {
                Site.showLoader();
                WebSite.assignValueForWebSite();
                var postData = $form.serializeObject();

                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("Signup/Index"),
                    data: postData,
                    dataType: "json",
                    cache: false,
                    contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                    success: function (response) {
                        _me.handleResponseResult(response);
                    },
                    complete: function () {
                        Site.hideLoader();
                    }
                });
            }

            return false;
        },

        handleResponseResult: function (response) {
            _me.refreshCaptcha();
            $("#messageSuccess").toggle(response.IsOk);
            $("#messageError").toggle(!response.IsOk).html(response.Message || resourceLanguage.generalError);
            if (response.IsOk) {
                $("#mainContent").remove();
            }
            $(window).scrollTop(0);
        },

        reset: function () {
            var inputList = $("input, select, textarea", $("#accountForm"));
            for (var i = 0; i < inputList.length; i++) {
                var element = inputList[i];
                if (element.type !== "radio" && element.type !== "hidden") {
                    $(element).val("");
                }
            }

            var validator = $("#accountForm").validate();
            validator.resetForm();

            $("#male").prop("checked", true);
            $("#chkAgree").prop("checked", false);
            $("#btnSubmit").prop("disabled", true);
            _me.resetWebSiteGroup();
        },

        resetWebSiteGroup: function () {
            $("#hasWebSite").prop("checked", true);
            $("#NoSiteDesc").prop("disabled", true);
            $("#SiteUrl").prop("disabled", false);
            $("#btnAddUrl").prop("disabled", false);
            $(".websiteInput").remove();
        },

        addUrl: function () {
            var lang = {
                requiredMoreWebSiteUrl: resourceLanguage.requiredMoreWebSiteUrl,
                webSiteUrl: resourceLanguage.webSiteUrl,
                deleteLabel: resourceLanguage.deleteLabel
            }

            WebSite.handleAddingUrl(lang);
        },

        refreshCaptcha: function () {
            var captchaUrl = Site.resolveRelativeUrl("Auth/CaptchaImage?type=1&_v=" + new Date().getTime());
            $("#signupCaptcha").attr("src", captchaUrl);
        }
    };

    return _me;
})(jQuery);

SignUp.initialize();