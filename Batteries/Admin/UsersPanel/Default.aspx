<%@ Page Title="Users List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Admin.UsersPanel.Default" %>

<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Users List</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New user" />
        </p>
        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <!--<div class="box-header">
                      <h3 class="box-title">Filter</h3>
                    </div>-->
                    <div class="box-body">
                        <div class="row">
                            <div class="col-sm-9">

                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="input-group">
                                            <span class="input-group-addon">Role</span>
                                            <asp:DropDownList ID="ddlRole" data-placeholder="Select Role" runat="server" AppendDataBoundItems="true" CssClass="form-control select2"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="input-group">
                                            <span class="input-group-addon">Research Group</span>
                                            <asp:DropDownList ID="ddlResearchGroup" data-placeholder="Select Research Group" runat="server" AppendDataBoundItems="true" CssClass="form-control select2"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3">
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
                    <%-- <div class="box-header">
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
                                                    <th>Username
                                                    </th>
                                                    <th>First Name
                                                    </th>
                                                    <th>Last Name
                                                    </th>
                                                    <th>Telephone
                                                    </th>
                                                    <th>E-mail
                                                    </th>
                                                    <%--<th>Active
                                                    </th>--%>
                                                    <th>Role
                                                    </th>
                                                    <th>Research group
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

        <%--<script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/jquery.dataTables.min.js")%>"></script>--%>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>

        <script type="text/javascript">
            var currentUserId;
            var theTable;

            function CreateUsersTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    "columnDefs": [{
                        "targets": 7,
                        "orderable": false
                    }],
                    //"order": [[0, "desc"]]
                    "order": []
                });
            }
            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";

                for (var i in res) {
                    output += '<tr ' + 'id="' + 'user' + res[i].userId + '"' + '>' +
                        '<td>' + res[i].userName + '</td>' +
                        '<td>' + res[i].firstName + '</td>' +
                        '<td>' + res[i].lastName + '</td>' +
                        '<td>' + res[i].phone + '</td>' +
                        '<td>' + res[i].email + '</td>' +
                        //'<td>' + res[i].active + '</td>' +
                        '<td>' + res[i].userRole.roleName + '</td>' +
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>';

                    //'<td>' + (res[i].lastLogin != null ? moment(res[i].lastLogin).format("DD.MM.YYYY hh:mm:ss") : "") + '</td>' +
                    output += '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Admin/UsersPanel/Edit/") %>' + res[i].userId + '">Edit</a> ';

                    if (currentUserId != res[i].userId) {
                        output += '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].userId + ')' + '">Delete</div> ';
                    }

                    output += ' </td>';

                    output += '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateUsersTable();
            }
            function OpenDeleteForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.userId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Delete user",
                    message: "Are you sure you want to delete user?",
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
                                url: "Default.aspx/DeleteUser",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        notify("User deleted!", "info");
                                        $("#user" + elementId).remove();
                                    } else {
                                        //notify(jsonResult.message, "warning");
                                        notify("User cannot be deleted!", "warning");
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

            $(function () {
                currentUserId = <%= currentUserId %>;
                CreateUsersTable();

                $('.select2').select2({
                    width: "100%",
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('#filter-btn').click(function () {

                    var newRequestObj = new Object();
                    newRequestObj.roleid = $('#<%= ddlRole.ClientID %>').val();
                    newRequestObj.researchGroupId = $('#<%= ddlResearchGroup.ClientID %>').val();

                    var RequestDataString = JSON.stringify(newRequestObj);
                    //alert(RequestDataString);

                    $.ajax({
                        type: "POST",
                        url: "Default.aspx/FilterList",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //alert(result.d);
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
