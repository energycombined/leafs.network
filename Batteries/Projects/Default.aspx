<%@ Page Title="Projects" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Batteries.Projects.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h3 style="font-weight: 700; color: grey;">Projects List</h3>
        <p>
            <asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="New project"><i class="fa fa-plus"></i> New project</asp:HyperLink>
            <%--<asp:HyperLink runat="server" NavigateUrl="AddResearchGroupToProject" CssClass="btn btn-info" Text="Add Research Groups To Project" />--%>
            <%--<asp:HyperLink runat="server" NavigateUrl="AddTestGroupToProject" CssClass="btn btn-warning" Text="Add Test Groups To Project" />--%>
        </p>
        <button type="button" id="filter-btn" class="btn btn-success pull-right hidden" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>

        <div class="row">
            <div class="col-xs-12">
                <div class="box box-warning">
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover table-striped text-center" style="width: 100%">
                                            <thead style="background-color: darkorange;" class="text-white">
                                                <tr>
                                                    <th class="text-center">Action
                                                    </th>
                                                    <th>Project Name
                                                    </th>
                                                    <th>Created by
                                                    </th>
                                                    <th>Technical coordinator
                                                    </th>
                                                    <th>Acronym
                                                    </th>
                                                    <th>Start project
                                                    </th>
                                                    <th>End project
                                                    </th>
                                                    <th>Research Group Name
                                                    </th>
                                                    <th>Date created
                                                    </th>
                                                    <th>Edited by
                                                    </th>
                                                    <th>Date edited
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
            var currentUserId;

            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    "columnDefs": [{
                        "width": "8%", "targets": 8,

                        "orderable": false
                    }],
                    //"order": [[6, "desc"]]
                    "order": []
                });
            }
            function RefreshTableData(data) {
                theTable.destroy();
                if (data != null) {
                    var currentRG = <%= currentRG %>;
                    var res = JSON.parse(data);
                    var output = "";

                    for (var i in res) {

                    var dateFormat = 'DD/MM/YYYY HH:mm';
                    var dateFormat2 = 'DD/MM/YYYY';
                    var startProject = res[i].startProject ? new moment.utc(res[i].startProject, dateFormat).local().format(dateFormat) : '#';
                    var endProject = res[i].endProject ? new moment.utc(res[i].endProject, dateFormat2).local().format(dateFormat2) : '#';
                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var lastChange = res[i].lastChange ? new moment.utc(res[i].lastChange, dateFormat).local().format(dateFormat) : '#';
                    //moment("22/10/2018 10:30", "DD/MM/YYYY HH:mm").format("DD/MM/YYYY HH:mm");

                    output += '<tr ' + 'id="' + 'project' + res[i].projectId + '"' + '>' +
                        '<td class="table-actions-col text-center">' +
                        '<a title="Edit" class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Projects/Edit/") %>' + res[i].projectId + '"><i class="fa fa-edit"></i></a> ' +
                        '<a title="Project participant" class="btn btn-warning btn-xs" href="<%= Page.ResolveUrl("~/Projects/ProjectResearchGroups/") %>' + res[i].projectId + '"><i class="fa fa-users"></i></a> ' + '</td>' +
                        '<td style="font-weight: 700;">' + res[i].projectName + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].technicalCoordinator + '</td>' +
                        '<td>' + res[i].projectAcronym + '</td>' +
                        '<td data-sort="' + moment(res[i].startProject, 'DD/MM/YYYY') + '">' + startProject + '</td>';
                    if (moment(res[i].endProject, 'DD/MM/YYYY') < moment())
                        output += '<td style="background-color:rgba(255, 0, 0, 0.2); font-weight: 700;" data-sort="' + moment(res[i].endProject, 'DD/MM/YYYY') + '">' + endProject + '</td>';
                    else
                        output += '<td data-sort="' + moment(res[i].endProject, 'DD/MM/YYYY') + '">' + endProject + '</td>';
                    output +=
                        '<td>' + res[i].researchGroupAcronym + " - " + res[i].researchGroupName + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td>' + res[i].editingOperatorUsername + '</td>' +
                        '<td data-sort="' + moment(res[i].lastChange, 'DD/MM/YYYY HH:mm') + '">' + lastChange + '</td>' +
                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                    CreateDataTable();
                }
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
                    newRequestObj.projectId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetAllProjectsList",
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
