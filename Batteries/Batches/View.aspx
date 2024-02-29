<%@ Page Title="Batch View" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Batteries.Batches.View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Batch General Info</h3>
                        <%--<legend>Batch General Info</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">

                        <div class="box-body table-responsive no-padding">
                            <table id="batchGeneralInfo" class="table table-hover">
                                <tbody>
                                    <%--<tr>
                                        <td>183</td>
                                        <td>John Doe</td>
                                        <td>11-7-2014</td>
                                        <td><span class="label label-success">Approved</span></td>
                                        <td>Bacon ipsum dolor sit amet salami venison chicken flank fatback doner.</td>
                                    </tr>--%>
                                </tbody>
                            </table>
                        </div>


                        <!-- /.box-body -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div class="row batchContentsRow">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div id="Batch" class="box box-warning collapsed-box">
                    <%--collapsed-box--%>
                    <div class="box-header with-border">
                        <h3 class="box-title">Batch Contents</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <input type="button" onclick="ShowFileAttachments('Batch', <%=batchId%>, 1, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Edit batch documents" value="Documents" />

                            <%--<input type="button" value="Edit" onclick="UnlockComponentEditing('Anode')" class="btn btn-xs btn-default editComponentButton hidden" />--%>
                            <button type="button" class="btn btn-xs btn-default startBatchOverButton hidden" onclick="StartBatchOver()">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>

                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <%--<input type="button" id="" value="Add step" onclick="AddNewStep('Anode')" class="newStepBtn btn btn-success" />
                                <input type="button" id="submitAnodeButton" value="Done" onclick="SubmitComponent('Anode')" class="submitComponentBtn btn btn-default" />
                                <input type="button" id="" value="Done" onclick="SubmitComponentCommercalType('Anode')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <%--<div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box box-solid">

                    <!-- form start -->
                        <!-- /.box-body -->
                        <div class="box-footer no-border">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="RecreateBatchBtn" Text="Recreate batch" CssClass="btn btn-success" OnClick="BtnRecreateBatch_Click" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                </div>
                <!-- /.box -->
            </div>
        </div>--%>

        <script src="<%= ResolveUrl("~/Scripts/html5sortable/html5sortable.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/batch.js")%>"></script>
        <script type="text/javascript">

            formsFolderURL = "<%= ResolveClientUrl("~/Forms/")%>"; //batch.js
            viewMode = true; //batch.js

            $(function () {
                var batchId = <%= batchId %>;
                var batch = <%= batch %>;
                //batch = GetBatchWithContent(batchId);
                ShowBatch(batch, true);
                setTimeout(function(){
                    SetBatchToViewMode();
                }, 50);

                $('#recreateBatchBtn').click(function () {
                    RecreateBatch(batchId);
                });
            });
        </script>
    </section>
</asp:Content>
