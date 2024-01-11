'use strict';

var PageTracking = (function ($) {
    var _me = {
        addTrackingForMediaPage: function addTrackingForMediaPage() {
            if (window.gtag) {
                var shortedLangCode = $('#shortedLangCode').val();
                var trackingAction = 'AffNewSeasonMedia-' + shortedLangCode;
                window.gtag('event', trackingAction, {
                    'event_category': 'AFFiliatePage',
                    'event_label': 'AFFBigBanner'
                });
            }
        },
        addTrackingForSignUpPage: function addTrackingForSignUpPage() {
            if (window.gtag) {
                var shortedLangCode = $('#shortedLangCode').val();
                var actionName = 'AffJoinJuly40Com-' + shortedLangCode;
                window.gtag('event', actionName, {
                    'event_category': 'AFFiliatePage',
                    'event_label': 'AFFBigBanner'
                });
            }
        }
    };

    return _me;
})(jQuery);

