<%@ Page Title="Edit Material" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Batteries.Materials.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Edit Material</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">                            
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-2 control-label" Text="Material Name (in text)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Material name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtChemicalFormula" CssClass="col-sm-2 control-label" Text="ChemicalFormula"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtChemicalFormula" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtChemicalFormula"
                                        CssClass="text-danger" ErrorMessage="Material chemical formula is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtMaterialLabel" CssClass="col-sm-2 control-label" Text="Label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtMaterialLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterialType" CssClass="col-sm-2 control-label" Text="Material Type"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMaterialType" runat="server" ItemType="Batteries.Models.MaterialType" DataTextField="MaterialType" DataValueField="MaterialTypeId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialType"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlStoredInType" CssClass="col-sm-2 control-label" Text="Stored In"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlStoredInType" runat="server" ItemType="Batteries.Models.StoredInType" DataTextField="storedInType" DataValueField="storedInTypeId" AppendDataBoundItems="true" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlStoredInType"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterialFunction" CssClass="col-sm-2 control-label" Text="Function in experiments"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMaterialFunction" runat="server" AppendDataBoundItems="true" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialFunction"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPercentageOfActive" CssClass="col-sm-2 control-label" Text="Percentage of active"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPercentageOfActive" runat="server" CssClass="form-control" type="text" disabled="true"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPercentageOfActive"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtPercentageOfActive"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                    <asp:RangeValidator ControlToValidate="TxtPercentageOfActive" MinimumValue="0" MaximumValue="100" Type="Double" CssClass="text-danger" ErrorMessage="The value must be from 0 to 100." runat="server"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtDescription" CssClass="col-sm-2 control-label" Text="Description"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMeasurementUnit" CssClass="col-sm-2 control-label" Text="Measurement Unit"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMeasurementUnit" runat="server" ItemType="Batteries.Models.MeasurementUnit" DataTextField="measurementUnitName" DataValueField="measurementUnitId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMeasurementUnit"
                                        CssClass="text-danger" ErrorMessage="Measurement unit is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPrice" CssClass="col-sm-2 control-label" Text="Price (€) per unit"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPrice" runat="server" CssClass="form-control" type="text"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPrice"
                                        CssClass="text-danger" ErrorMessage="Price is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtPrice"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlVendor" CssClass="col-sm-2 control-label" Text="Vendor"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlVendor" runat="server" ItemType="Batteries.Models.Vendor" DataTextField="vendorName" DataValueField="vendorId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlVendor"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtDateBought" CssClass="col-sm-2 control-label" Text="Date Bought"></asp:Label>--%>
                                <label class="col-sm-2 control-label">Date Bought</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDateBought" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtDateBought"
                                        CssClass="text-danger" ErrorMessage="Date Bought is required." Display="Dynamic" />
                                    <%--<input ID="DateBought" type="text" class="form-control datepicker">--%>
                                    <%--<asp:RangeValidator runat="server" ControlToValidate="TxtDateBought" ErrorMessage="Invalid Date" Type="Date" MinimumValue="01/01/1900" MaximumValue="<%= DateTime.Now.ToShortDateString() %>" Display="Dynamic"></asp:RangeValidator>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtBulkPrice" CssClass="col-sm-2 control-label" Text="Bulk Price (€/kg)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtBulkPrice" runat="server" CssClass="form-control" type="text"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtBulkPrice"
                                        CssClass="text-danger" ErrorMessage="Bulk price is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtBulkPrice"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtReference" CssClass="col-sm-2 control-label" Text="Reference"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtReference" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtCasNumber" CssClass="col-sm-2 control-label" Text="CAS Number"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtCasNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtLotNumber" CssClass="col-sm-2 control-label" Text="LOT Number"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtLotNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            
                            

                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <%--<div class="btn btn-xs btn-default" onclick="ShowFileAttachments(\'' + Material + '\',  ' + stepNumber + ')">Documents</div>--%>
                                    <div class="btn btn-xs btn-default" onclick="ShowFileAttachments('Material',  <%= materialId %> )">Documents</div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="UpdateButton" Text="Save" CssClass="btn btn-primary" OnClick="UpdateButton_OnClick" />
                                <asp:Button runat="server" ID="CancelButton" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="CancelButton_OnClick" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>

        <script>
            $(function () {
                $('.select2').select2({
                    width: "100%",
                    placeholder: 'Select',
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('.datepicker').datepicker({
                    format: 'dd/mm/yyyy'
                });

                
                
                $('#<%= DdlMaterialFunction.ClientID %>').change(function () {
                    var selectedFunctionId = $('#<%= DdlMaterialFunction.ClientID %>').val();

                    if (selectedFunctionId == "1" || selectedFunctionId == "5")
                        $('#<%= TxtPercentageOfActive.ClientID %>').removeAttr("disabled");
                    else {
                        $('#<%= TxtPercentageOfActive.ClientID %>').val("");
                        $('#<%= TxtPercentageOfActive.ClientID %>').attr("disabled", "true");
                    }
                });

                $('#<%= DdlMaterialFunction.ClientID %>').trigger('change');
                <%--var materialFunctionId = $('#<%= DdlMaterialFunction.ClientID %>').val();
                if (materialFunctionId == "1")
                    $('#<%= TxtPercentageOfActive.ClientID %>').removeAttr("disabled");--%>
            });
        </script>
    </section>
</asp:Content>
