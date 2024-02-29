<%@ Page Title="Public Experiments" Language="C#" MasterPageFile="~/PublicExp.Master" AutoEventWireup="true" CodeBehind="PublicExperiments.aspx.cs" Inherits="Batteries.Experiments.PublicExperiments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css")%>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">       
        <div class="callout callout-warning col-md-4">
            <h4>Welcome to public experiments list!</h4>
            <p>Here you can see some of our experiments we shared....</p>
        </div>
        <br/>
        <div class="row">
            <div class="col-xs-12">
                <div class="box box-warning">
                    <!-- /.box-header -->
                    <div class="container-fluid" >
                        <div class="box-body">
                            <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <%--<span id="office"></span>--%>
                                        <div class="table-responsive">
                                            <table id="listDataTable" class="table table-bordered table-striped table-hover table-success text-center text-dark" style="width: 100%; box-shadow: 5px 10px 18px grey; background-color:azure; font-weight: 700;">
                                                <thead style="background-color:darkorange;" class="text-white">
                                                    <tr>
                                                        <th style="width:2%" class="text-center">Action
                                                        </th>
                                                        <th style="width:8%" class="text-center">System Label
                                                        </th>
                                                        <th style="width:8%" class="text-center">Personal Label
                                                        </th>
                                                        <th style="width:25%" class="text-center">Anode A.M.
                                                        </th>
                                                        <th style="width:25%" class="text-center">Cathode A.M.
                                                        </th>
                                                        <th style="width:8%" class="text-center">Research group
                                                        </th>
                                                        <th style="width:8%" class="text-center">Project
                                                        </th>
                                                        <th style="width:8%" class="text-center">Date Created
                                                        </th>
                                                        <th style="width:8%" class="text-center">Last Change
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
                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
        </div>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <script type="text/javascript">

            var theTable;

            function CreateDataTable() {

                //DataTable
                theTable = $('#listDataTable').DataTable({
                    'ordering': true,  // Column ordering,
                    'paging': true,  // Table pagination                    
                    "columnDefs": [
                        {
                           /* "width": "3%", "targets": 0,*/
                        }
                    ]  
                });
            }

            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var dateFormat = 'DD/MM/YYYY HH:mm';
               
                for (var i in res) {

                    var dateCreated = res[i].dateCreated ? new moment.utc(res[i].dateCreated, dateFormat).local().format(dateFormat) : '#';
                    var dateModified = res[i].dateModified ? new moment.utc(res[i].dateModified, dateFormat).local().format(dateFormat) : '#';
                    var anodeTotalActiveMaterials = res[i].anodeTotalActiveMaterials ? (Number(res[i].anodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].anodeActiveMaterials + ' = ' + res[i].anodeActivePercentages + ' %') : ' /';
                    var cathodeTotalActiveMaterials = res[i].cathodeTotalActiveMaterials ? (Number(res[i].cathodeTotalActiveMaterials).toPrecision(4) + ' g : ' + res[i].cathodeActiveMaterials + ' = ' + res[i].cathodeActivePercentages + ' %') : '/';
                            
                    output += '<tr ' + 'id="' + 'experiment' + res[i].experimentId + '"' + '>' +
                        '<td class="table-actions-col text-center">';

                    output += '<a title="View" class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Shared/PublicView/") %>' + res[i].experimentId + '"><i class="fa fa-eye"></i></a> ';

                    output += '</td>' +

                        '<td style="font-weight: 700;" data-sort="' + res[i].experimentId + '">' + res[i].experimentSystemLabel + '</td>' +
                        '<td>' + res[i].experimentPersonalLabel + '</td>' +
                        '<td>' + anodeTotalActiveMaterials + '</td>' +
                        '<td>' + cathodeTotalActiveMaterials + '</td>' +
                        '<td>' + res[i].researchGroupName + " " + res[i].researchGroupAcronym + '</td>' +
                        '<td style="font-weight: 700;">' + res[i].projectName + '</td>' +
                        '<td data-sort="' + moment(res[i].dateCreated, 'DD/MM/YYYY HH:mm') + '">' + dateCreated + '</td>' +
                        '<td data-sort="' + moment(res[i].dateModified, 'DD/MM/YYYY HH:mm') + '">' + dateModified + '</td>' +

                        '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateDataTable();
            }

            $(function () {

                CreateDataTable();
                var newRequestObj = new Object();
                newRequestObj.experimentId = null;
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllPublicExperimentsList",
                    data: RequestDataString,
                    async: true,
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
        </script>
    </section>
</asp:Content>
