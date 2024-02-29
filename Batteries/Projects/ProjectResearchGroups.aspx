<%@ Page Title="Add Participants" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProjectResearchGroups.aspx.cs" Inherits="Batteries.Projects.ProjectResearchGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box box-warning">
                    <div class="box-header">
                        <h3 class="box-title">Add Participants to Project - <b><%= projectAcronym%></b></h3>
                        <div class="pull-right">
                            <input type="button" id="addRGroupBtn" value="Add Research group to project" class="btn btn-success" />
                            <asp:Button runat="server" ID="BtnCancel" Text="Back to List of Projects" CausesValidation="false" CssClass="btn btn-primary" OnClick="CancelButton_OnClick" />
                        </div>
                    </div>
                </div>
                <!-- /.box-header -->

                <div id="example2_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                    <div class="box box-primary">
                        <div class="box-body">
                            <div class="col-lg-12">
                                <div class="table-responsive">
                                    <table id="listDataTable2" class="table text-center table-bordered table-hover">
                                        <thead>
                                            <tr>
                                                <th>Research Group</th>
                                                <th>Date Added</th>
                                                <%--<th>Action</th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <%--  End row--%>
                    </div>
                </div>
            </div>
        </div>

        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>

        <script type="text/javascript">

            var theTable2;

            function CreateBatchTable() {

                // DataTable
                theTable2 = $('#listDataTable2').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 1,

                            "orderable": false
                        },
                        { "type": "de_datetime", targets: [1] },
                    ],
                    "order": [[1, "desc"]]
                });
            }

            function RefreshTableData2(data) {

                theTable2.destroy();
                var projectRG = <%= projectRG %>;
                var res2 = JSON.parse(data);
                var output2 = "";

                for (var i in res2) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    /* var fkComingExperiment = res2[i].fkExperiment ? res2[i].fkExperiment : '#';*/
                    var html = '<tr ' + 'id="' + 'projectRGroups' + res2[i].projectResearchGroupId + '"'
                        + 'class="';
                    if (res2[i].fkResearchGroup == res2[i].fkResearchGroupCreator)
                        html += 'bg-gray';
                    html += '"'
                        + '>' +

                        '<td>' + res2[i].researchGroupAcronym + " - " + res2[i].researchGroupName + '</td>' +
                        '<td>' + res2[i].dateCreated + '</td>' +
                        //'<td class="table-actions-col text-center">';
                        //if (projectRG != res2[i].fkResearchGroup)
                        //output2 += '<a title="Remove" class="btn btn-danger btn-xs remove disable" onclick="OpenDeleteRGInProjectForm(\'' + res2[i].projectResearchGroupId + '\')">' + '<span class="fa fa-remove"></span>' + '</a> ';
                        //output2 += '</td>'
                        '</tr>';

                    output2 += html;

                }
                $('#listDataTable2 tbody').html(output2);

                CreateBatchTable();
            }

            function GetRGroupData(projectId) {
                var newRequestObj = new Object();
                newRequestObj.projectId = projectId;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllRGInProject",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTableData2(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }

            //function refreshPage() {
            //    location.reload(true);
            //}

            //function OpenDeleteRGInProjectForm(elementId) {

            //    var newRequestObj = new Object();
            //    newRequestObj.projectResearchGroupId = elementId;
            //    var requestDataString = JSON.stringify(newRequestObj);

            //    swal({
            //        title: "Delete project/research group!",
            //        text: "Are you sure you want to delete the Research group, you will not be able to restore the data!?",
            //        icon: "warning",
            //        buttons: true,
            //        dangerMode: true

            //    }).then((result) => {
            //        if (result) {
            //            $.ajax({
            //                type: "POST",
            //                url: "/Helpers/WebMethods.asmx/DeleteRGFromProject",
            //                async: true,
            //                contentType: "application/json; charset=utf-8",
            //                dataType: "json",
            //                data: requestDataString,
            //                success: function (result) {
            //                    var jsonResult = JSON.parse(result.d);
            //                    if (jsonResult.status == "ok") {                                  
            //                        //setInterval('refreshPage()', 100);
            //                        notify("Success!", "success");
            //                        $("#projectRGroups" + elementId).remove();
            //                    } else {
            //                        notify(jsonResult.message, "warning");
            //                    }
            //                },
            //                error: function (p1, p2, p3) {
            //                    notify(p2 + " - " + p3, "danger");
            //                }
            //            });
            //            return true;
            //        }
            //    });
            //};

            function RefreshDataTableOnClick(data) {

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllRGInProject",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTableData2(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }

            $(function () {
                var projectId = <%= projectId%>;
              var researchGroupId = null;

              CreateBatchTable();
              GetRGroupData(projectId);


              $('#addRGroupBtn').click(function () {
                  var html = '<div class="form-horizontal">';
                  html += '<div class="form-group">';
                  html += '<div class="col-md-9">';
                  html += '<select id="selectRGroup" class="batch-data-ajax">    </select>';
                  html += '</div>';
                  html += '</div>';
                  html += '</div>';

                  var dialog = bootbox.dialog({
                      title: 'Add Research group',
                      message: html,
                      buttons: {
                          cancel: {
                              label: "Cancel",
                              className: 'btn-default',
                              callback: function () {

                              }
                          },
                          ok: {
                              label: "Add",
                              className: 'btn-primary',
                              callback: function () {
                                  var selectedData = $('#selectRGroup').select2('data')[0].data;


                                  var jsonRequestObject = {};
                                  jsonRequestObject["fkProject"] = projectId;
                                  jsonRequestObject["fkResearchGroup"] = selectedData.researchGroupId;
                                  //console.log(testGroupExperimentRequest);

                                  reqUrl = "/Helpers/WebMethods.asmx/AddProjectParticipant";
                                  var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });

                                  $.ajax({
                                      type: "POST",
                                      url: reqUrl,
                                      data: RequestDataString,
                                      //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                                      contentType: "application/json; charset=utf-8",
                                      dataType: "json",
                                      success: function (result) {
                                          var jsonResult = JSON.parse(result.d);
                                          if (jsonResult.status == "ok") {
                                              GetRGroupData(projectId);
                                              notify("Success", "info");
                                          }
                                          else {
                                              notify(jsonResult.message, "warning");
                                          }
                                      },
                                      error: function (p1, p2, p3) {
                                          alert(p1.status.toString() + " " + p3.toString());
                                      }
                                  });

                                  return true;
                              }
                          }
                      },
                      onEscape: true
                  });

                  dialog.on('shown.bs.modal', function () {
                      $("#selectRGroup").select2('open');
                  });

                  reqUrlBatches = "/Helpers/WebMethods.asmx/GetResearchGroupsOutsideProject";
                  $('#selectRGroup').select2({
                      ajax: {
                          type: "POST",
                          url: reqUrlBatches,
                          dataType: 'json',
                          contentType: "application/json; charset=utf-8",
                          delay: 250,
                          data: function (params) {
                              var term = "";
                              if (params.term) {
                                  var term = params.term;
                              }

                              return JSON.stringify({
                                  search: term,
                                  projectId: projectId,
                                  page: params.page || 1
                              });
                          },
                          processResults: function (data, params) {
                              var response = JSON.parse(data.d);
                              return {
                                  results: $.map(response.results, function (item) {
                                      return {
                                          id: item.researchGroupId,
                                          text: item.acronym + " - " + item.researchGroupName,
                                          data: item
                                      };
                                  }),
                                  pagination: {
                                      "more": response.pagination.more
                                  }
                              };
                          },
                          cache: true
                      },
                      dropdownParent: dialog,
                      width: "100%",
                      placeholder: 'Search for Research group',
                      escapeMarkup: function (markup) { return markup; },
                      templateResult: formatResearchGroup,
                  });
              });

              function formatResearchGroup(rGroup) {
                  //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                  var markup = rGroup.text;
                  //var markup = "" + batch.id + " | " + batch.text;
                  return markup;
              }

          });


        </script>
    </section>
</asp:Content>
