var MyAccount = (function ($) {
    function setFocusForLastInput() {
        $("#websiteInfo .urlInput").last().focus();
    }

    var _isClickSubmit = false;

    var _me = {
        initialize: function () {
            _me.initEvent();
        },
        initEvent: function () {
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

            $("#btnMyAccountSubmit").on("click", function () {
                _me.submit();
            });

            $("#btnCancel").on("click", function () {
                Site.redirect(Site.resolveRelativeUrl("UserPanel/Main/#AccountOverView"));
            });
        },

        submit: function () {
            _isClickSubmit = true;
            var $form = $("#accountForm");
            var validator = $form.validate();
            var isValidForm = validator.form();
            if (isValidForm) {
                Site.showLoader();
                WebSite.assignValueForWebSite();
                var postData = $form.serializeObject();

                $.ajax({
                    type: "POST",
                    url: Site.resolveRelativeUrl("UserPanel/MyAccount"),
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
            else {
                validator.focusInvalid();
            }

            return false;
        },

        handleResponseResult: function (response) {
            var message = response.IsOk ? resourceLanguage.updateSuccessfully : response.Message;
            Site.displayResult(response.IsOk, message);
            $(window).scrollTop(0);
        },

        addUrl: function () {
            var lang = {
                requiredMoreWebSiteUrl: resourceLanguage.requiredMoreWebSiteUrl,
                webSiteUrl: resourceLanguage.webSiteUrl,
                deleteLabel: resourceLanguage.deleteLabel
            }

            WebSite.handleAddingUrl(lang);
        }
    };

    return _me;
})(jQuery)

MyAccount.initialize();