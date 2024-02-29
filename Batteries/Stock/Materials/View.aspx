<%@ Page Title="Stock Transaction Info" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Batteries.Stock.Materials.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Stock Entry Info</h3>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="materialGeneralInfo" class="table table-hover">
                                <tbody>
                                    <tr>
                                        <td>Stock Transaction Id
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblStockTransactionId" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Material Name
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMaterialName" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Amount
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblAmount" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Operator(Added by)
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblOperator" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Transaction Type
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblTransactionType" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Chemical Formula
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblChemicalFormula" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Measurement Unit
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMeasurementUnit" CssClass="col-sm-4 control-label" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Documents
                                        </td>
                                        <td>
                                            <div class="btn btn-xs btn-default" onclick="ShowFileAttachments('StockTransaction',  <%= stockTransactionId %> )">Documents</div>
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
