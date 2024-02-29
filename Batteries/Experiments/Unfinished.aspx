<%@ Page Title="Resume Work" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Unfinished.aspx.cs" Inherits="Batteries.Experiments.Unfinished" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css")%>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Unfinished Experiments</h2>
        <hr />

        <%--<div class="row">
            <div class="col-xs-12">
                <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New experiment" />
                
            </div>
        </div>--%>
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
                                                    <th>Created by
                                                    </th>
                                                    <th>Research group
                                                    </th>
                                                    <th>Project
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
                            "targets": 6,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [4, 5] },
                    ],
                    //"order": [[4, "desc"]]
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

                    output += '<tr ' + 'id="' + 'experiment' + res[i].experimentId + '"' + '>' +
                        '<td data-sort="' + res[i].experimentId + '">' + res[i].experimentSystemLabel + '</td>' +
                        '<td>' + res[i].experimentPersonalLabel + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>' +
                        '<td>' + res[i].projectAcronym + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].dateModified, 'DD/MM/YYYY HH:mm') + '">' + dateModified + '</td>' +
                        '<td>' + changedBy + '</td>' +
                        '<td class="table-actions-col">' +
                                '<a class="btn btn-warning btn-xs" href="<%= Page.ResolveUrl("~/Experiments/ExperimentContents/Insert/") %>' + res[i].experimentId + '">Resume work</a> ' +
                                //'<a class="btn btn-info btn-xs" href="<%= Page.ResolveUrl("~/Results/Experiments/Insert/")%>' + res[i].experimentId + '">Results</a> ' +
                                //'<a class="btn btn-warning btn-xs" href="<%= Page.ResolveUrl("~/GraphResults/Upload/")%>' + res[i].experimentId + '">Upload test CSV</a> ' +
                                //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Edit/")%>' + res[i].experimentId + '">Edit</a> ' +
                                //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Delete/")%>' + res[i].experimentId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
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
                    url: "/Helpers/WebMethods.asmx/GetUnfinishedExperimentsList",
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
        </script>
    </section>
</asp:Content>
