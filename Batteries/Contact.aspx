<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Batteries.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <%--<div class="row">
            <img src="<%= ResolveUrl("~/img/logo_white.png") %>" height="200" alt="Logo" />
        </div>--%>
        <div class="row centered">
            <div class="col-lg-12">
                <%--<h1>Welcome to 
                                <b>
                                    <a href="#">LEAFS</a>
                                </b>
                            </h1>--%>

                <%--<h3>Please
                                sign in to continue
                            </h3>--%>
                <img src="<%= ResolveUrl("~/img/logo_white.png") %>" height="300" alt="Logo" />

                <%--<h2><%: Title %></h2>--%>
                <br />
                <br />
                <br />
                <div class="col-md-12 text-white">
                    <strong>LEAFS is a software network, focussed on battery research, to prevent others from making the same mistakes.<br />
                        It is used by several laboratory to organize their material flow, experiment design and results.
                    </strong>
                    <br />
                    If you want to learn more, please contact: 
                    <address>
                        <%--<strong>Support:</strong>--%>
                        <a class="text-white" href="mailto:info@leafs.network ">info@leafs.network </a>
                        <br />
                    </address>
                </div>
                <%--<address>
                    One Microsoft Way<br />
                    Redmond, WA 98052-6399<br />
                    <abbr title="Phone">P:</abbr>
                    425.555.0100
                </address>--%>
            </div>
            <div class="col-lg-2">
            </div>
            <div class="col-lg-8">

                <div class="col-lg-2">
                </div>
            </div>
        </div>
        <!--/ .container -->
    </div>
</asp:Content>
