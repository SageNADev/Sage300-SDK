<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="ValuedPartner.TU.Web.WebForms.ReportViewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=9,chrome=1" />
    <script src="../Scripts/jquery-1.11.3.js"></script>
    <script src="../Scripts/JQueryUI/iframeResizer.js"></script>
    <script src="../Scripts/Report/ReportBehaviour.js"></script>

    <style>
        #CrystalReportViewerSage300__UI, #CrystalReportViewerSage300 {
            margin: 0 auto;
        }

        .CrystalReportViewerWraper {
            overflow-x: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="errorLabel"></asp:Label>

        <div class="CrystalReportViewerWraper">
            <CR:CrystalReportViewer ID="CrystalReportViewerSage300" PrintMode="Pdf" runat="server" AutoDataBind="true" EnableDatabaseLogonPrompt="False" ToolPanelView="None" EnableParameterPrompt="False" ReuseParameterValuesOnRefresh="True" />
        </div>
        <asp:HiddenField ID="hiddenToken" runat="server"></asp:HiddenField>
        <div></div>
    </form>
</body>
</html>
