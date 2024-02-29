<%@ Page Title="Insert New Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Projects.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Project</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-4 control-label" Text="Project Title:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Project title is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAcronym" CssClass="col-sm-4 control-label" Text="Acronym:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtAcronym" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAcronym"
                                        CssClass="text-danger" ErrorMessage="Acronym is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAdminCoor" CssClass="col-sm-4 control-label" Text="Administrative Coordinator (Organisation):"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtAdminCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminCoor"
                                        CssClass="text-danger" ErrorMessage="Administrative Coordinator is required." Display="Dynamic" />--%>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAdminContact" CssClass="col-sm-4 control-label" Text="Administrative Coordinator, contact person:"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtAdminContact" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminContact"
                                        CssClass="text-danger" ErrorMessage="Administrative Coordinator, contact person is required." Display="Dynamic" />--%>
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                        <asp:TextBox ID="TxtAdminContactMail" type="email" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminContactMail"
                                            CssClass="text-danger" ErrorMessage="Administrative Coordinator, contact person email is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                            </div>

                            <%--  <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAdminContactMail" CssClass="col-sm-4 control-label" Text="Administrative Coordinator, contact person email:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtAdminContactMail" type="email" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminContactMail"
                                        CssClass="text-danger" ErrorMessage="Administrative Coordinator, contact person email is required." Display="Dynamic" />
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtTechCoor" CssClass="col-sm-4 control-label" Text="Technical Coordinator (Organisation):"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtTechCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoor"
                                        CssClass="text-danger" ErrorMessage="Technical Coordinator is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtTechCoorContact" CssClass="col-sm-4 control-label" Text="Technical Coordinator, contact person:"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtTechCoorContact" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoorContact"
                                        CssClass="text-danger" ErrorMessage="Technical Coordinator, contact person is required." Display="Dynamic" />--%>
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                        <asp:TextBox ID="TxtTechCoorMail" type="email" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoorMail"
                                            CssClass="text-danger" ErrorMessage="Technical Coordinator, contact person mail is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                            </div>
                            <%--   <div class="form-group">
                                <asp:Label runat="server"  AssociatedControlID="TxtTechCoorMail" CssClass="col-sm-4 control-label" Text="Technical Coordinator, contact person mail:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtTechCoorMail" type="email" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoorMail"
                                        CssClass="text-danger" ErrorMessage="Technical Coordinator, contact person mail is required." Display="Dynamic" />
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtInnManager" CssClass="col-sm-4 control-label" Text="Innovation manager:"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtInnManager" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-phone-square"></i></span>
                                        <asp:TextBox ID="TxtInnManagerContact" runat="server" CssClass="form-control" placeholder="Contact person"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <%-- <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtInnManagerContact" CssClass="col-sm-4 control-label" Text="Innovation manager, contact person:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtInnManagerContact" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtDissCoor" CssClass="col-sm-4 control-label" Text="Dissemination Coordinator:"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="TxtDissCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-phone-square"></i></span>
                                        <asp:TextBox ID="TxtDissCoorContact" runat="server" CssClass="form-control" placeholder="Contact person"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtDissCoorContact" CssClass="col-sm-4 control-label" Text="Dissemination Coordinator, contact person:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtDissCoorContact" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtGrantFund" CssClass="col-sm-4 control-label" Text="Grant funding organisation: "></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtGrantFund" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Grant funding organisation is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtFundProg" CssClass="col-sm-4 control-label" Text="Funding programme:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtFundProg" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Funding programme is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtCallIden" CssClass="col-sm-4 control-label" Text="Call identifier:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtCallIden" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCallIden"
                                        CssClass="text-danger" ErrorMessage="Call identifier is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtCallTop" CssClass="col-sm-4 control-label" Text="Call Topic:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtCallTop" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCallTop"
                                        CssClass="text-danger" ErrorMessage="Call Topic is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlFixedKey" CssClass="col-sm-4 control-label" Text="Fixed Keywords:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:DropDownList ID="DdlFixedKey" runat="server" CssClass="form-control select2">
                                        <asp:ListItem>Sodium-ion battery</asp:ListItem>
                                        <asp:ListItem>Lithium-ion battery</asp:ListItem>
                                        <asp:ListItem>Flow-battery</asp:ListItem>
                                        <asp:ListItem>PEM fuel cell</asp:ListItem>
                                        <asp:ListItem>Hydrogen storage </asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlFixedKey"
                                        CssClass="text-danger" ErrorMessage="Fixed Keywords is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtFreeKey" CssClass="col-sm-4 control-label" Text="Free Keywords:"></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtFreeKey" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtFreeKey"
                                        CssClass="text-danger" ErrorMessage="Free Keywords is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-4 control-label">Start of the project: </label>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="TxtStartDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtStartDate"
                                            CssClass="text-danger" ErrorMessage="Start of the project is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <label class="col-sm-2 control-label">Duration-End Date:</label>
                                <div class="col-sm-3">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="TxtEndDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEndDate"
                                            CssClass="text-danger" ErrorMessage="End date of the project is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtGoal" CssClass="col-sm-4 control-label" Text="Short summary "></asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtGoal" MaxLength='1999' onkeyDown="checkTextAreaMaxLength(this,event,'1999');" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtGoal"
                                        CssClass="text-danger" ErrorMessage="Project Description is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <%-- <div class="form-group">
                                <label class="col-sm-4 control-label">Duration in months (end date)</label>
                                <div class="col-sm-2">
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                        <asp:TextBox ID="TxtEndDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEndDate"
                                            CssClass="text-danger" ErrorMessage="End date of the project is required." Display="Dynamic" />
                                    </div>
                                   <%-- <asp:TextBox ID="TxtEndDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEndDate"
                                        CssClass="text-danger" ErrorMessage="End date." Display="Dynamic" />
                                </div>
                            </div>--%>
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
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script>

            $('.datepicker').datepicker({
                format: 'dd/mm/yyyy'
            });

            function formatRepo(repo) {
                var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }
        </script>
        <script>
            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }
            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>


        <script>
            //$(document).ready(function () {
            //    $('.chosen-select').chosen();
            //    //$('#BtnInsert').click(function () {
            //    //    $(".chosen-select").val('').trigger("chosen:updated");
            //    //})
            //});            
        </script>
    </section>
</asp:Content>
