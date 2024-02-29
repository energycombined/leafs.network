<%@ Page Title="New experiment" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Experiments.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">

        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Experiment General Info</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                               <input type="text" style="position: absolute; top: -1000px;" />
                                <asp:Label runat="server" AssociatedControlID="DdlProject" CssClass="col-sm-2 control-label" Text="Project"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfProjectSelectedValue" runat="server" />
                                    <%--  <asp:DropDownList ID="DdlProject" runat="server" ItemType="" AutoPostBack="true" OnSelectedIndexChanged="DdlProject_SelectedIndexChanged1" CssClass="form-control">--%>
                                    <asp:DropDownList ID="DdlProject" name="DdlProject" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlProject"
                                        CssClass="text-danger" ErrorMessage="Please assign experiment to a project." Display="Dynamic" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="BtnNewProject" CausesValidation="false" Text="New Project" target="_blank" runat="server" CssClass="btn btn-default" href="/Projects/Insert" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTestGroup" CssClass="col-sm-2 control-label" Text="Test Group"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfTestGroupSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlTestGroup" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlTestGroup"
                                        CssClass="text-danger" ErrorMessage="Please assign experiment to a test group." Display="Dynamic" />
                                </div>
                                <div class="col-sm-3">
                                    <%--<asp:LinkButton ID="LinkButton1" CausesValidation="false" Text="New Test Group" runat="server" CssClass="btn btn-default" OnClick="BtnNewTestGroup_Click"/>--%>
                                    <asp:LinkButton ID="BtnNewTestGroup" CausesValidation="false" Text="New Test Group" target="_blank" runat="server" CssClass="btn btn-default" href="/TestGroups/Insert" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtExperimentPersonalLabel" CssClass="col-sm-2 control-label" Text="Personal label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtExperimentPersonalLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtExperimentPersonalLabel"
                                        CssClass="text-danger" ErrorMessage="Personal label is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtExperimentDescription" CssClass="col-sm-2 control-label" Text="Description"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtExperimentDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtHypothesis" CssClass="col-sm-2 control-label" Text="Hypothesis"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtHypothesis" runat="server" CssClass="form-control" placeholder="Hypothesis within the selected test group"></asp:TextBox>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtTemplate" CssClass="col-sm-2 control-label" Text="Template"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtTemplate" ReadOnly="true" runat="server" CssClass="form-control" placeholder="No previous experiment template selected"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTemplate" CssClass="col-sm-2 control-label" Text="Template"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfDdlTemplateSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlTemplate" runat="server" ItemType="" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <a id="viewExperimentLink" target="_blank" class="btn btn-default disabled" href="<%= Page.ResolveUrl("~/Experiments/View/") %>">View Details</a>
                                    <%--<asp:LinkButton id="viewExperimentLink" class="btn btn-default disabled" Text="View Details" runat="server" />--%>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />
                            <div class="pull-right">
                                <%--<asp:Button runat="server" ID="BtnSelectTemplate" Text="Select Experiment Template" CssClass="btn btn-default" OnClick="BtnSelectTemplate_Click" />--%>
                                <%--<asp:Button runat="server" ID="Button1" Text="Start Fresh" CssClass="btn btn-success" OnClick="BtnStartFresh_Click" />--%>
                                <asp:Button runat="server" ID="BtnStart" Text="Start Experiment" CssClass="btn btn-lg btn-primary" OnClick="BtnStart_Click" />

                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script>
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //experiment.js
        </script>
        <script>
            $(function () {
                FillPreselectedProject();
                FillPreselectedTestGroup();
            });
            function FillPreselectedProject() {
                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetLastUsedProject",
                    data: JSON.stringify({}),
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var res = JSON.parse(result.d);
                        if (res != null) {
                            var text = res.projectAcronym;
                            var id = res.projectId;
                            var option = new Option(text, id, true, true);
                            $('#<%= DdlProject.ClientID %>').append(option).trigger('change.select2');
                            $('#<%=HfProjectSelectedValue.ClientID%>').val(id);
                        }                     
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });

            }
            function FillPreselectedTestGroup() {
                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetLastUsedTestGroup",
                    data: JSON.stringify({}),
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var res = JSON.parse(result.d);
                            var text = res.testGroupName;
                            var id = res.testGroupId;
                            var option = new Option(text, id, true, true);
                        $('#<%= DdlTestGroup.ClientID %>').append(option).trigger('change.select2');
                        $('#<%=HfTestGroupSelectedValue.ClientID%>').val(id);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });

            }
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
                                    goal: item.projectDescription,
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
                //var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }
            //$('#DdlProject').change(function () {
            //    var selectedMaterial = $('#DdlProject').val();

            //    if (selectedMaterial) {
            //        selectedMaterialItem = $('#DdlProject').select2('data')[0].data;
            //        FillMaterialInfo(selectedMaterialItem);
            //    }
            //    else {
            //        ClearMaterialInfo();
            //    }
            //});

            $('#<%= DdlProject.ClientID %>').on('change', function () {
                $('#<%=HfProjectSelectedValue.ClientID%>').val($(this).val());
            });

            reqUrl = "/Helpers/WebMethods.asmx/GetTestGroupsFromProject";
            $('#<%= DdlTestGroup.ClientID %>').select2({
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
                            page: params.page || 1,
                            projectId: $('#<%=HfProjectSelectedValue.ClientID%>').val()
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
                                    id: item.testGroupId,
                                    text: item.testGroupName,
                                    goal: item.testGroupGoal,
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
                placeholder: 'Select test group',
                allowClear: true,
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });
            $('#<%= DdlTestGroup.ClientID %>').on('change', function () {
                 $('#<%=HfTestGroupSelectedValue.ClientID%>').val($(this).val());
             });
           <%-- reqUrl = "/Helpers/WebMethods.asmx/GetTestGroupsFromProject";
            $('#<%= DdlTestGroup.ClientID %>').select2({
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
                            page: params.page || 1,
                            projectId: $('#<%=HfProjectSelectedValue.ClientID%>').val()
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
                                    id: item.testGroupId,
                                    text: item.testGroupName,
                                    goal: item.testGroupGoal,
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
                placeholder: 'Select test group',
                allowClear: true,
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });--%>

            $('#<%= DdlProject.ClientID %>').on('change', function () {
                $('#<%= DdlTestGroup.ClientID %>').val("").trigger('change');
            });

            <%--$('#<%= DdlTestGroup.ClientID %>').select2({
                allowClear: true,
                placeholder: '-Test Group-',
            });--%> 

            var projectId = null;
            //var researchGroupId = null;
            var testGroupId = null;
            var operatorId = null;

            reqUrlExperiments = "/Helpers/WebMethods.asmx/GetCompleteExperimentsPaged";
            $('#<%= DdlTemplate.ClientID %>').select2({
                ajax: {
                    type: "POST",
                    url: reqUrlExperiments,
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
                            projectId: projectId,
                            operatorId: operatorId,
                            testGroupId: testGroupId,
                            type: 'public',
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.experimentId,
                                    text: item.experimentSystemLabel + " | " + item.projectName + " | " + item.experimentPersonalLabel + " | " + item.operatorUsername + " | " + item.dateCreated,
                                    data: item
                                };
                            }),
                            pagination: {
                                "more": response.pagination.more
                            }
                        };
                    },
                    cache: true
                },
                //dropdownParent: dialog,
                width: "100%",
                allowClear: true,
                //theme: "bootstrap",
                placeholder: 'No template selected',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatExperiment,
                //templateSelection: formatRepoSelection
            });
            function formatExperiment(e) {
                //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                var markup = e.text;
                return markup;
            }

            $('#<%= DdlTemplate.ClientID %>').change(function () {
                //var selected = $('#selectExperiment').select2("data");
                var selected = $('#<%= DdlTemplate.ClientID %>').val();
                $('#<%=HfDdlTemplateSelectedValue.ClientID%>').val($(this).val());

                if (selected != null) {
                    //$("#viewExperimentLink")
                    $("#viewExperimentLink").attr("href", "<%= Page.ResolveUrl("~/Experiments/View/") %>" + selected);
                    $("#viewExperimentLink").removeClass("disabled");
                }
                else {
                    $("#viewExperimentLink").addClass("disabled");
                }
            });

           
        </script>
    </section>
</asp:Content>
