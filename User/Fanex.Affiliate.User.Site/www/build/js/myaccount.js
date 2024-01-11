var WebSite = (function ($) {
    function setFocusForLastInput() {
        $("#websiteInfo .urlInput").last().focus();
    }

    var _me = {
        getMoreWebsiteUrlData: function () {
            var moreUrlList = [];
            $("#websiteInfo .urlInput").each(function (index, data) {
                if (data.value) {
                    moreUrlList.push(Site.escapeHtml(data.value.trim()));
                }
            });

            return moreUrlList.join(";");
        },

        reoderInputUrl: function (webSiteLabel) {
            var index = 2;
            $(".moreWebsiteLabel").each(function (i, item) {
                var $item = $(item);
                var labelText = webSiteLabel + ' ' + index;
                $item.text(labelText);

                index++;
            });
        },

        deleteWebsiteUrl: function ($currentItem) {
            var $container = $currentItem.closest(".websiteInput");
            $container.remove();
        },

        buildUrlInput: function (webSiteLabel, deleteLabel, errorMessage) {
            var currentItemCount = $("#websiteInfo .urlInput").length + 2;
            var name = "MoreUrlInput" + currentItemCount;
            var inputItem = '<div class="form-group websiteInput">' +
                    '<label class="col-sm-3 control-label require moreWebsiteLabel">' + webSiteLabel + ' ' + currentItemCount + '</label>' +
                    '<div class="col-sm-4">' +
                        '<input class="form-control urlInput" type="text" name="' + name + '" data-val="true" data-val-required="' + errorMessage + '" value="" />' +
                    '</div>' +
                    '<div class="col-sm-1">' +
                        '<button type="button" class="btn btn-submit btnDeleteUrl" title="' + deleteLabel + '"><span class="glyphicon glyphicon-minus" aria-hidden="true"></span></button>' +
                    '</div>' +
                    '<div class="col-sm-offset-3 col-sm-9"><span class="field-validation-valid help-block hide" data-valmsg-for="' + name + '" data-valmsg-replace="true"></span></div>' +
                '</div>';

            $("#websiteInfo").append(inputItem);
            _me.reoderInputUrl(webSiteLabel);
        },

        handleAddingUrl: function (lang) {
            var hasWebSite = _me.hasWebSite();
            var isValid = _me.validateWebSiteGroup();

            if (hasWebSite && isValid) {
                _me.addMoreUrl(lang);
            }
        },

        addMoreUrl: function (lang) {
            var currentItemCount = $("#websiteInfo .urlInput").length + 2;
            var errorMessage = lang.requiredMoreWebSiteUrl.replace("{i}", currentItemCount);
            _me.buildUrlInput(lang.webSiteUrl, lang.deleteLabel, errorMessage);
            setFocusForLastInput();

            _me.refreshFormValidation();
        },

        refreshFormValidation: function () {
            var $form = $("#accountForm");
            $form.removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse($form);
        },

        validateWebSiteGroup: function () {
            var validator = _me.getValidator();
            var isValidSiteUrl = validator.element("#SiteUrl");
            var isValidNoSiteDesc = validator.element("#NoSiteDesc");
            var isValidMainInfo = isValidSiteUrl && isValidNoSiteDesc;

            var isValidMoreUrl = true;
            var moreInputUrl = $(".urlInput");
            for (var i = 0; i < moreInputUrl.length; i++) {
                var item = moreInputUrl[i];
                isValidMoreUrl = isValidMoreUrl && validator.element(item);
            }

            return isValidMainInfo && isValidMoreUrl;
        },

        changeWebsiteOption: function (hasWebSite, isClickSubmit) {
            isClickSubmit && _me.validateWebSiteGroup();
            var $mainUrlInput = $("#SiteUrl");
            $mainUrlInput.prop("disabled", !hasWebSite);
            $(".websiteInput").toggle(hasWebSite);
            $("#btnAddUrl").prop("disabled", !hasWebSite);

            var $noSiteDesc = $("#NoSiteDesc");
            $noSiteDesc.prop("disabled", hasWebSite);
        },

        assignValueForWebSite: function () {
            var $siteUrl = $("#SiteUrl");
            var $moreSiteUrl = $("#MoreSiteUrls");
            var $noSiteDesc = $("#NoSiteDesc");

            if (_me.hasWebSite()) {
                var moreWebSiteUrl = _me.getMoreWebsiteUrlData();
                $moreSiteUrl.val(moreWebSiteUrl);

                $noSiteDesc.val("");
            }
            else {
                $siteUrl.val("");
                $moreSiteUrl.val("");
            }
        },

        getValidator: function () {
            return $("#accountForm").validate();
        },

        hasWebSite: function () {
            return $("input[name=HasWebSite][type=radio]:checked").val() === "True";
        }
    }

    return _me;
})(jQuery);
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