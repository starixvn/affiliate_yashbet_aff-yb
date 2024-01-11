var MarketingBase = (function ($) {
    isCancelConfirm = false;

    function getDownloadParam(trigger) {
        return {
            selectedLanguage: $(trigger).data("selected-language"),
            selectedDomainId: $(trigger).data("selected-domain-id"),
            selectedPage: $(trigger).data("selected-page"),
            selectedChannel: $(trigger).data("selected-channel"),
            selectedLinkType: $(trigger).data("selected-link-type")
        };
    }

    function getToggleBtnStatus(cheked) {
        return cheked ? "on" : "off";
    }

    function handleFalseResponse(response) {
        let data;
        switch (response.status) {
            case 403:
                data = response.json();
                alert(data.ErrMsg);
                location.reload();
                break;
            default:
                alert("Internal Error");
                break;
        }
    }

    var _me = {
        resiterQrcodeEvent: function () {
            new ClipboardJS('.btn-copy-link', {
                text: function (trigger) {
                    let parent = $(trigger).parents(".qr-item").first();
                    return parent.data("link");
                }
            });

            new ClipboardJS('.btn-copy-qr', {
                text: function (trigger) {
                    let imgTag = $(trigger).parents(".qr-action").prev().children("img.qrcode")[0];
                    return imgTag.outerHTML;
                }
            });

            $('.btn-save-img').click(function () {
                let downloadTag = document.createElement("a");
                let param = getDownloadParam($(this).parents(".qr-item")[0]);
                var query = $.param(param);
                let url = `${Site.resolveRelativeUrl("UserPanel/Marketing/DownloadQrCode")}?${query}`;
                downloadTag.href = url;
                downloadTag.click();
            });
        },

        initChannelSelectEvent: function () {
            if ($("#Channel").children().length > 1) {
                document.getElementById("Channel").selectedIndex = 1;
            }

            $("#Channel").focusin(function () {
                window.previousChannelIndex = this.selectedIndex;
                this.selectedIndex = -1;
            })

            $("#Channel").focusout(function () {
                if (this.selectedIndex <= 0) {
                    this.selectedIndex = window.previousChannelIndex === 0 ? 1 : window.previousChannelIndex;
                }
            })

            $("#Channel").change(function () {
                if ($(this).val() === 'add-new') {
                    let text = prompt(window.dic.enterChannel);
                    if (text) {
                        let addPromise = MarketingBase.addChannel(text);
                        addPromise.then(result => {
                            if (result) {
                                if (result.IsOk) {
                                    alert(window.dic.addChannelSuccess)
                                    $(this).append(new Option(result.Data.ChannelName, result.Data.ChannelCode, false, true));
                                    $("button .btn-gen").removeAttr("disabled");
                                }
                                else {
                                    alert(result.Message);
                                }
                            }
                        })
                    }
                }
                $(this).blur();
            })
        },

        addChannel: function (text) {
            Site.showLoader();
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/AddChannel");
            let param = { channelName: text };
            let fetchData = fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
                body: new URLSearchParams(param)
            })
                .then((response => {
                    Site.hideLoader();
                    data = response.json();
                    if (response.ok) {
                        return data;
                    }
                    else {
                        handleFalseResponse(response);
                    }
                }))
            return Promise.resolve(fetchData);
        },

        renderLinkChannelStatisticPage: function () {
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/RenderUsingMarketLinkPage");
            return fetch(url, {
                method: 'GET',
                credentials: 'same-origin',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
            })
        },

        renderQrCodePage: function (param) {
            let url = new URL(Site.resolveRelativeUrl("UserPanel/Marketing/RenderQrCodePage"), window.location.origin);
            let query = new URLSearchParams(param);
            url.search = query.toString();
            return fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
            })
        },

        toggleMarketingLink: function (param) {
            let url = Site.resolveRelativeUrl("UserPanel/Marketing/ToggleMarketingLink");
            return fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                credentials: 'same-origin',
                body: new URLSearchParams(param)
            })
        },

        registerToggleLinkEvent: function (callback) {
            $checkBox = $(".toggle-link");
            $checkBox.off("change");

            $checkBox.on("change", function () {
                if (isCancelConfirm) {
                    isCancelConfirm = false;
                    return;
                }
                let checked = $(this).prop('checked');
                let confirmText = checked
                    ? window.dic.confirmActiveLink
                    : window.dic.confirmDeactiveLink

                if (confirm(confirmText)) {
                    let parent = $(this).parents(".qr-item").first();
                    let param = {
                        ChannelCode: parent.data("selected-channel"),
                        LanguageCode: parent.data("selected-language"),
                        AffRefLink: parent.data("link"),
                        IsOn: checked
                    }

                    _me.toggleMarketingLink(param)
                        .then(response => {
                            if (response.ok) {
                                $(parent).find("button").prop('disabled', !checked);
                                $(parent).find(".qr-img").toggleClass("disabled");
                                callback && callback();
                            }
                            else {
                                handleFalseResponse(response);
                                isCancelConfirm = true;
                                $(this).bootstrapToggle(getToggleBtnStatus(!checked));
                            }
                        })


                }
                else {
                    isCancelConfirm = true;
                    $(this).bootstrapToggle(getToggleBtnStatus(!checked));
                }

            })
        },

        initialize: function () {
            this.resiterQrcodeEvent();
        },
    };

    return _me;
})(jQuery);