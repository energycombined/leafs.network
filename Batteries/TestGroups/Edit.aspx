<%@ Page Title="Edit Test Group" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Batteries.TestGroups.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Edit Test Group</h3>
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
                                <asp:Label runat="server" AssociatedControlID="TxtProject" CssClass="col-sm-2 control-label" Text="Project"></asp:Label>
                                <%--<label class="col-sm-2 control-label">Project</label>--%>
                                <div class="col-sm-10">
                                    <asp:TextBox Disabled="true" ID="TxtProject" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<select id="DdlProject" class="form-control select2" name="DdlProject">
                                    </select>--%>

                                    <%--</div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="BtnNewTestGroup" CausesValidation="false" Text="New Project" target="_blank" runat="server" CssClass="btn btn-default" href="/Projects/Insert" />
                                </div>--%>
                                </div>
                                <asp:TextBox ID="txtProjectId" runat="server" CssClass="form-control hidden"></asp:TextBox>
                                <%-- <div class="form-group">
                                <label class="col-sm-2 control-label">Select project</label>
                                <div class="col-sm-10">
                                     <asp:HiddenField ID="HfProjectSelectedValue" runat="server" />
                                    <select id="DdlProject"  runat="server"  class="form-control select2" name="DdlProject">
                                    </select>
                                     <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlProject"
                                        CssClass="text-danger" ErrorMessage="Project name is required." Display="Dynamic" />
                                </div>
                            </div>--%>
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
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Experiments in test group</h3>
                        <div class="pull-right">
                            <input type="button" id="addExperimentBtn" value="Add experiment to test group" class="btn btn-success" />
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover">
                                            <thead>
                                                <tr>
                                                    <th></th>
                                                    <th>System Label
                                                    </th>
                                                    <th>Personal Label
                                                    </th>
                                                    <th>Hypothesis
                                                    </th>
                                                    <th>Conclusion
                                                    </th>
                                                    <th>Created by
                                                    </th>
                                                    <th>Research group
                                                    </th>
                                                    <th>Date Created
                                                    </th>
                                                    <th>Date Added
                                                    </th>
                                                    <th>Action
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>--%>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/date-de.js"></script>--%>
        <script>
            reqUrl = "/Helpers/WebMethods.asmx/GetProjectsFromOwnRG";
           <%-- $('#<%= DdlProject.ClientID %>').select2({--%>
            $('#DdlProject').select2({
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
            }

           <%-- $('#<%= DdlProject.ClientID %>').on('change', function () {
            $('#<%=HfProjectSelectedValue.ClientID%>').val($(this).val());
            });--%>
        </script>

        <script type="text/javascript">
            var theTable;

            function CreateTestGroupTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 8,
                            "orderable": false
                        },
                        { "type": "de_datetime", targets: [6, 7] },
                    ],
                    "order": [[6, "desc"]]
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    /* var lastChange = res[i].lastChange ? res[i].lastChange : '#';*/

                    output += '<tr ' + 'id="' + 'testGroupExperiment' + res[i].testGroupExperimentId + '"' + '>' +
                        '<td>' +
                        '<div title="Remove" class="btn btn-danger btn-xs remove" onclick="OpenDeleteTestGroupExperimentForm(\'' + res[i].testGroupExperimentId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ' +
                        '</td>' +
                        '<td>' + res[i].experimentSystemLabel + '</td>' +
                        '<td>' + res[i].experimentPersonalLabel + '</td>' +
                        '<td>' + res[i].experimentHypothesis + '</td>' +
                        '<td>' + res[i].conclusion + '</td>' +
                        '<td>' + res[i].experimentOperatorUsername + '</td>' +
                        '<td>' + res[i].experimentResearchGroupName + '</td>' +
                        '<td>' + res[i].dateCreatedExperiment + '</td>' +
                        '<td>' + res[i].dateCreated + '</td>' +
                        '<td class="table-actions-col">' +
                                '<a class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/View/") %>' + res[i].fkExperiment + '">View</a> ' +
                                //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Edit/") %>' + res[i].experimentId + '">Edit</a> ' +
                                //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Delete/") %>' + res[i].experimentId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateTestGroupTable();
            }

            function GetTestGroupData(testGroupId) {
                var newRequestObj = new Object();
                newRequestObj.testGroupId = testGroupId;
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllExperimentsInTestGroup",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTableData(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }


            function OpenDeleteTestGroupExperimentForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.testGroupExperimentId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Remove experiment from test group",
                    message: "Are you sure you want to remove experiment from test group?",
                    buttons: {
                        cancel: {
                            label: 'Cancel'
                        },
                        confirm: {
                            label: 'Confirm'
                        }
                    },
                    callback: function (result) {
                        if (result) {
                            $.ajax({
                                type: "POST",
                                url: "/Helpers/WebMethods.asmx/DeleteTestGroupExperiment",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "info");
                                        $("#testGroupExperiment" + elementId).remove();
                                    } else {
                                        notify(jsonResult.message, "warning");
                                    }
                                },
                                error: function (p1, p2, p3) {
                                    notify(p2 + " - " + p3, "danger");
                                }
                            });
                        }
                    }
                });
            }


            $(function () {
                var testGroupId = <%= testGroupId%>;
                var projectId = <%= projectId %>;
                var researchGroupId = null;

                CreateTestGroupTable();
                GetTestGroupData(testGroupId);


                $('#addExperimentBtn').click(function () {
                    var html = '<div class="form-horizontal">';
                    //var html = '<div class="row">';
                    html += '<div class="form-group">';
                    html += '<div class="col-md-9">';
                    html += '<select id="selectExperiment" class="batch-data-ajax">    </select>';
                    html += '</div>';
                    html += '<div class="col-md-3">';
                    html += '<a id="viewExperimentLink" class="btn btn-default disabled" target="_blank" href="<%= Page.ResolveUrl("~/Experiments/View/") %>' + 1 + '">View Details</a> ';
                    html += '</div>';
                    html += '</div>';
                    //html += '</div>';

                    html += '<div class="form-group">';
                    html += '<div class="col-md-9">';
                    html += '<input type="text" class="form-control" name="hypothesis" placeholder="Hypothesis within test group" value="" />';
                    html += '</div>';
                    html += '</div>';
                    html += '</div>';

                    //html += '<div class="row">';                    
                    //html += '<div class="form-group">';
                    //    html += '<div class="col-md-9">';
                    //        html += '<input type="text" class="form-control" name="hypothesis" value="" />';
                    //    html += '</div>';
                    //html += '</div>';
                    //html += '</div>';


                    var dialog = bootbox.dialog({
                        title: 'Add experiment to this test group',
                        message: html,
                        buttons: {
                            cancel: {
                                label: "Cancel",
                                className: 'btn-default',
                                callback: function () {

                                }
                            },
                            ok: {
                                label: "Add",
                                className: 'btn-info',
                                callback: function () {
                                    var selectedData = $('#selectExperiment').select2('data')[0].data;

                                    //submitTestGroupExperiment(selectedData);
                                    var jsonRequestObject = {};
                                    jsonRequestObject["fkTestGroup"] = testGroupId;
                                    jsonRequestObject["fkExperiment"] = selectedData.experimentId;
                                    jsonRequestObject["experimentHypothesis"] = $("input[name='hypothesis']").val();
                                    //console.log(testGroupExperimentRequest);

                                    reqUrl = "/Helpers/WebMethods.asmx/SubmitTestGroupExperiment";
                                    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });

                                    $.ajax({
                                        type: "POST",
                                        url: reqUrl,
                                        data: RequestDataString,
                                        //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (result) {
                                            var jsonResult = JSON.parse(result.d);
                                            if (jsonResult.status == "ok") {
                                                GetTestGroupData(testGroupId);
                                                notify("Success", "info");
                                                //window.location.reload();
                                                //window.location.replace("<%= Page.ResolveUrl("~/TestGroups/Edit/") %>" + testGroupId);
                                            }
                                            else {
                                                notify(jsonResult.message, "warning");
                                            }
                                        },
                                        error: function (p1, p2, p3) {
                                            alert(p1.status.toString() + " " + p3.toString());
                                        }
                                    });

                                    return true;
                                }
                            }
                        },
                        onEscape: true
                    });
                    dialog.on('shown.bs.modal', function () {
                        $("#selectExperiment").focus();
                    });

                    reqUrlExperiments = "/Helpers/WebMethods.asmx/GetAllExperimentsOutsideTestGroupFromProject";
                    $('#selectExperiment').select2({
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
                                    testGroupId: testGroupId,
                                    //projectId: projectId,
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
                                            text: item.experimentSystemLabel + " | " + item.experimentPersonalLabel,
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
                        dropdownParent: dialog,
                        width: "100%",
                        //theme: "bootstrap",
                        placeholder: 'Search through experiments',
                        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                        //minimumInputLength: 1,
                        templateResult: formatExperiment,
                        //templateSelection: formatRepoSelection
                    });
                    $('#selectExperiment').change(function () {
                        var selected = $('#selectExperiment').select2("data");
                        var selectedExperiment = selected[0].data;

                        $("#viewExperimentLink").attr("href", "<%= Page.ResolveUrl("~/Experiments/View/") %>" + selectedExperiment.experimentId);
                        $("#viewExperimentLink").removeClass("disabled");
                    });

                });

                function formatExperiment(e) {
                    //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                    var markup = e.text;
                    return markup;
                }
            });
            //});


        </script>
    </section>
</asp:Content>
