<%@ Page Title="Batches List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Batches.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>
<%@ Import Namespace="Batteries.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h3 style="font-weight: 700; color: grey;">Batches List</h3>
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
                <div class="box box-warning">
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
                                                    <th class="text-center" style="width: 13%;">Action
                                                    </th>
                                                    <th>System Label</th>
                                                    <th>Personal Label</th>
                                                    <th>Chemical Formula</th>
                                                    <th>Type</th>
                                                    <th>Available quantity</th>
                                                    <th>Measurement unit</th>
                                                    <th>Created by</th>
                                                    <th>Research group</th>
                                                    <th>Date Created</th>
                                                    <th>Last Change</th>
                                                    <th>Edited by</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                            <tfoot style="font-weight: 600;">
                                                <tr>
                                                    <th></th>
                                                    <th>System Label</th>
                                                    <th>Personal Label</th>
                                                    <th>Chemical Formula</th>
                                                    <th>Type</th>
                                                    <th>Available quantity</th>
                                                    <th>Measurement unit</th>
                                                    <th>Created by</th>
                                                    <th>Research group</th>
                                                    <th>Date Created</th>
                                                    <th>Last Change</th>
                                                    <th>Edited by</th>
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
        <script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>


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
                    fixedColumns: false,
                    //"order": [[0, "desc"]]
                    "order": [],
                    initComplete: function () {
                        count = 0;
                        this.api().columns([4, 7, 8]).every(function () {
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
                                placeholder: "   --  All  --",
                                allowClear: true
                            });

                            //initially clear select otherwise first option is selected
                            $('.select2').val(null).trigger('change');
                        });
                    }
                });
            }
            function RefreshTableData(data) {
                $.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                var dateFormat = 'DD/MM/YYYY HH:mm';

                for (var i in res) {
                    var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var lastChange = res[i].lastChange ? new moment.utc(res[i].lastChange, dateFormat).local().format(dateFormat) : '#';
                    var changedBy = res[i].editingOperatorUsername ? res[i].editingOperatorUsername : '#';

                    var hasTestResultsDoc = res[i].hasTestResultsDoc;
                    var graphButton = '<button class="btn btn-danger btnNew" title="No test results available!" disabled="true" value="Graphs" onclick="GoToGraphs(' + res[i].experimentId + ')" ><i class="fa fa-area-chart"></i></button> ';
                    if (hasTestResultsDoc)
                        graphButton = '<button title="Graph results" class="btn btn-danger btnNew" value="Graphs" onclick="GoToGraphs(' + res[i].batchId + ')" ><i class="fa fa-area-chart"></i></button> ';



                    output += '<tr ' + 'id="' + 'batch' + res[i].batchId + '"' + '>' +
                        '<td class="table-actions-col text-center">';
                    if (<%= UserHelper.GetCurrentUser().fkResearchGroup %> == res[i].fkResearchGroup) {
                        output += '<a title="View" class="btn btn-success btnNew" href="<%= Page.ResolveUrl("~/Batches/View/") %>' + res[i].batchId + '"><i class="fa fa-eye"></i></a> ';
                        output += '<a title="Edit" class="btn btn-warning btnNew" onclick="EditBatch(' + res[i].batchId + ')"><i class="fa fa-edit"></i></a> ';
                        output += '<a title="Upload test results" class="btn btn-primary btnNew" onclick="GoToUploadGraphs(' + res[i].batchId + ')" ><i class="fa fa-upload"></i></a> ';
                    }
                    else {
                        output += '<a title="View" class="btn btn-success btnNew" href="<%= Page.ResolveUrl("~/Batches/Shared/View/") %>' + res[i].batchId + '"><i class="fa fa-eye"></i></a> ';
                        output += '<a title="Edit" class="btn btn-warning btnNew disabled" onclick="EditBatch(' + res[i].batchId + ')"><i class="fa fa-edit"></i></a> ';
                        output += '<a title="Upload test results" class="btn btn-primary btnNew disabled" onclick="GoToUploadGraphs(' + res[i].batchId + ')" ><i class="fa fa-upload"></i></a> ';
                    }
                    output += '<a  title="Download test data" class="btn btn-success btnNew" onclick="ShowFileAttachmentsMeasurements(null, null, ' + res[i].batchId + ')" ><i class="fa fa-download"></i></a> ' +
                        graphButton +
                        '<a title="Empty Batch" class="btn btn-danger btnNew" onclick="OpenEmptyBatchForm(' + res[i].batchId + ')' + '"><i class="fa fa-battery-empty"></i></a> ' +
                        '</td > ' +
                                //'<a class="btn btn-danger btnNew" href="<%= Page.ResolveUrl("~/Batches/Delete/") %>' + res[i].materialId + '">Delete</a> ' +                                
                        '<td data-sort="' + res[i].batchId + '">' + res[i].batchSystemLabel + '</td>' +
                        '<td>' + res[i].batchPersonalLabel + '</td>' +
                        '<td>' + res[i].chemicalFormula + '</td>' +
                        '<td>' + res[i].materialType + '</td>' +
                        '<td>' + availableQuantity + '</td>' +
                        '<td>' + res[i].measurementUnitSymbol + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupName + " " + res[i].researchGroupAcronym + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].lastChange, 'DD/MM/YYYY HH:mm') + '">' + lastChange + '</td>' +
                        '<td>' + changedBy + '</td>' +
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
                    //url: "Default.aspx/GetAllBatchesList",
                    url: "/Helpers/WebMethods.asmx/GetAllBatchesList",
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

            function RefreshDataTableOnClick() {

                var newRequestObj = new Object();
                newRequestObj.batchId = null;
                newRequestObj.materialType = null;
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllBatchesList",
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

            function GoToUploadGraphs(batchId) {
                window.open("/GraphResults/Upload?btc=" + batchId);
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
            function OpenEmptyBatchForm(batchId) {

                bootbox.confirm({
                    title: "Empty batch",
                    message: "Are you sure you want to empty the batch?",
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
                                url: "Default.aspx/EmptyBatch",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{batchId:"' + batchId + '"}',
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        RefreshDataTableOnClick();
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

            function GoToGraphs(batchId) {
                window.open("/GraphResults/Default?btc=" + batchId);
            }

            function ShowFileAttachments(elementName, batchId) {
                //stepId = stepId || null;

                //var fileTypeHtml = '<div class="row"><div class="col-md-12">';
                //fileTypeHtml += '<select id="selectFileType" class="batch-data-ajax">    </select>';
                //fileTypeHtml += '</div></div>';

                var fileTypeHtml = '<div class="form-group">';
                fileTypeHtml += '<label for="file-attachment-document-type">Document type</label>';
                fileTypeHtml += '<select class="form-control input-sm" id="selectDocumentType" class="document-type-data-ajax">    </select>';
                fileTypeHtml += '</div>';



                var html = '<div class="row"><div class="col-md-12">';

                html += '<div class="table-responsive">' +
                    '<table class="table table-bordered table-hover table-striped text-center" style="width:100%">' +
                    '<thead>' +
                    '<th>Filename</th>' +
                    '<th>Document type</th>' +
                    /* '<th>Description</th>' +*/
                    '<th>Type</th>' +
                    '<th>Date Added</th>' +
                    '<th>Action</th>' +
                    '</thead>' +
                    '<tbody>';





                html += '</tbody></table></div></div></div>';




                dialog = bootbox.dialog({
                    title: 'Documents',
                    message: html,
                    closeButton: true,
                    onEscape: true,
                    size: "large"
                }).on('shown.bs.modal', function (e) {

                });
            }
        </script>
    </section>
</asp:Content>
