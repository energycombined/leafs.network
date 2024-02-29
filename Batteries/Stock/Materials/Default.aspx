<%@ Page Title="Material Stock Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Stock.Materials.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Stock Transactions List - Material</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="Insert Stock Material Amount" />
        </p>

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
                                    <span class="input-group-addon">Material name</span>
                                    <asp:DropDownList ID="ddlMaterialName" data-placeholder="" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
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
                                                    <th>Stock Transaction Id
                                                    </th>
                                                    <th>Material Name
                                                    </th>
                                                    <th>Chemical Formula
                                                    </th>
                                                    <th>Type
                                                    </th>
                                                    <th>Operator(Added by)
                                                    </th>
                                                    <th>Amount
                                                    </th>
                                                    <th>Measurement Unit
                                                    </th>
                                                    <th>Date Bought
                                                    </th>
                                                    <th>Vendor Name
                                                    </th>
                                                    <th>Transaction Type
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
        
        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    "columnDefs": [
                        {
                            "targets": 10,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [7] },
                    ],
                    "order": [[0, "desc"]]
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                var dateFormat = 'DD/MM/YYYY';
                
                for (var i in res) {
                    var dateBought = res[i].dateBought ? new moment.utc(res[i].dateBought, dateFormat).local().format(dateFormat) : '#';
                    var vendorName = res[i].vendorName ? res[i].vendorName : '#';
                    var transactionDirection = res[i].transactionDirection == 1 ? 'Addition' : 'Subtraction';

                    output += '<tr ' + 'id="' + 'stockTransaction' + res[i].stockTransactionId + '"' + '>' +
                                '<td data-sort="' + res[i].stockTransactionId + '">' + res[i].stockTransactionId + '</td>' +
                                '<td>' + res[i].materialName + '</td>' +
                                '<td>' + res[i].materialChemicalFormula + '</td>' +
                                '<td>' + res[i].materialType + '</td>' +
                                '<td>' + res[i].operatorUsername + '</td>' +
                                '<td>' + res[i].amount + '</td>' +
                                '<td>' + res[i].measurementUnitSymbol + '</td>' +
                                '<td data-sort="' + moment(res[i].dateBought, 'DD/MM/YYYY HH:mm') + '">' + dateBought + '</td>' +
                                '<td>' + vendorName + '</td>' +
                                '<td>' + transactionDirection + '</td>' +

                                '<td class="table-actions-col">' +
                                '<a class="btn btn-default btn-xs" href="<%= Page.ResolveUrl("~/Stock/Materials/View/") %>' + res[i].stockTransactionId + '">View</a> ' +                               
                                //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].materialId + ')' + '">Delete</div> ' +

                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                CreateDataTable();

                $('#<%= ddlMaterialName.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    newRequestObj.materialId = $('#<%= ddlMaterialName.ClientID %>').val();
                    //newRequestObj.batchId =
                    newRequestObj.batchId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);
                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/FilterList",
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
        </script>
    </section>
</asp:Content>
