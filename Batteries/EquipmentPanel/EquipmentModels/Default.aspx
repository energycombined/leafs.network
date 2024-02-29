<%@ Page Title="Equipment Models List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.EquipmentPanel.EquipmentModels.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Equipment List</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New equipment model" />
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
                                    <span class="input-group-addon">Equipment type</span>
                                    <asp:DropDownList ID="ddlEquipment" data-placeholder="" runat="server" AppendDataBoundItems="true" CssClass="form-control"></asp:DropDownList>
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
                                                    <th>Equipment
                                                    </th>
                                                    <th>Model Name
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
        <script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>

        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
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

                    output += '<tr ' + 'id="' + 'equipmentModel' + res[i].equipmentModelId + '"' + '>' +
                                '<td>' + res[i].equipmentName + '</td>' +
                                '<td>' + res[i].equipmentModelName + '</td>' +
                                '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/EquipmentPanel/EquipmentModels/Edit/") %>' + res[i].equipmentModelId + '">Edit</a> ' +                            
                                '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].equipmentModelId + ')' + '">Delete</div> ' +

                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                CreateDataTable();

                $('#<%= ddlEquipment.ClientID %>').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    newRequestObj.equipmentId = $('#<%= ddlEquipment.ClientID %>').val();
                    newRequestObj.equipmentModelId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetAllEquipmentModelsList",
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
                newRequestObj.equipmentModelId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete equipment model",
                    message: "Are you sure you want to delete this equipment model?",
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
                                url: "Default.aspx/DeleteEquipmentModel",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        $("#equipmentModel" + elementId).remove();
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
