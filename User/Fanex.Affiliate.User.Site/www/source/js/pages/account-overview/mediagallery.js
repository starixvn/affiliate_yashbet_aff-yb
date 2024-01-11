var MediaGallery = (function ($) {
    var CATEGORY_CASINO = "casino";
    var CATEGORY_SPORTSBOOK = "sportsbook";

    var optionAll = "<option value=''>" + window.dic.all + "</option>";

    var _pageSize = 20;
    var _pageIndex = 0;

    function initPager() {
        $("#marketingPager").pager({
            totalItems: parseInt($("#TotalItem").val()),
            pageSize: _pageSize,
            pageIndex: _pageIndex,
            languageCode: Site.getCurrentLanguage(),
            onChange: function (pageIndex, pageSize) {
                _pageIndex = pageIndex;
                _pageSize = pageSize;

                var searchParams = _me.getSearchParam();
                _me.getBannerList(searchParams);
            }
        });
    }

    var _me = {
        initialize: function () {
            _me.initEvent();
            initPager();
            _me.togglePager();
            MarketingBase.initChannelSelectEvent();
        },

        initEvent: function () {
            $("#Category").on("change", function () {
                var selectedValue = $(this).val().toLowerCase();
                var selectedLinkType = $("#AffLinkType").val();
                var param = {
                    category: selectedValue,
                    linkType: selectedLinkType
                }
                var $subCategoryContainer = $("#SubCategory").closest(".form-group");

                if (selectedValue === CATEGORY_CASINO || selectedValue === CATEGORY_SPORTSBOOK) {
                    $subCategoryContainer.removeClass("hide");
                    _me.getSubcategory(param);
                }
                else {
                    $("#SubCategory").html(optionAll);
                    $subCategoryContainer.addClass("hide");
                }

                _me.getSize();
            });

            $("#AffLinkType").change(function () {
                let selectedVal = $(this).val();
                let selectedItem = window.searchOption.RefLinks.find(r => r.AffLinkType == selectedVal);


                let domains = selectedItem.Domains.map(item => new Option(item.DomainName, item.DomainId));
                let pages = selectedItem.SitePages.map(item => new Option(item.PageName, item.PageCode));
                let category = [new Option(window.dic.all, "")];
                category.push(...window.searchOption.CategoryList[selectedVal].map(item => new Option(item.Text, item.Value)));

                $("#Page").empty().append(pages);
                $("#Domain").empty().append(domains);
                $("#Category").empty().append(category);
            })

            $("#SubCategory").on("change", function () {
                _me.getSize();
            });

            $("#Language").on("change", function () {
                _me.getSize();
            });

            $("#Currency").on("change", function () {
                _me.getSize();
            });

            $("#ImageType").on("change", function () {
                _me.getSize();
            });

            $("#DeviceType").on("change", function () {
                _me.getSize();
            });

            $("#btnMarketingSearch").on("click", function () {
                _me.submitSearch();
                _me.renderQrCodePage()
                    .then(function () {
                        MarketingBase.registerToggleLinkEvent();
                        MarketingBase.resiterQrcodeEvent();
                    });
            });

            $("#dataContainerList").on("change", "#DeviceType", function () {
                _me.submitSearch();
            });

            new ClipboardJS('.copy-banner-code');
        },

        togglePager: function () {
            var totalItem = parseInt($("#TotalItem").val());

            $("#marketingPager").toggle(totalItem > 0);
        },

        submitSearch: function(){
            _pageIndex = 0;
            var searchParams = _me.getSearchParam();
            _me.getBannerList(searchParams, _me.updatePager);
        },

        getSubcategory: function (param) {
            $.ajax({
                type: "GET",
                dataType: "json",
                cache: false,
                data: param,
                url: Site.resolveRelativeUrl("UserPanel/Marketing/GetSubCategory"),
                success: function (response) {
                    var html = _me.buildOptionList(response, "Value", "Text", true);
                    $("#SubCategory").html(html);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        getSize: function () {
            let param = {
                category: $("#Category").val(),
                subCategory: $("#SubCategory").val(),
                language: $("#Language").val(),
                currency: $("#Currency").val(),
                imageType: $("#ImageType").val(),
                deviceType: $("#DeviceType").val(),
                targetId: $("#AffLinkType").val()
            };

            $.ajax({
                type: "GET",
                dataType: "json",
                cache: false,
                data: param,
                url: Site.resolveRelativeUrl("UserPanel/Marketing/GetSize"),
                success: function (response) {
                    var html = _me.buildOptionList(response, "Size", "Size", true);
                    $("#Size").html(html);
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        buildOptionList: function (dataList, value, text, hasAllOption) {
            var optionList = dataList.map(function (item) {
                return "<option value='" + item[value] + "'>" + item[text] + "</option>"
            });

            if (hasAllOption) {
                optionList.unshift(optionAll);
            }

            return optionList.join("");
        },

        getSearchParam: function () {
            return {
                category: $("#Category").val(),
                subCategory: $("#SubCategory").val(),
                language: $("#Language").val(),
                imageType: $("#ImageType").val(),
                size: $("#Size").val(),
                currency: $("#Currency").val(),
                deviceType: $("#DeviceType").val(),
                selectedDomainId: $("#Domain").val(),
                selectedPage: $("#Page").val(),
                selectedChannel: $("#Channel").val(),
                targetId: $("#AffLinkType").val(),
                pageIndex: _pageIndex + 1,
                pageSize: _pageSize
            };
        },

        getBannerList: function (params, onSuccess) {
            $.ajax({
                type: "GET",
                dataType: "html",
                cache: false,
                data: params,
                url: Site.resolveRelativeUrl("UserPanel/Marketing/GetBanner"),
                success: function (response) {
                    $("#marketingDataList").html(response);
                    onSuccess && onSuccess();

                    _me.togglePager();
                }
            });
        },

        updatePager: function () {
            $("#marketingPager").pager("update", {
                pageIndex: _pageIndex,
                totalItems: parseInt($("#TotalItem").val())
            });
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

MediaGallery.initialize();