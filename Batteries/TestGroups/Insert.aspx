<%@ Page Title="New Test Group" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.TestGroups.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Test Group</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-2 control-label" Text="Test Group Name"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Test Group name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtGoal" CssClass="col-sm-2 control-label" Text="Test Group Goal"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtGoal" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtGoal"
                                        CssClass="text-danger" ErrorMessage="Test Group Goal is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Select project</label>
                                <div class="col-sm-10">
                                     <asp:HiddenField ID="HfProjectSelectedValue" runat="server" />
                                    <select id="DdlProject"  runat="server"  class="form-control select2" name="DdlProject">
                                    </select>
                                     <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlProject"
                                        CssClass="text-danger" ErrorMessage="Project name is required." Display="Dynamic" />
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

    <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>

    <script>
        <%--   reqUrl = "/Helpers/WebMethods.asmx/GetProjectsFromOwnRG";
       $('#<%= DdlProject.ClientID %>').select2({
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
                    var response = JSON.parse(data.d);
                    return {
                        results: $.map(response.results, function (item) {
                            return {
                                id: item.projectId,
                                text: item.projectName,
                                goal: item.projectDescription,
                                data: item
                            };
                        }),
                        pagination: {
                            "more": response.pagination.more
                        }
                    };
                },
            },
            dropdownParent: dialog,
            width: "100%",
            placeholder: 'Select project',
            escapeMarkup: function (markup) { return markup; }, 
            templateResult: formatRepo,
        });

        function formatRepo(repo) {
            var markup = "" + repo.text + ' | ' + repo.goal;
            return markup;
        }

        function formatRepoSelection(repo) {
            return repo.text;
        }--%>

        reqUrl = "/Helpers/WebMethods.asmx/GetProjectsForResearchGroup";
        $('#<%= DdlProject.ClientID %>').select2({
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
                                id: item.projectId,
                                text: item.projectAcronym,
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
            dropdownParent: dialog,
            width: "100%",
            //theme: "bootstrap",
            placeholder: 'Select project',
            allowClear: true,
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

        $('#<%= DdlProject.ClientID %>').on('change', function () {
            $('#<%=HfProjectSelectedValue.ClientID%>').val($(this).val());
        });



    </script>
</asp:Content>
