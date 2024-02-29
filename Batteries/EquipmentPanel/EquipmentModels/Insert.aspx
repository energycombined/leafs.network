<%@ Page Title="New equipment model" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.EquipmentPanel.EquipmentModels.Insert" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Equipment Model</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlEquipment" CssClass="col-sm-2 control-label" Text="Equipment"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlEquipment" runat="server" ItemType="Batteries.Models.Equipment" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlEquipment"
                                        CssClass="text-danger" ErrorMessage="Please select an equipment." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtEquipmentModelName" CssClass="col-sm-2 control-label" Text="Model Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtEquipmentModelName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEquipmentModelName"
                                        CssClass="text-danger" ErrorMessage="Equipment model name is required." Display="Dynamic" />
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
            $(function () {
                $('.select2').select2({
                    width: "100%",
                    placeholder: 'Select',
                    allowClear: true
                    //theme: "bootstrap"
                });
            });
        </script>
    </section>
</asp:Content>
