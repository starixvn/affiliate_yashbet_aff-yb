var AuthHelper = (function ($) {
    function encrypt(plainText, key) {
        var encrypt = new JSEncrypt();
        encrypt.setPublicKey(key);

        return encrypt.encrypt(plainText);
    }

    function base64DecodeUnicode(str) {
        return decodeURIComponent(Array.prototype.map.call(atob(str), function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)
        }).join(''))
    }

    function getUtcUnixTimeStamp() {
        var now = new Date();
        return Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(),
              now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
    }

    var _me = {
        encrypt: encrypt,
        base64DecodeUnicode: base64DecodeUnicode,
        getUtcUnixTimeStamp: getUtcUnixTimeStamp
    }

    return _me;
})(jQuery);