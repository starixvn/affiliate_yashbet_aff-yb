var PlayerTransferBalance = (function ($) {

    let fomatter = Intl.NumberFormat('en-US', {
        style: 'decimal',
        maximumFractionDigits: 2
    })

    let _curSearchName = "";
    let _paymentAccountExRate = 0;
    let _playerExRate = 0;
    let _baseCurrency = "";
    let _transferAmountLabel = "";
    let _playerCurrencyLabel = "";
    let _paymentAccLabel = "";

    var _me = {
        initialize: function () {
            _me.initEvent();
            _transferAmountLabel = $('#transfer-amount-label').text();
            _playerCurrencyLabel = $('#player-currency-label').text();
            _paymentAccLabel = $('#payment-acc-label').text();
        },

        initEvent: function () {
            $('#btn-search').on('click', function () {
                _me.serchPlayer();
            });

            $('#playerUserName').on('keypress', function (e) {
                if (e.which === 13) {
                    _me.serchPlayer();
                }
            })

            $('#btn-submit').on('click', function () {
                Site.hideResult();
                const confirmTxt = $('#confirm-transfer-text').val();
                if (confirm(confirmTxt)) {
                    let param = _me.buildParam();
                    _me.submitTransfer(param);
                }
            })

            $('#amount').on('input', function (e) {
                let amount = $(this).val();
                let baseAmount = amount * _playerExRate;
                $('#exchange-base-currency').text('~' + _baseCurrency + " " + fomatter.format(baseAmount));
            })

            $('#amount').on('keypress', function (e) {
                if (e.which < 48 || e.which > 57) {
                    e.preventDefault();
                }
            })

            $('#amount').on('paste', function (e) {
                let paste = (event.clipboardData || window.clipboardData).getData('text');
                var regex = /^[0-9]*$/;
                if (!regex.test(paste)) {
                    e.preventDefault();
                }
            })
        },

        serchPlayer: function (callback) {
            Site.hideResult();
            let playerName = $('#playerUserName').val();
            _curSearchName = playerName;
            _me.getPlayerInfo(playerName, callback);
        },

        getPlayerInfo: function (playerName, callback) {
            $.ajax({
                type: "POST",
                dataType: "json",
                cache: false,
                data: JSON.stringify({ playerName }),
                url: Site.resolveRelativeUrl("UserPanel/PlayerTransferBalance/SearchPlayer"),
                contentType: "application/json; charset=UTF-8",

                success: function (response) {
                    _me.showSearchResult(response);
                    callback && callback();
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },

        showSearchResult: function (response) {
            if (response.IsOk) {
                let data = response.Data;
                _baseCurrency = data.PaymentAccountCurrency;
                _paymentAccountExRate = data.PaymentAccountExRate;
                _playerExRate = data.PlayerExRate;
                let availBal = data.Balance * _paymentAccountExRate;
                let transferLimit = Math.min(data.RemainTransferLimit, data.Balance);
                if (transferLimit < 0) {
                    transferLimit = 0;
                }

                $('.transfer-info').removeClass('hidden');
                $('#transfer-amount-label').text(_transferAmountLabel.replace('{player}', _curSearchName));
                $('#player-currency-label').text(_playerCurrencyLabel.replace('{player}', _curSearchName));
                $('#payment-acc-label').text(_paymentAccLabel.replace('{account}', data.PaymentAccountName));

                $('#btn-submit').removeClass('disabled');

                $('#player-currency').text(data.PlayerCurrency);
                $('#avail-bal').text(data.PlayerCurrency + " " + fomatter.format(availBal)  );
                $('#avail-bal-base-currency').text(data.PaymentAccountCurrency + " " + fomatter.format(data.Balance) );
                $('#transfer-limit').text(data.PlayerCurrency + " " + fomatter.format(transferLimit * _paymentAccountExRate));
                $('#transfer-limit-base-currency').text(data.PaymentAccountCurrency + " " + fomatter.format(transferLimit));
                $('#exchange-player-currency').text(data.PlayerCurrency);
                $('#exchange-base-currency').text('~' + _baseCurrency + " 0");
                $('#amount').val('');
            }
            else {
                Site.displayResult(response.IsOk, response.Message);
                $('.transfer-info').addClass('hidden');
                $('#btn-submit').addClass('disabled');
            }
        },

        buildParam: function () {
            return {
                submitName: $('#playerUserName').val(),
                searchName: _curSearchName,
                amount: $('#amount').val(),
                paymentAccountExRate: _paymentAccountExRate,
                playerExRate: _playerExRate
            }
        },

        submitTransfer: function (param) {
            $.ajax({
                type: "POST",
                dataType: "json",
                cache: false,
                data: JSON.stringify(param),
                url: Site.resolveRelativeUrl("UserPanel/PlayerTransferBalance/SubmitTransferMoney"),
                contentType: "application/json; charset=UTF-8",

                success: function (response) {
                    if (response.IsOk) {
                        _me.serchPlayer(() => Site.displayResult(response.IsOk, response.Message));
                    }
                    else {
                        Site.displayResult(response.IsOk, response.Message);
                    }
                },
                beforeSend: function () {
                    Site.showLoader();
                },
                complete: function () {
                    Site.hideLoader();
                }
            });
        },
    };

    return _me;
})(jQuery)

PlayerTransferBalance.initialize();