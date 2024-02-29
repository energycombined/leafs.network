<%@ Page Title="Research Groups" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.ResearchGroups.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>
<%@ Import Namespace="Batteries.Helpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Research Groups</h2>
        <hr />
        <p>
            <% if (UserHelper.IsAdmin())
                {%>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New research group" />
            <% } %>
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
                                                <th>Id
                                                </th>
                                                <th>Research group name
                                                </th>
                                                <th>Acronym
                                                </th>
                                                <th>Date created
                                                </th>
                                                <th>Last change
                                                </th>
                                                <th>Created by
                                                </th>
                                                <% if (UserHelper.IsAdmin())
                                                    {%>
                                                <th>Action
                                                </th>
                                                <% } %>
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
        <%--<script type="text/javascript" src="https://cdn.datatables.net/plug-ins/1.10.16/sorting/datetime-moment.js"></script>--%>

        <script type="text/javascript">
            var theTable;
            var isAdmin = <%= isAdmin.ToString().ToLower()%>;
            function CreateDataTable() {             
                // DataTable
                if (isAdmin == true) {
                    theTable = $('#listDataTable').DataTable({
                        //"scrollX": true,
                        //"autoWidth": true,
                        "columnDefs": [{
                            "targets": 6,
                            "orderable": false
                        }]
                    });
                }
                else if (isAdmin == false) {
                    theTable = $('#listDataTable').DataTable({
                        //"scrollX": true,
                        //"autoWidth": true,
                        "columnDefs": [{
                            "targets": 5,
                            "orderable": false
                        }]
                    });
                }
            }
            function RefreshTableData(data) {

                theTable.destroy();
                var isAdmin = <%=isAdmin.ToString().ToLower()%>;
                var res = JSON.parse(data);
                var output = "";

                var dateFormat = 'DD/MM/YYYY';

                for (var i in res) {
                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var lastChange = res[i].lastChange ? new moment.utc(res[i].lastChange, dateFormat).local().format(dateFormat) : '#';

                    output += '<tr ' + 'id="' + 'researchGroup' + res[i].researchGroupId + '"' + '>' +
                        '<td>' + res[i].researchGroupId + '</td>' +
                        '<td>' + res[i].researchGroupName + '</td>' +
                        '<td>' + res[i].acronym + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].lastChange, 'DD/MM/YYYY HH:mm') + '">' + lastChange + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>';
                        if(isAdmin) {
                            output +='<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/ResearchGroups/Edit/") %>' + res[i].researchGroupId + '">Edit</a> ' +
                            '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].researchGroupId + ')' + '">Delete</div> ' +

                            '</tr>';
                    }
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {
                //$.fn.dataTable.moment('DD/MM/YYYY HH:mm');
                CreateDataTable();

                $('#filter-btn').click(function () {
                    var newRequestObj = new Object();
                    newRequestObj.researchGroupId = null;

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

            function OpenDeleteForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.researchGroupId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete research group",
                    message: "Are you sure you want to delete the research group?",
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
                                url: "Default.aspx/DeleteResearchGroup",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("Success!", "success");
                                        $("#researchGroup" + elementId).remove();
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
