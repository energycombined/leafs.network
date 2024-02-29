<%@ Page Title="View Prefab type Component" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Batteries.PrefabTypes.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">View Prefab type of component</h3>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="materialGeneralInfo" class="table table-hover">
                                <tbody>
                                    <tr>
                                        <td>Prefab type
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblComponentType" CssClass="col-sm-2 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Prefab type Name
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblName" CssClass="col-sm-2 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Prefab type Model
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblModel" CssClass="col-sm-2 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td> Documents list
                                        </td>
                                        <td>
                                            <div class="btn btn-xs btn-default" onclick="ShowFileAttachments('CommercialTypeComponent',  <%= commercialComponentId %> )">Documents</div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="CancelButton" Text="Back to list" CausesValidation="false" CssClass="btn btn-default" OnClick="CancelButton_OnClick" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
            <!-- /.box -->
        </div>

        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>
    </section>
</asp:Content>
