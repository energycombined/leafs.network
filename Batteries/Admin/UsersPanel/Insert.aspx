<%@ Page Title="New User" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Admin.UsersPanel.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert User</h3>
                        <%--<legend>New User</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <asp:Label ID="ErrorLabel" runat="server" Text="" CssClass="alert alert-dismissable alert-danger" Visible="false"></asp:Label>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlRoles" CssClass="col-sm-2 control-label" Text="Role"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlRoles" runat="server" ItemType="Batteries.Models.Role" DataTextField="RoleName" DataValueField="RoleId" CssClass="form-control select2Custom">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtUsername" CssClass="col-sm-2 control-label" Text="Username"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtUsername"
                                        CssClass="text-danger" ErrorMessage="The username is requred." Display="Dynamic" />
                                    <asp:RegularExpressionValidator runat="server" ValidationExpression="[A-Za-z][A-Za-z0-9._]{5,19}" ControlToValidate="TxtUsername" CssClass="text-danger" ErrorMessage="The username should be at least 6 and max 20 characters. Allowed characters are A-Z 0-9 . and _" Display="Dynamic"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-sm-2 control-label" Text="Password"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPassword"
                                        CssClass="text-danger" ErrorMessage="The password is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPassword" CssClass="col-sm-2 control-label" Text="Confirm Password"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtConfirmPassword" CssClass="form-control" TextMode="Password" runat="server"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator1" Display="Dynamic" runat="server" ControlToValidate="TxtPassword" ControlToCompare="TxtConfirmPassword" ValueToCompare="TxtPassword" Operator="Equal" CssClass="text-danger" ErrorMessage="The passwords do not match!"></asp:CompareValidator>
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
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlResearchGroup" CssClass="col-sm-2 control-label" Text="Research Group"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:HiddenField ID="HfResearchGroupSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlResearchGroup" runat="server" ItemType="Batteries.Models.ResearchGroup" DataTextField="ResearchGroupName" DataValueField="ResearchGroupId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlResearchGroup"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
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

            var reqUrl = "/Helpers/WebMethods.asmx/GetResearchGroups";
            $('#<%= DdlResearchGroup.ClientID %>').select2({
                ajax: {
                    type: "POST",
                    url: reqUrl,
                    dataType: 'json',
                    contentType: "application/json; charset=utf-8",
                    delay: 250,
                    data: function (params) {
                        var term = "";
                        if (params.term) {
                            var term = params.term;
                        }

                        return JSON.stringify({
                            search: term,
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.researchGroupId,
                                    text: item.acronym + " - " + item.researchGroupName,
                                    data: item
                                };
                            }),
                            pagination: {
                                "more": response.pagination.more
                            }
                        };
                    },
                    //cache: true
                },
                //dropdownParent: dialog,
                width: "100%",
                //theme: "bootstrap",
                placeholder: 'Select',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });

            function formatRepo(repo) {
                var markup = "" + repo.text;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }




            $('#<%= DdlResearchGroup.ClientID %>').on('change', function () {
                $('#<%=HfResearchGroupSelectedValue.ClientID%>').val($(this).val());
            });
        </script>
        <script type="text/javascript">
            $(function () {
                $('.select2Custom').select2({
                    width: "100%",
                    placeholder: 'Select',
                    //allowClear: true
                    //theme: "bootstrap"
                });
            });
        </script>
    </section>
</asp:Content>
