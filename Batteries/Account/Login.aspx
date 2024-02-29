<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Blank.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Batteries.Account.Login" Async="true" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<link href="<%= ResolveUrl("~/AdminLTE/plugins/iCheck/square/blue.css") %>" rel="stylesheet" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-box">
        <div class="login-logo">
            <%--<a href="../../index2.html"><b>LEAFS</b>|Login</a>--%>
            <img src="<%= ResolveUrl("~/img/logo_white.png") %>" height="300" alt="Logo" />
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body">

            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
                <p class="text-success"><%: SuccessMessage %></p>
            </asp:PlaceHolder>

            <div class="form-group has-feedback">
                <asp:TextBox runat="server" placeholder="Username" ID="Username" CssClass="form-control" TextMode="SingleLine" />
                <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                    CssClass="text-danger" ErrorMessage="The username is required" Display="Dynamic" />
            </div>
            <div class="form-group has-feedback">
                <asp:TextBox runat="server" placeholder="Password" ID="Password" TextMode="Password" CssClass="form-control" />
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password is required" Display="Dynamic" />
            </div>
            <div class="row">
                <div class="col-xs-8">
                    <div class="checkbox icheck ">
                        <label>
                            <input type="checkbox">
                            Remember Me
                        </label>
                    </div>
                </div>
                <!-- /.col -->
                <div class="col-xs-4">
                    <asp:Button runat="server" OnClick="LogIn" Text="Sign In" CssClass="btn btn-default btn-block btn-flat loginBtn" />
                </div>
                <!-- /.col -->
            </div>

            <a href="<%=ResolveUrl("~/Account/Forgot.aspx") %>">I forgot my password</a><br>
            <!--<a href="register.html" class="text-center">Register a new membership</a>-->

        </div>
        <!-- /.login-box-body -->
    </div>
    <!-- /.login-box -->

    <script src="<%= ResolveUrl("~/AdminLTE/plugins/iCheck/icheck.min.js") %>"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-aero',
                radioClass: 'iradio_square-aero',
                increaseArea: '20%' // optional
            });
        });
    </script>
</asp:Content>
