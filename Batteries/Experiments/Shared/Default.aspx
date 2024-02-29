<%@ Page Title="Shared Experiments List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Experiments.Shared.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css")%>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Experiments List</h2>
        <hr />
        <input type="text" name="TxtHidden" value=" " style="position: relative; z-index: -1;" />
        <div class="row">
            <div class="col-xs-12">
                <%--<asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New experiment" />--%>

                <div class="pull-right">
                    <%--<asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="Download Materials Csv" />--%>
                    <%--<a href="#" class="btn btn-default" onclick="DownloadCsvFileMaterials()">Download Materials Csv</a>--%>
                    <%--<a href="#" class="btn btn-default" onclick="DownloadCsvFileProcesses()">Download Processes Csv</a>--%>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <!--<div class="box-header">
                      <h3 class="box-title">Filter</h3>
                    </div>-->
                    <div class="box-body">
                        <div class="row col-margin">
                            <div class="col-xs-9 col-sm-6 col-md-4">
                                <div class="input-group">
                                    <span class="input-group-addon">Research Group</span>
                                    <asp:DropDownList ID="DdlResearchGroup" data-placeholder="" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-3 col-sm-6 col-md-8">
                                <button type="button" id="filter-btn" class="btn btn-success pull-right" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <%--<div class="box-header">
                        <h3 class="box-title">List Users</h3>
                    </div>--%>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover">
                                            <thead>
                                                <tr>
                                                    <th>System Label
                                                    </th>
                                                    <th>Personal Label
                                                    </th>
                                                    <th>Anode A.M.
                                                    </th>
                                                    <th>Cathode A.M.
                                                    </th>
                                                    <th>Created by
                                                    </th>
                                                    <th>Research group
                                                    </th>
                                                    <th>Date Created
                                                    </th>
                                                    <th>Last Change
                                                    </th>
                                                    <th>Edited by
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
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>--%>
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/date-de.js"></script>--%>



        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 9,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [4, 5] },
                    ],
                    //"order": [[6, "desc"]]
                    "order": []
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var dateFormat = 'DD/MM/YYYY';

                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';

                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var dateModified = res[i].dateModified ? new moment.utc(res[i].dateModified, dateFormat).local().format(dateFormat) : '#';
                    var changedBy = res[i].editingOperatorUsername ? res[i].editingOperatorUsername : '#';
                    var anodeTotalActiveMaterials = res[i].anodeTotalActiveMaterials ? (Number(res[i].anodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].anodeActiveMaterials + ' = ' + res[i].anodeActivePercentages + ' %') : ' /';
                    var cathodeTotalActiveMaterials = res[i].cathodeTotalActiveMaterials ? (Number(res[i].cathodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].cathodeActiveMaterials + ' = ' + res[i].cathodeActivePercentages + ' %') : '/';

                    var hasTestResultsDoc = res[i].hasTestResultsDoc;
                    var graphButton = '<input type="button" class="btn btn-default btn-xs" title="No test results available!" disabled="true"  value="Graphs" onclick="GoToGraphs(' + res[i].experimentId + ')" /> ';
                    if (hasTestResultsDoc)
                        graphButton = '<input type="button" class="btn btn-default btn-xs" value="Graphs" onclick="GoToGraphs(' + res[i].experimentId + ')" /> ';

              /*      var viewButton = '<a title="Edit" class="btn btn-warning btn-xs" onclick="ViewExperiment(' + res[i].experimentId + ')"><i class="fa fa-edit"></i></a> ';*/

                    output += '<tr ' + 'id="' + 'experiment' + res[i].experimentId + '"' + '>' +
                                '<td data-sort="' + res[i].experimentId + '">' + res[i].experimentSystemLabel + '</td>' +
                                '<td>' + res[i].experimentPersonalLabel + '</td>' +
                                '<td>' + anodeTotalActiveMaterials + '</td>' +
                                '<td>' + cathodeTotalActiveMaterials + '</td>' +
                                '<td>' + res[i].operatorUsername + '</td>' +
                                '<td>' + res[i].researchGroupName + '</td>' +
                                '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                                '<td data-sort="' + moment(res[i].dateModified, 'DD/MM/YYYY HH:mm') + '">' + dateModified + '</td>' +
                                '<td>' + changedBy + '</td>' +
                                '<td class="table-actions-col">' +
                                //viewButton +
                                '<a class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Shared/View/") %>' + res[i].experimentId + '">View</a> ' +
                                //'<a class="btn btn-info btn-xs" href="<%= Page.ResolveUrl("~/Results/Experiments/Insert/")%>' + res[i].experimentId + '">Results</a> ' +
                                graphButton +
                        '</tr>';
                        }
                        $('#listDataTable tbody').html(output);

                        CreateDataTable();
                    }


                    var reqUrl = "/Helpers/WebMethods.asmx/GetOtherResearchGroups";
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
                                            text: item.researchGroupName,
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
                        //templateSelection: formatRepoSelection,
                        allowClear: true
                    });

                    function formatRepo(repo) {
                        var markup = "" + repo.text;
                        return markup;
                    }

                    function formatRepoSelection(repo) {
                        return repo.text;
                    }


                    function GetList() {
                        //$(function () {
                        var newRequestObj = new Object();
                        newRequestObj.experimentId = null;
                        //newRequestObj.materialType = $('#<procent= ddlMaterialType.ClientID %>').val();
                        newRequestObj.researchGroupId = null;
                        newRequestObj.otherResearchGroupId = $('#<%= DdlResearchGroup.ClientID %>').val();

                        var RequestDataString = JSON.stringify(newRequestObj);

                        $.ajax({
                            type: "POST",
                            url: "/Helpers/WebMethods.asmx/GetExperimentsListFromOtherRG",
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

                    $('#<%= DdlResearchGroup.ClientID %>').on('change', function () {
                        GetList();
                    });

            $('#filter-btn').click(function () {
                GetList();
            });

            $(function () {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');

                CreateDataTable();

                <%--$('#<%= ddlMaterialType.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });--%>

                //$('#filter-btn').click();
                GetList();
            });


            function DownloadCsvFileMaterials() {
                var win = window.open('/Helpers/DownloadMaterialsCsvFile.ashx', '_blank');
            }
            function DownloadCsvFileProcesses() {
                var win = window.open('/Helpers/DownloadProcessesCsvFile.ashx', '_blank');
            }

            function GoToGraphs(experimentId) {
                window.open("/GraphResults/Default?exp=" + experimentId);
            }

            function ViewExperiment(experimentId) {

                $.ajax({
                    type: "POST",
                    url: "Default.aspx/ViewExperiment",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{experimentId:"' + experimentId + '"}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {

                            window.location.replace("/Experiments/Shared/View/" + experimentId);

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
        </script>
    </section>
</asp:Content>
