<%@ Page Title="Materials List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.MeasurementUnits.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Measurement Units</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New measurement unit" />
        </p>
        <button type="button" id="filter-btn" class="btn btn-success pull-right hidden" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>
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
                                    <table id="listDataTable" class="table table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th>Name
                                                </th>
                                                <th>Symbol
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
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>

        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    "columnDefs": [{
                        "targets": 2,
                        "orderable": false
                    }]
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                for (var i in res) {
                    output += '<tr ' + 'id="' + 'measurementUnit' + res[i].measurementUnitId + '"' + '>' +
                                '<td>' + res[i].measurementUnitName + '</td>' +
                                '<td>' + res[i].measurementUnitSymbol + '</td>' +
                                '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/MeasurementUnits/Edit/") %>' + res[i].measurementUnitId + '">Edit</a> ' +
                                '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].measurementUnitId + ')' + '">Delete</div> ' +

                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                CreateDataTable();

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    newRequestObj.measurementUnitId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/FilterList",
                        //url: "/Helpers/WebMethods.asmx/GetAllMaterialsList",
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
                $('#filter-btn').click();
            });



            function OpenDeleteForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.measurementUnitId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete measurement unit",
                    message: "Are you sure you want to delete the measurement unit?",
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
                                url: "Default.aspx/DeleteMeasurementUnit",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        $("#measurementUnit" + elementId).remove();
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
