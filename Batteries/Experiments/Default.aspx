<%@ Page Title="Experiments List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Experiments.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css")%>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <!-- Small boxes (Stat box) -->
        <div class="row">
            <div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-purple">
                    <div class="inner">
                        <h3>
                            <span runat="server" id="noProjects"></span>
                        </h3>
                        <p>
                            Total Projects
                        </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-ios-gear"></i>
                    </div>
                    <a href="<%= ResolveUrl("~/Projects/Insert.aspx") %>" class="small-box-footer">Add New Project <i class="fa fa-arrow-circle-right"></i>
                    </a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-green">
                    <div class="inner">
                        <h3>
                            <span runat="server" id="noTestGroups"></span>
                        </h3>
                        <p>
                            Total Test Groups
                        </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-stats-bars"></i>
                    </div>
                    <a href="<%= ResolveUrl("~/TestGroups/Insert.aspx") %>" class="small-box-footer">Add New Test Group <i class="fa fa-arrow-circle-right"></i>
                    </a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h3>
                            <span runat="server" id="noResearchGrop"></span>
                        </h3>
                        <p>
                            Total Research Groups
                        </p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-ios-people-outline"></i>
                    </div>
                    <a href="<%= ResolveUrl("~/ResearchGroups/Insert.aspx") %>" class="small-box-footer">Add New Research Group <i class="fa fa-arrow-circle-right"></i>
                    </a>
                </div>
            </div>
            <!-- ./col -->
            <div class="col-lg-3 col-xs-6">
                <!-- small box -->
                <div class="small-box bg-red">
                    <div class="inner">
                        <h3>
                            <span runat="server" id="noBatch"></span>
                        </h3>
                        <p>
                            Total Batches
                        </p>
                    </div>
                    <div class="icon">
                        <i class="ion-ios-analytics-outline"></i>
                    </div>
                    <a href="<%= ResolveUrl("~/Batches/Insert.aspx") %>" class="small-box-footer">Add New Batch <i class="fa fa-arrow-circle-right"></i>
                    </a>
                </div>
            </div>
            <!-- ./col -->
        </div>
        <!-- /.row -->

        <h3 style="font-weight: 700; color: grey;">Experiment List</h3>
        <div class="row">
            <div class="col-xs-12">
                <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New experiment" />
                <div class="pull-right">
                    <%--<asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="Download Materials Csv" />--%>
                    <a href="#" class="btn btn-info" onclick="DownloadCsvFileMaterials()">Download Materials Csv</a>
                    <a href="#" class="btn btn-success" onclick="DownloadCsvFileProcesses()">Download Processes Csv</a>
                </div>
            </div>
        </div>
        <br />
        <%--<div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <!--<div class="box-header">
                      <h3 class="box-title">Filter</h3>
                    </div>-->
                    <div class="box-body">
                        <div class="row col-margin">
                            <div class="col-xs-9 col-sm-6 col-md-4">
                                <div class="input-group">
                                    <span class="input-group-addon">Batch</span>
                                    <asp:DropDownList ID="ddlMaterialType" data-placeholder="" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-3 col-sm-6 col-md-8">
                                <button type="button" id="filter-btn" class="hidden btn btn-success pull-right" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

        <div class="row">
            <div class="col-xs-12">
                <div class="box box-warning">
                    <%--<div class="box-header">
                        <h3 class="box-title">List Users</h3>
                    </div>--%>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <%--<span id="office"></span>--%>
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover table-striped text-center" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Action</th>
                                                    <th>Sys.Label</th>
                                                    <th>Per.Label</th>
                                                    <th>Anode A.M.</th>
                                                    <th>Cathode A.M.</th>
                                                    <th>Created</th>
                                                    <th>Research group</th>
                                                    <th>Project</th>
                                                    <th>Date Created</th>
                                                    <th>Last Change</th>
                                                    <th>Edited by</th>
                                                    <th>Sharing status</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                            <tfoot class="text-dark">
                                                <tr>
                                                    <th style="width: 14%; background-color: white"></th>
                                                    <th>Sys.Label</th>
                                                    <th>Per.Label</th>
                                                    <th style="width: 15%">Anode A.M.</th>
                                                    <th style="width: 15%">Cathode A.M.</th>
                                                    <th>Created by</th>
                                                    <th>Research group</th>
                                                    <th>Project</th>
                                                    <th style="width: 7%">Date Created</th>
                                                    <th style="width: 7%">Last Change</th>
                                                    <th>Edited by</th>
                                                    <th>Sharing status</th>
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
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>

        <script type="text/javascript">
            var theTable;
            function CreateDataTable() {

                //DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    'ordering': true,  // Column ordering,
                    'paging': true,  // Table pagination                    
                    "columnDefs": [
                        {
                            "width": "13%", "targets": 0,
                            //"targets": 11,
                            //"orderable": false
                        }
                        //{
                        //    "targets": 0,
                        //    "orderable": true,
                        //    "data": "experimentId",
                        //},
                        //{ "type": "de_datetime", targets: [6,7] },
                    ],
                    fixedColumns: false,
                    //"order": [[0, "desc"]]
                    "order": [],
                    initComplete: function () {
                        count = 0;
                        this.api().columns([6, 7]).every(function () {
                            var title = this.header();
                            //replace spaces with dashes
                            title = $(title).html().replace(/[\W]/g, '-');
                            var column = this;
                            var select = $('<select style="width:120px;" id="' + title + '" class="form-control form-control-sm select2"  ></select>')
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
                                closeOnSelect: true,
                                placeholder: "   -  All  -",
                                allowClear: true
                            });

                            //initially clear select otherwise first option is selected
                            $('.select2').val(null).trigger('change');
                        });
                    }
                    /*  //polni drop za research grupa nadvor od datatable*/
                    // initComplete: function () {
                    //    var column = this.api().column(6);
                    //    var select = $('<select><option value=""></option></select>')
                    //        .appendTo($('#office').empty().text('Select research Group: '))
                    //        .on('change', function () {
                    //            var val = $.fn.dataTable.util.escapeRegex(
                    //                $(this).val()
                    //            );
                    //            column
                    //                .search(val ? '^' + val + '$' : '', true, false)
                    //                .draw();

                    //        });
                    //    column.data().unique().sort().each(function (d, j) {
                    //        select.append('<option value="' + d + '">' + d + '</option>');
                    //    });
                    //}                  
                });
            }

            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var dateFormat = 'DD/MM/YYYY HH:mm';
                var currentRG = <%= currentRG %>;
                var statusId = 0;


                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';

                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var dateModified = res[i].dateModified ? new moment.utc(res[i].dateModified, dateFormat).local().format(dateFormat) : '#';
                    var changedBy = res[i].editingOperatorUsername ? res[i].editingOperatorUsername : '#';
                    var anodeTotalActiveMaterials = res[i].anodeTotalActiveMaterials ? (Number(res[i].anodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].anodeActiveMaterials + ' = ' + res[i].anodeActivePercentages + ' %') : ' /';
                    var cathodeTotalActiveMaterials = res[i].cathodeTotalActiveMaterials ? (Number(res[i].cathodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].cathodeActiveMaterials + ' = ' + res[i].cathodeActivePercentages + ' %') : '/';

                    //var editButton = '<a title="Edit" class="btn btn-warning btnNew" onclick="EditExperiment(' + res[i].experimentId + ')"><i class="fa fa-edit"></i></a> ';

                    var hasTestResultsDoc = res[i].hasTestResultsDoc;
                    var graphButton = '<button class="btn btn-danger btnNew" title="No test results available!" disabled="true" value="Graphs" onclick="GoToGraphs(' + res[i].experimentId + ')" ><i class="fa fa-area-chart"></i></button> ';
                    if (hasTestResultsDoc)
                        graphButton = '<button title="Graph results" class="btn btn-danger btnNew" value="Graphs" onclick="GoToGraphs(' + res[i].experimentId + ')" ><i class="fa fa-area-chart"></i></button> ';

                    var statusId = res[i].fkSharingType;
                    if (currentRG == res[i].fkResearchGroup) {
                        if (statusId == 3)
                            var statusBtn = '<button title="Public!" type="button" disabled="true" class="btn btn-success btnNew disabled"><i class="fa fa-unlock"></i></button >';
                        if (statusId == 2)
                            statusBtn = '<button title="Shared!" type="button" id="myBtn" class="btn btn-warning btnNew"btn btn-warning onclick="GoToStatus(' + res[i].experimentId + ')"><i class="fa fa-unlock"></i></button >';
                        if (statusId == 1)
                            statusBtn = '<button title="Private!" type="button" id="myBtn" class="btn btn-danger btnNew" onclick="GoToStatus(' + res[i].experimentId + ')"><i style="margin:2px;" class="fa fa-lock"></i></button >';
                    }
                    else {
                        if (statusId == 3)
                            var statusBtn = '<button title="Public!" type="button" disabled="true" class="btn btn-success btnNew disabled"><i class="fa fa-unlock"></i></button >';
                        if (statusId == 2)
                            statusBtn = '<button title="Shared!" type="button" disabled="true" id="myBtn" class="btn btn-warning btnNew disabled" onclick="GoToStatus(' + res[i].experimentId + ')"><i class="fa fa-unlock"></i></button >';
                        if (statusId == 1)
                            statusBtn = '<button title="Private!" type="button" disabled="true" id="myBtn" class="btn btn-danger btnNew disabled" onclick="GoToStatus(' + res[i].experimentId + ')"><i style="margin:2px;" class="fa fa-lock"></i></button >';
                    }

                    output += '<tr ' + 'id="' + 'experiment' + res[i].experimentId + '"' + '>' +
                        '<td class="table-actions-col text-center">';

                    if (currentRG == res[i].fkResearchGroup)
                        output += '<a  title="View" class="btn btn-success btnNew" href="<%= Page.ResolveUrl("~/Experiments/View/") %>' + res[i].experimentId + '"><i class="fa fa-eye"></i></a> ';
                    else
                        output += '<a  title="View" class="btn btn-success btnNew" href="<%= Page.ResolveUrl("~/Experiments/Shared/View/") %>' + res[i].experimentId + '"><i class="fa fa-eye"></i></a> ';

                    if (currentRG == res[i].fkResearchGroup) {
                        output += '<a  title="Edit" class="btn btn-warning btnNew" onclick="EditExperiment(' + res[i].experimentId + ')"><i class="fa fa-edit"></i></a> ';
                    }
                    else {
                        output += '<a  title="Edit" class="btn btn-warning btnNew disabled" disabled="true" onclick="EditExperiment(' + res[i].experimentId + ')"><i class="fa fa-edit"></i></a> ';
                    }

                    output += '<a  title="Results" class="btn btn-info btnNew" href="<%= Page.ResolveUrl("~/Results/Experiments/Insert/")%>' + res[i].experimentId + '"><i class="fa fa-list"></i></a> ';

                    if (currentRG == res[i].fkResearchGroup) {
                        output += '<a  title="Upload test results" class="btn btn-primary btnNew" onclick="GoToUploadGraphs(' + res[i].experimentId + ')" ><i class="fa fa-upload"></i></a> ';
                        output += '<a  title="Download test data" class="btn btn-success btnNew" onclick="ShowFileAttachmentsMeasurements(null, ' + res[i].experimentId + ')" ><i class="fa fa-download"></i></a> ';
                    }
                    else {
                        output += '<a  title="Upload test results" disabled="true" class="btn btn-primary btnNew disabled" href="<%= Page.ResolveUrl("~/GraphResults/Upload/")%>' + res[i].experimentId + '"><i class="fa fa-upload"></i></a> ';
                        output += '<a  title="Download test data" class="btn btn-success btnNew" onclick="ShowFileAttachmentsMeasurements(null, ' + res[i].experimentId + ')" ><i class="fa fa-download"></i></a> ';
                    }
                    output += graphButton + '</td>' +

                        '<td style="font-weight: 700;" data-sort="' + res[i].experimentId + '">' + res[i].experimentSystemLabel + '</td>' +
                        '<td>' + res[i].experimentPersonalLabel + '</td>' +
                        '<td>' + anodeTotalActiveMaterials + '</td>' +
                        '<td>' + cathodeTotalActiveMaterials + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>' +
                        '<td style="font-weight: 700;">' + res[i].projectAcronym + '</td>' + //projectAcronym
                        /* '<td>' + res[i].testGroupName + '</td>' +*/
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].dateModified, 'DD/MM/YYYY HH:mm') + '">' + dateModified + '</td>' +
                        '<td>' + changedBy + '</td>' +
                        '<td class="text-center">' + statusBtn + '</td>' +


                                //'<a class="btn btn-primary btnNew" href="<%= Page.ResolveUrl("~/Experiments/Edit/")%>' + res[i].experimentId + '">Edit</a> ' +
                                //'<a class="btn btn-danger btnNew" href="<%= Page.ResolveUrl("~/Experiments/Delete/")%>' + res[i].experimentId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btnNew" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');

                CreateDataTable();

                <%--$('#<%= ddlMaterialType.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });--%>

                //$('#filter-btn').click(function () {
                //$(function () {
                var newRequestObj = new Object();
                newRequestObj.experimentId = null;
                //newRequestObj.materialType = $('#<procent= ddlMaterialType.ClientID %>').val();
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllExperimentsList",
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
            });
            //});



            function OpenDeleteForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.materialId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete material",
                    message: "Are you sure you want to delete the batch?",
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
                                url: "Default.aspx/DeleteBatch",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        $("#batch" + elementId).remove();
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

            text_truncate = function (str, length, ending) {
                if (length == null) {
                    length = 100;
                }
                if (ending == null) {
                    ending = '...';
                }
                if (str.length > length) {
                    return str.substring(0, length - ending.length) + ending;
                } else {
                    return str;
                }
            };

            function DownloadCsvFileMaterials() {
                //var win = window.open('/Helpers/DownloadMaterialsCsvFile.ashx?fileAttachmentId=' + fileAttachmentId, '_blank');
                var win = window.open('/Helpers/DownloadMaterialsCsvFile.ashx', '_blank');
            }
            function DownloadCsvFileProcesses() {
                var win = window.open('/Helpers/DownloadProcessesCsvFile.ashx', '_blank');
            }

            function EditExperiment(experimentId) {

                $.ajax({
                    type: "POST",
                    url: "Default.aspx/EditExperiment",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{experimentId:"' + experimentId + '"}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {

                            window.location.replace("/Experiments/ExperimentContents/Insert/" + experimentId);

                        } else {
                            notify(jsonResult.message, "warning");
                        }
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }

            function GoToGraphs(experimentId) {
                //var experimentIds = ["1", "2"];
                //window.open("/GraphResults/Default?exp=" + experimentIds);

                window.open("/GraphResults/Default?exp=" + experimentId);
            }

            function GoToUploadGraphs(experimentId) {
                window.open("/GraphResults/Upload?exp=" + experimentId);
            }

            function GoToStatus(experimentId) {


                var html = '<div class="form-horizontal">';
                html += '<div class="form-group">';
                html += '<div class="col-md-10">';
                html += '<p>Do you want to set this experiment as public? <br /></p>';
                html += '<p class="text-danger"><b>Note: You cannot change the status of the experiment once you set as public</b> </p>';
                html += '</div>';
                html += '</div>';
                html += '</div>';

                var dialog = bootbox.dialog({
                    title: 'Set status of experiment',
                    message: html,
                    buttons: {
                        cancel: {
                            label: "NO",
                            className: 'btn-danger',
                            callback: function () {

                            }
                        },
                        ok: {
                            label: "YES",
                            className: 'btn-success',
                            callback: function () {
                                var jsonRequestObject = {};
                                jsonRequestObject["experimentId"] = experimentId;
                                reqUrl = "/Helpers/WebMethods.asmx/UpdateStatusExperiment";
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
                                            notify("Your experiment is now set as public!", "info");
                                            window.location.replace("/Experiments/");
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
                    }
                });
                dialog.on('shown.bs.modal', function () {

                });
            }

        </script>
    </section>
</asp:Content>
