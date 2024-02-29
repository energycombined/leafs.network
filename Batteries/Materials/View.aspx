<%@ Page Title="View Material" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Batteries.Materials.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">View Material</h3>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="materialGeneralInfo" class="table table-hover">
                                <tbody>
                                    <tr>
                                        <td>Material Name
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMaterialName" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Chemical Formula
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblChemicalFormula" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Label
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMaterialLabel" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>CAS Number
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblCasNumber" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>LOT Number
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblLotNumber" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Material Type
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMaterialType" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Stored In
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblStoredInType" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Function in experiments
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblFunctionInExperiment" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Percentage of active
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblPercentageOfActive" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Description
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblDescription" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Available quantity
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblAmount" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Measurement Unit
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblMeasurementUnit" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Price (€) per unit
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblPrice" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Bulk Price (€/kg) 
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblBulkPrice" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Date Bought
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblDateBought" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Vendor
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="LblVendorName" CssClass="" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Vendor Website
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="HlVendorSite"
                                                NavigateUrl="#"
                                                Text=""
                                                runat="server" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Material Documents
                                        </td>
                                        <td>
                                            <div class="btn btn-xs btn-default" onclick="ShowFileAttachments('Material',  <%= materialId %> )">Documents</div>
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

        <script>
            sharedViewMode = <%=isFromOtherResearchGroup.ToString().ToLower()%>;
        </script>
    </section>
</asp:Content>
