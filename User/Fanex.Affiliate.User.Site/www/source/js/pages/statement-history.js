var StatementHistory = (function ($) {
    const _currentPage = "StatementHistory";
    const urlMapping = [
        { transType: 201, url: "UserPanel/Main/#DailyEarning" },
        { transType: 204, url: "UserPanel/Main/#SubAffEarning" }
    ];

    let _pageSize = 20;
    let _pageIndex = 0;
    let _scrollY = 0;
    let _expandElementList = [];

    function initPager() {
        $("#statementPager").pager({
            totalItems: parseInt($("#TotalItem").val()),
            pageSize: _pageSize,
            pageIndex: _pageIndex,
            languageCode: Site.getCurrentLanguage(),
            onChange: function (pageIndex, pageSize) {
                _pageIndex = pageIndex;
                _pageSize = pageSize;
                _expandElementList = [];

                var params = _me.getStatementHistoryParam();
                _me.renderStatementHistory(params);
            }
        });
    }

    function toggleChildrenElem(children, id) {
        if (children.hasClass("display-none")) {
            _expandElementList.push(id);
        } else {
            const index = _expandElementList.indexOf(id);
            if (index > -1) {
                _expandElementList.splice(index, 1);
            }
        }

        $(children).toggleClass("display-none");
    }

    var _me = {
        initialize: function () {
            _me.handleIfIsClickBreadcumb();
            initPager();
            _me.initEvent();
        },

        initEvent: function () {
            $("#base-currency").change(function () {
                _expandElementList = [];
                var params = _me.getStatementHistoryParam();
                _me.renderStatementHistory(params);
            })

            $("#statementDataList").on("click", ".amountItem", function () {
                let transtype = Number($(this).data("transtype"));
                let url = urlMapping.find(elem => elem.transType === transtype).url;
                let params = {
                    monthDate: $(this).data("date"),
                    requestPage: _currentPage,
                    baseCurrency: $("#base-currency").val(),
                    isOnlyContent: false
                };
                _me.setStatementHistoryParamIntoStorage();
                Site.redirect(Site.resolveRelativeUrl(`${url}?${$.param(params)}`));
            });

            $("#statementDataList").on("click", ".btn.btn-transparent", function () {
                let id = $(this).parents("tr").data("id");
                let children = $(this).parents("tr").siblings(`tr[data-parent='${id}']`);

                toggleCollapseButton(this);
                toggleChildrenElem(children, id);
            });
        },

        getStatementHistoryParam: function () {
            return {
                baseCurrency: $("#base-currency").val(),
                pageSize: _pageSize,
                pageIndex: _pageIndex + 1
            };
        },

        handleIfIsClickBreadcumb: function () {
            const isClickBreadcumb = $("#isClickBreadcumb").val() === 'True';

            if (isClickBreadcumb) {
                let statementHistoryState = sessionStorage.getItem("statementHistoryState");
                const oldState = JSON.parse(statementHistoryState) || { pageSize: _pageSize, pageIndex: _pageIndex, scrollY: _scrollY, expandElementList: _expandElementList }

                _pageSize = oldState.pageSize;
                _pageIndex = oldState.pageIndex;
                _scrollY = oldState.scrollY;
                _expandElementList = oldState.expandElementList;

                var params = _me.getStatementHistoryParam();
                _me.renderStatementHistory(params, isClickBreadcumb);
            }
        },

        handleUpdateByOldState: function () {
            window.scroll({
                top: _scrollY,
                behavior: 'smooth',
            });

            _expandElementList.forEach(_me.handleUpdateExpandingElementByOldState);
        },

        handleUpdateExpandingElementByOldState: function (id) {
            var btnGlyphicon = $(`[data-id="${id}"] span.glyphicon`)[0];

            if (!!btnGlyphicon) {
                let childrenOfId = $(`tr[data-parent='${id}']`);
                $(childrenOfId).toggleClass("display-none");

                $(btnGlyphicon).toggleClass("glyphicon-chevron-right");
                $(btnGlyphicon).toggleClass("glyphicon-chevron-down");
            }
        },

        setStatementHistoryParamIntoStorage: function () {
            const param = {
                pageSize: _pageSize,
                pageIndex: _pageIndex,
                scrollY: window.scrollY,
                expandElementList: _expandElementList
            }
            sessionStorage.setItem("statementHistoryState", JSON.stringify(param))
        },

        renderStatementHistory: function (params, isClickBreadcumb = false) {
            $.ajax({
                type: "POST",
                dataType: "html",
                cache: false,
                data: JSON.stringify(params),
                contentType: "application/json; charset=UTF-8",
                url: Site.resolveRelativeUrl("UserPanel/StatementHistory"),
                success: function (response) {
                    $("#statementDataList").html(response);

                    !!isClickBreadcumb && _me.handleUpdateByOldState();
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        }
    };

    return _me;
})(jQuery);

StatementHistory.initialize();