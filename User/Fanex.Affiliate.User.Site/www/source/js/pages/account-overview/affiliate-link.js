var AffiliateLink = (function ($) {

    function handleFalseResponse(response) {
        let data;
        switch (response.status) {
            case 403:
                data = response.json();
                alert(data.ErrMsg);
                location.reload();
                break;
            default:
                alert('Internal error');
                break;
        }
    }


    var _me = {
        initialize: function () {
            _me.initEvent();
            _me.renderLinkChannelStatisticPage();
        },

        initEvent: function () {
            $("#AffLinkType").change(function () {
                let selectedVal = $(this).val();
                let selectedItem = window.searchOption.RefLinks.find(r => r.AffLinkType == selectedVal);

                let domains = selectedItem.Domains.map(item => new Option(item.DomainName, item.DomainId));
                let pages = selectedItem.SitePages.map(item => new Option(item.PageName, item.PageCode));

                $("#Page").empty().append(pages);
                $("#Domain").empty().append(domains);
            })

            $("#btnGenQRCode").click(function () {
                Site.showLoader();
                Promise.all([_me.renderLinkChannelStatisticPage(), _me.renderQrCodePage()])
                    .then(() => {
                        Site.hideLoader();
                        MarketingBase.registerToggleLinkEvent(_me.renderLinkChannelStatisticPage);
                        MarketingBase.resiterQrcodeEvent();
                    })
            });

            MarketingBase.initChannelSelectEvent();
        },

        renderLinkChannelStatisticPage: function () {
             return MarketingBase.renderLinkChannelStatisticPage()
                .then((response) => {
                    if (response.ok)
                        return response.text()
                    else {
                        handleFalseResponse(response);
                    }
                })
                .then(html => {
                    $("#link-statistic").empty().html(html);
                })
        },

        renderQrCodePage: function () {
            let param = {
                selectedLanguage: $('#Language').val(),
                selectedPage: $('#Page').val(),
                selectedChannel: $('#Channel').val(),
                selectedDomainId: $('#Domain').val(),
                selectedLinkType: $('#AffLinkType').val()
            }
            return MarketingBase.renderQrCodePage(param)
                .then((response) => {
                    if (response.ok)
                        return response.text()
                    else {
                        handleFalseResponse(response);
                    }
                })
                .then(html => {
                    $("#qr-code").empty().html(html);
                });
        },
    };

    return _me;
})(jQuery);

AffiliateLink.initialize();