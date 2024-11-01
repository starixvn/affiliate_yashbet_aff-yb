<%@ Page Language="C#" EnableSessionState="False" %>
<%@ Import Namespace="Fanex.Affiliate.Admin.Core.Constants" %>
<%@ Import Namespace="Fanex.Affiliate.Admin.Core.Extensions" %>

<html>
<head runat="server">
    <title>Who Am I?</title>
</head>
<body>
    <div>
        Web Name: <%= Request.ServerVariables["Server_Name"] %>
    </div>
    <div class="port">
        Port: <%= Request.Url.Port %>
    </div>
    <div class="backend">
        Server IP: <%= Request.ServerVariables["LOCAL_ADDR"]%>
    </div>
    <div class="servertime">
        Server Time: <%= DateTime.Now%>
    </div>
    <div class="haproxy">
        Client IP: <%= Request.UserHostAddress%>
    </div> 
    <div>
        Actual Client IP: <%= Request.GetClientIP()%>
    </div>
    <div>
        HTTP_VIA: <%= Request.ServerVariables["HTTP_VIA"]%>
    </div>
    <div>
        HTTP_PROXY_CONNECTION: <%= Request.ServerVariables["HTTP_PROXY_CONNECTION"]%>
    </div>
    <br />
    <div>
         HTTP_X_HEADERS: -----------------------------
    </div>
    <%  var allKeys = Request.ServerVariables.AllKeys;
        foreach (var key in allKeys)
        {
            if (key.Contains(HeaderKey.HTTP_X))
            {%>
                <div>
                    <%= key%>: <%= Request.ServerVariables[key]%>
                </div>
            <%}
        }%>
</body>
</html>