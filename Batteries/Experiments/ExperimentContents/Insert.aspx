<%@ Page Title="New experiment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Experiments.ExperimentContents.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div id="GeneralInfo" class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Experiment General Info</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="projectName" class="col-sm-2 control-label">Project</label>
                                <div class="col-sm-10">
                                    <input name="projectName" type="text" id="projectName" disabled="true" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtExperimentSystemLabel" CssClass="col-sm-2 control-label" Text="System label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtExperimentSystemLabel" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                                <label for="experimentSystemLabel" class="col-sm-2 control-label">System label</label>
                                <div class="col-sm-10">
                                    <input name="experimentSystemLabel" type="text" id="experimentSystemLabel" disabled="true" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="experimentDateCreated" class="col-sm-2 control-label">Date Created</label>
                                <div class="col-sm-10">
                                    <input name="experimentDateCreated" type="text" id="experimentDateCreated" disabled="true" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="experimentCreatedBy" class="col-sm-2 control-label">Created by</label>
                                <div class="col-sm-10">
                                    <input name="experimentCreatedBy" type="text" id="experimentCreatedBy" disabled="true" class="form-control">
                                </div>
                            </div>

                            <div id="TemplateExperimentField" class="form-group">
                                <asp:Label ID="LblTemplateExperimentSystemLabel" Visible="false" runat="server" AssociatedControlID="TxtTemplateExperimentSystemLabel" CssClass="col-sm-2 control-label" Text="Template chosen"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtTemplateExperimentSystemLabel" Visible="false" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtExperimentPersonalLabel" CssClass="col-sm-2 control-label" Text="Personal label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtExperimentPersonalLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                                <label for="experimentPersonalLabel" class="col-sm-2 control-label">Personal Label</label>
                                <div class="col-sm-10">
                                    <input name="experimentPersonalLabel" type="text" id="experimentPersonalLabel" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtExperimentDescription" CssClass="col-sm-2 control-label" Text="Description"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtExperimentDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>
                                <label for="experimentDescription" class="col-sm-2 control-label">Description</label>
                                <div class="col-sm-10">
                                    <input name="experimentDescription" type="text" id="experimentDescription" class="form-control">
                                </div>
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right experimentGeneralInfoFooterButtons">
                                <span class="lastSave">Last save: <i></i></span>
                                <input type="button" id="submitExperimentGeneralDataBtn" value="Save" title="Temporary save" class="btn btn-default" onclick="SubmitExperimentGeneralData(<%=experimentId%>)" />

                                <%--<input type="button" value="Proceeed to content" onclick="UpdateExperimentInfo()" class="newStepBtn btn btn-primary" />--%>
                                <%--<asp:Button runat="server" ID="UpdateExperimentInfoButton" Text="Update General Info" CssClass="btn btn-success" OnClick="UpdateExperimentInfoButton_OnClick" />--%>
                            </div>

                            <div class="pull-left">
                                <%--<input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 6, 0)" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Edit Experiment Documents" />--%>
                            </div>

                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div class="row experimentContentsRow">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div id="Anode" class="box box-warning collapsed-box batteryComponent">
                    <div class="box-header with-border">
                        <h3 class="box-title">Anode</h3>
                        <div class="box-tools pull-right">

                            <button type="button" class="btn btn-box-tool component-box-tool" data-widget="collapse">
                                <i class="fa fa-plus"></i>
                            </button>
                        </div>
                        <div class="boxButtons pull-right">
                            <%--<a class="btn btn-primary btn-xs addMeasurementsComponentLevelBtn" data-item=" " onclick="AddMeasurements(this, 1)" > Add measurements</a>--%>
                            <%--<input type="button" value="Documents" onclick="ShowFileAttachmentsExperiment('Component', experimentId, 1, null)" class="btn btn-xs btn-default" />--%>
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 1, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />

                            <input type="button" onclick="ShowFileAttachmentsForAllSteps('Step', <%=experimentId%>, 1, 'Anode')" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Documents by step" />
                            <input type="button" value="Edit" onclick="UnlockComponentEditing('Anode')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <%--<input type="button" value="Start Over" onclick="StartComponentOver('Anode')" class="btn btn-xs btn-default startComponentOverButton hidden" />--%>
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('Anode')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>

                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('Anode')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('Anode')" class="previousTypeBtn btn btn-default" />
                            </div>

                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <input type="button" value="Add step" onclick="AddNewStep('Anode')" class="newStepBtn btn btn-primary" />
                                <input type="button" id="submitAnodeButton" value="Save" title="Save Component" onclick="SubmitComponent('Anode', <%=experimentId%>)" class="submitComponentBtn btn btn-default" />
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('Anode')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
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
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 2, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />
                            <input type="button" onclick="ShowFileAttachmentsForAllSteps('Step', <%=experimentId%>, 2, 'Cathode')" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Documents by step" />
                            <input type="button" value="Edit" onclick="UnlockComponentEditing('Cathode')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('Cathode')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('Cathode')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('Cathode')" class="previousTypeBtn btn btn-default" />
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <input type="button" value="Add step" onclick="AddNewStep('Cathode')" class="newStepBtn btn btn-primary" />
                                <input type="button" id="submitCathodeButton" value="Save" title="Save Component" onclick="SubmitComponent('Cathode',<%=experimentId%>)" class="submitComponentBtn btn btn-default" />
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('Cathode')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
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
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 3, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />
                            <input type="button" onclick="ShowFileAttachmentsForAllSteps('Step', <%=experimentId%>, 3, 'Separator')" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Documents by step" />
                            <input type="button" value="Edit" onclick="UnlockComponentEditing('Separator')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('Separator')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('Separator')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('Separator')" class="previousTypeBtn btn btn-default" />
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <input type="button" value="Add step" onclick="AddNewStep('Separator')" class="newStepBtn btn btn-primary" />
                                <input type="button" id="submitSeparatorButton" value="Save" title="Save Component" onclick="SubmitComponent('Separator', <%=experimentId%>)" class="submitComponentBtn btn btn-default" />
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('Separator')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
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
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 4, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />
                            <input type="button" onclick="ShowFileAttachmentsForAllSteps('Step', <%=experimentId%>, 4, 'Electrolyte')" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Documents by step" />
                            <input type="button" value="Edit" onclick="UnlockComponentEditing('Electrolyte')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('Electrolyte')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('Electrolyte')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('Electrolyte')" class="previousTypeBtn btn btn-default" />
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <input type="button" value="Add step" onclick="AddNewStep('Electrolyte')" class="newStepBtn btn btn-primary" />
                                <input type="button" id="submitElectrolyteButton" value="Save" title="Save Component" onclick="SubmitComponent('Electrolyte', <%=experimentId%>)" class="submitComponentBtn btn btn-default" />
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('Electrolyte')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
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
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 5, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />

                            <input type="button" value="Edit" onclick="UnlockComponentEditing('ReferenceElectrode')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('ReferenceElectrode')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('ReferenceElectrode')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('ReferenceElectrode')" class="previousTypeBtn btn btn-default" />
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <%--<input type="button" value="Add step" onclick="AddNewStep('ReferenceElectrode')" class="newStepBtn btn btn-primary" />--%>
                                <%--<input type="button" id="submitReferenceElectrodeButton" value="Done" onclick="SubmitComponent('ReferenceElectrode')" class="submitComponentBtn btn btn-default" />--%>
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('ReferenceElectrode')" class="submitComponentCommercialBtn btn btn-default" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
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
                            <input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 6, 0)" class="btn btn-xs btn-default documentsButton" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" />
                            <input type="button" onclick="ShowFileAttachmentsForAllSteps('Step', <%=experimentId%>, 6, 'Casing')" class="btn btn-xs btn-default experimentDocumentsButton" data-toggle="tooltip" title="" value="Documents by step" />
                            <input type="button" value="Edit" onclick="UnlockComponentEditing('Casing')" class="btn btn-xs btn-default editComponentButton hidden" />
                            <button type="button" class="btn btn-xs btn-default startComponentOverButton hidden" onclick="StartComponentOver('Casing')">
                                <i class="fa fa-refresh"></i>Start Over
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->

                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="quickSelectButtons">
                                <input type="button" value="Select prefab type" onclick="SelectComponentCommercialTypePopup('Casing')" class="commercialTypeBtn btn btn-default" />
                                <input type="button" value="Select from previous experiment" onclick="SelectComponentFromPreviousPopup('Casing')" class="previousTypeBtn btn btn-default" />
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right componentFooterButtons">
                                <input type="button" value="Add step" onclick="AddNewStep('Casing')" class="newStepBtn btn btn-primary" />
                                <input type="button" id="submitCasingButton" value="Save" title="Save Component" onclick="SubmitComponent('Casing', <%=experimentId%>)" class="submitComponentBtn btn btn-default" />
                                <input type="button" value="Save" title="Save Component" onclick="SubmitComponentCommercalType('Casing')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <%--FINISH EXPERIMENT ROW--%>
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box box-solid">
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->
                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="cancelBatchBtn" value="Cancel" class="btn btn-default" />--%>
                                <%--<input type="button" id="discardExperimentBtn" value="Discard experiment" class="btn btn-danger" onclick="OpenDeleteForm(<%=experimentId%>)" />--%>
                                <input type="button" id="finishExperimentBtn" value="Finish" title="Save everything and finish experiment" class="btn btn-block btn-default" />
                            </div>
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
        <script src="<%= ResolveUrl("~/Scripts/jquery.validate/jquery.validate.min.js")%>"></script>
        <script>
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //experiment.js
            viewMode = false; //experiment.js
        </script>
        <script>
            var materialFunctionsJson = <%=materialFunctionsJson%>;
            $(function () {
                experimentId = <%= experimentId %>;
                experiment = <%= experiment %>;
                var editing = false;
                
                if(experiment.experimentInfo.fkEditedBy != null){
                    editing = true;
                    var saveInfoAndFinishBtn = '<input type="button" id="submitGeneralDataAndFinishBtn" value="Save and Close" title="Save general info and close editing!" class="btn btn-default" onclick="SubmitExperimentGeneralDataAndFinish(' + experimentId + ')" />';
                    //Append the edit info and close batch button
                    $('.experimentGeneralInfoFooterButtons').append(saveInfoAndFinishBtn);
                }

                $("#finishExperimentBtn").attr('onclick', 'FinishExperimentCreation(' + experimentId + ', ' + editing +')');                
                ShowExperiment(experiment, false);

                
            });
        </script>

    </section>
</asp:Content>
