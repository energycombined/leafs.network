<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Public.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headerwrap-background">
        <div class="headerwrap-overlay">
            <div id="headerwrap-new">
                <div class="container">
                    <div class="row centered">
                        <div class="col-lg-12">
                            <%--<h1>Welcome to 
                                <b>
                                    <a href="#">ECF</a>
                                </b>
                            </h1>--%>
                            <img id="about" class="rotate_01" src="<%= ResolveUrl("~/img/logo_white_notext2.png") %>"/><br /><br />
                            <img src="<%= ResolveUrl("~/img/text2.png") %>" alt="Logo"/><br />
                            <%--<h3>Please
                                sign in to continue
                            </h3>--%>
                            <h3><a href="<%= ResolveUrl("~/Account/Login") %>" class="btn btn-lg btn-default loginBtn">Sign in</a></h3>
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
            </div>
            <!--/ #headerwrap -->
        </div>
    </div>

    <div id="headerwrap">
        <div class="container">
            <div class="row centered">
                <div class="col-lg-12">                    
                    <%--<h1><a href="#">Discover</a> amazing people!
                        <b>
                            <!-- <a href="https://github.com/acacha/adminlte-laravel">adminlte-laravel</a> -->
                        </b>
                    </h1>
                    <h3>
                    If you are a <a href="#">tour guide</a>, or if you are looking for one, <a href="#">this</a> is the right place to be!
                    <a href="#">Create an account</a> and travel safe!
                       
                    </h3>--%>
                    <h3>
                        <%--<a href="{{ url('/register') }}" class="btn btn-lg btn-success">s</a>--%>
                    </h3>
                </div>                
            </div>
        </div> <!--/ .container -->
    </div><!--/ #headerwrap -->
</asp:Content>
