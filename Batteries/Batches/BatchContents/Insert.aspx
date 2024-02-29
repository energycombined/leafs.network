<%@ Page Title="Batch Content" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Batches.BatchContents.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div id="GeneralInfo" class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Batch General Info</h3>
                        <%--<legend>Batch General Info</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label for="batchSystemLabel" class="col-sm-2 control-label">System Label</label>
                                <div class="col-sm-10">
                                    <input name="batchSystemLabel" type="text" id="batchSystemLabel" disabled="true" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="batchDateCreated" class="col-sm-2 control-label">Date Created</label>
                                <div class="col-sm-10">
                                    <input name="batchDateCreated" type="text" id="batchDateCreated" disabled="true" class="form-control">
                                </div>
                            </div>
                            <div id="TemplateBatchField" class="form-group">
                                <asp:Label ID="LblTemplateBatchSystemLabel" Visible="false" runat="server" AssociatedControlID="TxtTemplateBatchSystemLabel" CssClass="col-sm-2 control-label" Text="Template chosen"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtTemplateBatchSystemLabel" Visible="false" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtLabel" CssClass="col-sm-2 control-label" Text="Batch Label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtLabel"
                                        CssClass="text-danger" ErrorMessage="Batch label is required." Display="Dynamic" />
                                </div>--%>
                                <label for="batchPersonalLabel" class="col-sm-2 control-label">Personal Label</label>
                                <div class="col-sm-10">
                                    <input name="batchPersonalLabel" type="text" id="batchPersonalLabel" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtDescription" CssClass="col-sm-2 control-label" Text="Description"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>--%>

                                <label for="batchDescription" class="col-sm-2 control-label">Description</label>
                                <div class="col-sm-10">
                                    <input name="batchDescription" type="text" id="batchDescription" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="batchChemicalFormula" class="col-sm-2 control-label">Chemical Formula</label>
                                <div class="col-sm-10">
                                    <input name="batchChemicalFormula" type="text" id="batchChemicalFormula" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="DdlMeasurementUnit" CssClass="col-sm-2 control-label" Text="Measurement Unit"></asp:Label>--%>
                                <label for="selectMeasurementUnit" class="col-sm-2 control-label">Measurement Unit</label>
                                <div class="col-sm-10">
                                    <%--<asp:DropDownList ID="DdlMeasurementUnit" runat="server" ItemType="" DataTextField="measurementUnitName" DataValueField="measurementUnitId" CssClass="form-control select2">
                                    </asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMeasurementUnit"
                                        CssClass="text-danger" ErrorMessage="Measurement unit is required." Display="Dynamic" />--%>
                                    <select id="selectMeasurementUnit" class="measurement-unit-data-ajax"></select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="totalBatchOutput" class="col-sm-2 control-label">Total Batch Output</label>
                                <div class="col-sm-10">
                                    <input name="totalBatchOutput" type="number" id="totalBatchOutput" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="batchOutput" class="col-sm-2 control-label">Useable Batch Output</label>
                                <div class="col-sm-10">
                                    <input name="batchOutput" type="number" id="batchOutput" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="wasteAmount" class="col-sm-2 control-label">Waste Amount</label>
                                <div class="col-sm-10">
                                    <input name="wasteAmount" type="number" id="wasteAmount" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="releasedAs" class="col-sm-2 control-label">Released as liquid, solid or gas</label>
                                <div class="col-sm-10">
                                    <input name="releasedAs" type="text" id="releasedAs" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="wasteChemicalComposition" class="col-sm-2 control-label">Waste chemical composition</label>
                                <div class="col-sm-10">
                                    <input name="wasteChemicalComposition" type="text" id="wasteChemicalComposition" class="form-control">
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="wasteComment" class="col-sm-2 control-label">Waste Comment</label>
                                <div class="col-sm-10">
                                    <input name="wasteComment" type="text" id="wasteComment" class="form-control">
                                </div>
                            </div>
                            <%--fkMeasurementUnit--%>
                            <%--fkMaterialType--%>
                            
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="DdlMaterialType" CssClass="col-sm-2 control-label" Text="Material Type"></asp:Label>--%>
                                <label for="selectMaterialType" class="col-sm-2 control-label">Material Type</label>
                                <div class="col-sm-10">
                                    <select id="selectMaterialType" class="material-type-data-ajax"></select>
                                    <%--<asp:DropDownList ID="DdlMaterialType" runat="server" ItemType="Batteries.Models.MaterialType" DataTextField="MaterialType" DataValueField="MaterialTypeId" CssClass="form-control select2">
                                    </asp:DropDownList>--%>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialType"
                                        CssClass="text-danger" ErrorMessage="Material type is required." Display="Dynamic" />--%>
                                </div>
                            </div>                            
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer no-border">
                            <div class="pull-right batchGeneralInfoFooterButtons">
                                <span class="lastSave">Last save: <i></i>   </span>
                                <input type="button" id="submitBatchGeneralDataBtn" value="Save" title="Temporary save" class="btn btn-default" onclick="SubmitBatchGeneralData(<%=batchId%>)" />
                                
                            </div>
                        </div>
                        <!-- /.box-footer -->
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
                            <div class="pull-right batchFooterButtons">
                                <%--<input type="button" id="" value="Add step" onclick="AddNewStep('Anode')" class="newStepBtn btn btn-success" />
                                <input type="button" id="submitAnodeButton" value="Done" onclick="SubmitComponent('Anode')" class="submitComponentBtn btn btn-default" />
                                <input type="button" id="" value="Done" onclick="SubmitComponentCommercalType('Anode')" class="submitComponentCommercialBtn btn btn-default hidden" />
                                <span class="componentSaved hidden"><i class="fa fa-fw fa-check"></i>Saved</span>--%>
                                <span class="lastSave">Last save: <i></i>   </span>
                                <input type="button" id="submitBatchContentBtn" value="Save" title="Temporary save" class="btn btn-default" onclick="SubmitBatchContent(<%=batchId%>)" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box box-solid">

                    <!-- form start -->
                    <fieldset class="form-horizontal">

                        <!-- /.box-body -->
                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="cancelBatchBtn" value="Cancel" class="btn btn-default" />--%>
                                <%--<asp:Button runat="server" ID="cancelBatchBtn" Text="Cancel" CssClass="btn btn-default" OnClick="BtnCancel_Click" />--%>
                                <%--<input type="button" id="submitBatchBtn" value="Save batch" class="btn btn-success" onclick="SubmitBatchEdit(<%=batchId%>)" />--%>
                                <input type="button" id="discardBatchBtn" value="Discard batch" class="btn btn-danger" onclick="OpenDeleteForm(<%=batchId%>)" />
                                <input type="button" title="Save everything and finish batch" id="finishBatchBtn" value="Finish batch" class="btn btn-lg btn-default pull-right" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>
        <%--FinishBatchCreation(<%=batchId%>, editing)--%>
        <script src="<%= ResolveUrl("~/Scripts/html5sortable/html5sortable.js")%>"></script>
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/batch.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/jquery.validate/jquery.validate.min.js")%>"></script>
        
        <script type="text/javascript">
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //batch.js
            viewMode = false;

            var materialFunctionsJson = <%=materialFunctionsJson%>;
            var batchId;
            var batch;
            $(function () {
                batchId = <%= batchId %>;
                batch = <%= batch %>;
                var editing = false;
                if(batch.batchInfo.fkEditedBy != null){
                    editing = true;
                    var saveInfoAndFinishBtn = '<input type="button" id="submitGeneralDataAndFinishBtn" value="Save and Close" title="Save general info and close editing!" class="btn btn-default" onclick="SubmitBatchGeneralDataAndFinish(' + batchId + ')" />';
                    //Append the edit info and close batch button
                    $('.batchGeneralInfoFooterButtons').append(saveInfoAndFinishBtn);
                }
                //else{
                    
                //    $('#submitGeneralDataAndFinishBtn').remove();
                //}

                $("#finishBatchBtn").attr('onclick', 'FinishBatchCreation(' + batchId + ', ' + editing +')');
                ShowBatch(batch, false);
                
                setTimeout(function(){
                    ExpandBatchBox();
                }, 50);
            });

            function OpenDeleteForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.batchId = elementId;
                //newRequestObj.researchGroupId = null;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Discard batch",
                    message: "Are you sure you want to discard the batch?",
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
                                url: "/Helpers/WebMethods.asmx/DiscardBatch",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        window.location.replace("/Batches/");
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
