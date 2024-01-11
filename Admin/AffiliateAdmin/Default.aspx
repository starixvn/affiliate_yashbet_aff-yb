<%@ Page Language="C#" AutoEventWireup="true" Inherits="_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html
    PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Affiliate Admin</title>
    <link runat="server" id="IconICO" rel="shortcut icon" />
    <![if ! IE]>
    <link runat="server" id="IconPNG" rel="shortcut icon" />
    <![endif]>

    <style>
        html,
        body {
            height: 100%;
            margin: 0;
        }

        #fMain {
            height: calc(100% - 80px)
        }
    </style>
</head>

<body>
    <iframe id="fMenu" name="fMenu" frameborder="0" noresize="noresize" scrolling="no" src="Header.aspx" height="60px"
        width="100%"></iframe>
    <iframe id="fMain" name="fMain" noresize="noresize" frameborder="0" scrolling="auto" src="Home/Home.aspx"
        width="100%"></iframe>
    <iframe id="fFooter" name="fFooter" frameborder="0" noresize="noresize" scrolling="no" src="Footer.htm"
        height="20px" width="100%"></iframe>
</body>

</html>