<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddResearchGroupToProject.aspx.cs" Inherits="Batteries.Projects.AddResearchGroupToProject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <h2>Research Groups to Project</h2>
        <hr />
        <p>
            <asp:HyperLink runat="server" NavigateUrl="EditPRG" CssClass="btn btn-primary" Text="Add research groups to project" />
        </p>
        <button type="button" id="filter-btn" class="btn btn-success pull-right hidden" data-toggle="tooltip" data-placement="left"><span class="glyphicon glyphicon-filter"></span>Filter</button>


        <div class="row">
            <div class="col-xs-12">
                <div class="box">
                    <div class="box-body">
                        <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive">
                                        <table id="listDataTable" class="table table-bordered table-hover table-striped">
                                            <thead>
                                                <tr>
                                                    <th>Project Name
                                                    </th>
                                                    <th>Research Group Name
                                                    </th>
                                                    <th>Date created
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
            var currentUserId;


           
            function CreateDataTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,
                    "columnDefs": [{
                        "targets": 3,
                        "orderable": false
                    }],
                    "order": [[2, "desc"]]
                });
            }
            function RefreshTableData(data) {
                theTable.destroy();


                var res = JSON.parse(data);
                var output = "";


                for (var i in res) {
                    //var dateCreated = new Date(Date.parse(res[i].dateCreated, "dd/MM/yyyy"));
                    var dateFormat = 'DD/MM/YYYY HH:mm';
                    var dateCreated = new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat);
                    //moment("22/10/2018 10:30", "DD/MM/YYYY HH:mm").format("DD/MM/YYYY HH:mm");


                    output += '<tr ' + 'id="' + 'projectRG' + res[i].projectResearchGroupId + '"' + '>' +
                        '<td>' + res[i].projectName + '</td>' +
                        '<td>' + res[i].researchGroupName + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +                
                        '<td class="table-actions-col">' + ' ' +
                        '<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].projectResearchGroupId + ')' + '">Delete</div> ' +
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
                    newRequestObj.researchGroupId = null;
                    newRequestObj.projectResearchGroupId = null;

                    var RequestDataString = JSON.stringify(newRequestObj);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetAllProjectsRGList",
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
                newRequestObj.projectResearchGroupId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                swal({
                    title: "Delete project/research group!",
                    text: "Are you sure you want to delete the environment type, you will not be able to restore the data!?",
                    icon: "warning",
                    buttons: true,
                    dangerMode: true

                }).then((result) => {
                    if (result) {
                        $.ajax({
                            type: "POST",
                            url: "AddResearchGroupToProject.aspx/DeleteProject",
                            async: true,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: requestDataString,
                            success: function (result) {
                                var jsonResult = JSON.parse(result.d);
                                if (jsonResult.status == "ok") {
                                    notify("Success!", "success");
                                    $("#projectRG" + elementId).remove();
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
            };

            
        </script>
    </section>
</asp:Content>
