<%-- Copyright (c) 1994-2021 Sage Software, Inc.  All rights reserved. --%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ValuedPartner.TU.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="Sage_logo">
                <img src="~/Assets/images/login/sage300-logo-sq.png" height="132" width="134" alt="Sage 300" runat="server" style="padding-left: 80px; padding-top: 15px"/>
            </div>
           <h1 style="font-family:Arial; color:#555">Sign In to Sage 300</h1>  
           <asp:Panel ID="p" runat="server" DefaultButton="LoginButton">
                <table class="auto-style1">  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="UserLabel" style="font-family:Arial; color:#555" runat="server" Text="User Name "></asp:Label></td>  
                        <td>  
                            <asp:TextBox ID="UserText" runat="server" style='text-transform:uppercase' CssClass="auto-style2"></asp:TextBox></td>  
                    </tr>  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="PwdLabel" style="font-family:Arial; color:#555" runat="server" Text="User Password "></asp:Label></td>  
                        <td>  
                            <asp:TextBox ID="PwdText" TextMode="Password" runat="server" style='text-transform:uppercase' CssClass="auto-style2"></asp:TextBox></td>  
                    </tr>  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="CompanyLabel" style="font-family:Arial; color:#555" runat="server" Text="Company Database "></asp:Label></td>  
                        <td>  
                            <asp:TextBox ID="CompanyText" runat="server" style='text-transform:uppercase' CssClass="auto-style2"></asp:TextBox></td>  
                    </tr>  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="SystemLabel" style="font-family:Arial; color:#555" runat="server" Text="System Database "></asp:Label></td>  
                        <td>  
                            <asp:TextBox ID="SystemText" runat="server" style='text-transform:uppercase' CssClass="auto-style2"></asp:TextBox></td>  
                    </tr>  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="VersionLabel" style="font-family:Arial; color:#555" runat="server" Text="Version "></asp:Label></td>  
                        <td>  
                            <asp:TextBox ID="VersionText" runat="server" style='text-transform:uppercase' CssClass="auto-style2"></asp:TextBox></td>  
                    </tr>  
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="DateLabel" Style="font-family: Arial; color: #555" runat="server" Text="Session Date "></asp:Label></td>
                        <td>
                            <asp:TextBox ID="SessionDateText" runat="server" Style='text-transform: uppercase' CssClass="auto-style2"></asp:TextBox></td>
                        <td class="auto-style3">
                            <asp:Label ID="DateLabelInfo" Style="font-family: Arial; color: #555" runat="server" Text="(YYYYMMDD only) "></asp:Label></td>
                    </tr>  
                </table>  
                <p>  
                    <asp:Button ID="LoginButton" runat="server" Text="Login" style="padding-left: 50px; padding-right: 50px; padding-top: 5px; padding-bottom: 5px; margin-left: 80px; font-family:Arial; color:#555; background-color:#0077c8; color:white" OnClick="LoginButton_Click" />  
                </p>  
                <table class="auto-style1">  
                    <tr>  
                        <td class="auto-style3">  
                            <asp:Label ID="ErrorLabel" style="font-family:Arial; color:red" runat="server" Text=""></asp:Label></td>
                    </tr>  
                </table>  
            </asp:Panel>
        </div>    
    </form>
</body>
</html>
