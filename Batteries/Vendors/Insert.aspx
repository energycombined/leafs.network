<%@ Page Title="Insert Vendor" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Vendors.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Vendor</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtVendorName" CssClass="col-sm-2 control-label" Text="Vendor Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtVendorName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtVendorName"
                                        CssClass="text-danger" ErrorMessage="Vendor Name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtVendorSite" CssClass="col-sm-2 control-label" Text="Vendor Website"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtVendorSite" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtContactPerson" CssClass="col-sm-2 control-label" Text="Contact person(s)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtContactPerson" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPhoneNumber" CssClass="col-sm-2 control-label" Text="Phone number(s)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPhoneNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtComment" CssClass="col-sm-2 control-label" Text="Comment"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtComment" runat="server" CssClass="form-control"></asp:TextBox>
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
    </section>
</asp:Content>
