"use strict";

var PaymentRequest = (function ($) {
    function isValidKey(charCode) {
        return !(charCode > 31 && (charCode < 48 || charCode > 57)) || charCode === 46;
    }

    function buildMessage(message, isSuccess) {
        var className = isSuccess ? "alert-success" : "alert-danger";
        return '<p class="alert ' + className + '" role="alert">' + message + '</p>';
    }

    var _me = {
        initialize: function initialize() {
            _me.initEvent();
            $("#resultMessage").show();
        },

        initEvent: function initEvent() {
            $("#paymentRequest").on("keypress", "#Amount", function (event) {
                var charCode = event.which ? event.which : event.keyCode;
                return isValidKey(charCode);
            });

            $("#paymentRequest").on("input propertychange paste", "#Amount", function () {
                var reg = /^0+/gi;

                if (this.value.match(reg)) {
                    this.value = this.value.replace(reg, '');
                }

                if (Number(this.value) > 0) {
                    $("#btnRequestPayout").removeClass("disabled");
                } else {
                    $("#btnRequestPayout").addClass("disabled");
                }
            });

            $("#paymentRequest").on("submit", "#requestPayoutForm", function (e) {
                e.preventDefault();
                var isValid = _me.validatePayoutForm();
                if (isValid) {
                    _me.submitForm($(this), "CreatePaymentRequest");
                }
                return false;
            });

            $("#paymentRequest").on("submit", "#cancelPaymentForm", function (e) {
                e.preventDefault();
                _me.submitForm($(this), "CancelPaymentRequest");
                return false;
            });

            $('#paymentRequest').on("submit", "#paymentListForm", function (e) {
                e.preventDefault();
                _me.getDataByYear();
            });
        },

        validatePayoutForm: function validatePayoutForm() {
            var pattern = /^\d+(\.\d{1,2})?$/;
            var balance = parseFloat($("#BalBroughtForward").val());
            var minAmount = parseFloat($("#MinAmount").val());

            var amountString = $("#Amount").val();
            var isValidNumber = pattern.test(amountString);
            var amount = parseFloat($("#Amount").val());

            var canSubmit = isValidNumber && amount >= minAmount && amount <= balance;

            if (!canSubmit) {
                if (isNaN(amount) || !isValidNumber) {
                    $("#resultMessage").html(buildMessage(resourceLang.invalidAmount, false));
                } else {
                    $("#resultMessage").html(buildMessage(resourceLang.invalidRange, false));
                }
            }

            return canSubmit;
        },

        submitForm: function submitForm($form, action) {
            var postData = $form.serializeObject();
            $.ajax({
                type: "POST",
                url: Site.resolveRelativeUrl("UserPanel/PaymentRequest/" + action),
                data: postData,
                dataType: "json",
                cache: false,
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                success: function success(response) {
                    _me.refreshPage(response);
                },
                beforeSend: function beforeSend() {
                    Site.showLoader();
                },
                complete: function complete() {
                    Site.hideLoader();
                }
            });
        },

        refreshPage: function refreshPage(result) {
            $.ajax({
                type: "GET",
                url: Site.resolveRelativeUrl("UserPanel/PaymentRequest/PaymentRequestPartial"),
                dataType: "html",
                cache: false,
                success: function success(response) {
                    $("#paymentRequest").html(response);
                    $("#resultMessage").html(buildMessage(result.Message, result.IsOk)).show();
                }
            });
        },

        getDataByYear: function getDataByYear() {
            var year = $("#year").val();
            $.ajax({
                type: "POST",
                url: Site.resolveRelativeUrl("UserPanel/PaymentRequest/GetPaymentListByYear"),
                data: JSON.stringify({ "year": year }),
                dataType: "html",
                cache: false,
                contentType: "application/json; charset=UTF-8",
                success: function success(response) {
                    $("#payment-history").html(response);
                },
                beforeSend: function beforeSend() {
                    Site.showLoader();
                },
                complete: function complete() {
                    Site.hideLoader();
                }
            });
        }
    };

    return _me;
})(jQuery);

PaymentRequest.initialize();

