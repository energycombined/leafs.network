<%@ Page Title="New commercial type" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.PrefabTypes.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Prefab Type <%=componentType %></h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-2 control-label" Text="Prefab Type Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Prefab type name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtModel" CssClass="col-sm-2 control-label" Text="Prefab Type Model"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtModel" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtModel"
                                        CssClass="text-danger" ErrorMessage="Component model is required." Display="Dynamic" />--%>
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
                                <asp:Button runat="server" ID="BtnSaveAndEdit" Text="Save and Edit" data-toggle="tooltip" title="Save and edit immediately" CssClass="btn btn-info" OnClick="BtnInsert_Click" />
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
