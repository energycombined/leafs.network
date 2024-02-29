<%@ Page Title="Experiment results" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Results.Experiments.Insert1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css")%>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Experiment Results</h2>
        <hr />

        <div class="row">
            <div class="col-md-10 col-lg-10">
                <div class="box box-solid box-success">
                    <div class="box-header with-border">
                        <h3 class="box-title">Experiment Summary</h3>
                        <div class="pull-right box-tools">
                            <%--<button class="btn btn-success btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                            <button class="btn btn-success btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-minus"></i></button>
                            <%--<button class="btn btn-success btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="experimentGeneralInfo" class="table table-hover">
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <%--<asp:LinkButton runat="server" ID="ViewExperimentButton" Text="View Experiment" CssClass="btn btn-success"  />--%>
                                <%--<asp:HyperLink runat="server" NavigateUrl="/Experiments/View/<%= experimentId %>" CssClass="btn btn-primary" Text="View Experiment" />--%>
                                <%--<asp:HyperLink runat="server" NavigateUrl=<%= Page.ResolveUrl("~/Experiments/View/") %> CssClass="btn btn-primary" Text="View Experiment" />--%>

                                <%--<a class="btn btn-primary" href="<%= Page.ResolveUrl("~/Experiments/View/" + experimentId) %>"> View experiment 2 </a>--%>
                                <asp:HyperLink runat="server" ID="ViewExperimentLink" NavigateUrl='<%# String.Format("/Experiments/View/{0}", experimentId) %>' CssClass="btn btn-primary" Text="View Experiment" />


                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>
        <div class="row">
            <div class="col-md-10 col-lg-10">
                <div class="box box-solid box-danger collapsed-box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Materials List</h3>
                        <div class="pull-right box-tools">
                            <%--<button class="btn btn-danger btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                            <button class="btn btn-danger btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-plus"></i></button>
                            <%--<button class="btn btn-danger btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="experimentSummaryMaterials" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Component</th>
                                        <th>Name</th>
                                        <th>Chemical Formula</th>
                                        <th>Weight</th>
                                        <th>Measurement Unit</th>
                                        <th>Function In Experiment</th>
                                        <th>Percentage Of Active</th>
                                        <th>Measured Time</th>
                                        <th>Measured Width</th>
                                        <th>Measured Length</th>
                                        <th>Measured Conductivity</th>
                                        <th>Measured Thickness</th>
                                        <th>Measured Weight</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div class="row">
            <div class="col-md-10 col-lg-10">
                <div class="box box-solid box-primary collapsed-box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Processes List</h3>
                        <div class="pull-right box-tools">
                            <%--<button class="btn btn-primary btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                            <button class="btn btn-primary btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-plus"></i></button>
                            <%--<button class="btn btn-primary btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="experimentSummaryProcesses" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Component</th>
                                        <th>Process</th>
                                        <%--<th>Subcategory</th>--%>
                                        <th>Equipment</th>
                                        <th>Model</th>
                                        <th>Brand</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div class="row">
            <div class="col-md-10 col-lg-10">
                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Tests Done</h3>
                        <div class="pull-right">
                            <%--<input type="button" id="addTestBtn" onclick="OpenInsertTestForm()" value="Insert test" class="btn btn-success" />--%>
                            <input type="button" id="addTestBtn" onclick="GoToGraphs()" value="Results viewer" class="btn btn-success" />
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div id="example2_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listTestsDataTable" class="table table-bordered table-hover table-striped" style="width:100%">
                                            <thead>
                                                <tr>                                                    
                                                    <th>Test
                                                    </th>
                                                    <th>Test subcategory
                                                    </th>
                                                    <th>Battery Component
                                                    </th>
                                                    <th>Step
                                                    </th>
                                                    <th>Test label
                                                    </th>
                                                    <th>Comment
                                                    </th>
                                                    <th>Date Added
                                                    </th>
                                                    <th>Operator
                                                    </th>
                                                    <th>Research Group
                                                    </th>
                                                    <th>Action
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                             <tfoot style="font-weight: 700;">
                                                <tr>
                                                    <th>Test
                                                    </th>
                                                    <th>Test subcategory
                                                    </th>
                                                    <th>Battery Component
                                                    </th>
                                                    <th>Step
                                                    </th>
                                                    <th>Test label
                                                    </th>
                                                    <th>Comment
                                                    </th>
                                                    <th>Date Added
                                                    </th>
                                                    <th>Operator
                                                    </th>
                                                    <th>Research Group
                                                    </th>
                                                    <th>Action
                                                    </th>
                                                </tr>
                                            </tfoot>
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

        <div class="row">
            <div class="col-md-10 col-lg-10">
                <div class="box">
                    <div class="box-header">
                        <h3 class="box-title">Test Groups</h3>
                        <div class="pull-right">
                            <input type="button" id="addTestGroupBtn" value="Assign to another test group" onclick="OpenAddTestGroupForm()" class="btn btn-success" />
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
                                                    <th>Test Group
                                                    </th>
                                                    <th>Test Group Goal
                                                    </th>
                                                    <th>Experiment hypothesis
                                                    </th>
                                                    <th>Conclusion
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

        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/date-de.js"></script>--%>

        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>

        <script>
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //experiment.js

            var testGroupTable;

            //Test group table
            function CreateTestGroupTable() {

                // DataTable
                testGroupTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 4,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [3] },
                    ],
                    //"order": [[6, "desc"]]
                });
            }

            function RefreshTestGroupTableData(data) {

                testGroupTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    var lastChange = res[i].lastChange ? res[i].lastChange : '#';
                    var item = JSON.stringify(res[i]);
                    //console.log(JSON.stringify(res[i]));
                    output += '<tr ' + 'id="' + 'testGroupExperiment' + res[i].testGroupExperimentId + '"' + '>' +
                        '<td>' +
                        '<div title="Remove" class="btn btn-danger btn-xs remove" onclick="OpenDeleteTestGroupExperimentForm(\'' + res[i].testGroupExperimentId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ' +
                        '</td>' +
                        '<td>' + res[i].testGroupName + '</td>' +
                        '<td>' + res[i].testGroupGoal + '</td>' +
                        //'<td>' + res[i].experimentSystemLabel + '</td>' +
                        //'<td>' + res[i].experimentPersonalLabel + '</td>' +
                        '<td>' + res[i].experimentHypothesis + '</td>' +
                        '<td>' + res[i].conclusion + '</td>' +
                        //'<td>' + res[i].operatorUsername + '</td>' +
                        //'<td>' + res[i].researchGroupName + '</td>' +
                        //'<td>' + res[i].dateCreated + '</td>' +
                        //'<td>' + lastChange + '</td>' +
                        '<td class="table-actions-col">' +
                                '<a class="btn btn-success btn-xs" target="_blank" href="<%= Page.ResolveUrl("~/TestGroups/Edit/") %>' + res[i].fkTestGroup + '">Test Group Contents</a> ' +
                                //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Edit/") %>' + res[i].experimentId + '">Edit</a> ' +
                                //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Delete/") %>' + res[i].experimentId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                        //'<div class="btn btn-info btn-xs" onclick="OpenConclusionForm(' + this + ')' + '">Conclusion</div> ' +
                        '<div class="btn btn-info btn-xs" onclick="OpenConclusionForm(this)"' + ' data-item=\'' + JSON.stringify({ "type": "testGroupExperiment", "item": res[i] }) + '\'' + '>Conclusion</div> ' +
                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateTestGroupTable();
            }

            function GetTestGroupData(experimentId) {
                var newRequestObj = new Object();

                var researchGroupId = null;
                newRequestObj.experimentId = experimentId;
                newRequestObj.researchGroupId = researchGroupId;
                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllTestGroupsByExperiment",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTestGroupTableData(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }


            //Tests Table;
            var testsTable;

            function CreateTestsTable() {

                // DataTable
                testsTable = $('#listTestsDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 7,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [3] },
                    ], fixedColumns: false,
                    //"order": [[0, "desc"]]
                    "order": [],
                    initComplete: function () {
                        count = 0;
                        this.api().columns([0, 1, 2, 3, 4, 5, 7, 8]).every(function () {
                            var title = this.header();
                            //replace spaces with dashes
                            title = $(title).html().replace(/[\W]/g, '-');
                            var column = this;
                            var select = $('<select style="width:90px; text-align: center;" id="' + title + '" class="select2" ></select>')
                                .appendTo($(column.header()).empty())
                                .on('change', function () {
                                    //Get the "text" property from each selected data 
                                    //regex escape the value and store in array
                                    var data = $.map($(this).select2('data'), function (value, key) {
                                        return value.text ? '^' + $.fn.dataTable.util.escapeRegex(value.text) + '$' : null;
                                    });

                                    //if no data selected use ""
                                    if (data.length === 0) {
                                        data = [""];
                                    }

                                    //join array into string with regex or (|)
                                    var val = data.join('|');

                                    //search for the option(s) selected
                                    column
                                        .search(val ? val : '', true, false)
                                        .draw();
                                });

                            column.data().unique().sort().each(function (d, j) {
                                select.append('<option value="' + d + '">' + d + '</option>');
                            });

                            //use column title as selector and placeholder
                            $('#' + title).select2({
                                multiple: true,
                                closeOnSelect: false,
                                placeholder: "-- All --" 
                            });

                            //initially clear select otherwise first option is selected
                            $('.select2').val(null).trigger('change');
                        });
                    }
                });
            }

            function RefreshTestsTableData(data) {

                testsTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                for (var i in res) {

                    /* var lastChange = res[i].lastChange ? res[i].lastChange : '#';*/
                    //var item = JSON.stringify(res[i]);
                    //console.log(JSON.stringify(res[i]));
                    output += '<tr ' + 'id="' + 'test' + res[i].testId + '"' + '>' +
                        '<td>' + res[i].testType + '</td>' +
                        '<td>' + res[i].testTypeSubcategory + '</td>' +
                        '<td>' + (res[i].batteryComponentType == null ? "/" : res[i].batteryComponentType) + '</td>' +
                        '<td>' + (res[i].stepId == null ? "/" : res[i].stepId) + '</td>' +
                        '<td>' + res[i].testLabel + '</td>' +
                        '<td>' + res[i].comment + '</td>' +
                        '<td>' + res[i].dateCreated + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>' +
                        //'<td>' + lastChange + '</td>' +
                        '<td class="table-actions-col">' +
                            //'<a class="btn btn-success btn-xs" target="_blank" href="<%= Page.ResolveUrl("~/TestGroups/Edit/") %>' + res[i].fkTestGroup + '">Action</a> ' +

                            //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Edit/") %>' + res[i].experimentId + '">Edit</a> ' +
                            //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Delete/") %>' + res[i].experimentId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                        //'<div class="btn btn-info btn-xs" onclick="OpenConclusionForm(' + this + ')' + '">Conclusion</div> ' +
                        //'<div class="btn btn-info btn-xs" onclick="OpenConclusionForm(this)"' + ' data-item=\'' + JSON.stringify({ "type": "testGroupExperiment", "item": res[i] }) + '\'' + '>Conclusion</div> ' +
                        '</tr>';


                }
                $('#listTestsDataTable tbody').html(output);

                CreateTestsTable();
            }

            function GetTestsData(experimentId) {
                var newRequestObj = new Object();

                var researchGroupId = null;
                newRequestObj.experimentId = experimentId;
                newRequestObj.testTypeId = null;
                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllTestsDoneByExperiment",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTestsTableData(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }



            function OpenInsertTestForm() {
                //var item = $(element).data("item").item;
                //console.log(item);

                var html = "";
                html += '<div class="row"><div class="col-md-12">';
                html += '<fieldset class="form-horizontal">';

                html += '<div class="form-group">';
                html += '<label for="selectTestType" class="col-sm-2 control-label">Test</label>';
                html += '<div class="col-sm-10">';
                html += '<select required="true" id="selectTestType" class="test-type-data-ajax">    </select>';
                html += '</div>';
                html += '</div>';

                //html += '<div class="form-group">';
                //html += '<label for="test" class="col-sm-2 control-label">Test</label>';
                //html += '<div class="col-sm-10">';
                ////html += '<input name="experimentHypothesis" type="text" id="experimentHypothesis"' + ' value="' + item.experimentHypothesis + '" class="form-control">';
                //html += '<input name="test" type="text" id="experimentHypothesis"' + ' value="' +  '" class="form-control">';
                //html += '</div>';
                //html += '</div>';

                html += '<div class="form-group">';
                html += '<label for="comment" class="col-sm-2 control-label">Comment</label>';
                html += '<div class="col-sm-10">';
                html += '<input name="comment" type="text" id="conclusion"' + ' value="' + '" class="form-control">';
                html += '</div>';
                html += '</div>';

                html += '</fieldset>';
                html += '</div></div>';

                //html += '<div class="row"><div class="col-md-12">';
                //html += '<div id="alpaca-universal-form"> </div>';

                //html += '</div></div>';


                var insertTestDialog = bootbox.dialog({
                    title: 'Experiment Conclusion',
                    message: html,
                    buttons: {
                        cancel: {
                            label: "Cancel",
                            className: 'btn-default',
                            callback: function () {

                            }
                        },
                        ok: {
                            label: "Save",
                            className: 'btn-info save-button',
                            callback: function () {

                                var testTypeId = $("#selectTestType").val();
                                //var comment = $("#comment").val();
                                var comment = $('input[name="comment"]').val();
                                //newRequestObj.conclusion = $("#conclusion").val();

                                var jsonRequestObject = {};
                                jsonRequestObject["fkExperiment"] = experimentId;
                                jsonRequestObject["fkTestType"] = testTypeId;
                                jsonRequestObject["comment"] = comment;

                                var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });

                                //console.log(RequestDataString);

                                $.ajax({
                                    type: "POST",
                                    url: "/Helpers/WebMethods.asmx/AddTest",
                                    data: RequestDataString,
                                    //async: true,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        //RefreshTestGroupTableData(result.d);
                                        GetTestsData(experimentId);
                                        notify("Success", "success");
                                    },
                                    error: function (p1, p2, p3) {
                                        alert(p1.status);
                                        alert(p3);
                                    }
                                });


                                bootbox.hideAll();

                                return true;
                            }
                        }
                    }
                });

                reqUrlTestTypes = "/Helpers/WebMethods.asmx/GetTestTypes";
                $('#selectTestType').select2({
                    ajax: {
                        type: "POST",
                        url: reqUrlTestTypes,
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        delay: 250,
                        data: function (params) {
                            var term = "";
                            if (params.term) {
                                var term = params.term;
                            }

                            return JSON.stringify({ search: term, type: 'public' });
                        },
                        processResults: function (data, params) {

                            //console.log(JSON.parse(data.d).results);
                            return {
                                results: $.map(JSON.parse(data.d), function (item) {
                                    return {
                                        id: item.testTypeId,
                                        text: item.testType,
                                        //data: item
                                    };
                                })
                            };
                        },
                        cache: true
                    },
                    dropdownParent: insertTestDialog,
                    width: "100%",
                    //theme: "bootstrap",
                    placeholder: 'Search for a test',
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    //minimumInputLength: 1,
                    //templateResult: formatProcess,
                    //templateSelection: formatRepoSelection
                });
            }


            function OpenAddTestGroupForm() {
                //var item = $(element).data("item").item;
                //console.log(item);

                var html = "";
                html += '<div class="row"><div class="col-md-12">';
                html += '<fieldset class="form-horizontal">';

                html += '<div class="form-group">';
                html += '<label for="selectTestGroup" class="col-sm-2 control-label">Test Group</label>';
                html += '<div class="col-sm-10">';
                html += '<select required="true" id="selectTestGroup" class="test-type-data-ajax">    </select>';
                html += '</div>';
                html += '</div>';

                html += '<div class="form-group">';
                html += '<label for="test" class="col-sm-2 control-label">Hypothesis</label>';
                html += '<div class="col-sm-10">';
                //html += '<input name="experimentHypothesis" type="text" id="experimentHypothesis"' + ' value="' + item.experimentHypothesis + '" class="form-control">';
                html += '<input name="experimentHypothesis" type="text" id="experimentHypothesis"' + ' value="' + '" class="form-control">';
                html += '</div>';
                html += '</div>';

                //html += '<div class="form-group">';
                //html += '<label for="comment" class="col-sm-2 control-label">Comment</label>';
                //html += '<div class="col-sm-10">';
                //html += '<input name="comment" type="text" id="conclusion"' + ' value="'  + '" class="form-control">';
                //html += '</div>';
                //html += '</div>';

                html += '</fieldset>';
                html += '</div></div>';

                //html += '<div class="row"><div class="col-md-12">';
                //html += '<div id="alpaca-universal-form"> </div>';

                //html += '</div></div>';


                var insertTestGroupDialog = bootbox.dialog({
                    title: 'Add this experiment to a test group',
                    message: html,
                    buttons: {
                        cancel: {
                            label: "Cancel",
                            className: 'btn-default',
                            callback: function () {

                            }
                        },
                        ok: {
                            label: "Save",
                            className: 'btn-info save-button',
                            callback: function () {

                                var testGroupId = $("#selectTestGroup").val();
                                if (testGroupId == null) {
                                    notify("Please select Test group!", "info");
                                    return false;
                                }
                                else {
                                    var hypothesis = $('input[name="experimentHypothesis"]').val();
                                    //newRequestObj.conclusion = $("#conclusion").val();

                                    var jsonRequestObject = {};
                                    jsonRequestObject["fkExperiment"] = experimentId;
                                    jsonRequestObject["fkTestGroup"] = testGroupId;
                                    jsonRequestObject["experimentHypothesis"] = hypothesis;

                                    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });

                                    //console.log(RequestDataString);

                                    $.ajax({
                                        type: "POST",
                                        url: "/Helpers/WebMethods.asmx/SubmitTestGroupExperiment",
                                        data: RequestDataString,
                                        //async: true,
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function (result) {
                                            //RefreshTestGroupTableData(result.d);
                                            GetTestGroupData(experimentId);
                                            notify("Success", "info");
                                        },
                                        error: function (p1, p2, p3) {
                                            alert(p1.status);
                                            alert(p3);
                                        }
                                    });
                                }

                                bootbox.hideAll();

                                return true;
                            }
                        }
                    }
                });

                reqUrlTestTypes = "/Helpers/WebMethods.asmx/GetAllTestGroupsByExperimentNotInside";
                $('#selectTestGroup').select2({
                    ajax: {
                        type: "POST",
                        url: reqUrlTestTypes,
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
                                experimentId: experimentId,
                                researchGroupId: null,
                                page: params.page || 1
                            });
                        },
                        processResults: function (data, params) {
                            var response = JSON.parse(data.d);
                            //console.log(JSON.parse(data.d).results);
                            return {
                                results: $.map(response.results, function (item) {
                                    return {
                                        id: item.testGroupId,
                                        text: item.testGroupName,
                                        //data: item
                                    };
                                }),
                                pagination: {
                                    "more": response.pagination.more
                                }
                            };
                        },
                        cache: true
                    },
                    dropdownParent: insertTestGroupDialog,
                    width: "100%",
                    //theme: "bootstrap",
                    placeholder: 'Search for a test group',
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    //minimumInputLength: 1,
                    //templateResult: formatProcess,
                    //templateSelection: formatRepoSelection
                });
            }

            function OpenConclusionForm(element) {
                var item = $(element).data("item").item;
                //console.log(item);

                var html = "";
                html += '<div class="row"><div class="col-md-12">';
                html += '<fieldset class="form-horizontal">';

                html += '<div class="form-group">';
                html += '<label class="col-sm-2 control-label">Test Group: </label>';
                html += '<div class="col-sm-10">';
                html += '<span class="">' + item.testGroupName + '</span>';
                //form-control
                html += '</div>';
                html += '</div>';

                html += '<div class="form-group">';
                html += '<label for="experimentHypothesis" class="col-sm-2 control-label">Hypothesis</label>';
                html += '<div class="col-sm-10">';
                html += '<input name="experimentHypothesis" type="text" id="experimentHypothesis"' + ' value="' + item.experimentHypothesis + '" class="form-control">';
                html += '</div>';
                html += '</div>';

                html += '<div class="form-group">';
                html += '<label for="conclusion" class="col-sm-2 control-label">Conclusion</label>';
                html += '<div class="col-sm-10">';
                html += '<input name="conclusion" type="text" id="conclusion"' + ' value="' + item.conclusion + '" class="form-control">';
                html += '</div>';
                html += '</div>';

                html += '</fieldset>';
                html += '</div></div>';

                //html += '<div class="row"><div class="col-md-12">';
                //html += '<div id="alpaca-universal-form"> </div>';

                //html += '</div></div>';


                var editDialog = bootbox.dialog({
                    title: 'Experiment Conclusion',
                    message: html,
                    buttons: {
                        cancel: {
                            label: "Cancel",
                            className: 'btn-default',
                            callback: function () {

                            }
                        },
                        ok: {
                            label: "Save",
                            className: 'btn-info save-button',
                            callback: function () {

                                //$("#alpaca-submit-button").prop("disabled", true);
                                //var alpacaFormData = alpacaForm.alpaca().getValue();
                                //var alpacaFormDataJson = JSON.stringify(alpacaFormData);

                                //var selectedProcessData = {};
                                //selectedProcessData.processTypeId = processTypeId;
                                //selectedProcessData.processType = processType;

                                //var row = GenerateProcessHtml(selectedProcessData, alpacaFormData);
                                //ReplaceRow(currentRowElement, row);

                                var newRequestObj = new Object();

                                var researchGroupId = null;
                                newRequestObj.testGroupExperimentId = item.testGroupExperimentId;
                                newRequestObj.experimentHypothesis = $("#experimentHypothesis").val();
                                newRequestObj.conclusion = $("#conclusion").val();

                                var RequestDataString = JSON.stringify(newRequestObj);
                                //console.log(RequestDataString);

                                $.ajax({
                                    type: "POST",
                                    url: "/Helpers/WebMethods.asmx/UpdateTestGroupExperiment",
                                    data: RequestDataString,
                                    //async: true,
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (result) {
                                        //RefreshTestGroupTableData(result.d);
                                        GetTestGroupData(item.fkExperiment);
                                        notify("Success", "success");
                                    },
                                    error: function (p1, p2, p3) {
                                        alert(p1.status);
                                        alert(p3);
                                    }
                                });


                                bootbox.hideAll();

                                return true;
                            }
                        }
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
                experimentId = <%= experimentId %>;
                var experiment = <%= experiment %>;
                var calculations = <%= calculationsJsonString %>;

                ShowExperimentGeneralInfo(experiment);
                ShowExperimentCalculations(calculations);
                var allContent = <%= allContentJsonString %>;
                var allProcesses = <%= allProcessesJsonString %>;
                //ShowExperimentSummary(experimentContentList, allbatchesContentList, experiment);
                ShowExperimentSummary(allContent, allProcesses, experiment);

                CreateTestGroupTable();
                CreateTestsTable();

                GetTestGroupData(experimentId);
                GetTestsData(experimentId);


            });

            function GoToGraphs(experimentId) {
                experimentId = <%= experimentId %>;
                //var experimentIds = ["1", "2"];
                //window.open("/GraphResults/Default?exp=" + experimentIds);

                window.open("/GraphResults/Default?exp=" + experimentId);
            }

            //GetAllTestsDoneByExperiment

        </script>

    </section>
</asp:Content>
