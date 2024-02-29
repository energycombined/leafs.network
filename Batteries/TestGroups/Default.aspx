<%@ Page Title="Test Groups List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.TestGroups.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h3 style="font-weight:700; color:grey;">Test Groups List</h3>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New test group" />
        </p>
        <button type="button" id="filter-btn" class="btn btn-success pull-right hidden" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>

        <%--<div class="row">
            <div class="col-xs-12">
                <div class="box">
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
        </div>--%>

        <div class="row">
            <div class="col-xs-12">
                <div class="box box-primary">
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover table-striped" style="width:100%">
                                            <thead class=" text-white" style="font-weight: 700; background-color:cornflowerblue;"  >
                                                <tr>
                                                    <th class="text-center">ID</th>
                                                    <th class="text-center">Action</th>
                                                    <th>Test Group Name</th>
                                                    <%--<th>Goal</th>         --%>                                           
                                                    <th>Project</th>
                                                    <th>Date created</th>
                                                    <th>Research Group</th>     
                                                    <th>Created by</th>
                                                    
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                            <tfoot style="font-weight: 700;">
                                                <tr>
                                                    <th style="background-color:white;"></th>
                                                    <th class="text-center"></th>
                                                    <th>Test Group</th>
                                                   <%-- <th></th>          --%>                                          
                                                    <th>Project</th>
                                                    <th></th>
                                                    <th>Research Group</th>
                                                    <th>Created by</th>
                                                    
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
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>--%>

        <script type="text/javascript">
            var theTable;

            function CreateDataTable() {
                theTable = $('#listDataTable').DataTable({
                    'ordering': true,  // Column ordering,
                    'paging': true,  // Table pagination                    
                    "columnDefs": [
                        {
                            "width": "7%", "targets": 1,
                        }
                    ],
                    fixedColumns: false,
                    //"order": [[0, "desc"]]
                    "order": [],
                    initComplete: function () {
                        count = 0;
                        this.api().columns([2, 3, 5, 6]).every(function () {
                            var title = this.header();
                            //replace spaces with dashes
                            title = $(title).html().replace(/[\W]/g, '-');
                            var column = this;
                            var select = $('<select style="width:175px;" id="' + title + '" class="select2" ></select>')
                                .appendTo($(column.footer()).empty())
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
                                placeholder: " Select a " + title
                            });

                            //initially clear select otherwise first option is selected
                            $('.select2').val(null).trigger('change');
                        });
                    }
                });
            }
         

            function RefreshTableData(data) {
                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var currentRG = <%= currentRG %>;

                for (var i in res) {
                    //var dateCreated = new Date(Date.parse(res[i].dateCreated, "dd/MM/yyyy"));
                    var dateFormat = 'DD/MM/YYYY HH:mm';
                    var dateCreated = new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat);
                    //moment("22/10/2018 10:30", "DD/MM/YYYY HH:mm").format("DD/MM/YYYY HH:mm");
                    

                    output += '<tr ' + 'id="' + 'testGroup' + res[i].testGroupId + '"' + '>' +
                        '<td class="text-center" style="font-weight: 700;">' + res[i].testGroupId + '</td>' +
                        '<td class="table-actions-col text-center">' +
                        '<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/TestGroups/Edit/") %>' + res[i].testGroupId + '"><i class="fa fa-edit"></i></a> ';
                        if (currentRG == res[i].fkResearchGroup) {
                            output += '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].testGroupId + ')' + '"><i class="fa fa-trash"></i></div> ' + '</td>';
                        }
                     output += '<td style="font-weight: 700;">' + res[i].testGroupName + '</td>' +
                               /* '<td>' + res[i].testGroupGoal + '</td>' +   */                             
                                '<td>' + res[i].projectAcronym + '</td>' +
                                '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                                '<td>' + res[i].researchGroupName + '</td>' +
                                '<td>' + res[i].operatorUsername + '</td>' +                               
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

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    //newRequestObj.researchGroupId = null;
                    newRequestObj.testGroupId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetAllTestGroupsList",
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
                newRequestObj.testGroupId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                swal({
                    title: "Delete test group?",
                    text: "Are you sure you want to delete the test group, you will not be able to restore the data!?",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true

                }).then((result) => {
                    if (result) {
                        $.ajax({
                            type: "POST",
                            url: "Default.aspx/DeleteTestGroup",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: requestDataString,
                            success: function (result) {
                                var jsonResult = JSON.parse(result.d);
                                if (jsonResult.status == "ok") {
                                    notify("Success!", "success");
                                    $("#testGroup" + elementId).remove();
                                } else {
                                    notify(jsonResult.message, "warning");
                                }
                            },
                            error: function (p1, p2, p3) {
                                notify(p2 + " - " + p3, "danger");
                            }
                        });
                    }
                });
            }




            //function OpenDeleteForm(elementId) {

            //    var newRequestObj = new Object();
            //    newRequestObj.testGroupId = elementId;
            //    var requestDataString = JSON.stringify(newRequestObj);

            //    bootbox.confirm({
            //        title: "Delete test group",
            //        message: "Are you sure you want to delete the test group?",
            //        buttons: {
            //            cancel: {
            //                label: 'Cancel'
            //            },
            //            confirm: {
            //                label: 'Confirm'
            //            }
            //        },
            //        callback: function (result) {
            //            if (result) {
            //                $.ajax({
            //                    type: "POST",
            //                    url: "Default.aspx/DeleteTestGroup",
            //                    async: true,
            //                    contentType: "application/json; charset=utf-8",
            //                    dataType: "json",
            //                    data: requestDataString,
            //                    success: function (result) {
            //                        var jsonResult = JSON.parse(result.d);
            //                        if (jsonResult.status == "ok") {
            //                            notify("Success!", "success");
            //                            $("#testGroup" + elementId).remove();
            //                        } else {
            //                            notify(jsonResult.message, "warning");
            //                        }
            //                    },
            //                    error: function (p1, p2, p3) {
            //                        notify(p2 + " - " + p3, "danger");
            //                    }
            //                });
            //            }
            //        }
            //    });
            //}
        </script>
    </section>
</asp:Content>
