<%@ Page Title="Upload test document" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadOld.aspx.cs" Inherits="Batteries.GraphResults.Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Upload test results document</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="FuDoc" CssClass="col-sm-2 control-label" Text="Test results data"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:FileUpload ID="FuDoc" runat="server"></asp:FileUpload>
                                    <asp:RequiredFieldValidator CssClass="text-danger" ErrorMessage="Document upload is required" ControlToValidate="FuDoc" runat="server" Display="Dynamic" />
                                    <%--<a href="#" class="btn btn-link" id="btnDownloadDoc" target="_blank" runat="server" visible="false">download</a>--%>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="BtnInsert" Text="Upload" CssClass="btn btn-primary" OnClick="BtnInsert_Click" />
                                <%--<asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />--%>
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
