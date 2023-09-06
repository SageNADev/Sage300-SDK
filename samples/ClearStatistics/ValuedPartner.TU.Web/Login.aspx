<%-- Copyright (c) 1994-2022 Sage Software, Inc.  All rights reserved. --%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ValuedPartner.TU.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
	    <link href="Assets/styles/css/default.min.css" rel="stylesheet" />
	    <link href="Assets/styles/css/stand-alone.css" rel="stylesheet" />
    </head>
    <body>
        <form id="form1" runat="server">
            <asp:Panel ID="p" runat="server" DefaultButton="LoginButton">
		        <div class="Sage_logo">
			        <img src="~/Assets/images/login/sage300-logo-brilliant-green.png" height="75" alt="Sage 300" runat="server" />
		        </div>
		        <section class="header login" id="sectionLogin">
			        <div class="wrapper">
				        <div class="main-body login">
					        <h1 class="page-header">Sign in to Sage 300</h1>
                            <div class="version error-message"><asp:Label ID="ErrorLabel" runat="server" Text="" CssClass="login auto-style2" Style="color: #C7384F"></asp:Label></div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="UserLabel"  runat="server" Text="User Name"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="UserText" runat="server" CssClass="login txt-upper auto-style2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="PwdLabel" runat="server" Text="User Password"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="PwdText" TextMode="Password" runat="server" CssClass="login auto-style2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="CompanyLabel"  runat="server" Text="Company Database"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="CompanyText" runat="server" style='text-transform:uppercase' CssClass="txt-upper login auto-style2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="SystemLabel" runat="server" Text="System Database "></asp:Label>
                                    </label>
                                    <asp:TextBox ID="SystemText" runat="server" CssClass="txt-upper login auto-style2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="VersionLabel" runat="server" Text="Version"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="VersionText" runat="server" CssClass="txt-upper login auto-style2"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="input-group">
                                    <label class="label-login">
                                        <asp:Label ID="DateLabel" runat="server" Text="Session Date"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="SessionDateText" runat="server" CssClass="txt-upper login auto-style2"></asp:TextBox>
                                    <asp:Label ID="DateLabelInfo" CssClass="inline-txt" runat="server" Text="(YYYYMMDD only)"></asp:Label>
                                </div>
                            </div>
                            <div class="button-group">
                                <asp:Button ID="LoginButton" runat="server" CssClass="btn btn-primary btn-login" Text="Sign In" OnClick="LoginButton_Click" />
                            </div>
				        </div>
                    </div>
		        </section>
            </asp:Panel>
        </form>
    </body>
</html>
