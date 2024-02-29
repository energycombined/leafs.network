<%@ Page Title="Materials List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Materials.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h3 style="font-weight: 700; color: grey;">Materials List</h3>
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New material" />
        </p>
        <div class="row">
            <div class="col-xs-12">
                <div class="box box-warning">
                    <!--<div class="box-header">
                      <h3 class="box-title">Filter</h3>
                    </div>-->
                    <div class="box-body">
                        <div class="row col-margin">
                            <div class="col-xs-9 col-sm-6 col-md-4">
                                <div class="input-group">
                                    <span class="input-group-addon">Type of material</span>
                                    <asp:DropDownList ID="ddlMaterialType" data-placeholder="" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
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
                <div class="box box-primary">
                    <%--<div class="box-header">
                        <h3 class="box-title">List Users</h3>
                    </div>--%>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover table-striped text-center" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th class="text-center" style="width: 15%;">Action
                                                    </th>
                                                    <th style="width: 20%;">Name
                                                    </th>
                                                    <th>Chemical Formula
                                                    </th>
                                                    <th style="width: 20%;">Label
                                                    </th>
                                                    <th>Type
                                                    </th>
                                                    <th>Stored In
                                                    </th>
                                                    <th style="width: 20%;">Description
                                                    </th>
                                                    <th>Bought
                                                    </th>
                                                    <th>Operator(Added by)
                                                    </th>
                                                    <th>Available quantity
                                                    </th>
                                                    <th>Measurement unit
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
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>


        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    "columnDefs": [{
                        "targets": 9,
                        "orderable": false
                    },
                        //{ "type": "de_datetime", targets: [7] },
                    ],
                    "order": []
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var dateFormat = 'DD/MM/YYYY';

                for (var i in res) {
                    var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    var dateBought = res[i].dateBought ? new moment.utc(res[i].dateBought, dateFormat).local().format(dateFormat) : '#';

                    output += '<tr ' + 'id="' + 'material' + res[i].materialId + '"' + '>' +
                        '<td class="table-actions-col text-center">' +
                                '<a title="View" class="btn btn-success btnNew" href="<%= Page.ResolveUrl("~/Materials/View/") %>' + res[i].materialId + '"><i class="fa fa-eye"></i></a> ' +
                                '<a title="Edit" class="btn btn-primary btnNew" href="<%= Page.ResolveUrl("~/Materials/Edit/") %>' + res[i].materialId + '"><i class="fa fa-edit"></i></a> ' +
                        '<a title="Upload test results" class="btn btn-warning btnNew" onclick="GoToUploadGraphs(' + res[i].materialId + ')" ><i class="fa fa-upload"></i></a> ' +
                        '<a title="Download test data" class="btn btn-success btnNew" onclick="ShowFileAttachmentsMeasurements(null, null, null, ' + res[i].materialId + ')" ><i class="fa fa-download"></i></a> ' +
                        '<div title="Delete" class="btn btn-danger btnNew" onclick="OpenDeleteForm(' + res[i].materialId + ')' + '"><i class="fa fa-trash"></i></div> ' + '</td>' +
                        '<td>' + res[i].materialName + '</td>' +
                        '<td>' + res[i].chemicalFormula + '</td>' +
                        '<td>' + res[i].materialLabel + '</td>' +
                        '<td>' + res[i].materialType + '</td>' +
                        '<td>' + res[i].storedInType + '</td>' +
                        '<td>' + res[i].description + '</td>' +
                        '<td data-sort="' + moment(res[i].dateBought, 'DD/MM/YYYY HH:mm') + '">' + dateBought + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + availableQuantity + '</td>' +
                        '<td>' + res[i].measurementUnitSymbol + '</td>' +


                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                CreateDataTable();

                $('#<%= ddlMaterialType.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    //newRequestObj.materialId = "null";
                    newRequestObj.materialType = $('#<%= ddlMaterialType.ClientID %>').val();
                    newRequestObj.materialId = null;
                    //newRequestObj.materialId = 1;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        //url: "Default.aspx/FilterList",
                        url: "/Helpers/WebMethods.asmx/GetAllMaterialsList",
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
                newRequestObj.materialId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete material",
                    message: "Are you sure you want to delete the material?",
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
                                url: "Default.aspx/DeleteMaterial",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        $("#material" + elementId).remove();
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

            function GoToUploadGraphs(materialId) {
                window.open("/GraphResults/Upload?mat=" + materialId);
            }

            //function DeleteMaterial(materialId) {
            //    if (confirm("Are you sure you want to delete material?")) {

            //        var newRequestObj = new Object();
            //        newRequestObj.materialId = materialId;
            //        var requestDataString = JSON.stringify(newRequestObj);

            //        $.ajax({
            //            type: "POST",
            //            url: "Default.aspx/DeleteMaterial",
            //            data: requestDataString,
            //            //async: true,
            //            contentType: "application/json; charset=utf-8",
            //            dataType: "json",
            //            success: function (result) {
            //                if (result.d != "") {
            //                    alert(result.d);
            //                } else {
            //                    location.reload();
            //                }
            //            },
            //            error: function (p1, p2, p3) {
            //                alert(p1.status);
            //                alert(p3);
            //            }
            //        });
            //    }
            //}

        </script>
    </section>
</asp:Content>
