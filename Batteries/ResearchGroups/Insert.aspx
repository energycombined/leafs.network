<%@ Page Title="New Research Group" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.ResearchGroups.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Research Group</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtResearchGroupName" CssClass="col-sm-2 control-label" Text="Research Group Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtResearchGroupName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtResearchGroupName"
                                        CssClass="text-danger" ErrorMessage="Research group name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAcronym" CssClass="col-sm-2 control-label" Text="Research Group Acronym"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtAcronym" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAcronym"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                    *Cannot be changed later
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
