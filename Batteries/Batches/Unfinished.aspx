<%@ Page Title="Resume Work" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Unfinished.aspx.cs" Inherits="Batteries.Batches.Unfinished" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Batches List</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New batch" />
        </p>
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
                                                    <th>Chemical Formula
                                                    </th>
                                                    <th>Type
                                                    </th>
                                                    <%--<th>Available quantity
                                                    </th>--%>
                                                    <th>Measurement unit
                                                    </th>
                                                    <th>Created by
                                                    </th>
                                                    <th>Research Group
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
                        //{ "type": "de_datetime", targets: [7]},
                    ],
                    //"order": [[7, "desc"]]
                    "order": []
                });
            }
            function RefreshTableData(data) {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                var dateFormat = 'DD/MM/YYYY';

                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var lastChange = res[i].lastChange ? new moment.utc(res[i].lastChange, dateFormat).local().format(dateFormat) : '#';
                    var changedBy = res[i].editingOperatorUsername ? res[i].editingOperatorUsername : '#';

                    output += '<tr ' + 'id="' + 'batch' + res[i].batchId + '"' + '>' +
                        '<td data-sort="' + res[i].batchId + '">' + res[i].batchSystemLabel + '</td>' +
                        '<td>' + res[i].batchPersonalLabel + '</td>' +
                        '<td>' + res[i].chemicalFormula + '</td>' +
                        '<td>' + res[i].materialType + '</td>' +
                        //'<td>' + availableQuantity + '</td>' +
                        '<td>' + res[i].measurementUnitSymbol + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].lastChange, 'DD/MM/YYYY HH:mm') + '">' + lastChange + '</td>' +
                        '<td>' + changedBy + '</td>' +
                        '<td class="table-actions-col">' +
                                '<a class="btn btn-warning btn-xs" href="<%= Page.ResolveUrl("~/Batches/BatchContents/Insert/") %>' + res[i].batchId + '">Resume work</a> ' +

                                //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Batches/Delete/") %>' + res[i].materialId + '">Delete</a> ' +
                        //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].batchId + ')' + '">Delete</div> ' +
                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {

                CreateDataTable();

                <%--$('#<%= ddlMaterialType.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });--%>

                //$('#filter-btn').click(function () {
                //$(function () {
                var newRequestObj = new Object();
                newRequestObj.batchId = null;
                //newRequestObj.materialType = $('#<procent= ddlMaterialType.ClientID %>').val();
                newRequestObj.materialType = null;
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetUnfinishedBatchesList",
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


            function EditBatch(batchId) {

                $.ajax({
                    type: "POST",
                    url: "Default.aspx/EditBatch",
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{batchId:"' + batchId + '"}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {

                            window.location.replace("/Batches/BatchContents/Insert/" + batchId);

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
        </script>
    </section>
</asp:Content>
