"use strict";

var ReCheckIn = (function ($) {
    var _timeInterval = 30000;
    var _handler;
    var _url = Site.resolveRelativeUrl("Auth/ReCheckin");

    var _me = {
        initialize: function initialize() {
            _handler = setTimeout(_me.reCheckin, _timeInterval);
        },

        reCheckin: function reCheckin() {
            $.ajax({
                url: _url,
                async: true,
                type: "GET",
                dataType: "json",
                cache: false,
                success: function success(result) {
                    if (result.IsKickOut) {
                        _me.kickout(result.Message);
                    }
                    if (result.ErrCode > 0) {
                        window.location.href = Site.resolveRelativeUrl("");
                    }

                    clearTimeout(_handler);
                    _handler = setTimeout(_me.reCheckin, _timeInterval);
                }
            });
        },

        kickout: function kickout(message) {
            clearTimeout(_handler);
            alert($("<div/>").html(message).text());

            setTimeout(function () {
                window.location.href = Site.resolveRelativeUrl("");
            }, 500);
        }
    };

    return _me;
})(jQuery);

ReCheckIn.initialize();

