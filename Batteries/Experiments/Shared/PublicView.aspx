<%@ Page Title="" Language="C#" MasterPageFile="~/PublicExp.Master" AutoEventWireup="true" CodeBehind="PublicView.aspx.cs" Inherits="Batteries.Experiments.Shared.PublicView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <section class="content">
        <div id="GeneralInfo" class="row">
            <div class="col-lg-8 text-white col-lg-offset-2 connectedSortable">
                <div class="box bg-blue-gradient">
                    <div class="box-header with-border">
                        <h3 class="box-title text-white"><i class="fa fa-th"></i> Experiment General Info</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body table-responsive no-padding">
                            <table id="experimentGeneralInfo" class="table ">
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div class="row experimentContentsRow connectedSortable">
            <div class="col-lg-8 col-lg-offset-2">
                <div id="Anode" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Anode</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 1, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>

                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

                <div id="Cathode" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Cathode</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 2, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

                <div id="Separator" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Separator</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 3, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

                <div id="Electrolyte" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Electrolyte</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 4, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

                <div id="ReferenceElectrode" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Reference electrode</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 5, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

                <div id="Casing" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Casing</h3>
                        <div class="box-tools pull-right">
                            
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <div onclick="ShowFileAttachments('Component', <%=experimentId %>, 6, 0)" class="btn btn-xs btn-default documentsButton">Documents</div>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>


        <script src="<%= ResolveUrl("~/Scripts/html5sortable/html5sortable.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script>
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //experiment.js
            viewMode = true; //experiment.js
            sharedViewMode = true; //experiment.js

            $(function () {
                experimentId = <%= experimentId %>;
                var experiment = <%= experiment %>;
                //batch = GetBatchWithContent(batchId);
                ShowExperiment(experiment, true);
                //setTimeout(SetComponentsToViewMode(), 10000);
                setTimeout(function(){
                    SetComponentsToViewMode()
                }, 50);
               
            });
        </script>

    </section>
</asp:Content>
