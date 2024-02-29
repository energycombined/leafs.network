<%@ Page Title="New Stock Material amount" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Stock.Materials.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert Stock</h3>
                        <%--<legend>New Material</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <%--<asp:Label ID="ErrorLabel" runat="server" Text="" CssClass="alert alert-dismissable alert-danger" Visible="false"></asp:Label>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterialName" CssClass="col-sm-2 control-label" Text="Material Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMaterialName" runat="server" ItemType="Batteries.Models.MaterialName" DataTextField="materialName" DataValueField="materialId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialName"
                                        CssClass="text-danger" ErrorMessage="Please select a material." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAmount" CssClass="col-sm-2 control-label" Text="Amount"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control" ></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAmount"
                                        CssClass="text-danger" ErrorMessage="Amount is required." Display="Dynamic" />
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtAmount"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                    <%--"^-?(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$"--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtMeasurementUnit" CssClass="col-sm-2 control-label" Text="Measurement Unit"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtMeasurementUnit" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAvailableQuantity" CssClass="col-sm-2 control-label" Text="Available Quantity"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtAvailableQuantity" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAmount"
                                        CssClass="text-danger" ErrorMessage="Amount is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="RbTransactionDirection" CssClass="col-sm-2 control-label" Text="Transaction Type"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:RadioButtonList ID="RbTransactionDirection" runat="server" CssClass="minimal">
                                        <asp:ListItem Text="Addition" Value="1" Selected="True" />
                                        <asp:ListItem Text="Subtraction" Value="-1" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="RbTransactionDirection" CssClass="col-sm-2 control-label" Text="Transaction Type"></asp:Label>
                                <div class="col-sm-10">
                                    <label for="baz[1]">
                                        <input type="radio" name="quux[2]" id="baz[1]" checked>
                                        Bar
                                    </label>
                                    <label for="baz[2]">
                                        <input type="radio" name="quux[2]" id="baz[2]">
                                        Bar
                                    </label>
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtDateBought" CssClass="col-sm-2 control-label" Text="Date Bought"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDateBought" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    <%--<asp:RangeValidator runat="server" ControlToValidate="TxtDateBought" ErrorMessage="Invalid Date" Type="Date" MinimumValue="01/01/1900" MaximumValue="<%= DateTime.Now.ToShortDateString() %>" Display="Dynamic"></asp:RangeValidator>--%>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtDateBought"
                                        CssClass="text-danger" ErrorMessage="Date Bought is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlVendor" CssClass="col-sm-2 control-label" Text="Vendor"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="DdlVendor" runat="server" ItemType="Batteries.Models.Vendor" DataTextField="vendorName" DataValueField="vendorId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-2">
                                    <asp:LinkButton ID="BtnNewVendor" CausesValidation="false" Text="Add new" target="_blank" runat="server" CssClass="btn btn-default" href="/Vendors/Insert"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <div class="btn btn-xs btn-default" disabled="true" data-toggle="tooltip" title="Adding documents is available after save!">Documents</div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="BtnInsert" Text="Save" CssClass="btn btn-primary" OnClick="BtnInsert_Click" />
                                <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <script>
            function RefreshMaterialInfo(data) {
                var res = JSON.parse(data);
                var availableQuantity = res[0].availableQuantity != null ? res[0].availableQuantity : '0';
                $('#<%= TxtAvailableQuantity.ClientID %>').val(availableQuantity);
                $('#<%= TxtMeasurementUnit.ClientID %>').val(res[0].measurementUnitSymbol);
            }

            function FillMaterialInfo() {
                var selectedMaterial = $('#<%= DdlMaterialName.ClientID %>').val();

                if (selectedMaterial) {
                    var newRequestObj = new Object();
                    newRequestObj.materialType = null;
                    newRequestObj.materialId = $('#<%= DdlMaterialName.ClientID %>').val();
                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetAllMaterialsList",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //alert(result.d);
                            RefreshMaterialInfo(result.d);
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }
                else {
                    $('#<%= TxtAvailableQuantity.ClientID %>').val("Select a material to see available quantity");
                    $('#<%= TxtMeasurementUnit.ClientID %>').val("");
                }
            }

            $(function () {
                $('input[type="checkbox"], input[type="radio"]').iCheck({
                    checkboxClass: 'icheckbox_minimal-blue',
                    radioClass: 'iradio_minimal-blue'
                });

                <%-- $('#<%= ddlMaterialType.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });--%>

                FillMaterialInfo();
                $('#<%= DdlMaterialName.ClientID %>').change(function () {
                    FillMaterialInfo();
                });

            });
        </script>

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
            });
        </script>

    </section>
</asp:Content>
