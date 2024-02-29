<%@ Page Title="Edit process name" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Batteries.ProcessTypes.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Edit Process Type</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtProcessType" CssClass="col-sm-2 control-label" Text="Process Type"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtProcessType" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtProcessType"
                                        CssClass="text-danger" ErrorMessage="Process type is required." Display="Dynamic" />
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
    </section>
</asp:Content>
