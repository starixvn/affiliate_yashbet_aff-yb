function _affChangeMediaText() {
    var _affscript = document.createElement('script');
    _affscript.type = 'text/javascript';
    _affscript.id = '_affScript';
    _affscript.src = '{url}{affid}&targetId={targetId}';
    var _affcross = document.getElementsByTagName('script')[0];
    var _affscriptExist = document.getElementById('_affScript');
    if (_affscriptExist != null) {
        _affcross.parentNode.replaceChild(_affscript, _affcross);
    }
    else {
        _affcross.parentNode.insertBefore(_affscript, _affcross);
    }
}

_affChangeMediaText();
var _afftimer = self.setInterval("_affChangeMediaText()", {requesttime});