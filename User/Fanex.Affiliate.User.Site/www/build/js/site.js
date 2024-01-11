'use strict';

var Site = (function ($) {
    var _rootUrl = null;
    var _language = null;

    function createNewUrlByLanguage(url, language) {
        var hasQueryString = url.indexOf('?') !== -1;
        var updatedQueryParam = 'lang=' + language;
        var key = 'lang';

        if (hasQueryString) {
            var regex = new RegExp('([?&])' + key + '=.*?(&|$)', 'i');
            return regex.test(url) ? url.replace(regex, '$1' + key + '=' + language + '$2') : url + '&' + updatedQueryParam;
        } else {
            return url + '?' + updatedQueryParam;
        }
    }

    var _me = {
        initialize: function initialize() {
            _rootUrl = $("#RootUrl").val();
            _language = $("#Language").val();
        },
        resolveRelativeUrl: function resolveRelativeUrl(url) {
            return _rootUrl + url;
        },
        getCurrentLanguage: function getCurrentLanguage() {
            return _language;
        },
        escapeHtml: function escapeHtml(inputString) {
            var div = document.createElement('div');
            div.appendChild(document.createTextNode(inputString));
            return div.innerHTML;
        },
        showLoader: function showLoader() {
            $("#blocker").show();
        },
        hideLoader: function hideLoader() {
            $("#blocker").hide();
        },
        redirect: function redirect(url) {
            window.location.href = url;
        },
        reload: function reload() {
            window.location.reload(true);
        },
        displayResult: function displayResult(isSuccess, message, selector) {
            selector = selector || "#messageResult";
            var $result = $(selector);
            $result.toggleClass("alert-success", isSuccess);
            $result.toggleClass("alert-danger", !isSuccess);
            $result.html(message);
            $result.show();
        },
        hideResult: function hideResult(selector) {
            selector = selector || "#messageResult";
            var $result = $(selector);
            $result.hide();
        },
        getCookie: function getCookie(cname) {
            var name = cname + "=";
            var decodedCookie = decodeURIComponent(document.cookie);
            var ca = decodedCookie.split(';');
            var _iteratorNormalCompletion = true;
            var _didIteratorError = false;
            var _iteratorError = undefined;

            try {
                for (var _iterator = ca[Symbol.iterator](), _step; !(_iteratorNormalCompletion = (_step = _iterator.next()).done); _iteratorNormalCompletion = true) {
                    var c = _step.value;

                    while (c.charAt(0) == ' ') {
                        c = c.substring(1);
                    }
                    if (c.indexOf(name) == 0) {
                        return c.substring(name.length, c.length);
                    }
                }
            } catch (err) {
                _didIteratorError = true;
                _iteratorError = err;
            } finally {
                try {
                    if (!_iteratorNormalCompletion && _iterator['return']) {
                        _iterator['return']();
                    }
                } finally {
                    if (_didIteratorError) {
                        throw _iteratorError;
                    }
                }
            }

            return "";
        },

        emailValidationPattern: /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/,

        toggleErrorMessage: function toggleErrorMessage($container, isValid) {
            $container.toggleClass("has-error", !isValid);
            $container.find(".help-block").toggleClass("hide", isValid);
        },

        changeLanguage: function changeLanguage(selectedLanguage) {
            var hash = window.location.hash;
            var href = window.location.href;
            var actualUrl = !!hash ? href.substring(0, href.indexOf(hash)) : href;
            var newActualUrl = createNewUrlByLanguage(actualUrl, selectedLanguage);

            var url = newActualUrl + hash;

            _me.redirect(url);
        }
    };

    return _me;
})(jQuery);

Site.initialize();

