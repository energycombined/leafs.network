<%@ Page Title="Edit User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Batteries.Admin.UsersPanel.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <asp:Label ID="ErrorLabel" runat="server" Text="" CssClass="alert alert-dismissable alert-danger" Visible="false"></asp:Label>
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Edit User</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <asp:ValidationSummary runat="server" CssClass="alert alert-danger" />
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtUsername" CssClass="col-sm-2 control-label" Text="Username"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtUsername" runat="server" CssClass="form-control" ReadOnly="True"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtFirstname" CssClass="col-sm-2 control-label" Text="Firstname"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtFirstname" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtFirstname"
                                        CssClass="text-danger" ErrorMessage="The firstname is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtLastname" CssClass="col-sm-2 control-label" Text="Lastname"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtLastname" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtLastname"
                                        CssClass="text-danger" ErrorMessage="The lastname is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPhone" CssClass="col-sm-2 control-label" Text="Tel. Number"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPhone" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPhone"
                                        CssClass="text-danger" ErrorMessage="Telephone number is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtEmail" CssClass="col-sm-2 control-label" Text="Email"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEmail"
                                        CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" />
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="CbActive" CssClass="col-sm-2 control-label" Text="Active"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:CheckBox ID="CbActive" runat="server" />
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtRGroup" CssClass="col-sm-2 control-label" Text="Research Group"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox Disabled="true" ID="TxtRGroup" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:DropDownList ID="DdlResearchGroup" runat="server" ItemType="Batteries.Models.ResearchGroup" DataTextField="ResearchGroupName" DataValueField="ResearchGroupId" CssClass="form-control select2">
                                    </asp:DropDownList>--%>
                                   <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlResearchGroup"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />--%>
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
        <script type="text/javascript">
            $(function () {
                $('.select2').select2({
                    width: "100%",
                    placeholder: 'Select',
                    //allowClear: true
                    //theme: "bootstrap"
                });                
            });
        </script>
    </section>
</asp:Content>
