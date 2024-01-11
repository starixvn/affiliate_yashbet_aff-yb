var AffB2CTesting = (function ($) {
    function getUrl(actionName) {
        var rootAction = $("#rootAction").val();
        var format = $('input[name=Format]:checked').val() || "cshtml";
        var language = $('input[name=Language]:checked').val() || "en-US";
        var functionName = $('input[name=Function]:checked').val();
        return rootAction + "Test/" + actionName + "?language=" + language + "&function=" + functionName + "&format=" + format;
    }

    return {
        initialize: function () {
            AffB2CTesting.initEvent();
        },

        initEvent: function () {
            $("#btnPreview").on("click", function () {
                $("#resultContent").html("");
                
                var url = getUrl("Preview");

                $.ajax({
                    type: "GET",
                    dataType: "html",
                    cache: false,
                    url: url,
                    success: function (response) {
                        $("#resultContent").html(response);
                    }
                });
            });

            $("#btnSendMail").on("click", function () {
                $("#resultContent").html("");
                var emailAddress = $("#emailAddress").val();
                if (!emailAddress) {
                    $("#resultContent").html("Please enter email address");
                    return;
                }

                var url = getUrl("SendEmail");
                url += "&EmailAddress=" + emailAddress;

                $.ajax({
                    type: "POST",
                    dataType: "json",
                    cache: false,
                    url: url,
                    success: function (response) {
                        if (response.Success) {
                            $("#resultContent").html("Send Successfully");
                        }
                        else {
                            var message = "Send Failed<br />" + response.Message;
                            $("#resultContent").html(message);
                        }
                    }
                });
            });
        }
    };
})(jQuery);

AffB2CTesting.initialize();