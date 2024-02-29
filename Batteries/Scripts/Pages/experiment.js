var formsFolderURL;
var alpacaForm;
var dialog;
var experimentId;
var projectId;

var viewMode = false;
var sharedViewMode = false;

var anodeStepNumber = 1;
var cathodeStepNumber = 1;
var separatorStepNumber = 1;
var electrolyteStepNumber = 1;
var referenceElectrodeStepNumber = 1;
var casingStepNumber = 1;

var ajaxResultsValidForSubmit = true;

var momentDateFormat = "DD/MM/YYYY";
var momentDateTimeFormat = "DD/MM/YYYY HH:mm";

function ToLocalDateTime(date) {
    return new moment.utc(date, momentDateTimeFormat).local().format(momentDateTimeFormat);
}
function ToLocalDate(date) {
    return new moment.utc(date, momentDateFormat).local().format(momentDateFormat);
}

function guid(type) {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    if (type != "short")
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    else
        return 'i' + s4() + s4();
}

function MakeTablesSortable(componentType) {
    //this is called every time a component is finished regenerating and after appending a new row
    //console.log('make sortable first time for ' + componentType);
    sortable('#' + componentType + ' .js-sortable-table', {
        items: "tr.js-sortable-tr",
        handle: '.js-handle',
        placeholder: "<tr><td colspan=\"3\"><span class=\"center\">Place here</span></td></tr>",
        forcePlaceholderSize: false,
        //draggingClass: "sortable-dragging",
    });
}

function AddNewStep(componentType) {
    //console.log('#' + componentType + ' box-body');
    var html = '';
    switch (componentType) {
        case 'Anode':
            html = GenerateNewStepHtml(componentType, anodeStepNumber, "");
            document.location.hash = "#AnodeStep" + anodeStepNumber;
            anodeStepNumber = anodeStepNumber + 1;
            break;
        case 'Cathode':
            html = GenerateNewStepHtml(componentType, cathodeStepNumber, "");
            cathodeStepNumber = cathodeStepNumber + 1;
            break;
        case 'Separator':
            html = GenerateNewStepHtml(componentType, separatorStepNumber, "");
            separatorStepNumber = separatorStepNumber + 1;
            break;
        case 'Electrolyte':
            html = GenerateNewStepHtml(componentType, electrolyteStepNumber, "");
            electrolyteStepNumber = electrolyteStepNumber + 1;
            break;
        case 'ReferenceElectrode':
            html = GenerateNewStepHtml(componentType, referenceElectrodeStepNumber, "");
            referenceElectrodeStepNumber = referenceElectrodeStepNumber + 1;
            break;
        case 'Casing':
            html = GenerateNewStepHtml(componentType, casingStepNumber, "");
            casingStepNumber = casingStepNumber + 1;
            break;
    }
    //console.log(html);
    $("#" + componentType + " .commercialTypeBtn").addClass("hidden");
    $("#" + componentType + " .box-body .previousTypeBtn").addClass("hidden");

    $('#' + componentType + ' .box-body:first').append(html);
    $('#' + componentType + ' .startComponentOverButton').removeClass('hidden');
}
function GenerateViewMeasurementsButton(measurementsData, measurementsLevelType, ifHidden) {
    var html = "";
    if (measurementsLevelType == 1) {
        html += '<a class="btn btn-primary btn-xs measurementsButton viewMeasurementsContentLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="ViewMeasurements(this, 1)" >' + ' View measurements</a> ';
    }
    else if (measurementsLevelType == 2) {
        html += '<a class="btn btn-primary btn-xs measurementsButton viewMeasurementsStepLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="ViewMeasurements(this, 2)" >' + ' Step measurements</a> ';
    }
    else if (measurementsLevelType == 3) {
        html += '<a class="btn btn-primary btn-xs measurementsButton viewMeasurementsComponentLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="ViewMeasurements(this, 3)" >' + ' Component measurements</a> ';
    }
    return html;
}
function GenerateAddMeasurementsButton(measurementsData, measurementsLevelType, ifHidden) {
    //var measurements = "";
    //var measurementsData = JSON.stringify({ "type": "measurements", "componentTypeId": "", "item": measurements });
    //console.log(measurementsData);
    var html = "";

    if (measurementsLevelType == 1) {
        html += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsContentLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="AddMeasurements(this, 1)" >' + ' Add measurements</a> ';
    }
    else if (measurementsLevelType == 2) {
        html += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsStepLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="AddMeasurements(this, 2)" >' + ' Step measurements</a> ';
    }
    else if (measurementsLevelType == 3) {
        html += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsComponentLevelBtn ' + ifHidden + '" data-item=\''
            + measurementsData + '\' onclick="AddMeasurements(this, 3)" >' + ' Component measurements</a> ';
    }

    return html;
}
function GenerateAddDocumentsButton(componentType) {
    var componentTypeId;
    switch (componentType) {
        case 'Anode':
            componentTypeId = 1;
            break;
        case 'Cathode':
            componentTypeId = 2;
            break;
        case 'Separator':
            componentTypeId = 3;
            break;
        case 'Electrolyte':
            componentTypeId = 4;
            break;
        case 'ReferenceElectrode':
            componentTypeId = 5;
            break;
        case 'Casing':
            componentTypeId = 6;
            break;
    }

    var html = "";

    html += '<div onclick=\"ShowFileAttachmentsExperiment(\'Component\', ' + experimentId + ', ' + componentTypeId + ', 0)\"' + 'class="btn btn-xs btn-default documentsButton">Documents</div>';


    return html;
}
function GenerateViewTestDocButton(componentType, stepId) {
    var componentTypeId;
    switch (componentType) {
        case 'Anode':
            componentTypeId = 1;
            break;
        case 'Cathode':
            componentTypeId = 2;
            break;
        case 'Separator':
            componentTypeId = 3;
            break;
        case 'Electrolyte':
            componentTypeId = 4;
            break;
        case 'ReferenceElectrode':
            componentTypeId = 5;
            break;
        case 'Casing':
            componentTypeId = 6;
            break;
    }

    var html = "";

    //html += '<div title="Download test data" onclick=\"ShowFileAttachmentsMeasurementsLowLevel(null, ' + experimentId + ', ' + componentTypeId + ', ' + stepId + ')\"' + 'class="btn btn-xs btn-default documentsButton">Test Documents</div>';
    html += '<a  title="Download test data" class="btn btn-success btn-xs" onclick="ShowFileAttachmentsMeasurementsLowLevel(null, ' + experimentId + ', ' + componentTypeId + ', ' + stepId + ')" ><i class="fa fa-download"></i></a> ';
    var url = "/GraphResults/UploadLowLevel?expId=" + experimentId + "&componentId=" + componentTypeId + ((stepId != undefined) ? "&step=" + stepId : "");
    html += '<a  title="Upload test results" class="btn btn-primary btn-xs" href="' + url + '" target="_blank"><i class="fa fa-upload"></i></a> ';

    return html;
}
function GenerateNewStepHtml(componentType, stepNumber, measurements, viewMode) {
    var componentTypeId;
    switch (componentType) {
        case 'Anode':
            componentTypeId = 1;
            break;
        case 'Cathode':
            componentTypeId = 2;
            break;
        case 'Separator':
            componentTypeId = 3;
            break;
        case 'Electrolyte':
            componentTypeId = 4;
            break;
        case 'ReferenceElectrode':
            componentTypeId = 5;
            break;
        case 'Casing':
            componentTypeId = 6;
            break;
    }

    var hide = "hidden"
    //if (viewMode == false) {
    //    hide = "hidden";
    //}

    var stepNumber = $('#' + componentType + ' .box-body').find(".row.step").length + 1;

    var html = "";
    var materialsAndBatchesTable = 'materialsAndBatchesTable' + componentType + 'Step' + stepNumber;
    var processesTable = 'processesTable' + componentType + 'Step' + stepNumber;

    //var measurements = "";
    //var attr = 'measurements';
    //if (attr in data) {
    //    //var measurementsItem = "";
    //    //console.log(measurements.item);
    //    if (data.measurements != null) {
    //        measurements = data.measurements;
    //        //measurementsItem = measurements.item;
    //    }
    //}

    //var measurements = "";
    var measurementsData = JSON.stringify({ "type": "measurements", "step": stepNumber, "item": measurements });
    //var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 1);
    //if (viewMode) {
    //    var measurementsButton = GenerateViewMeasurementsButton(measurementsData, 2);
    //}
    //else {
    //    var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 2);
    //}

    var testDocumentsButton = "";
    if (viewMode) {
        var measurementsButtonView = GenerateViewMeasurementsButton(measurementsData, 2, "");
        var measurementsButtonAdd = GenerateAddMeasurementsButton(measurementsData, 2, "hidden");

        testDocumentsButton = GenerateViewTestDocButton(componentType, stepNumber);
    }
    else {
        var measurementsButtonView = GenerateViewMeasurementsButton(measurementsData, 2, "hidden");
        var measurementsButtonAdd = GenerateAddMeasurementsButton(measurementsData, 2, "");
    }
    var documentsButton = '<input type="button" class="btn btn-xs btn-default documentsButton" onclick="ShowFileAttachmentsExperiment(\'' + "Step" + '\',  ' + experimentId + ',  ' + componentTypeId + ',  ' + stepNumber + ')" disabled="true" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" /> ';
    //<input type="button" onclick="ShowFileAttachmentsExperiment('Component', <%=experimentId%>, 6, 0)" class="btn btn-xs btn-default documentsButton" disabled="true" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents"/>
    //var documentsButton = '<input type="button" value="Documents" onclick="ShowFileAttachmentsExperiment(\'' + "Step" + '\', \'' + stepNumber + '\')" class="btn btn-xs btn-default">';
    //var documentsButton = '<a class="btn btn-xs btn-default" onclick="ShowFileAttachmentsExperiment(\'' + "Step" + '\',  ' + 1 + ')">Documents</a> ';
    //html += '<a class="btn btn-primary btn-xs addMeasurementsContentLevelBtn" data-item=\'' + measurementsData + '\' onclick="AddMeasurements(this, 1)" >' + ' Add measurements</a> ';

    html += '<div class="row step"' + ' id=' + componentType + 'Step' + stepNumber + '>' +
        '<div class="col-md-12 col-lg-12 col-lg-offset-0">' +
        '<div class="box">' +
        '<div class="box-header with-border">' +
        '<h3 class="box-title">Step ' + stepNumber + ' </h3>';
    html += '<div class="stepButtons">';
    html += measurementsButtonAdd;
    html += measurementsButtonView;
    html += documentsButton;
    html += testDocumentsButton;
    html += '</div>';

    if (viewMode != true) {
        html += '<div class="btn btn-box-tool pull-right removeStepBtn" data-toggle="tooltip" title="Remove step" onclick="RemoveStepConfirm(\'' + componentType + '\', \'' + stepNumber + '\')">' + '<i class="fa fa-remove"></i>' + '</div> ';
    }
    //<i class="fa fa-plus"></i>
    //res += '<div class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
    html += '</div>' +
        '<div class="row">' +
        '<div class="col-sm-12">' +
        '<div class="pull-right stepContentButtons">' +
        '<input type="button" onclick=" ' + 'NewMaterialPopup(\'' + materialsAndBatchesTable + '\')" value="Add material" class="newMaterialBtn btn bg-olive" /> ' +
        '<input type="button" onclick=" ' + 'NewBatchPopup(\'' + materialsAndBatchesTable + '\')" value="Add batch" class="newBatchBtn btn btn-warning" /> ' +
        '<input type="button" onclick=" ' + 'NewProcessPopup(\'' + processesTable + '\')" value="Add process" class="newProcessBtn btn btn-default" /> ' +
        '<input type="button" onclick=" ' + 'NewProcessSequencePopup(\'' + processesTable + '\')" value="Add sequence" class="newSequenceBtn btn btn-default" /> ' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div class="box-body">' +
        '<div class="table-responsive">' +
        '<table id="materialsAndBatchesTable' + componentType + 'Step' + stepNumber + '" class="table table-condensed table-hover materialsAndBatchesTable ' + hide + '">' +
        '<tbody class="js-sortable-table">' +
        '<tr>' +
        '<th class="moveColumn"></th>' +
        '<th></th>' +
        '<th>Material</th>' +
        '<th>Quantity</th>' +
        '<th>M.Unit</th>' +
        '<th>Action</th>' +
        '</tr>' +
        '</tbody>' +
        '</table>' +
        '</div>' +

        '<div class="table-responsive">' +
        '<table id="processesTable' + componentType + 'Step' + stepNumber + '" class="table table-condensed table-hover processesTable ' + hide + '">' +
        '<tbody class="js-sortable-table">' +
        '<tr>' +
        '<th class="moveColumn"></th>' +
        '<th></th>' +
        '<th>Process</th>' +
        '<th>Equipment</th>' +
        '<th>Label</th>' +
        '<th></th>' +
        '<th></th>' +
        '<th></th>' +
        '</tr>' +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '<div class="box-footer">';
    //html += '<div class="pull-left stepFooterButtons">';
    //html += measurementsButton;
    //html += documentsButton;
    //html += '</div>';

    html += '<div class="pull-right stepFooterButtons">';

    //html += '<a class="btn btn-primary btn-xs addMeasurementsStepLevelBtn" data-item=\'' +
    //    measurementsData + '\' onclick="AddMeasurements(this, 2)" >' + ' Add measurements</a> ';


    html += '<input type="button" value="Save as batch"' + ' data-item=\'' + JSON.stringify({ "type": "batchData", "item": "" }) + '\'' + ' onclick="SaveStepAsBatch(\'' + componentType + '\', \'' + stepNumber + '\', \'' + materialsAndBatchesTable + '\', this' + ')" class="saveAsBatchBtn btn btn-default hidden"> ' +
        '<input type="button" value="Save as sequence"' + ' data-item=\'' + JSON.stringify({ "type": "sequenceData", "item": "" }) + '\'' + ' onclick="SaveStepAsSequence(\'' + componentType + '\', \'' + stepNumber + '\', \'' + materialsAndBatchesTable + '\', this' + ')" class="saveAsBatchBtn btn btn-default hidden">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    //console.log(html);
    return html;
}
function SelectComponentCommercialTypePopup(componentType) {
    var html = '<div class="form-horizontal">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectCommercialType" class="commercial-type-data-ajax">    </select>';
    html += '</div>';
    html += '<div class="col-sm-3">';
    html += '<a id="viewExperimentLink" class="btn btn-default" href=\"/PrefabTypes/Insert.aspx?componentType=' + componentType + '\" target="_blank">Add new</a> ';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    var dialog = bootbox.dialog({
        title: 'Choose ' + componentType,
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
                    var selectedData = $('#selectCommercialType').select2('data')[0].data;

                    var row = GenerateCommercialTypeHtml(selectedData);
                    //AppendRow($('#' + componentType), row);
                    //$('#' + componentType + " .box-body").html(row);
                    $("#" + componentType + " .box-body .commercialTypeBtn").addClass("hidden");
                    $("#" + componentType + " .box-body .previousTypeBtn").addClass("hidden");

                    $('#' + componentType + " .box-body").append(row);
                    //$("#" + componentType + " .componentFooterButtons input").replaceWith("Save component");
                    $("#" + componentType + " .componentFooterButtons .submitComponentBtn").addClass("hidden");
                    $("#" + componentType + " .componentFooterButtons .newStepBtn").addClass("hidden");
                    $("#" + componentType + " .componentFooterButtons .submitComponentCommercialBtn").removeClass("hidden");

                    $('#' + componentType + ' .startComponentOverButton').removeClass('hidden');

                    return true;
                }
            }
        },
        onEscape: true
    });
    dialog.on('shown.bs.modal', function () {
        //console.log($("#selectMaterial").val());
        $("#selectCommercialType").select2('open');
    });

    reqUrl = "/Helpers/WebMethods.asmx/GetBatteryComponentCommercialTypes";
    $('#selectCommercialType').select2({
        ajax: {
            type: "POST",
            url: reqUrl,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }

                return JSON.stringify({ search: term, componentType: componentType });
            },
            processResults: function (data, params) {
                // parse the results into the format expected by Select2
                // since we are using custom formatting functions we do not need to
                // alter the remote JSON data, except to indicate that infinite
                // scrolling can be used

                //console.log(JSON.parse(data.d).results);
                return {
                    results: $.map(JSON.parse(data.d), function (item) {
                        return {
                            id: item.batteryComponentCommercialTypeId,
                            text: item.batteryComponentCommercialType,
                            data: item
                        };
                    })
                };
            },
            //cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Search for prefab types',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });

    function formatRepo(repo) {
        var markup = "" + repo.text;
        return markup;
    }

    function formatRepoSelection(repo) {
        return repo.text;
    }
}

function SelectComponentFromPreviousPopup(componentType) {
    var componentTypeId;
    switch (componentType) {
        case 'Anode':
            componentTypeId = 1;
            break;
        case 'Cathode':
            componentTypeId = 2;
            break;
        case 'Separator':
            componentTypeId = 3;
            break;
        case 'Electrolyte':
            componentTypeId = 4;
            break;
        case 'ReferenceElectrode':
            componentTypeId = 5;
            break;
        case 'Casing':
            componentTypeId = 6;
            break;
    }

    var html = '<div class="form-horizontal">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectPreviousType" class="commercial-type-data-ajax">    </select>';
    html += '</div>';
    html += '<div class="col-sm-3">';
    html += '<a id="viewExperimentLink" class="btn btn-default disabled" href=\"/Experiments/View/' + 1 + '\" target="_blank">View Experiment</a> ';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    var dialog = bootbox.dialog({
        title: 'Choose ' + componentType,
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
                    var selectedData = $('#selectPreviousType').select2('data')[0].data;
                    //console.log(selectedData);

                    RedrawComponent(selectedData, false);

                    return true;
                }
            }
        },
        onEscape: true
    });
    dialog.on('shown.bs.modal', function () {
        //console.log($("#selectMaterial").val());
        $("#selectPreviousType").select2('open');
    });

    reqUrl = "/Helpers/WebMethods.asmx/GetAllPreviousExperimentComponents";
    $('#selectPreviousType').select2({
        ajax: {
            type: "POST",
            url: reqUrl,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }

                return JSON.stringify({ search: term, experimentId: experimentId, componentTypeId: componentTypeId });
            },
            processResults: function (data, params) {
                // parse the results into the format expected by Select2
                // since we are using custom formatting functions we do not need to
                // alter the remote JSON data, except to indicate that infinite
                // scrolling can be used

                //console.log(JSON.parse(data.d).results);
                return {
                    results: $.map(JSON.parse(data.d), function (item) {
                        return {
                            id: item.fk_experiment,
                            text: item.battery_component_type + "_" + item.experiment_system_label + " | " + item.experiment_personal_label + " | " + moment(item.date_created, "YYYY-MM-DD").format("DD/MM/YYYY"),
                            data: item
                        };
                    })
                };
            },
            //cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Search for previous components',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });

    $('#selectPreviousType').change(function () {
        var selectedData = $('#selectPreviousType').select2('data')[0].data;

        $("#viewExperimentLink").attr("href", "/Experiments/View/" + selectedData.fk_experiment);
        $("#viewExperimentLink").removeClass("disabled");
    });

    function formatRepo(repo) {
        var markup = "" + repo.text;
        return markup;
    }

    function formatRepoSelection(repo) {
        return repo.text;
    }
}

function RedrawComponent(data, viewMode) {
    var experimentId = data.fk_experiment;
    var componentTypeId = data.fk_battery_component_type;

    var newRequestObj = new Object();
    newRequestObj.experimentId = experimentId;
    /*newRequestObj.peojectId = peojectId;*/
    newRequestObj.componentTypeId = componentTypeId;

    var RequestDataString = JSON.stringify(newRequestObj);
    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetExperimentComponent",
        data: RequestDataString,
        //async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //alert(result.d);
            //RefreshBatchInfo(result.d);

            //console.log(result.d);
            var batteryComponent = JSON.parse(result.d);
            RegenerateComponent(batteryComponent, viewMode);
        },
        error: function (p1, p2, p3) {
            alert(p1.status);
            alert(p3);
        }
    });

    //console.log(experimentId + " " + componentTypeId);


}

function NewMaterialPopup(table) {
    var selectedMaterialFunction = null;
    //console.log('selected function: ' + selectedMaterialFunction);
    //Add new material popup

    //console.log(table);
    //var html = '<div class="row"><div class="col-md-12">';
    //html += '<select id="selectMaterial" class="material-data-ajax">    </select>';
    //html += '</div></div>';

    var html = '<div class="form-horizontal">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectMaterial" class="material-data-ajax">    </select>';
    html += '</div>';
    html += '<div class="col-sm-3">';
    html += '<a id="newMaterialLink" class="btn btn-default" href="/Materials/Insert" target="_blank">New material</a> ';
    html += '</div>';
    html += '</div>';

    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectMaterialFunction" class="material-function-data-ajax">  <option></option>  </select>';
    html += '</div>';
    html += '</div>';

    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<input id="percentageOfActive" class="form-control hidden"  type="text" name="percentageOfActive" placeholder="Percentage of active material" />';
    html += '</div>';
    html += '</div>';

    html += '<div id="selectedInfo" class="hidden">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-3">';
    html += 'Left in stock: ';
    html += '</div>';
    html += '<div class="col-sm-9">';
    html += '<div id="availableQuantity">  </div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';


    html += '</div>';

    var dialog = bootbox.dialog({
        title: 'Add material',
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
                    var selectedData = $('#selectMaterial').select2('data')[0].data;
                    var materialFunctionId = $('#selectMaterialFunction').val();
                    var percentageOfActive = "";
                    if (materialFunctionId == 1) {
                        percentageOfActive = $('#percentageOfActive').val();
                    }
                    selectedData.fkFunction = materialFunctionId;
                    selectedData.percentageOfActive = percentageOfActive.replace(',', '.');

                    var row = GenerateMaterialHtml(selectedData);
                    if ($('#' + table).hasClass("hidden"))
                        $('#' + table).removeClass("hidden");
                    if ($('#' + table).parents(".box").find(".saveAsBatchBtn").hasClass("hidden"))
                        $('#' + table).parents(".box").find(".saveAsBatchBtn").removeClass("hidden");

                    AppendRow($('#' + table), row);

                    var rowID = $(row).attr('id');
                    setTimeout(function () { $("tr#" + rowID).find('td > input[name="quantity"]').focus() }, 50);

                    return true;
                }
            }
        },
        onEscape: true
    });
    dialog.on('shown.bs.modal', function () {
        //console.log($("#selectMaterial").val());
        $("#selectMaterial").select2('open');
    });

    reqUrl = "/Helpers/WebMethods.asmx/GetMaterialsWithQuantity";
    $('#selectMaterial').select2({
        ajax: {
            type: "POST",
            url: reqUrl,
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
                    materialFunction: selectedMaterialFunction,
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.materialId,
                            text: item.materialName,
                            chemicalFormula: item.chemicalFormula,
                            materialLabel: item.materialLabel,
                            vendorName: item.vendorName,
                            dateCreated: item.dateCreated,
                            availableQuantity: item.availableQuantity,
                            measurementUnitSymbol: item.measurementUnitSymbol,
                            lotNumber: item.lotNumber,
                            data: item
                        };
                    }),
                    pagination: {
                        "more": response.pagination.more
                    }
                };
            },
            //cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Search for material',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });

    function formatRepo(repo) {
        //if (item.rotationSpeed != null && item.rotationSpeed != "") {
        //    res += " | " + "Rotation Speed: " + item.rotationSpeed;
        //}
        var markup = repo.text;
        markup += " | " + repo.chemicalFormula;
        if (repo.materialLabel != null && repo.materialLabel != "")
            markup += " | " + repo.materialLabel;
        if (repo.vendorName != null && repo.vendorName != "")
            markup += " | " + repo.vendorName;
        if (repo.dateCreated != null && repo.dateCreated != "")
            markup += " | " + repo.dateCreated;
        if (repo.availableQuantity != null && repo.availableQuantity != "")
            markup += " | " + repo.availableQuantity + " " + repo.measurementUnitSymbol;
        if (repo.lotNumber != null && repo.lotNumber != "")
            markup += " | LOT Number: " + repo.lotNumber;
        return markup;
    }

    function formatRepoSelection(repo) {
        return repo.materialName || repo.operatorUsername;
    }


    reqUrl = "/Helpers/WebMethods.asmx/GetMaterialFunctions";
    $('#selectMaterialFunction').select2({
        data: materialFunctionsJson,
        dropdownParent: dialog,
        width: "100%",
        allowClear: true,
        //placeholder: {
        //    id: '0',
        //    text: 'Select function in experiment'
        //},
        //theme: "bootstrap",
        placeholder: 'Select function in experiment',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });

    var selectedMaterialItem;
    $('#selectMaterial').change(function () {

        var selectedMaterial = $('#selectMaterial').val();
        selectedMaterialItem = $('#selectMaterial').select2('data')[0].data;

        //var selectedMaterialAvailableQuantity = "";

        //console.log(selectedMaterial);
        if (selectedMaterial) {
            //var newRequestObj = new Object();
            //newRequestObj.materialType = null;
            //newRequestObj.materialId = selectedMaterial;
            //var RequestDataString = JSON.stringify(newRequestObj);

            //$.ajax({
            //    type: "POST",
            //    url: "/Helpers/WebMethods.asmx/GetAllMaterialsList",
            //    data: RequestDataString,
            //    //async: true,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
            //        //alert(result.d);
            //        RefreshMaterialInfo(result.d);
            //    },
            //    error: function (p1, p2, p3) {
            //        alert(p1.status);
            //        alert(p3);
            //    }
            //});

            RefreshMaterialInfo(selectedMaterialItem);
        }

        $('#selectMaterialFunction').val(selectedMaterialItem.fkFunction).trigger('change');
    });
    $('#selectMaterialFunction').change(function () {


        $('#percentageOfActive').val("");
        selectedMaterialFunction = $(this).val();

        //if (selectedMaterialFunction == 0)
        //    selectedMaterialFunction = null;

        //console.log('selected function: ' + selectedMaterialFunction);

        //console.log(JSON.stringify(selectedMaterialItem));
        if (selectedMaterialFunction == 1) {

            //console.log(selectedMaterialItem);

            if (selectedMaterialItem && selectedMaterialItem.percentageOfActive != null) {
                $('#percentageOfActive').val(selectedMaterialItem.percentageOfActive);
            }

            $('#percentageOfActive').removeClass('hidden');
            //$("#percentageOfActive").trigger('click');
            setTimeout(function () { $("#percentageOfActive").focus() }, 50);
        }
        else {
            $('#percentageOfActive').addClass('hidden');
        }
    });


    function RefreshMaterialInfo(data) {
        //var res = JSON.parse(data);
        var res = data;
        var availableQuantity = res.availableQuantity != null || res.availableQuantity != '0.0' ? res.availableQuantity : '0';
        var html = availableQuantity + " " + res.measurementUnitSymbol;
        $('#availableQuantity').html(html);
        $('div#selectedInfo').removeClass('hidden');
        //$('#<%= TxtMeasurementUnit.ClientID %>').val(res[0].measurementUnitSymbol);
    }
}
function NewBatchPopup(table) {
    var selectedMaterialFunction = null;

    //Add new batch popup
    //var html = '<div class="row"><div class="col-md-12">';
    //html += '<select id="selectBatch" class="batch-data-ajax">    </select>';
    //html += '</div></div>';

    var html = '<div class="form-horizontal">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectBatch" class="batch-data-ajax">    </select>';
    html += '</div>';
    html += '<div class="col-sm-3">';
    html += '<a id="viewExperimentLink" class="btn btn-default" href="/Batches/Insert" target="_blank">New batch</a> ';
    html += '</div>';
    html += '</div>';

    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<select id="selectMaterialFunction" class="material-function-data-ajax">  <option></option>  </select>';
    html += '</div>';
    html += '</div>';

    html += '<div class="form-group">';
    html += '<div class="col-sm-9">';
    html += '<input id="percentageOfActive" class="form-control hidden"  type="text" name="percentageOfActive" placeholder="Percentage of active material" />';
    html += '</div>';
    html += '</div>';

    html += '<div id="selectedInfo" class="hidden">';
    html += '<div class="form-group">';
    html += '<div class="col-sm-3">';
    html += 'Left in stock: ';
    html += '</div>';
    html += '<div class="col-sm-9">';
    html += '<div id="availableQuantity">  </div>';
    html += '</div>';
    html += '</div>';
    html += '</div>';

    html += '</div>';

    var dialog = bootbox.dialog({
        title: 'Add batch',
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
                    var selectedData = $('#selectBatch').select2('data')[0].data;
                    var materialFunctionId = $('#selectMaterialFunction').val();
                    var percentageOfActive = "";
                    if (materialFunctionId == 1) {
                        percentageOfActive = $('#percentageOfActive').val();
                    }
                    selectedData.fkFunction = materialFunctionId;
                    selectedData.percentageOfActive = percentageOfActive;

                    var row = GenerateBatchHtml(selectedData);
                    if ($('#' + table).hasClass("hidden"))
                        $('#' + table).removeClass("hidden");
                    if ($('#' + table).parents(".box").find(".saveAsBatchBtn").hasClass("hidden"))
                        $('#' + table).parents(".box").find(".saveAsBatchBtn").removeClass("hidden");

                    AppendRow($('#' + table), row);

                    var rowID = $(row).attr('id');
                    setTimeout(function () { $("tr#" + rowID).find('td > input[name="quantity"]').focus() }, 50);

                    return true;
                }
            }
        },
        onEscape: true
    });
    dialog.on('shown.bs.modal', function () {
        //console.log($("#selectMaterial").val());
        $("#selectBatch").select2('open');
    });

    reqUrlBatches = "/Helpers/WebMethods.asmx/GetBatches";
    $('#selectBatch').select2({
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
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.batchId,
                            text: item.batchSystemLabel + " | " + item.batchPersonalLabel + " | " + item.dateCreated,
                            //chemicalFormula: item.chemicalFormula,
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
        //theme: "bootstrap",
        placeholder: 'Search for batch',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        templateResult: formatBatch,
        //templateSelection: formatRepoSelection
    });

    function formatBatch(batch) {
        //var markup = "" + repo.chemicalFormula + " | " + repo.text;
        var markup = batch.text;
        //var markup = "" + batch.id + " | " + batch.text;
        return markup;
    }

    $('#selectBatch').change(function () {

        var selectedBatchId = $('#selectBatch').val();
        var selected = $('#selectBatch').select2("data")[0];
        //var selectedBatchItem = $('#selectMaterial').select2('data')[0].data;
        selectedBatchItem = selected.data;

        //console.log(batchData);
        if (selectedBatchId) {
            var newRequestObj = new Object();
            newRequestObj.materialType = null;
            newRequestObj.researchGroupId = null;
            newRequestObj.batchId = selectedBatchId;
            var RequestDataString = JSON.stringify(newRequestObj);

            $.ajax({
                type: "POST",
                url: "/Helpers/WebMethods.asmx/GetAllBatchesList",
                data: RequestDataString,
                //async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    //alert(result.d);
                    RefreshBatchInfo(result.d);
                },
                error: function (p1, p2, p3) {
                    alert(p1.status);
                    alert(p3);
                }
            });

            $('#selectMaterialFunction').val(selectedBatchItem.fkFunction).trigger('change');
        }
    });
    function RefreshBatchInfo(data) {
        var res = JSON.parse(data);
        var availableQuantity = res[0].availableQuantity != null || res[0].availableQuantity != '0.0' ? res[0].availableQuantity : '0';
        var html = availableQuantity + " " + res[0].measurementUnitSymbol;
        $('#availableQuantity').html(html);
        $('div#selectedInfo').removeClass('hidden');
    }

    reqUrl = "/Helpers/WebMethods.asmx/GetMaterialFunctions";
    $('#selectMaterialFunction').select2({
        data: materialFunctionsJson,
        dropdownParent: dialog,
        width: "100%",
        allowClear: true,
        //placeholder: {
        //    id: '0',
        //    text: 'Select function in experiment'
        //},
        //theme: "bootstrap",
        placeholder: 'Select function in experiment',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });


    $('#selectMaterialFunction').change(function () {

        selectedMaterialFunction = $(this).val();

        if (selectedMaterialFunction == 1) {
            $('#percentageOfActive').removeClass('hidden');
        }
        else {
            $('#percentageOfActive').addClass('hidden');
        }
    });
}
var alpacaSubmitProcessFormFunction = function (e) {
    $("#alpaca-submit-button").prop("disabled", true);
    var alpacaFormData = this.getValue();
    console.log(this);
    var alpacaFormDataJson = JSON.stringify(this.getValue());

    var selectedProcessData = $('#selectProcess').select2('data')[0].data;
    var row = GenerateProcessHtml(selectedProcessData, alpacaFormData);
    AppendRow($('#processesTable'), row);

    bootbox.hideAll();
}

var alpacaSubmitFormFunction = function (e) {
    $("#alpaca-submit-button").prop("disabled", true);
    var data = JSON.stringify(this.getValue());
    //console.log(data);
}

var alpacaFormPostRender = function (e) {
    $("select.alpaca-control").select2({
        //vaka postavuvam parent za da se distrojnat avtomatski posle zatvaranje na formata
        dropdownParent: $(e.domEl[0]),
        width: "100%"//,
        //theme: "bootstrap"
    });
}
function GenerateAttributesForm(attributes) {
    var html = "";
    for (const val of attributes) {
        if (val.isParent == false && val.fkParentAttribute == null) {
            html += '<div class="col-md-12">';
            html += '<div class="form-group"><label>' + val.attributeName + " " + '(' + val.attMeasurementUnit + ')' + '</label><input type="' + val.type + '" data-id="' + val.fkAttribute + '" name="' + val.attributeName + '" placeholder="' + val.attributeName + '" data-attMeasurementUnit="' + val.attMeasurementUnit + '" data-isParent="' + val.isParent + '" data-fkParentAttribute="' + val.fkParentAttribute + '" class="form-control"></div>';
            html += '</div>';
        }
        if (val.isParent == true) {
            html += '<div class="col-sm-12 parentChildWrapper">';
            html += '<div class="parentNode" style="border-top: 1px solid #F5F5F5;"><h4>' + val.attributeName + '</h4><input hidden value=" " data-isParent="' + val.isParent + '"name="' + val.attributeName + '"data-id="' + val.fkAttribute + '" >';
            //html += '<div class="form-group"></div>';
            html += '<div class="duplicateChildBtn btn btn-xs" onclick="DuplicateChildNodes(' + val.fkAttribute + ')"><i class="fa fa-plus"></i></div>';
            html += '</div>';
            html += '<div class="" id="' + val.fkAttribute + '">';
            html += '<div class="col-sm-12 childNodesWrapper" id="pchild-' + val.fkAttribute + '-1"><div class="childNodes">';
            for (const res of val.children) {
                html += '<div class="form-group">';
                html += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                html += '</div>';
            }
            html += '</div></div>';
            html += '</div>';
        }
        html += '</div>';
    }
    return html;
}

function GenerateAttributesFormWithValues(attributeValues) {
    html = "";
    for (const val of attributeValues) {
        if (val.isParent == false && val.fkParentAttribute == null) {
            html += '<div class="col-md-12">';
            html += '<div class="form-group"><label>' + val.attributeName + " " + '(' + val.attMeasurementUnit + ')' + '</label><input type="' + val.type + '" data-id="' + val.fkAttribute + '" name="' + val.attributeName + '" placeholder="' + val.attributeName + '" value="' + val.value + '" data-attMeasurementUnit="' + val.attMeasurementUnit + '" data-isParent="' + val.isParent + '" data-fkParentAttribute="' + val.fkParentAttribute + '" class="form-control"></div>';
            html += '</div>';
        }
        else if (val.isParent == true) {
            html += '<div class="col-sm-12 parentChildWrapper">';
            html += '<div style = "border-top: 1px solid #F5F5F5;" ><h4>' + val.attributeName + '</h4><input hidden value=" " data-isParent="' + val.isParent + '"name="' + val.attributeName + '"data-id="' + val.fkAttribute + '" >';
            html += '<div class="duplicateChildBtn btn btn-xs pull-right" onclick="DuplicateChildNodes(' + val.fkAttribute + ')"><i class="fa fa-plus"></i></div>';
            html += '</div>';
            html += '<div class="" id="' + val.fkAttribute + '">';
            var firstChildId = 0;
            var counter = 1;
            if (val.children.length > 0) {
                firstChildId = val.children[0].fkAttribute;
            }
            var child = '<div class="col-sm-12 childNodesWrapper" id="pchild-' + val.fkAttribute + '-1">';
            child += '<div class="childNodes">';

            $.each(val.children, function (index, res) {

                if (index == 0 || (firstChildId != res.fkAttribute)) {
                    child += '<div class="form-group">';
                    child += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" value="' + res.value + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                    child += '</div>';
                }
                else if (firstChildId == res.fkAttribute) {
                    counter = counter + 1;
                    child += '</div>'; //childNodes end
                    child += '</div>'; //childNodesWrapper end

                    html += child;
                    child = "";
                    child += '<div class="col-sm-12 childNodesWrapper" id="pchild-' + val.fkAttribute + '-' + counter + '">';
                    child += '<div class="childNodes">';
                    child += '<div class="btn btn-xs removeChild" onclick="RemoveParentNodes(this)"><i class="fa fa-minus"></i></div>';
                    child += '<div class="form-group">';
                    child += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" value="' + res.value + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                    child += '</div>';
                }
                if (index == val.children.length - 1) {
                    child += '</div>'; //childNodes end
                    child += '</div>'; //childNodesWrapper end
                    html += child;
                }
            });
            html += '</div>'
            html += '</div>' //parentChildWrapper end
        }
    }
    return html;
}

function CreateAttributeValuesArrayFromForm(formId) {
    var arr = new Array();
    //$('#form').find("input").each(function (index, input) {
    $('#' + formId).find("input").each(function (index, input) {
        var obj = {};
        obj.attributeName = input.name;
        obj.value = input.value;
        obj.fkAttribute = $(input).attr("data-id");
        obj.type = input.type;
        obj.attMeasurementUnit = $(input).attr("data-attMeasurementUnit");
        obj.isParent = JSON.parse($(input).attr("data-isParent"));
        obj.fkParentAttribute = $(input).attr("data-fkParentAttribute") != "null" ? $(input).attr("data-fkParentAttribute") : null;
        arr.push(obj);
    });
    var arr1 = new Array();
    for (const res of arr) {
        obj1 = {};
        obj1.attributeName = res.attributeName;
        obj1.value = res.value;
        obj1.fkAttribute = res.fkAttribute;
        obj1.type = res.type;
        obj1.attMeasurementUnit = res.attMeasurementUnit;
        obj1.isParent = res.isParent;
        obj1.fkParentAttribute = res.fkParentAttribute;
        obj1.children = [];
        if (res.isParent == true) {
            for (const val of arr) {
                var child = {};
                if (res.fkAttribute == val.fkParentAttribute) {
                    child.attributeName = val.attributeName;
                    child.value = val.value;
                    child.fkAttribute = val.fkAttribute;
                    child.type = val.type;
                    child.attMeasurementUnit = val.attMeasurementUnit;
                    child.isParent = val.isParent;
                    child.fkParentAttribute = val.fkParentAttribute;
                    obj1.children.push(child);
                }

            }
        }
        if (obj1.fkParentAttribute == null) {
            arr1.push(obj1);
        }
    }

    return arr1;
}

var selectedProcess = "";
var selectedProcessTypeId = "";
function NewProcessPopup(table) {
    var html = '<div class="row">';
    html += '<div class="col-md-12">';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<input type="text" id="processLabel" name="processLabel" placeholder="Label" class="form-control"></input>';
    html += '</div></div>';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<select id="selectProcess" class="batch-data-ajax">    </select>';
    html += '</div></div>';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<select id="selectPreviousProcess" class="process-data-ajax hidden">';
    html += '</select>';
    html += '</div></div>';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<select id="selectEquipment" class="batch-data-ajax">    </select>';
    html += '</div></div>';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<select id="selectEquipmentModel" class="batch-data-ajax">    </select>';
    html += '</div></div>';
    html += '</div>'; //col-md-12 end
    html += '</div>'; //row end

    //var previousProcessSettings = '<div class="row"><div class="col-md-12">';
    //previousProcessSettings += '<div class="form-group">';
    //previousProcessSettings += '<select id="selectPreviousProcess" class="process-data-ajax hidden">';
    ////previousProcessSettings +=   '<option value="Manual" selected="">Manual</option>';
    //previousProcessSettings += '</select>';
    //previousProcessSettings += '</div></div>';
    //previousProcessSettings += '</div>';
    //html += previousProcessSettings;

    html += '<div class="row"><div class="col-md-12">';
    html += '<form class="addAttributesForm" method="get" action="">';
    html += '<div id="form"></div>';
    html += '</form>';
    html += '</div></div>';
    dialog = bootbox.dialog({
        title: 'Add process',
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
                className: 'btn-primary add-button',
                callback: function () {
                    /*$('input.comment').each(function () {
                        $(this).rules("add",
                            {
                                required: true
                            })
                    });*/

                    // prevent default submit action         
                    //event.preventDefault();

                    //check if there are no attributes for the process
                    if ($("form.addAttributesForm input").length == 0) {
                        notify("No attributes found!");
                        return false;
                    }
                    // Validate form
                    if ($('form.addAttributesForm').validate().form()) {
                        //$("#alpaca-submit-button").prop("disabled", true);
                        //var alpacaFormData = alpacaForm.alpaca().getValue();
                        //var alpacaFormDataJson = JSON.stringify(alpacaFormData);

                        var selectedProcess = $('#selectProcess').val();
                        var selectedEquipmentSettings;
                        if (selectedProcess != null) {
                            selectedProcess = $('#selectProcess').select2('data')[0].data;
                            var label = $('#processLabel').val();
                            var selectedPreviousProcessId = $('#selectPreviousProcess').val();
                            var selectedPreviousProcess = null;
                            var selectedEquipmentModelId = $('#selectEquipmentModel').val();
                            var selectedEquipmentModel = null;
                            var equipmentModelName = null;
                            var modelBrand = null;

                            if (selectedPreviousProcessId != null) {
                                selectedPreviousProcess = $('#selectPreviousProcess').select2('data')[0].data;
                                selectedEquipmentSettings = $('#selectEquipment').select2('data')[0].data;

                                var selectedProcessData = {};
                                selectedProcessData.fkProcessType = $('#selectProcess').val(),
                                    selectedProcessData.processType = selectedProcess.processType,
                                    //selectedProcessData.subcategory = selectedProcess.subcategory,
                                    selectedProcessData.fkEquipment = selectedPreviousProcess.fkEquipment,
                                    selectedProcessData.equipmentName = selectedPreviousProcess.equipmentName,
                                    selectedProcessData.equipmentModelId = $('#selectEquipmentModel').val(),
                                    selectedProcessData.equipmentModelName = selectedPreviousProcess.equipmentModelName,
                                    selectedProcessData.equipmentModelBrand = selectedPreviousProcess.modelBrand

                                var arr1 = CreateAttributeValuesArrayFromForm("form");
                            }
                            else {
                                var selectedEquipmentId = $('#selectEquipment').val();
                                if (selectedEquipmentId != null) {
                                    selectedEquipment = $('#selectEquipment').select2('data')[0].data;
                                    if (selectedEquipmentModelId != null) {
                                        selectedEquipmentModel = $('#selectEquipmentModel').select2('data')[0].data;
                                        equipmentModelName = selectedEquipmentModel != null ? selectedEquipmentModel.equipmentModelName : $('#selectEquipmentModel').select2("data")[0].element.getAttribute("equipmentModelName");
                                        modelBrand = selectedEquipmentModel != null ? selectedEquipmentModel.modelBrand : $('#selectEquipmentModel').select2("data")[0].element.getAttribute("equipmentModelBrand");
                                    }
                                    var selectedProcessData = {};
                                    selectedProcessData.fkProcessType = $('#selectProcess').val(),
                                        selectedProcessData.processType = selectedProcess.processType,
                                        //selectedProcessData.subcategory = selectedProcess.subcategory,
                                        //selectedProcessData.equipmentName = selectedEquipmentSettings.equipmentName,
                                        selectedProcessData.equipmentName = (selectedEquipment != undefined) ? selectedEquipment.equipmentName : $('#selectEquipment').select2('data')[0].text,
                                        selectedProcessData.fkEquipment = $('#selectEquipment').val(),
                                        selectedProcessData.equipmentModelId = $('#selectEquipmentModel').val(),
                                        selectedProcessData.equipmentModelName = equipmentModelName,
                                        selectedProcessData.equipmentModelBrand = modelBrand

                                    var arr1 = CreateAttributeValuesArrayFromForm("form");

                                }
                                else {
                                    notify("Select an equipment", "danger");
                                    return false;
                                }
                            }

                        }
                        else {
                            notify("Select a process", "danger");
                            return false;
                        }
                        var row = GenerateProcessHtml(label, selectedProcessData, arr1);
                        if ($('#' + table).hasClass("hidden"))
                            $('#' + table).removeClass("hidden");
                        if ($('#' + table).parents(".box").find(".saveAsBatchBtn").hasClass("hidden"))
                            $('#' + table).parents(".box").find(".saveAsBatchBtn").removeClass("hidden");

                        AppendRow($('#' + table), row);
                        bootbox.hideAll();
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        },
        onEscape: true
    });
    dialog.on('shown.bs.modal', function () {
        $("#selectProcess").select2('open');
    });
    // $(".add-button").attr('disabled', 'disabled');
    reqUrlProcesses = "/Helpers/WebMethods.asmx/GetProcessTypes";
    $('#selectProcess').select2({
        ajax: {
            type: "POST",
            url: reqUrlProcesses,
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
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.processTypeId,
                            text: item.processType,
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
        //theme: "bootstrap",
        placeholder: 'Search for a process',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });
    reqUrlEquipments = "/Helpers/WebMethods.asmx/GetEquipment";
    $('#selectEquipment').select2({
        ajax: {
            type: "POST",
            url: reqUrlEquipments,
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
                    fkProcess: $('#selectProcess').val(),
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.equipmentId,
                            text: item.equipmentName + " " + item.equipmentLabel,
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
        //theme: "bootstrap",
        placeholder: 'Search for an equipment',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });
    reqUrlEquipments = "/Helpers/WebMethods.asmx/GetEquipmentModels";
    $('#selectEquipmentModel').select2({
        ajax: {
            type: "POST",
            url: reqUrlEquipments,
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
                    processTypeId: $('#selectProcess').val(),
                    equipmentId: $('#selectEquipment').val(),
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.equipmentModelId,
                            text: formatEquipmentModelDropdown(item),
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
        //theme: "bootstrap",
        placeholder: 'Search for an equipment model',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });
    $('#selectProcess').on('change', function () {
        $('#selectEquipment').val("").trigger('change');
        $('#selectEquipmentModel').val("").trigger('change');
        $('#selectPreviousProcess').val("").trigger('change');
        $("#selectPreviousProcess").removeAttr('disabled');
    });
    //$('#selectEquipment').on('change', function () {
    //    $('#selectEquipmentModel').val("").trigger('change');
    //    //$('#selectPreviousProcess').val("").trigger('change');
    //});
    var reqUrlProcessSettings = "/Helpers/WebMethods.asmx/GetRecentlyUsedProcess";
    $('#selectPreviousProcess').select2({
        ajax: {
            type: "POST",
            url: reqUrlProcessSettings,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }

                return JSON.stringify({
                    processId: $('#selectProcess').val() == null ? 0 : $('#selectProcess').val()
                    // processType: selectedProcess
                });
            },
            processResults: function (data, params) {

                return {
                    results: $.map(JSON.parse(data.d).response, function (item) {
                        return {
                            id: batchOrExperimentProcessId(item),
                            text: formatPreviousProcessDropdown(item),
                            data: item
                        }
                    })
                };
            },
            cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        allowClear: true,
        //theme: "bootstrap",
        //placeholder: 'Select previous setting',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcessSettings,
        //templateResult: function(e) { 
        //                return "<div class='select2-user-result'>" + JSON.stringify(e.data) + "</div>"; 
        //                },
        //templateSelection: formatRepoSelection
        placeholder: {
            id: 'manual', // or whatever the placeholder value is
            text: 'Select previous setting...' // the text to display as the placeholder
        },
        disabled: true,
        minimumResultsForSearch: Infinity
    });

    //$("#selectPreviousProcess").select2('data', { id: 1, text: "Manual" });
    $('#selectPreviousProcess').change(function () {
        $("#form").empty();
        var selectedData = $('#selectPreviousProcess').select2("data")[0];
        if (selectedData != undefined) {
            var selectedExperimentProcess = selectedData.data.experimentProcessId;
            var selectedBatchProcess = selectedData.data.batchProcessId;
            var batchOrProcessId;
            var equipmentName = selectedData.data.equipmentName;
            var equipmentId = selectedData.data.fkEquipment;
            var equipmentModelName = selectedData.data.equipmentModelName;
            var equipmentModelBrand = selectedData.data.modelBrand;
            var fkEquipmentModel = selectedData.data.fkEquipmentModel;
            var equipment = new Option(equipmentName, equipmentId, true, true);
            $('#selectEquipment').append(equipment).trigger('change.select2');
            if (fkEquipmentModel != null) {
                var model = new Option(equipmentModelName + " | " + equipmentModelBrand, fkEquipmentModel, true, true);
                model.setAttribute("equipmentModelName", equipmentModelName);
                model.setAttribute("equipmentModelBrand", equipmentModelBrand);
                model.setAttribute("equipmentName", equipmentName);
                model.setAttribute("equipmentId", equipmentId);
                $('#selectEquipmentModel').append(model).trigger('change.select2');
            }
            var comingFromBatch = false;
            if (selectedData.data.batchProcessId != null) {
                comingFromBatch = true;
                batchOrProcessId = selectedBatchProcess;
            }
            else {
                batchOrProcessId = selectedExperimentProcess;
            }
            GenerateEquipmentAttributeSettings(batchOrProcessId, comingFromBatch);
        }
    });
    function GenerateEquipmentAttributeSettings(selectedProcess, comingFromBatch) {
        $.ajax({
            type: "POST",
            url: "/Helpers/WebMethods.asmx/GetEquipmentSettingsByProcess",
            data: JSON.stringify({ experimentOrBatchProcessId: selectedProcess, comingFromBatch: comingFromBatch }),
            //async: true,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var jsonResult = JSON.parse(result.d);
                if (jsonResult.status == "ok") {

                    //console.log(JSON.stringify(jsonResult));
                    const resp = jsonResult.response;
                    if (resp != null) {
                        var html = GenerateAttributesFormWithValues(resp.equipmentSettingsValues);
                        //var html = "";
                        //for (const val of resp.equipmentSettingsValues) {
                        //    if (val.isParent == false && val.fkParentAttribute == null) {
                        //        html += '<div class="col-md-12">';
                        //        html += '<div class="form-group"><label>' + val.attributeName + " " + '(' + val.attMeasurementUnit + ')' + '</label><input type="' + val.type + '" data-id="' + val.fkAttribute + '" name="' + val.attributeName + '" placeholder="' + val.attributeName + '" value="' + val.value + '" data-attMeasurementUnit="' + val.attMeasurementUnit + '" data-isParent="' + val.isParent + '" data-fkParentAttribute="' + val.fkParentAttribute + '" class="form-control"></div>';
                        //        html += '</div>';
                        //    }
                        //    if (val.isParent == true) {
                        //        html += '<div class="col-sm-12">';
                        //        html += '<div style="border-top: 1px solid #F5F5F5;"><h4>' + val.attributeName + '</h4><input hidden value=" " data-isParent="' + val.isParent + '"name="' + val.attributeName + '"data-id="' + val.fkAttribute + '" ><div class="form-group"></div>';
                        //        html += '<div class=" duplicateChildBtn btn btn-xs" onclick="DuplicateChildNodes(' + val.fkAttribute + ')"><i class="fa fa-plus"></i></div>';
                        //        html += '</div></div>';
                        //        html += '<div class="col-sm-12" id="' + val.fkAttribute + '">';
                        //        html += '<div class="col-sm-12" id="pchild-' + val.fkAttribute + '-1"><div class="childNodes col-md-12">';
                        //        //html += ' <div class="btn btn-xs pull-right removeChild" onclick="DuplicateChildNodes(' + val.fkAttribute + ')"><i class="fa fa-minus"></i></div>';
                        //        for (const res of val.children) {
                        //            html += '<div class="form-group">';
                        //            html += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" value="' + res.value + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                        //            html += '</div>';
                        //        }
                        //        html += '</div></div>';

                        //        // html += '</div>'
                        //    }
                        //    html += '</div>';
                        //}


                        $("#form").empty();
                        $(html).appendTo('#form');
                        AddAutoHideEvent();
                    }
                    else {
                        $("#form").empty();
                        notify("No attributes found");
                    }
                }
            },
            error: function (p1, p2, p3) {
                alert(p1.status);
                alert(p3);
            }

        });
    }

    $('#selectEquipment').change(function () {
        $('#selectEquipmentModel').val("").trigger('change');

        var equipmentData = $('#selectEquipment').select2("data")[0];
        var processId = $('#selectProcess').val();
        if (equipmentData != undefined) {
            var id;
            var eqdata = equipmentData.data;
            if (eqdata == undefined) {
                id = equipmentData.id;
            } else {
                id = equipmentData.data.equipmentId;
            }
            var equipmentModelId = $('#selectEquipmentModel').val();
            $.ajax({
                type: "POST",
                url: "/Helpers/WebMethods.asmx/GetEquipmentSettingsByEquipmentId",
                data: JSON.stringify({ equipmentId: id, processId: processId, equipmentModelId: equipmentModelId }),
                //async: true,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var jsonResult = JSON.parse(result.d);
                    if (jsonResult.status == "ok") {
                        //console.log(JSON.stringify(jsonResult));
                        if (eqdata != undefined) {
                            const resp = jsonResult.response;
                            if (resp != null) {
                                var html = GenerateAttributesForm(resp);
                                $("#form").empty();
                                $(html).appendTo('#form');
                                AddAutoHideEvent();
                            }
                            else {
                                $("#form").empty();
                                notify("No attributes found ");
                            }
                        }
                    }
                },
                error: function (p1, p2, p3) {
                    alert(p1.status);
                    alert(p3);
                }

            });
            if (eqdata == undefined) {
                /* $('#selectEquipment').prop('disabled', true);
                 $('#selectEquipmentModel').prop('disabled', true);*/
            }
        } else {
            $("#form").empty();
        }
    });

    /*  $('#selectEquipmentModel').change(function () {
         // $("#form").empty();
          var selectedData = $('#selectEquipmentModel').select2("data")[0];
          if (selectedData != undefined) {
              var equipmentName = selectedData.data.equipmentName;
              var equipmentId = selectedData.data.fkEquipment;
              var equipment = new Option(equipmentName, equipmentId, true, true);
              $('#selectEquipment').append(equipment).trigger('change');
              //GenerateEquipmentAttributeSettings(selectedExperimentProcess, comingFromBatch);
          }
      });*/
    $('#selectEquipmentModel').change(function (e) {
        $('#selectPreviousProcess').val("").trigger('change.select2');

        $("#form").empty();
        var equipmentData;
        var equipmentId = $('#selectEquipment').val();
        if (equipmentId == null) {
            var selectedData = $('#selectEquipmentModel').select2("data")[0];
            if (selectedData != undefined) {
                var equipmentName = selectedData.data != undefined ? selectedData.data.equipmentName : selectedData.element.getAttribute("equipmentName");
                equipmentId = selectedData.data != undefined ? selectedData.data.fkEquipment : selectedData.element.getAttribute("equipmentId");
                var equipment = new Option(equipmentName, equipmentId, true, true);
                $('#selectEquipment').append(equipment).trigger('change.select2');
                //return true;
            }
        }
        if (equipmentId != null) {
            equipmentData = $('#selectEquipment').select2("data")[0];
        }
        var processId = $('#selectProcess').val();
        var equipmentModelId = $('#selectEquipmentModel').val();
        if (equipmentData != undefined) {
            if (equipmentData.data != undefined) {
                equipmentId = equipmentData.data.equipmentId;
            } else {
                equipmentId = equipmentData.id;
            }
            if (equipmentModelId != null) {
                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetEquipmentSettingsByEquipmentId",
                    data: JSON.stringify({ equipmentId: equipmentId, processId: processId, equipmentModelId: equipmentModelId }),
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {

                            //console.log(JSON.stringify(jsonResult));
                            if (equipmentData != undefined) {
                                const resp = jsonResult.response;
                                if (resp != null) {
                                    var html = GenerateAttributesForm(resp);
                                    $("#form").empty();
                                    $(html).appendTo('#form');
                                    AddAutoHideEvent();
                                }
                                else {
                                    $("#form").empty();
                                    notify("No attributes found");
                                }
                            }
                        }
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }

                });
            }
            //if (equipmentData == undefined) {
            //    // $('#selectEquipment').prop('disabled', true);
            //}
        } else {
            $("#form").empty();
        }
    });
}

function formatPreviousProcessDropdown(item) {
    //console.log("item " + item.ballMillCupsMaterial);
    var markup = "";

    processId = item.experimentProcessId;

    //item.lists.forEach(element => console.log(element.value));
    // if (item.label != null && item.label != "") {

    markup += item.dateCreated + " | " + item.label + " | " + item.processType + " | " + item.equipmentName + ((item.equipmentModelName !="") ? (" | " + item.equipmentModelName) : "");
    /*markup += item.dateCreated + " | ";
    for (const val of item.lists) {
        markup += val.attributeName + " : " + val.value + " | ";
    }*/
    //}
    // else { 
    //     markup += item.dateCreated + " | ";
    //     for (const val of item.lists) {
    //         markup +=  val.attributeName + " : " + val.value + " | ";
    //     }

    //}
    return markup;
    /* return markup.slice(0, -2);*/


}
function batchOrExperimentProcessId(item) {
    var id = null;
    processId = item.experimentProcessId;
    batchId = item.batchProcessId;
    if (processId == null) {
        id = batchId;
    } else {
        id = processId;
    }

    return id;
}
function formatProcessDropdown(item) {
    var markup = "";
    processId = item.experimentProcessId;

    markup += item.processType + " | " + item.subcategory;

    return markup;
}
function formatEquipmentModelDropdown(item) {
    var markup = "";
    processId = item.experimentProcessId;

    markup += item.equipmentModelName + " | " + item.modelBrand;

    return markup;
}

function formatProcess(e) {
    var markup = "" + e.id + " | " + e.text;
    return markup;
}
function NewProcessSequencePopup(table) {
    var html = '<div class="row">';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<select id="selectProcessSequence" class="batch-data-ajax"></select>';
    html += '</div></div>';
    html += '</div>';
    dialog = bootbox.dialog({
        title: 'Add process sequence',
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
                className: 'btn-primary add-button',
                callback: function () {

                    var selectedProcessSequence = $('#selectProcessSequence').val();
                    if (selectedProcessSequence != null) {
                        $.ajax({
                            type: "POST",
                            url: "/Helpers/WebMethods.asmx/GetValuesBySequenceId",
                            data: JSON.stringify({ processSequenceId: selectedProcessSequence }),
                            //async: true,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                var jsonResult = JSON.parse(result.d);
                                if (jsonResult.status == "ok") {
                                    const resp = jsonResult.response;
                                    if (resp != null) {
                                        for (const val of resp.processesList) {
                                            var processLabel = val.sequenceContent.processLabel;
                                            if (processLabel == null) {
                                                processLabel = "";
                                            }
                                            var processData = {};
                                            processData.fkProcessType = val.sequenceContent.fkProcessType,
                                                processData.processType = val.sequenceContent.processType,
                                                //processData.subcategory = val.sequenceContent.subcategory,
                                                processData.equipmentName = val.sequenceContent.equipmentName,
                                                processData.fkEquipment = val.sequenceContent.fkEquipment,
                                                processData.equipmentModelId = val.sequenceContent.fkEquipmentModel,
                                                processData.equipmentModelName = val.sequenceContent.equipmentModelName,
                                                processData.equipmentModelBrand = val.sequenceContent.modelBrand
                                            var row = GenerateProcessHtml(processLabel, processData, val.processAttributes);
                                            if ($('#' + table).hasClass("hidden"))
                                                $('#' + table).removeClass("hidden");
                                            if ($('#' + table).parents(".box").find(".saveAsBatchBtn").hasClass("hidden"))
                                                $('#' + table).parents(".box").find(".saveAsBatchBtn").removeClass("hidden");
                                            AppendRow($('#' + table), row);
                                        }

                                        bootbox.hideAll();
                                        return true;
                                    }
                                }
                            },
                            error: function (p1, p2, p3) {
                                alert(p1.status);
                                alert(p3);
                            }

                        });
                    }
                }
            },
        },
        onEscape: true
    });

    dialog.on('shown.bs.modal', function () {
        $("#selectProcessSequence").select2('open');
    });
    // $(".add-button").attr('disabled', 'disabled');
    reqUrlProcesses = "/Helpers/WebMethods.asmx/GetProcessSequence";
    $('#selectProcessSequence').select2({
        ajax: {
            type: "POST",
            url: reqUrlProcesses,
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
                    page: params.page || 1
                });
            },
            processResults: function (data, params) {
                var response = JSON.parse(data.d);
                return {
                    results: $.map(response.results, function (item) {
                        return {
                            id: item.processSequenceId,
                            text: formatProcessSequenceDropdown(item),
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
        //theme: "bootstrap",
        placeholder: 'Search for a process sequence',
        allowClear: true,
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });

}
function formatProcessSequenceDropdown(item) {

    var markup = "";
    markup += item.dateCreated + " | " + "Label: " + item.label + " | " + "Username: " + item.username + " | " + "Research Group: " + item.researchGroupName;
    return markup;
}
function AppendRow(table, row) {
    table.children('tbody').append(row);

    var tableId = table.attr('id');

    //sortable('#' + tableId + ' tbody.js-sortable-table', 'reload'); //does not reload options


    //sortable('#' + tableId + ' .js-sortable-table', {
    //    items: "tr.js-sortable-tr",
    //    handle: '.js-handle',
    //    placeholder: "<tr><td colspan=\"3\"><span class=\"center\">Place here</span></td></tr>",
    //    forcePlaceholderSize: false
    //});
    if (viewMode == false)
        MakeTablesSortable(tableId);
}
function RemoveRow(rowId) {
    $("#" + rowId).remove();
}
function ReplaceRow(oldRow, newRow) {
    var tableId = oldRow.parents('table').attr('id');
    oldRow.replaceWith(newRow);

    if (viewMode == false) {
        MakeTablesSortable(tableId);
    }
}
function ClearStepContent(componentType, stepNumber) {
    //var materialsBatchesTable = "materialsAndBatchesTable" + componentType + "Step" + stepNumber;
    //var processesTable = "processesTable" + componentType + "Step" + stepNumber;

    $("#" + "materialsAndBatchesTable" + componentType + "Step" + stepNumber + " tbody .experimentContentRow").remove();
    $("#" + "processesTable" + componentType + "Step" + stepNumber + " tbody .experimentProcessRow").remove();

}
function RemoveStepConfirm(componentType, stepId) {
    //console.log('confirm clicked');
    bootbox.confirm({
        message: "Are you sure you want to remove this step?",
        buttons: {
            confirm: {
                label: 'Yes',
                className: 'btn-primary'
            },
            cancel: {
                label: 'Cancel',
                className: 'btn-default'
            }
        },
        callback: function (result) {
            if (result) {
                RemoveStep(componentType, stepId);
                RenumberComponentSteps(componentType);
            }
        }
    });
}
function RemoveStep(componentType, stepId) {
    $("#" + componentType + 'Step' + stepId).remove();
}
function RenumberComponentSteps(componentType) {
    var stepsNumber = $('#' + componentType + ' .box-body').find(".row.step").length;
    //var stepNumber = 1;
    $('#' + componentType + ' .box-body').find(".row.step").each(function (index, value) {
        $(this).find('.box-title').html('Step ' + (index + 1));
    });
}
function GenerateCommercialTypeHtml(data) {
    //console.log(data);
    var rowId = guid('short');
    res = "";
    res += '<div class="table-responsive">';
    res += '<table id="commercialComponentTable' + data.fkBatteryComponentType + '" class="table table-condensed table-hover commercialComponentTable">' +
        '<tbody>' +
        '<tr>' +
        '<th>Selected component type</th>' +
        '<th>Model</th>' +
        '</tr>';


    res += '<tr id=\'' + rowId + '\'' + ' class=\'componentCommercialTypeRow\'' + ' data-item=\'' + JSON.stringify({ "type": "componentCommercialType", "item": data }) + '\'' + '>';
    res += '<td>' + data.batteryComponentCommercialType + '</td>';
    res += '<td>' + data.model + '</td>';
    res += '</tr>';

    res += '</tbody>';
    res += '</table>';
    res += '</div>';

    return res;
}

function GenerateMaterialHtml(data, viewMode) {
    var rowId = guid('short');
    var materialId = 0;
    var attr = 'materialId';
    if (attr in data) {
        materialId = data.materialId;
    }
    else materialId = data.fkStepMaterial;

    res = '<tr id=\'' + rowId + '\'' + ' class=\'experimentContentRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "material", "item": data }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div title="Remove" class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
    res += '</td>';

    res += '<td>';
    res += '<a href="/Materials/View/' + materialId + '" >';
    res += '<span class="badge bg-olive">' + data.chemicalFormula + '</span>';
    res += " " + data.materialName;
    res += '</a>';
    res += '</td>';

    var value = "";
    var attr = 'weight';
    if (attr in data) {
        value = data.weight != null ? data.weight : "";
    }
    res += '<td>';
    res += '<input type="text" name="quantity" value=\"' + value + '\" />';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-gray">' + data.measurementUnitSymbol + '</span>';
    res += '</td>';

    var measurements = "";
    var attr = 'measurements';
    if (attr in data) {
        //var measurementsItem = "";
        //console.log(measurements.item);
        if (data.measurements != null) {
            measurements = data.measurements;
            //measurementsItem = measurements.item;
        }
    }
    var measurementsData = JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements });
    //if (viewMode) {
    //    var measurementsButton = GenerateViewMeasurementsButton(measurementsData, 1);
    //}
    //else {
    //    var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 1);
    //}

    res += '<td class="table-actions-col">';
    if (sharedViewMode == false)
        res += '<a class="btn btn-primary btn-xs" target="_blank" href="/Materials/' + '"> Check Quantity</a> ';

    if (viewMode) {
        res += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsContentLevelBtn hidden" data-item=\'';
    }
    else {
        res += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsContentLevelBtn" data-item=\'';
    }
    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 1)" >' + ' Add measurements</a> ';


    if (viewMode) {
        res += '<a class="btn btn-primary btn-xs measurementsButton viewMeasurementsContentLevelBtn" data-item=\'';
    }
    else {
        res += '<a class="btn btn-primary measurementsButton btn-xs viewMeasurementsContentLevelBtn hidden" data-item=\'';
    }
    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="ViewMeasurements(this, 1)" >' + ' View measurements</a> ';

    //res += measurementsButton;
    res += '</td>';

    res += '</tr>';
    return res;
}
function GenerateBatchHtml(data, viewMode) {
    var rowId = guid('short');
    var batchId = 0;
    var attr = 'batchId';
    if (attr in data) {
        batchId = data.batchId;
    }
    else batchId = data.fkStepBatch;

    res = '<tr id=\'' + rowId + '\'' + ' class=\'experimentContentRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "batch", "item": data }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div title="Remove" class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
    res += '</td>';

    res += '<td>';
    if (sharedViewMode == true)
        res += '<a href="/Batches/Shared/View/' + batchId + '" >';
    else
        res += '<a href="/Batches/View/' + batchId + '" >';
    res += '<span class="badge bg-yellow-active">' + data.batchSystemLabel + '</span>';
    //res += "Batch_" + data.batchId;
    res += " " + data.batchPersonalLabel + ", " + data.description;
    res += '</a>';
    res += '</td>';

    var value = "";
    var attr = 'weight';
    if (attr in data) {
        value = data.weight != null ? data.weight : "";
    }
    res += '<td>';
    res += '<input type="text" name="quantity" value=\"' + value + '\" />';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-gray">' + data.measurementUnitSymbol + '</span>';
    res += '</td>';

    var measurements = "";
    var attr = 'measurements';
    if (attr in data) {
        if (data.measurements != null) {
            measurements = data.measurements;
        }
    }

    res += '<td class="table-actions-col">';
    if (sharedViewMode == true)
        res += '<a class="btn btn-primary btn-xs" target="_blank" href="/Batches/Shared/View/' + batchId + '"> View Contents</a> ';
    else
        res += '<a class="btn btn-primary btn-xs" target="_blank" href="/Batches/View/' + batchId + '"> View Contents</a> ';
    //res += '<a class="btn btn-primary btn-xs  addMeasurementsContentLevelBtn" data-item=\'';
    //if (viewMode) {
    //    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="ViewMeasurements(this, 1)" >' + ' View measurements</a> ';
    //}
    //else {
    //    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 1)" >' + ' Add measurements</a> ';
    //}

    if (viewMode) {
        res += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsContentLevelBtn hidden" data-item=\'';
    }
    else {
        res += '<a class="btn btn-primary btn-xs measurementsButton addMeasurementsContentLevelBtn" data-item=\'';
    }
    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 1)" >' + ' Add measurements</a> ';


    if (viewMode) {
        res += '<a class="btn btn-primary btn-xs measurementsButton viewMeasurementsContentLevelBtn" data-item=\'';
    }
    else {
        res += '<a class="btn btn-primary measurementsButton btn-xs viewMeasurementsContentLevelBtn hidden" data-item=\'';
    }
    res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="ViewMeasurements(this, 1)" >' + ' View measurements</a> ';

    res += '</td>';

    res += '</tr>';
    return res;
}

function GenerateProcessHtml(label, selectedProcessData, arr) {
    var fkProcessType = selectedProcessData.fkProcessType;
    var processType = selectedProcessData.processType;
    var fkEquipment = selectedProcessData.fkEquipment;
    var equipmentName = selectedProcessData.equipmentName;
    var equipmentModelId = selectedProcessData.equipmentModelId;
    var equipmentModelName = selectedProcessData.equipmentModelName;
    var equipmentModelBrand = selectedProcessData.equipmentModelBrand;
    //var subcategory = selectedProcessData.subcategory;
    var rowId = guid('short');

    res = '<tr id=\'' + rowId + '\'' + ' class=\'experimentProcessRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "process", "item": arr, "data": selectedProcessData, "label": label }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div title="Remove" class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + processType + '</span>';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + equipmentName + '</span>';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + label + '</span>';
    res += '</td>';

    //console.log(alpacaFormData);

    res += '<td> ';
    res += '</td>';

    res += '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs editSelectedProcessButton" data-item=\'';
    res += JSON.stringify({ "type": "process", "rowId": rowId, "processType": processType, "fkProcessType": fkProcessType, "fkEquipment": fkEquipment, "equipmentName": equipmentName, "equipmentModelId": equipmentModelId, "equipmentModelName": equipmentModelName, "equipmentModelBrand": equipmentModelBrand, "item": arr, "label": label }) + '\' onclick="EditSelectedProcess(this)" >' + ' Process Settings</a> ';
    res += '<a class="btn btn-primary btn-xs viewSelectedProcessButton hidden" data-item=\'';
    res += JSON.stringify({ "type": "process", "rowId": rowId, "processType": processType, "fkProcessType": fkProcessType, "fkEquipment": fkEquipment, "equipmentName": equipmentName, "equipmentModelId": equipmentModelId, "equipmentModelName": equipmentModelName, "equipmentModelBrand": equipmentModelBrand, "item": arr, "label": label }) + '\' onclick="ViewSelectedProcess(this)" >' + 'Process Settings</a> ';

    //var equipmentSettings = "";
    //var attr = 'equipmentSettings';
    //if (attr in data) {
    //    if (data.equipmentSettings != null) {
    //        equipmentSettings = data.equipmentSettings;
    //    }
    //}
   /* var equipmentId = null;
    if (alpacaFormData.fkEquipment) {
        equipmentId = alpacaFormData.fkEquipment
    }*/

   /* console.log("Eq id: " + equipmentId);
    var disabledTrue = "";
    if (equipmentId == null || equipmentId >= 15 && equipmentId <= 43) {
        disabledTrue = ' disabled="true" ';
    }*/
/*
    res += '</td>';
    res += '<td class="table-actions-col">' + '<input type="button" class="btn btn-primary btn-xs addEquipmentSettingsButton" data-item=\'';
    res += JSON.stringify({ "type": "equipment", "rowId": rowId, "item": equipmentSettings }) + '\' onclick="AddEquipmentSettings(this, ' + equipmentId + ')" ' + disabledTrue + ' value="Equipment Settings" /> ';
    res += '<input type="button" class="btn btn-primary btn-xs viewEquipmentSettingsButton hidden" data-item=\'';
    res += JSON.stringify({ "type": "equipment", "rowId": rowId, "item": equipmentSettings }) + '\' onclick="ViewEquipmentSettings(this, ' + equipmentId + ')" ' + disabledTrue + ' value="Equipment Settings" /> ';
    res += '</td>'*/;


    res += '</tr>';
    return res;
}
function GenerateProcessHtmlFromDatabase(experimentProcess, processAttributes, equipmentSettings, viewMode) {
    experimentProcessObj = {};
    experimentProcessObj.fkProcessType = experimentProcess.fkProcessType;
    experimentProcessObj.processType = experimentProcess.processType;
    experimentProcessObj.fkEquipment = experimentProcess.fkEquipment;
    experimentProcessObj.equipmentModelId = experimentProcess.equipmentModelId;
    experimentProcessObj.equipmentName = experimentProcess.equipmentName;
    experimentProcessObj.processDatabaseType = experimentProcess.processDatabaseType;
    experimentProcessObj.equipmentModelName = experimentProcess.equipmentModelName;
    //experimentProcessObj.subcategory = experimentProcess.subcategory;
    experimentProcessObj.equipmentModelBrand = experimentProcess.modelBrand;

    var arr = processAttributes;
    var rowId = guid('short');

    res = '<tr id=\'' + rowId + '\'' + ' class=\'experimentProcessRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "process", "item": arr, "data": experimentProcessObj, "label": experimentProcess.label }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div title="Remove" class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + experimentProcess.processType + '</span>';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + experimentProcess.equipmentName + '</span>';
    res += '</td>';

    res += '<td>';
    res += '<span class="badge bg-default">' + experimentProcess.label + '</span>';
    res += '</td>';

    //res += '<td>';
    //res += '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs editSelectedProcessButton" data-item=\'' + JSON.stringify({ "type": "process", "rowId": rowId, "item": processAttributes }) + '\' onclick="EditSelectedProcess(this)" >' + ' Edit Settings</a> ';
    //res += '</td>';
    res += '<td> ';
    res += '</td>';



    res += '<td class="table-actions-col">';
    res += '<a class="btn btn-primary btn-xs editSelectedProcessButton" data-item=\'';
    res += JSON.stringify({ "type": "process", "rowId": rowId, "processType": experimentProcess.processType, "fkProcessType": experimentProcess.fkProcessType, "fkEquipment": experimentProcess.fkEquipment, "equipmentName": experimentProcess.equipmentName, "equipmentModelId": experimentProcess.equipmentModelId, "equipmentModelName": experimentProcess.equipmentModelName, "equipmentModelBrand": experimentProcess.modelBrand, "item": arr, "label": experimentProcess.label }) + '\' onclick="EditSelectedProcess(this)" >' + ' Process Settings</a> ';

    //if (viewMode) {
    //    res += JSON.stringify({ "type": "process", "rowId": rowId, "item": processAttributes }) + '\' onclick="ViewSelectedProcess(this)" >' + ' View Settings</a> ';
    //}
    //else {
    //    res += JSON.stringify({ "type": "process", "rowId": rowId, "item": processAttributes }) + '\' onclick="EditSelectedProcess(this)" >' + ' Edit Settings</a> ';
    //}
    res += '<a class="btn btn-primary btn-xs viewSelectedProcessButton" data-item=\'';
    res += JSON.stringify({ "type": "process", "rowId": rowId, "processType": experimentProcess.processType, "fkProcessType": experimentProcess.fkProcessType, "fkEquipment": experimentProcess.fkEquipment, "equipmentName": experimentProcess.equipmentName, "equipmentModelId": experimentProcess.equipmentModelId, "equipmentModelName": experimentProcess.equipmentModelName, "equipmentModelBrand": experimentProcess.modelBrand, "item": arr, "label": experimentProcess.label }) + '\' onclick="ViewSelectedProcess(this)" >' + 'Process Settings</a> ';

    res += '</td>';

    /*  //var equipmentSettings = "";
      var equipmentId = null;
      if (processAttributes.fkEquipment) {
          equipmentId = processAttributes.fkEquipment
      }
  
      //console.log("Eq id: " + equipmentId);
      var disabledTrue = "";
      if (equipmentId == null) {
          disabledTrue = ' disabled="true" ';
      }
  
      if (equipmentSettings == null || $.isEmptyObject(equipmentSettings)) {
          equipmentSettings = "";
      }
      //console.log('eq settings from db: ' + $.isEmptyObject(equipmentSettings));
  
      res += '<td class="table-actions-col">' + '<input type="button" class="btn btn-primary btn-xs addEquipmentSettingsButton" data-item=\'';
      res += JSON.stringify({ "type": "equipment", "rowId": rowId, "item": equipmentSettings }) + '\' onclick="AddEquipmentSettings(this, ' + processAttributes.fkEquipment + ')" ' + disabledTrue + ' value="Equipment Settings" /> ';
      res += '<input type="button" class="btn btn-primary btn-xs viewEquipmentSettingsButton hidden" data-item=\'';
      res += JSON.stringify({ "type": "equipment", "rowId": rowId, "item": equipmentSettings }) + '\' onclick="ViewEquipmentSettings(this, ' + processAttributes.fkEquipment + ')" ' + disabledTrue + ' value="Equipment Settings" /> ';
      res += '</td>';
  
      res += '</td>';*/


    res += '</tr>';
    return res;
}
function RegenerateComponent(batteryComponent, viewMode) {
    var measurements = "";
    var attr = 'measurements';
    if (attr in batteryComponent) {
        if (batteryComponent.measurements != null) {
            measurements = batteryComponent.measurements;
            //console.log("componentMeasurement: " + measurements);
        }
    }
    var componentType = batteryComponent.componentType;

    var measurementsData = JSON.stringify({ "type": "measurements", "componentTypeId": "", "item": measurements });

    var viewTestDocumentsButton = "";
    if (viewMode) {
        //$("#" + componentType + " .box-tools .addMeasurementsComponentLevelBtn").remove();
        //$(" .box-tools .addMeasurementsComponentLevelBtn").remove();

        var measurementsButtonView = GenerateViewMeasurementsButton(measurementsData, 3, "");
        var measurementsButtonAdd = GenerateAddMeasurementsButton(measurementsData, 3, "hidden");


        var documentsButton = GenerateAddDocumentsButton(componentType);
        $("#" + componentType + " .boxButtons").prepend(documentsButton);
        viewTestDocumentsButton = GenerateViewTestDocButton(componentType);

    }
    else {
        $("#" + componentType + " .boxButtons .addMeasurementsComponentLevelBtn").remove();
        $("#" + componentType + " .boxButtons .viewMeasurementsComponentLevelBtn").remove();

        var measurementsButtonView = GenerateViewMeasurementsButton(measurementsData, 3, "hidden");
        var measurementsButtonAdd = GenerateAddMeasurementsButton(measurementsData, 3, "");

        $("#" + componentType + " .boxButtons .documentsButton").removeAttr('disabled').removeAttr("title");
        //$(" .box-tools .documentsButton").removeAttr('disabled');
        //$("#" + componentType + " .box-tools .documentsButton").remove();
    }

    $("#" + componentType + " .boxButtons").prepend(measurementsButtonView);
    $("#" + componentType + " .boxButtons").prepend(measurementsButtonAdd);
    $("#" + componentType + " .boxButtons").prepend(viewTestDocumentsButton);


    //$('.batteryComponent .box-tools').prepend(measurementsButton);
    //$("#" + componentType + " .box-tools .addMeasurementsComponentLevelBtn").data("item").item = measurements;

    if (batteryComponent.isCommercialType == true) {
        //html += GenerateStepMaterialViewHtml(value);
        var row = GenerateCommercialTypeHtml(batteryComponent.commercialType);

        //$('#' + componentType + " .box-body").html(row);
        $("#" + componentType + " .box-body .commercialTypeBtn").addClass("hidden");
        $("#" + componentType + " .box-body .previousTypeBtn").addClass("hidden");

        $('#' + componentType + " .box-body").append(row);

        $("#" + componentType + " .componentFooterButtons .submitComponentBtn").addClass("hidden");
        $("#" + componentType + " .componentFooterButtons .newStepBtn").addClass("hidden");
        $("#" + componentType + " .componentFooterButtons .submitComponentCommercialBtn").removeClass("hidden");

    }
    else {
        //html += GenerateStepBatchViewHtml(value);
        $.each(batteryComponent.batteryComponentSteps, function (key, step) {
            var measurements = "";
            var attr = 'measurements';
            if (attr in step) {
                if (step.measurements != null) {
                    measurements = step.measurements;
                    //console.log("stepMeasurement: " + measurements);
                }
            }
            var html = GenerateNewStepHtml(componentType, step.stepNumber, measurements, viewMode);


            $("#" + componentType + " .commercialTypeBtn").addClass("hidden");
            $("#" + componentType + " .box-body .previousTypeBtn").addClass("hidden");

            $('#' + componentType + ' .box-body:first').append(html);

            //if step saved as batch before, mark in html
            //if (step.isSavedAsBatch == true) {
            //    $('#' + componentType + "Step" + step.stepNumber).addClass('savedAsBatch');
            //}

            var materialsBatchesTable = "materialsAndBatchesTable" + componentType + "Step" + step.stepNumber;

            $.each(step.stepContent, function (key, stepContent) {

                if (stepContent.fkStepMaterial != null) {
                    var row = GenerateMaterialHtml(stepContent, viewMode);
                    AppendRow($('#' + materialsBatchesTable), row, viewMode);
                }
                else if (stepContent.fkStepBatch != null) {
                    var row = GenerateBatchHtml(stepContent, viewMode);
                    AppendRow($('#' + materialsBatchesTable), row, viewMode);
                }
            });
            if (step.stepContent != null) {
                if ($('#' + materialsBatchesTable).hasClass("hidden"))
                    $('#' + materialsBatchesTable).removeClass("hidden");
            }

            var processesTable = "processesTable" + componentType + "Step" + step.stepNumber;
            $.each(step.stepProcesses, function (key, stepProcess) {
                //var row = GenerateProcessHtml(stepProcess);
                var experimentProcess = stepProcess.stepProcess;
                var processAttributes = stepProcess.processAttributes;
                var equipmentSettings = stepProcess.equipmentSettings;
                var row = GenerateProcessHtmlFromDatabase(experimentProcess, processAttributes, equipmentSettings, viewMode);
                AppendRow($('#' + processesTable), row, viewMode);
                //console.log(stepProcess);
            });
            if (step.stepProcesses != null) {
                if ($('#' + processesTable).hasClass("hidden"))
                    $('#' + processesTable).removeClass("hidden");
            }

            //GenerateComponentStep(step);
        });
    }
}
function RegenerateCompletedContents(experiment, viewMode) {
    //console.log(experiment);
    //var experimentGeneralInfoHtml = GenerateExperimentGeneralHtml(experiment.experimentInfo);
    //$('#experimentGeneralInfo tbody').appendexperimentGeneralInfoHtml);

    var batteryComponents = experiment.batteryComponents;
    //console.log("cpts: " + batteryComponents);

    //if (batteryComponents == ""){
    //    var measurementsData = JSON.stringify({ "type": "measurements", "componentTypeId": "", "item": "" });
    //    var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 3);
    //    $('.batteryComponent .box-tools').prepend(measurementsButton);
    //}
    if (viewMode == false) {
        var measurementsData = JSON.stringify({ "type": "measurements", "componentTypeId": "", "item": "" });
        var measurementsButtonAdd = GenerateAddMeasurementsButton(measurementsData, 3, "");
        var measurementsButtonView = GenerateViewMeasurementsButton(measurementsData, 3, "hidden");
        $('.batteryComponent .boxButtons').prepend(measurementsButtonView);
        $('.batteryComponent .boxButtons').prepend(measurementsButtonAdd);
    }
    else {
        $(".boxButtons .documentsButton").remove();
    }

    $.each(batteryComponents, function (key, batteryComponent) {

        var componentType = batteryComponent.componentType;
        RegenerateComponent(batteryComponent, viewMode);

        if (viewMode == false)
            MakeTablesSortable(componentType);

        LockComponentEditing(componentType);
    });
    //var batchContentHtml = GenerateBatchContentViewHtml(batch.batchContentList);
    //$('#materialsAndBatchesTable tbody').append(batchContentHtml);
    //var batchProcessHtml = GenerateBatchProcessViewHtml(batch.batchProcessList);
    //$('#processesTable tbody').append(batchProcessHtml);

    //if (viewMode == true) {
    //    //change everything to view mode
    //    SetComponentsViewMode();
    //}
}

function EditSelectedProcess(element) {
    var processData = $(element).data("item");
    var fkProcessType = processData.fkProcessType;
    var processType = processData.processType;
    var fkEquipment = processData.fkEquipment;
    var equipmentName = processData.equipmentName;
    var equipmentModelId = processData.equipmentModelId;
    var equipmentModel = processData.equipmentModelName;
    var modelBrand = processData.equipmentModelBrand;
    //var subcategory = processData.subcategory;
    var label = processData.label;
    //var processDatabaseType = processData.item.processDatabaseType;
    var currentRowElement = $('tr#' + processData.rowId);

    var html = "";
    html += '<div class="row"><div class="col-md-12">';

    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Process: </label>';
    html += '<div class="col-sm-10">';
    html += '<span class="">' + processType + '</span>';
    html += '</div>';
    html += '</div>';
    html += '</div></div>';
    html += '<div class="row"><div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Equipment: </label>';
    html += '<div class="col-sm-10">';
    html += '<span class="">' + equipmentName + '</span>';
    html += '</div>';
    html += '</div>';
    html += '</div></div>';
    html += '<div class="row"><div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Model: </label>';
    html += '<div class="col-sm-10">';
    if (equipmentModel && modelBrand != null) {
        html += '<span class="">' + equipmentModel + " | " + modelBrand + '</span>';
    } else {
        html += '<span class="">' + "/ " + '</span>';
    }
    html += '</div>';
    html += '</div>';
    html += '</div></div>'
    html += '</br>';

    html += '<div class="row">';
    html += '<div class="col-md-12">';
    html += '<div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label>Label</label>';
    html += '<input type="text" id="label" name="label" value="' + label + '" placeholder="Label" class="form-control" >';
    html += '</div></div>';
    html += '<form id="editAttributesForm">';

    html += GenerateAttributesFormWithValues(processData.item);

    html += '</form></div>';
    html += '</div>';
    var editDialog = bootbox.dialog({
        title: 'Edit process attributes',
        message: html,
        buttons: {
            cancel: {
                label: "Cancel",
                className: 'btn-default',
                callback: function () {

                }
            },
            ok: {
                label: "Save",
                className: 'btn-primary save-button',
                callback: function () {

                    // Validate form
                    if ($('form#editAttributesForm').validate().form()) {
                        /*$("#alpaca-submit-button").prop("disabled", true);
                        var alpacaFormData = alpacaForm.alpaca().getValue();
                        var alpacaFormDataJson = JSON.stringify(alpacaFormData);*/

                        var label = $('#label').val();
                        var selectedProcessData = {};
                        selectedProcessData.processType = processType;
                        selectedProcessData.equipmentName = equipmentName;
                        selectedProcessData.fkProcessType = fkProcessType;
                        selectedProcessData.fkEquipment = fkEquipment;
                        selectedProcessData.equipmentModelId = equipmentModelId;
                        selectedProcessData.equipmentModelName = equipmentModel;
                        selectedProcessData.equipmentModelBrand = modelBrand;
                        //selectedProcessData.subcategory = subcategory;

                        var arr1 = CreateAttributeValuesArrayFromForm("editAttributesForm");

                        var row = GenerateProcessHtml(label, selectedProcessData, arr1);

                        ReplaceRow(currentRowElement, row);

                        bootbox.hideAll();

                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        },
        onEscape: true
    });
    editDialog.on('shown.bs.modal', function () {
        //$("#alpaca-universal-form").find('*').filter(':input:visible:first').focus();
        AddAutoHideEvent();
    });

    //var processTypeClassName = processType;
    //if (processType == "Phase inversion") {
    //    processTypeClassName = "PhaseInversion";
    //}
    //var processTypeClassName = processData.item.processDatabaseType;

    /*  var alpacaConfigJson = (function () {
          var json = null;
          $.ajax({
              'async': false,
              'global': false,
              'url': formsFolderURL + "/Definitions/" + processTypeClassName + ".json?nocache=123",
              'dataType': "json",
              'success': function (data) {
                  json = data;
              },
              'error': function (p1, p2, p3) {
                  notify(p2 + " - " + p3, "danger");
              }
          });
          return json;
      })();*/

    /*if (alpacaConfigJson != null) {
        alpacaConfigJson.postRender = alpacaFormPostRender;
        var formData = {};

        formData = processData.item;
        formData["form_action"] = 'New';
        formData["form_element"] = processType;
        alpacaConfigJson.data = formData;

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
        //$(".add-button").removeAttr('disabled');
    }*/

}

function ViewSelectedProcess(element) {
    var processData = $(element).data("item");
    var fkProcessType = processData.fkProcessType;
    var processType = processData.processType;
    var fkEquipment = processData.fkEquipment;
    var equipmentName = processData.equipmentName;
    var equipmentModel = processData.equipmentModelName;
    var modelBrand = processData.equipmentModelBrand;
    //var subcategory = processData.subcategory;
    var label = processData.label;
    var currentRowElement = $('tr#' + processData.rowId);

    //var processTypeClassName = processData.item.processDatabaseType;

    var html = "";
    html += '<div class="row"><div class="col-md-12">';

    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Process: </label>';
    html += '<div class="col-sm-10">';
    html += '<span class="">' + processType + '</span>';
    html += '</div>';
    html += '</div>';
    html += '</div></div>';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div id="alpaca-universal-form"> </div>';

    html += '</div></div>';

    var html = "";
    html += '<div class="row"><div class="col-md-12">';

    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Process: </label>';
    html += '<div class="col-sm-10">';
    html += '<span class="">' + processType + '</span>';
    html += '</div>';
    html += '</div>';
    html += '</div></div>';
    html += '<div class="row"><div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Equipment: </label>';
    html += '<div class="col-sm-10">';
    html += '<span class="">' + equipmentName + '</span>';
    html += '</div>';
    html += '</div>';
    html += '</div></div>';
    html += '<div class="row"><div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label class="col-sm-2 control-label">Model: </label>';
    html += '<div class="col-sm-10">';
    if (equipmentModel && modelBrand != null) {
        html += '<span class="">' + equipmentModel + " | " + modelBrand + '</span>';
    } else {
        html += '<span class="">' + "/ " + '</span>';
    }
    html += '</div>';
    html += '</div>';
    html += '</div></div>'
    html += '</br>';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div class="form-group">';
    html += '<label>Label</label>';
    html += '<input type="text" id="label" name="label" value="' + label + '" placeholder="Label" class="form-control" >';
    html += '</div></div>';
    html += '<form id="viewform">';

    for (const val of processData.item) {
        if (val.isParent == false && val.fkParentAttribute == null) {
            html += '<div class="col-md-12">';
            html += '<div class="form-group"><label>' + val.attributeName + " " + '(' + val.attMeasurementUnit + ')' + '</label><input type="' + val.type + '" data-id="' + val.fkAttribute + '" name="' + val.attributeName + '" placeholder="' + val.attributeName + '" value="' + val.value + '" data-attMeasurementUnit="' + val.attMeasurementUnit + '" data-isParent="' + val.isParent + '" data-fkParentAttribute="' + val.fkParentAttribute + '" class="form-control"></div>';
            html += '</div>';
        }
        else if (val.isParent == true) {
            html += '<div class="col-sm-12">';
            html += '<div style = "border-top: 1px solid #F5F5F5;" ><h4>' + val.attributeName + '</h4><input hidden value=" " data-isParent="' + val.isParent + '"name="' + val.attributeName + '"data-id="' + val.fkAttribute + '" ></div>';
            // html += '<div class="duplicateChildBtn btn btn-xs pull-right" onclick="DuplicateChildNodes(' + val.fkAttribute + ')"><i class="fa fa-plus"></i></div>';
            html += '</div>';
            html += '<div class="col-sm-12" id="' + val.fkAttribute + '">';
            //for (const res of processData.item){
            var firstChildId = 0;
            var counter = 1;
            if (val.children.length > 0) {
                firstChildId = val.children[0].fkAttribute;
            }
            var child = '<div class="col-sm-12" id="pchild-' + val.fkAttribute + '-1"><div class="childNodes col-md-12">';

            $.each(val.children, function (index, res) {

                if (index == 0 || (firstChildId != res.fkAttribute)) {
                    child += '<div class="form-group">';
                    child += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" value="' + res.value + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                    child += '</div>';
                }
                else if (firstChildId == res.fkAttribute) {
                    counter = counter + 1;
                    child += '</div></div>';
                    html += child;
                    child = "";
                    child += '<div class="col-sm-12" id="pchild-' + val.fkAttribute + '-' + counter + '"><div class="childNodes col-md-12">';
                    child += '<div class="form-group">';
                    child += '<label>' + res.attributeName + " " + '(' + res.attMeasurementUnit + ')' + '</label><input type="' + res.type + '" data-id="' + res.fkAttribute + '" name="' + res.attributeName + '" placeholder="' + res.attributeName + '" value="' + res.value + '" data-attMeasurementUnit="' + res.attMeasurementUnit + '" data-isParent="' + res.isParent + '" data-fkParentAttribute="' + res.fkParentAttribute + '" class="form-control">';
                    child += '</div>';

                }
                if (index == val.children.length - 1) {
                    child += '</div></div>';
                    html += child;
                }
            });
            html += '</div>'
        }
    }

    $(html).appendTo("#viewform");
    html += '</form></div>';

    var viewDialog = bootbox.dialog({
        title: 'View process settings',
        message: html,
        closeButton: true,
        onEscape: true,
        buttons: {
            cancel: {
                label: "Close",
                className: 'btn-default',
                callback: function () {

                }
            }
        }
    });

    // alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);

}

function AddEquipmentSettings(element, equipmentId) {

    var eqID = null;
    if (equipmentId != null) {
        eqID = equipmentId;
    }
    console.log(eqID);

    var settingsData = $(element).data("item");
    //var processTypeId = processData.item.processTypeId;
    //var equipmentId = processData.item.processTypeId;


    //var processType = settingsData.item.fkEquipment;
    //var currentRowElement = $('tr#' + settingsData.rowId);
    //console.log(measurementsData.rowId);


    var html = "";

    var previousProcessSettings = '<div class="row"><div class="col-md-12">';
    previousProcessSettings += '<div class="form-group">';
    previousProcessSettings += '<select id="selectPreviousEquipmentSetting" class="process-data-ajax hidden">';
    previousProcessSettings += '</select>';
    previousProcessSettings += '</div></div>';
    previousProcessSettings += '</div>';
    html += previousProcessSettings;

    //html += '<div class="row"><div class="col-md-12">';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div id="alpaca-universal-form"> </div>';
    html += '</div></div>';


    var editDialog = bootbox.dialog({
        title: 'Equipment settings',
        message: html,
        buttons: {
            cancel: {
                label: "Cancel",
                className: 'btn-default',
                callback: function () {

                }
            },
            ok: {
                label: "Save",
                className: 'btn-primary save-button',
                callback: function () {

                    $("#alpaca-submit-button").prop("disabled", true);
                    var alpacaFormData = alpacaForm.alpaca().getValue();
                    //var alpacaFormDataJson = JSON.stringify(alpacaFormData);

                    console.log("alpaca form data: " + JSON.stringify(alpacaFormData));
                    //$('tr#' + measurementsData.rowId).data("item", alpacaFormDataJson)

                    //ne se gleda promenata vo DOM
                    $(element).data("item").item = alpacaFormData;
                    //console.log("sibling data: " + $(element).siblings('.viewEquipmentSettingsButton'));
                    $(element).siblings('.viewEquipmentSettingsButton').data("item").item = alpacaFormData;
                    //$(element).data("item", alpacaFormDataJson);
                    //console.log("element data: " + JSON.stringify($(element).data("item")));
                    //var selectedProcessData = {};
                    //selectedProcessData.processTypeId = processTypeId;
                    //selectedProcessData.processType = processType;

                    //var row = GenerateProcessHtml(selectedProcessData, alpacaFormData);
                    //ReplaceRow(currentRowElement, row);

                    bootbox.hideAll();

                    return true;
                }
            }
        },
        onEscape: true
    });
    editDialog.on('shown.bs.modal', function () {
        $("#alpaca-universal-form").find('*').filter(':input:visible:first').focus();
    });


    //SELECT PREVIOUS SETTINGS
    var reqUrlProcessSettings = "/Helpers/WebMethods.asmx/GetRecentlyUsedEquipmentSettings";
    $('#selectPreviousEquipmentSetting').select2({
        ajax: {
            type: "POST",
            url: reqUrlProcessSettings,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }

                return JSON.stringify({ equipmentId: eqID });
            },
            processResults: function (data, params) {

                //console.log(JSON.parse(data.d).response);
                return {
                    results: $.map(JSON.parse(data.d).response, function (item) {
                        switch (eqID) {

                            case 1:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 2:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 3:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 4:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 5:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 6:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 7:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 8:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 9:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 10:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 11:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 12:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 13:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };
                            case 14:
                                return {
                                    id: item.settingsId,
                                    text: formatEquipmentSettingsDropdown(item, eqID),
                                    data: item
                                };

                        }
                    })
                };
            },
            cache: true
        },
        dropdownParent: editDialog,
        width: "100%",
        //theme: "bootstrap",
        //placeholder: 'Select previous setting',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcessSettings,
        //templateResult: function(e) { 
        //                return "<div class='select2-user-result'>" + JSON.stringify(e.data) + "</div>"; 
        //                },
        //templateSelection: formatRepoSelection
        allowClear: true,
        placeholder: {
            id: 'manual', // or whatever the placeholder value is
            text: 'Select previous setting...' // the text to display as the placeholder
        },
        //disabled: true,
        minimumResultsForSearch: Infinity
    });



    $('#selectPreviousEquipmentSetting').change(function () {
        setTimeout(function () {
            alpacaExists = $("#alpaca-universal-form").alpaca("exists");
            if (alpacaExists) {
                alpacaForm.alpaca("destroy");
            }

            var equipmentSettingsData = {};

            if ($('#selectPreviousEquipmentSetting').val() != null) {
                var selected = $('#selectPreviousEquipmentSetting').select2("data")[0];
                equipmentSettingsData = selected.data;
            }

            //console.log(selected);
            //console.log(equipmentSettingsData);

            //var selectedProcess = selected[0].data.processType;

            //reinitiate alpaca with values

            var alpacaConfigJson = (function () {
                var json = null;
                $.ajax({
                    'async': false,
                    'global': false,
                    //'url': "/Forms/Definitions/" + selectedProcess + ".json?fresh=1",
                    'url': formsFolderURL + "/Definitions/Equipment/" + eqID + ".json?nocache=123",
                    'dataType': "json",
                    'success': function (data) {
                        json = data;
                    },
                    'error': function (p1, p2, p3) {
                        notify(p2 + " - " + p3, "danger");
                    }
                });
                return json;
            })();

            if (alpacaConfigJson != null) {
                alpacaConfigJson.postRender = alpacaFormPostRender;
                var formData = equipmentSettingsData;

                formData["form_action"] = 'Add';
                formData["form_element"] = "equipment";
                formData["fkExperiment"] = experimentId;
                formData["fkEquipment"] = eqID;
                alpacaConfigJson.data = formData;

                //form-helpers.js
                setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
                setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

                alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
            }
        }, 50);
    });

    var alpacaConfigJson = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            //'url': formsFolderURL + "/Definitions/Measurements.json?nc=123",
            'url': formsFolderURL + "/Definitions/Equipment/" + eqID + ".json?nocache=123",
            'dataType': "json",
            'success': function (data) {
                json = data;
            },
            'error': function (p1, p2, p3) {
                notify(p2 + " - " + p3, "danger");
            }
        });
        return json;
    })();

    if (alpacaConfigJson != null) {
        alpacaConfigJson.postRender = alpacaFormPostRender;
        var formData = {};

        formData = settingsData.item;
        formData["form_action"] = 'Add';
        formData["form_element"] = "equipment";
        formData["fkExperiment"] = experimentId;
        formData["fkEquipment"] = eqID;
        alpacaConfigJson.data = formData;

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
    }



}

function formatEquipmentSettingsDropdown(item, equipmentId) {
    //console.log("item " + item.ballMillCupsMaterial);
    var markup = "";


    if (item.label != null && item.label != "") {
        markup += "Label: " + item.label;
        markup += " | " + item.dateCreated + " ";
    }
    else {
        markup += item.dateCreated + " ";
    }

    switch (equipmentId) {
        case 1:
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.ballPowderRatio != null && item.ballPowderRatio != "") {
                res += " | " + "Ball Powder Ratio: " + item.ballPowderRatio;
            }
            if (item.millingSpeed != null && item.millingSpeed != "") {
                res += " | " + "Milling Speed: " + item.millingSpeed;
            }
            if (item.millingTime != null && item.millingTime != "") {
                res += " | " + "Milling Time: " + item.millingTime;
            }
            if (item.restingTime != null && item.restingTime != "") {
                res += " | " + "Resting Time: " + item.restingTime;
            }
            if (item.loopCount != null && item.loopCount != "") {
                res += " | " + "Loop Count: " + item.loopCount;
            }
            if (item.cupVolume != null && item.cupVolume != "") {
                res += " | " + "Cup Volume: " + item.cupVolume;
            }
            if (item.cupMaterial != null && item.cupMaterial != "") {
                res += " | " + "Cup Material: " + item.cupMaterial;
            }
            if (item.ballsSize != null && item.ballsSize != "") {
                res += " | " + "Balls Size: " + item.ballsSize;
            }
            if (item.ballsMaterial != null && item.ballsMaterial != "") {
                res += " | " + "Balls Material: " + item.ballsMaterial;
            }
            if (item.amountOfBalls != null && item.amountOfBalls != "") {
                res += " | " + "Amount Of Balls: " + item.amountOfBalls;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}

            markup += res;
            break;

        case 2:
            //Planetary Mixer
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;

            }
            if (item.containerDiameterSize != null && item.containerDiameterSize != "") {
                res += " | " + "Container Diameter Size: " + item.containerDiameterSize;
            }
            if (item.amountOfContainers != null && item.amountOfContainers != "") {
                res += " | " + "Amount Of Containers: " + item.amountOfContainers;
            }
            if (item.programChannel != null && item.programChannel != "") {
                res += " | " + "Program Channel: " + item.programChannel;
            }
            if (item.manual != null && item.manual != "") {
                res += " | " + "Manual: " + item.manual;
            }
            if (item.rotationSpeed != null && item.rotationSpeed != "") {
                res += " | " + "Rotation Speed: " + item.rotationSpeed;
            }
            if (item.rotationTime != null && item.rotationTime != "") {
                res += " | " + "Rotation Time: " + item.rotationTime;
            }
            if (item.restTime != null && item.restTime != "") {
                res += " | " + "Rest Time: " + item.restTime;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 3:
            //Heating plate

            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.heatingTime != null && item.heatingTime != "") {
                res += " | " + "Heating Time: " + item.heatingTime;
            }
            if (item.stirring != null && item.stirring != "") {
                res += " | " + "Stirring: " + item.stirring;
            }
            if (item.stirringSpeed != null && item.stirringSpeed != "") {
                res += " | " + "Stirring Speed: " + item.stirringSpeed;
            }
            if (item.stirBarSize != null && item.stirBarSize != "") {
                res += " | " + "Stir Bar Size: " + item.stirBarSize;
            }
            if (item.atmosphere != null && item.atmosphere != "") {
                res += " | " + "Atmosphere: " + item.atmosphere;
            }
            if (item.flow != null && item.flow != "") {
                res += " | " + "Flow: " + item.flow;
            }
            if (item.flowRate != null && item.flowRate != "") {
                res += " | " + "Flow Rate: " + item.flowRate;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}            

            markup += res;
            break;

        case 4:
            //Tube furnace
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }
            //var value = (item.equipmentModelName != null && item.equipmentModelName != "") ? item.equipmentModelName : '/';
            //res += " | " + "Equipment Model: " + value;

            if (item.tubeMaterial != null && item.tubeMaterial != "") {
                res += " | " + "Tube Material: " + item.tubeMaterial;
            }
            if (item.tubeSize != null && item.tubeSize != "") {
                res += " | " + "Tube Size: " + item.tubeSize;
            }
            if (item.numberOfOpenings != null && item.numberOfOpenings != "") {
                res += " | " + "Number Of Openings: " + item.numberOfOpenings;
            }
            if (item.tubeVendor != null && item.tubeVendor != "") {
                res += " | " + "Tube Vendor: " + item.tubeVendor;
            }
            if (item.environmentType != null && item.environmentType != "") {
                res += " | " + "Environment Type: " + item.environmentType;
            }
            if (item.environmentVendor != null && item.environmentVendor != "") {
            }
            if (item.flowRate != null && item.flowRate != "") {
                res += " | " + "Flow Rate: " + item.flowRate;
            }
            if (item.pressure != null && item.pressure != "") {
                res += " | " + "Pressure: " + item.pressure;
            }
            if (item.rampUpTime != null && item.rampUpTime != "") {
                res += " | " + "Ramp Up Time: " + item.rampUpTime;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.duration != null && item.duration != "") {
                res += " | " + "Duration: " + item.duration;
            }
            if (item.rampDownTime != null && item.rampDownTime != "") {
                res += " | " + "Ramp Down Time: " + item.rampDownTime;
            }
            if (item.loopCount != null && item.loopCount != "") {
                res += " | " + "Loop Count: " + item.loopCount;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}

            markup += res;
            break;

        case 5:
            //Manual Hydraulic Press
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.substrate != null && item.substrate != "") {
                res += " | " + "Substrate: " + item.substrate;
            }
            if (item.material != null && item.material != "") {
                res += " | " + "Material: " + item.material;
            }
            if (item.materialType != null && item.materialType != "") {
                res += " | " + "Material Type: " + item.materialType;
            }
            if (item.thickness != null && item.thickness != "") {
                res += " | " + "Thickness: " + item.thickness;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.pressure != null && item.pressure != "") {
                res += " | " + "Pressure: " + item.pressure;
            }
            if (item.speed != null && item.speed != "") {
                res += " | " + "Speed: " + item.speed;
            }
            markup += res;
            break;

        case 6:
            //Heat Roller-Press
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.pressingFoil != null && item.pressingFoil != "") {
                res += " | " + "Pressing Foil: " + item.pressingFoil;
            }
            if (item.pressingFoilMaterial != null && item.pressingFoilMaterial != "") {
                res += " | " + "Pressing Foil Material: " + item.pressingFoilMaterial;
            }
            if (item.thickness != null && item.thickness != "") {
                res += " | " + "Thickness: " + item.thickness;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.pressure != null && item.pressure != "") {
                res += " | " + "Pressure: " + item.pressure;
            }
            if (item.speed != null && item.speed != "") {
                res += " | " + "Speed: " + item.speed;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 7:
            //Manual Press (Hans)
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.pressingFoil != null && item.pressingFoil != "") {
                res += " | " + "pressingFoil: " + item.pressingFoil;
            }
            if (item.pressingFoilMaterial != null && item.pressingFoilMaterial != "") {
                res += " | " + "pressingFoilMaterial: " + item.pressingFoilMaterial;
            }
            if (item.thickness != null && item.thickness != "") {
                res += " | " + "thickness: " + item.thickness;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "temperature: " + item.temperature;
            }
            if (item.pressure != null && item.pressure != "") {
                res += " | " + "pressure: " + item.pressure;
            }
            if (item.speed != null && item.speed != "") {
                res += " | " + "speed: " + item.speed;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "dateCreated: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 8:
            //Oven
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.heatingTime != null && item.heatingTime != "") {
                res += " | " + "Heating Time: " + item.heatingTime;
            }
            if (item.atmosphere != null && item.atmosphere != "") {
                res += " | " + "Atmosphere: " + item.atmosphere;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 9:
            //Heating Manual
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            if (item.heatingTime != null && item.heatingTime != "") {
                res += " | " + "Heating Time: " + item.heatingTime;
            }
            if (item.atmosphere != null && item.atmosphere != "") {
                res += " | " + "Atmosphere: " + item.atmosphere;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 10:
            //Pressing Manual

            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.pressingBlocks != null && item.pressingBlocks != "") {
                res += " | " + "Pressing Blocks: " + item.pressingBlocks;
            }
            if (item.substrateMaterial != null && item.substrateMaterial != "") {
                res += " | " + "Substrate Material: " + item.substrateMaterial;
            }
            if (item.pressure != null && item.pressure != "") {
                res += " | " + "Pressure: " + item.pressure;
            }
            if (item.pressingTime != null && item.pressingTime != "") {
                res += " | " + "Pressing Time: " + item.pressingTime;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 11:
            //Calendering Manual

            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 12:
            //Mortar and Pestle

            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.material != null && item.material != "") {
                res += " | " + "Material: " + item.material;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 13:
            //Mixing Ball mill

            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.ballPowderRatio != null && item.ballPowderRatio != "") {
                res += " | " + "Ball Powder Ratio: " + item.ballPowderRatio;
            }
            if (item.millingSpeed != null && item.millingSpeed != "") {
                res += " | " + "Milling Speed: " + item.millingSpeed;
            }
            if (item.millingTime != null && item.millingTime != "") {
                res += " | " + "Milling Time: " + item.millingTime;
            }
            if (item.restingTime != null && item.restingTime != "") {
                res += " | " + "Resting Time: " + item.restingTime;
            }
            if (item.loopCount != null && item.loopCount != "") {
                res += " | " + "Loop Count: " + item.loopCount;
            }
            if (item.cupVolume != null && item.cupVolume != "") {
                res += " | " + "Cup Volume: " + item.cupVolume;
            }
            if (item.cupMaterial != null && item.cupMaterial != "") {
                res += " | " + "Cup Material: " + item.cupMaterial;
            }
            if (item.ballsSize != null && item.ballsSize != "") {
                res += " | " + "Balls Size: " + item.ballsSize;
            }
            if (item.ballsMaterial != null && item.ballsMaterial != "") {
                res += " | " + "Balls Material: " + item.ballsMaterial;
            }
            if (item.amountOfBalls != null && item.amountOfBalls != "") {
                res += " | " + "Amount Of Balls: " + item.amountOfBalls;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;

        case 14:
            //Hot plate stirrer
            var res = "";

            if (item.equipmentModelName != null && item.equipmentModelName != "") {
                res += " | " + "Equipment Model: " + item.equipmentModelName;
            }

            if (item.rotationSpeed != null && item.rotationSpeed != "") {
                res += " | " + "Rotation Speed: " + item.rotationSpeed;
            }
            if (item.rotationTime != null && item.rotationTime != "") {
                res += " | " + "Rotation Time: " + item.rotationTime;
            }
            if (item.restTime != null && item.restTime != "") {
                res += " | " + "Rest Time: " + item.restTime;
            }
            if (item.temperature != null && item.temperature != "") {
                res += " | " + "Temperature: " + item.temperature;
            }
            //if (item.dateCreated != null && item.dateCreated != "") {
            //    res += " | " + "Date: " + item.dateCreated;
            //}
            markup += res;
            break;
    }
    return markup;
}

function ViewEquipmentSettings(element, equipmentId) {
    var eqID = null;
    if (equipmentId != null) {
        eqID = equipmentId;
    }

    var settingsData = $(element).data("item");
    //var processTypeId = settingsData.item.processTypeId;
    var equipmentModelName = settingsData.item.equipmentModelName;
    var currentRowElement = $('tr#' + settingsData.rowId);

    var html = "";
    //html += '<div class="row"><div class="col-md-12">';
    //html += '<div class="form-group">';
    //html += '<label class="col-sm-2 control-label">Equipment model: </label>';
    //html += '<div class="col-sm-10">';
    //html += '<span class="">' + equipmentModelName + '</span>';
    //html += '</div>';
    //html += '</div>';
    //html += '</div></div>';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div id="alpaca-universal-form"> </div>';

    html += '</div></div>';


    var alpacaConfigJson = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': formsFolderURL + "/Definitions/Equipment/" + eqID + ".json?nocache=123",
            'dataType': "json",
            'success': function (data) {
                json = data;
            },
            'error': function (p1, p2, p3) {
                notify(p2 + " - " + p3, "danger");
            }
        });
        return json;
    })();

    if (alpacaConfigJson != null) {
        alpacaConfigJson.postRender = alpacaFormPostRender;
        //alpacaConfigJson.options.form.buttons = {};
        var formData = {};
        formData = settingsData.item;
        formData["form_action"] = 'View';
        formData["form_element"] = "equipment";
        formData["fkExperiment"] = experimentId;
        formData["fkEquipment"] = eqID;
        alpacaConfigJson.data = formData;
        alpacaConfigJson.view.parent = "bootstrap-display";

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        var viewDialog = bootbox.dialog({
            title: 'Equipment settings',
            message: html,
            closeButton: true,
            onEscape: true,
            buttons: {
                cancel: {
                    label: "Close",
                    className: 'btn-default',
                    callback: function () {

                    }
                }
            }
        });

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
    }

}

//function AddMeasurements(element, experimentId, batteryComponentTypeId, stepId, batteryComponentContentId) {
function AddMeasurements(element, measurementLevelTypeId) {
    var measurementsData = $(element).data("item");
    //var processTypeId = processData.item.processTypeId;
    //var processType = processData.item.processType;
    //var currentRowElement = $('tr#' + measurementsData.rowId);
    //console.log(measurementsData.rowId);


    var html = "";
    html += '<div class="row"><div class="col-md-12">';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div id="alpaca-universal-form"> </div>';

    html += '</div></div>';


    var editDialog = bootbox.dialog({
        title: 'Add measurements',
        message: html,
        buttons: {
            cancel: {
                label: "Cancel",
                className: 'btn-default',
                callback: function () {

                }
            },
            ok: {
                label: "Save",
                className: 'btn-primary save-button',
                callback: function () {

                    $("#alpaca-submit-button").prop("disabled", true);
                    var alpacaFormData = alpacaForm.alpaca().getValue();
                    //var alpacaFormDataJson = JSON.stringify(alpacaFormData);

                    console.log("alpaca form data: " + JSON.stringify(alpacaFormData));
                    //$('tr#' + measurementsData.rowId).data("item", alpacaFormDataJson)
                    $(element).data("item").item = alpacaFormData;
                    $(element).siblings(".measurementsButton").data("item").item = alpacaFormData;
                    //$(element).data("item", alpacaFormDataJson);
                    console.log("element data: " + JSON.stringify($(element).data("item")));
                    //var selectedProcessData = {};
                    //selectedProcessData.processTypeId = processTypeId;
                    //selectedProcessData.processType = processType;

                    //var row = GenerateProcessHtml(selectedProcessData, alpacaFormData);
                    //ReplaceRow(currentRowElement, row);

                    bootbox.hideAll();

                    return true;
                }
            }
        },
        onEscape: true
    });
    editDialog.on('shown.bs.modal', function () {
        $("#alpaca-universal-form").find('*').filter(':input:visible:first').focus();
    });

    var alpacaConfigJson = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': formsFolderURL + "/Definitions/Measurements.json?nc=123",
            'dataType': "json",
            'success': function (data) {
                json = data;
            },
            'error': function (p1, p2, p3) {
                notify(p2 + " - " + p3, "danger");
            }
        });
        return json;
    })();

    if (alpacaConfigJson != null) {
        alpacaConfigJson.postRender = alpacaFormPostRender;
        var formData = {};

        formData = measurementsData.item;
        formData["form_action"] = 'Add';
        formData["form_element"] = "measurements";
        formData["fkExperiment"] = experimentId;
        formData["fkMeasurementLevelType"] = measurementLevelTypeId;
        if (measurementLevelTypeId == 2) {
            formData["stepId"] = measurementsData.step;
        }
        alpacaConfigJson.data = formData;

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
        //$(".add-button").removeAttr('disabled');
    }
}

function ViewMeasurements(element, measurementLevelTypeId) {
    var measurementsData = $(element).data("item");

    var html = "";
    html += '<div class="row"><div class="col-md-12">';

    html += '<div class="row"><div class="col-md-12">';
    html += '<div id="alpaca-universal-form"> </div>';

    html += '</div></div>';


    var alpacaConfigJson = (function () {
        var json = null;
        $.ajax({
            'async': false,
            'global': false,
            'url': formsFolderURL + "/Definitions/Measurements.json?nc=123",
            'dataType': "json",
            'success': function (data) {
                json = data;
            },
            'error': function (p1, p2, p3) {
                notify(p2 + " - " + p3, "danger");
            }
        });
        return json;
    })();

    if (alpacaConfigJson != null) {
        alpacaConfigJson.options.form.buttons = {};
        var formData = {};
        formData = measurementsData.item;
        formData["form_action"] = 'View';
        formData["form_element"] = "measurements";
        formData["fkExperiment"] = experimentId;
        formData["fkMeasurementLevelType"] = measurementLevelTypeId;
        if (measurementLevelTypeId == 2) {
            formData["stepId"] = measurementsData.step;
        }
        alpacaConfigJson.data = formData;
        alpacaConfigJson.view.parent = "bootstrap-display";

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        var viewDialog = bootbox.dialog({
            title: 'View measurements',
            message: html,
            closeButton: true,
            onEscape: true,
            buttons: {
                cancel: {
                    label: "Close",
                    className: 'btn-default',
                    callback: function () {

                    }
                }
            }
        });

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
        //$(".add-button").removeAttr('disabled');


    }
}

function CreateComponentsJsonRequestObject(componentTypes) {
    var jsonRequestObject = {};
    var componentsList = [];

    $.each(componentTypes, function (index, componentType) {
        var BatteryComponentObject = {};

        var batteryComponent = $('#' + componentType);
        var batteryComponentSteps = $('#' + componentType + ' .step');

        var experimentInfo = {};
        var component = {};

        var componentStepList = [];
        var step = 0;

        batteryComponentSteps.each(function (id) {
            var materialsAndBatches = $(this).find(".materialsAndBatchesTable tbody tr.experimentContentRow");
            var processes = $(this).find(".processesTable tbody tr.experimentProcessRow");

            step = id + 1;
            var componentStep = {};
            componentStep["isSavedAsBatch"] = false;
            if ($(this).hasClass("savedAsBatch")) {
                //console.log("step " + id +" is saved as a batch.");
                componentStep["isSavedAsBatch"] = true;
            }

            var stepContentList = [];
            var stepProcessList = [];

            materialsAndBatches.each(function (id) {
                var quantity = parseFloat($(this).find("input[name='quantity']").val());
                var row = $(this).data("item");
                var itemType = row.type;
                var item = row.item;

                var stepContent = {};
                stepContent["fkExperiment"] = experimentId;
                stepContent["step"] = step;
                if (itemType == "material") {
                    var attr = 'materialId';
                    if (attr in item) {
                        stepContent["fkStepMaterial"] = item.materialId;
                    }
                    else stepContent["fkStepMaterial"] = item.fkStepMaterial;

                    stepContent["materialName"] = item.materialName;
                }
                else if (itemType == "batch") {
                    var attr = 'batchId';
                    if (attr in item) {
                        stepContent["fkStepBatch"] = item.batchId;
                    }
                    else stepContent["fkStepBatch"] = item.fkStepBatch;

                    stepContent["batchSystemLabel"] = item.batchSystemLabel;
                    stepContent["batchPersonalLabel"] = item.batchPersonalLabel;
                }
                stepContent["weight"] = quantity;
                stepContent["fkFunction"] = 1;
                stepContent["fkStoredInType"] = 1;
                stepContent["orderInStep"] = id + 1;

                stepContentList.push(stepContent);
            });
            processes.each(function (id) {
                var row = $(this).data("item");
                var itemType = row.type;
                var item = row.item;

                var stepProcess = {};
                stepProcess["step"] = step;
                stepProcess["fkProcessType"] = item.processTypeId;
                stepProcess["processOrderInStep"] = id + 1;
                stepProcess["processAttributes"] = item;

                stepProcessList.push(stepProcess);
            });

            componentStep["stepContent"] = stepContentList;
            componentStep["stepProcesses"] = stepProcessList;
            componentStepList.push(componentStep);
        });

        BatteryComponentObject["experimentId"] = experimentId;
        BatteryComponentObject["componentType"] = componentType;
        BatteryComponentObject["componentSteps"] = componentStepList;
        componentsList.push(BatteryComponentObject);
    });
    jsonRequestObject["batteryComponents"] = componentsList;

    return jsonRequestObject;


    //nadole ne treba

    //reqUrl = "/Helpers/WebMethods.asmx/SubmitExperimentComponent";

    //var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });
    //$.ajax({
    //    type: "POST",
    //    url: reqUrl,
    //    data: RequestDataString,
    //    //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (result) {
    //        var jsonResult = JSON.parse(result.d);
    //        //console.log(jsonResult);
    //        if (jsonResult.status == "ok") {
    //            //notify("Component step saved", "success");

    //            //var html = "<span><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";
    //            //$('#submit' + componentType + 'Button').parent().html(html);
    //            LockComponentEditing(componentType);


    //            //collapse component box
    //            //box.find('.box-header button').trigger('click')
    //            //$('#' + componentType).addClass('collapsed-box');
    //            //$('#' + componentType + " .btn-box-tool").trigger('click');
    //        }
    //        else {
    //            notify(jsonResult.message, "warning");
    //        }
    //    },
    //    error: function (p1, p2, p3) {
    //        alert(p1.status.toString() + " " + p3.toString());
    //    }
    //}).done(function () {


    //});

}

function CreateComponentJsonRequestObject(componentType) {
    var jsonRequestObject = {};

    var batteryComponent = $('#' + componentType);
    var batteryComponentSteps = $('#' + componentType + ' .step');

    var batteryComponentStepsNumber = $('#' + componentType + ' .step').length;
    //console.log(batteryComponentSteps);

    //var stepInfo = [];
    var experimentInfo = {};
    var component = {};

    var componentStepList = [];
    var step = 0;

    batteryComponentSteps.each(function (id) {
        var materialsAndBatches = $(this).find(".materialsAndBatchesTable tbody tr.experimentContentRow");
        var processes = $(this).find(".processesTable tbody tr.experimentProcessRow");
        //console.log(materialsAndBatches);
        step = id + 1;
        var componentStep = {};
        componentStep["stepNumber"] = step;
        componentStep["isSavedAsBatch"] = false;
        if ($(this).hasClass("savedAsBatch")) {
            //console.log("step " + id +" is saved as a batch.");
            componentStep["isSavedAsBatch"] = true;
        }
        var stepContentList = [];
        var stepProcessList = [];

        materialsAndBatches.each(function (id) {
            //var quantity = parseFloat($(this).find("input[name='quantity']").val());
            var quantity = $(this).find("input[name='quantity']").val().replace(',', '.');
            //var percentageOfActive = parseFloat($(this).find("input[name='percentageOfActive']").val());
            var row = $(this).data("item");
            var itemType = row.type;
            var item = row.item;

            var measurements = $(this).find(".addMeasurementsContentLevelBtn").data("item");
            var measurementsItem = measurements.item;
            //console.log("data: " + JSON.stringify(measurementsItem));

            var stepContent = {};
            stepContent["fkExperiment"] = experimentId;
            stepContent["step"] = step;
            if (itemType == "material") {
                var attr = 'materialId';
                if (attr in item) {
                    stepContent["fkStepMaterial"] = item.materialId;
                }
                else stepContent["fkStepMaterial"] = item.fkStepMaterial;
                stepContent["materialName"] = item.materialName;
                //return;
            }
            else if (itemType == "batch") {
                var attr = 'batchId';
                if (attr in item) {
                    stepContent["fkStepBatch"] = item.batchId;
                }
                else stepContent["fkStepBatch"] = item.fkStepBatch;
                stepContent["batchLabel"] = item.batchLabel;
            }
            stepContent["weight"] = quantity;
            stepContent["fkFunction"] = item.fkFunction;
            var percentageOfActive = item.percentageOfActive;
            if (percentageOfActive != null)
                percentageOfActive = percentageOfActive.toString().replace(',', '.');
            stepContent["percentageOfActive"] = percentageOfActive;
            //stepContent["fkStoredInType"] = item.fkStoredInType;
            stepContent["orderInStep"] = id + 1;

            stepContent["measurements"] = measurementsItem;

            stepContentList.push(stepContent);
        });
        processes.each(function (id) {
            var row = $(this).data("item");
            var itemType = row.type;
            var item = row.item;
            //var equipmentSettings = $(this).find(".editSelectedProcessButton").data("item");
            //var equipmentSettingsItem = equipmentSettings.item;

            var stepProcess = {};
            stepProcess["step"] = step;
            stepProcess["fkProcessType"] = row.data.fkProcessType;
            stepProcess["processType"] = row.data.processType;
            stepProcess["equipmentId"] = row.data.fkEquipment;
            stepProcess["equipmentModelId"] = row.data.equipmentModelId;
            stepProcess["label"] = row.label;
            stepProcess["processOrderInStep"] = id + 1;

            // stepProcess["processAttributes"] = item;
            stepProcess["equipmentSettings"] = item;

            stepProcessList.push(stepProcess);
        });

        componentStep["stepContent"] = stepContentList;
        componentStep["stepProcesses"] = stepProcessList;

        var measurements = $(this).find(".addMeasurementsStepLevelBtn").data("item");
        var measurementsItem = measurements.item;
        console.log("step level measurements: " + JSON.stringify(measurementsItem));
        componentStep["measurements"] = measurementsItem;

        //console.log(componentStep);
        componentStepList.push(componentStep);
        //console.log(JSON.stringify(componentStepList));

    });

    jsonRequestObject["experimentId"] = experimentId;
    jsonRequestObject["componentType"] = componentType;
    jsonRequestObject["componentSteps"] = componentStepList;

    jsonRequestObject["componentEmpty"] = false;
    if (batteryComponentStepsNumber == 0) {
        jsonRequestObject["componentEmpty"] = true;
    }

    var measurements = $('#' + componentType).find(".addMeasurementsComponentLevelBtn").data("item");
    var measurementsItem = measurements.item;
    //console.log("component level measurements: " + JSON.stringify(measurementsItem));
    jsonRequestObject["measurements"] = measurementsItem;

    return jsonRequestObject;

}

function CreateExperimentGeneralDataJsonRequestObject() {
    var jsonRequestObject = {};


    var experimentInfo = {};
    var experimentPersonalLabel = $('#experimentPersonalLabel').val();
    var experimentDescription = $('#experimentDescription').val();
    var projectName = $('#projectName').val();

    experimentInfo["experimentPersonalLabel"] = experimentPersonalLabel;
    experimentInfo["experimentDescription"] = experimentDescription;
    experimentInfo["projectName"] = projectName;

    //var batteryComponents = [];

    jsonRequestObject["experimentInfo"] = experimentInfo;
    //jsonRequestObject["batteryComponents"] = batteryComponents;

    //console.log(jsonObject);
    //jsonString = JSON.stringify(jsonRequestObject);


    //var measurements = $("#Batch .addMeasurementsBatchLevelBtn").data("item");
    //var measurementsItem = measurements.item;
    ////console.log("experiment level measurements: " + JSON.stringify(measurementsItem));
    //jsonRequestObject["measurements"] = measurementsItem;

    return jsonRequestObject;
}
function SubmitExperimentGeneralData(experimentId) {
    jsonRequestObject = CreateExperimentGeneralDataJsonRequestObject();

    reqUrl = "/Helpers/WebMethods.asmx/UpdateExperimentGeneralData";

    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject), experimentId: experimentId });

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
                notify("General data updated", "success");

                //console.log(JSON.stringify(jsonResult));

                if (jsonResult.message != null) {
                    $('#' + 'GeneralInfo' + ' .box-footer .lastSave').css("display", "none");
                    $('#' + 'GeneralInfo' + ' .box-footer .lastSave i').html(jsonResult.message);
                    $('#' + 'GeneralInfo' + ' .box-footer .lastSave').fadeToggle("slow", "linear");
                }
            }
            else {
                $('#' + 'GeneralInfo' + ' .box-footer .lastSave').css("display", "none");
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}
function SubmitExperimentGeneralDataAndFinish(experimentId) {
    jsonRequestObject = CreateExperimentGeneralDataJsonRequestObject();

    reqUrl = "/Helpers/WebMethods.asmx/UpdateExperimentGeneralDataAndFinish";

    var RequestDataString = JSON.stringify({
        formData: JSON.stringify(jsonRequestObject),
        experimentId: experimentId
    });

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
                //notify("General data updated", "success");
                //console.log(JSON.stringify(jsonResult));
                window.location.replace("/Experiments/");

            }
            else {
                $('#' + 'GeneralInfo' + ' .box-footer .lastSave').css("display", "none");
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}
function SubmitComponent(componentType, experimentId) {
    var jsonRequestObject = CreateComponentJsonRequestObject(componentType);

    //console.log(component);
    //console.log(JSON.stringify(component));
    //console.log("component json request: " + JSON.stringify(jsonRequestObject));

    reqUrl = "/Helpers/WebMethods.asmx/SubmitExperimentComponent";

    var RequestDataString = JSON.stringify({
        formData: JSON.stringify(jsonRequestObject),
        experimentId: experimentId
    });
    var ajaxData = $.ajax({
        type: "POST",
        url: reqUrl,
        data: RequestDataString,
        //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            //console.log(jsonResult);
            if (jsonResult.status == "ok") {
                //notify("Component step saved", "success");

                //var html = "<span><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";
                //$('#submit' + componentType + 'Button').parent().html(html);
                LockComponentEditing(componentType);

                //collapse component box
                //box.find('.box-header button').trigger('click')
                //$('#' + componentType).addClass('collapsed-box');
                //$('#' + componentType + " .btn-box-tool").trigger('click');
                if (!$('#' + componentType).hasClass('collapsed-box')) {
                    //collapse component after submitting 
                    $('#' + componentType + " .btn-box-tool.component-box-tool").trigger('click');
                    //console.log('trigerred 1');
                }
            }
            else {
                notify(jsonResult.message, "warning");
                ajaxResultsValidForSubmit = false;
                MarkComponentInvalid(componentType);
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
            ajaxResultsValidForSubmit = false;
        }
    }).done(function () {
        //LockComponentEditing(componentType);
        //UnlockComponentEditing(componentType);


        //$(this).addClass("done");
        //$('#recreateBatchBtn').removeAttr('disabled');

        //var html = "<span><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";
        //$('#submitAnodeButton').parent().html(html);

        //var boxFooter = ('#submitAnodeButton').parent();        
        //$('#submitAnodeButton').parent().children('input').remove();

        //boxFooter

    });

    return ajaxData;
}
function SubmitComponentCommercalType(componentType) {
    var jsonRequestObject = {};

    var batteryComponent = $('#' + componentType);
    var selectedCommercialType = $('#' + componentType + ' .componentCommercialTypeRow');
    jsonRequestObject["experimentId"] = experimentId;
    jsonRequestObject["componentType"] = componentType;

    if (selectedCommercialType.length > 0) {
        jsonRequestObject["componentEmpty"] = false;
        var row = $(selectedCommercialType).data("item");
        var itemType = row.type;
        var item = row.item;
        var commercialTypeId = item.batteryComponentCommercialTypeId;
        //{"type":"componentCommercialType","item":{"batteryComponentCommercialTypeId":1,"fkBatteryComponentType":1,"batteryComponentCommercialType":"anode1"}}

        jsonRequestObject["commercialTypeId"] = commercialTypeId;
        //console.log(JSON.stringify(jsonRequestObject));
    }
    else {
        jsonRequestObject["componentEmpty"] = true;
    }

    reqUrl = "/Helpers/WebMethods.asmx/SubmitExperimentComponentCommercialType";
    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject) });
    var ajaxData = $.ajax({
        type: "POST",
        url: reqUrl,
        data: RequestDataString,
        //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            //console.log(jsonResult);
            if (jsonResult.status == "ok") {
                LockComponentEditing(componentType);
                if (!$('#' + componentType).hasClass('collapsed-box')) {
                    //collapse component after submitting 
                    $('#' + componentType + " .btn-box-tool.component-box-tool").trigger('click');
                    //console.log('trigerred');
                }
            }
            else {
                notify(jsonResult.message, "warning");
                ajaxResultsValidForSubmit = false;
                MarkComponentInvalid(componentType);
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
            ajaxResultsValidForSubmit = false;
        }
    }).done(function () {

    });

    return ajaxData;
}



function StartComponentOver(componentType) {
    $('#' + componentType + ' .box-body .table-responsive').remove();
    $('#' + componentType + ' .box-body .step').remove();
    $('#' + componentType + ' .box-body .commercialTypeBtn ').removeClass('hidden');
    $("#" + componentType + " .box-body .previousTypeBtn").removeClass("hidden");

    $('#' + componentType + ' .componentFooterButtons input').removeClass('hidden');
    if (componentType != "ReferenceElectrode")
        $('#' + componentType + ' .componentFooterButtons input.submitComponentCommercialBtn').addClass('hidden');
}

function LockComponentEditing(componentType) {
    $("#" + componentType + " .commercialTypeBtn").addClass("hidden");
    $("#" + componentType + " .box-body .previousTypeBtn").addClass("hidden");

    $('#' + componentType + ' .editComponentButton').removeClass('hidden');
    $('#' + componentType + ' .startComponentOverButton').addClass('hidden');

    //var savedHtml = "<span class = \"saved\"><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";

    //"input[name='quantity']"
    $('#' + componentType).removeClass('box-warning');
    $('#' + componentType).addClass('box-success');

    $('#' + componentType + ' .removeStepBtn').addClass('hidden');

    $('#' + componentType + ' .stepContentButtons input').attr('disabled', 'disabled');
    $('#' + componentType + ' .removeRow').addClass('hidden');
    $('#' + componentType + ' input[name="quantity"]').attr('disabled', 'disabled');
    $('#' + componentType + ' .experimentProcessRow .editSelectedProcessButton').addClass('hidden');
    $('#' + componentType + ' .experimentProcessRow .viewSelectedProcessButton').removeClass('hidden');

    $('#' + componentType + ' .experimentProcessRow .addEquipmentSettingsButton').addClass('hidden');
    $('#' + componentType + ' .experimentProcessRow .viewEquipmentSettingsButton').removeClass('hidden');

    $('#' + componentType + ' .componentFooterButtons input').addClass('hidden');
    //$('#submit' + componentType + 'Button').parent().append(savedHtml);

    //$('#' + componentType + ' .componentFooterButtons').append(savedHtml);
    $('#' + componentType + ' .componentFooterButtons .componentSaved').removeClass('hidden');

    $("#" + componentType + " .documentsButton").removeAttr('disabled').removeAttr("title");
    $("#" + componentType + " .stepButtons .documentsButton").removeAttr('disabled').removeAttr("title");


    $('#' + componentType + ' .js-handle').addClass('hidden');
    $('#' + componentType + ' .moveColumn').addClass('hidden');

    $('#' + componentType + ' .addMeasurementsContentLevelBtn').addClass('hidden');
    $('#' + componentType + ' .viewMeasurementsContentLevelBtn').removeClass('hidden');

    $('#' + componentType + ' .addMeasurementsStepLevelBtn').addClass('hidden');
    $('#' + componentType + ' .viewMeasurementsStepLevelBtn').removeClass('hidden');

    $('#' + componentType + ' .addMeasurementsComponentLevelBtn').addClass('hidden');
    $('#' + componentType + ' .viewMeasurementsComponentLevelBtn').removeClass('hidden');



    //DISABLE MOVE SORTING
    if (viewMode == false) {
        //console.log('disabling sort for ' + componentType);
        var materialsTable = $('#' + componentType + ' .materialsAndBatchesTable tbody ');
        var processesTable = $('#' + componentType + ' .processesTable tbody ');
        materialsTable.each(function (i, e) {
            if (e.children.length > 1)
                sortable(e, 'enable');
        });
        processesTable.each(function (i, e) {
            if (e.children.length > 1)
                sortable(e, 'enable');
        });
    }

}
function UnlockComponentEditing(componentType) {
    $('#' + componentType + ' .startComponentOverButton').removeClass('hidden');
    //$('#' + componentType).addClass('collapsed-box');
    if ($('#' + componentType).hasClass('collapsed-box')) {
        $('#' + componentType + " .btn-box-tool.component-box-tool").trigger('click');
    }

    //var savedHtml = "<span class = \"saved\"><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";
    $('#' + componentType).removeClass('box-success');
    $('#' + componentType).addClass('box-warning');
    $('#' + componentType + ' .stepContentButtons input').removeAttr('disabled');
    $('#' + componentType + ' .editComponentButton').addClass('hidden');
    $('#' + componentType + ' .removeStepBtn').removeClass('hidden');
    $('#' + componentType + ' .removeRow').removeClass('hidden');
    $('#' + componentType + ' input[name="quantity"]').removeAttr('disabled');
    $('#' + componentType + ' .experimentProcessRow .editSelectedProcessButton').removeClass('hidden');
    $('#' + componentType + ' .experimentProcessRow .viewSelectedProcessButton').addClass('hidden');
    $('#' + componentType + ' .experimentProcessRow .addEquipmentSettingsButton').removeClass('hidden');
    $('#' + componentType + ' .experimentProcessRow .viewEquipmentSettingsButton').addClass('hidden');

    var commercial = $('#' + componentType + ' .box-body').find("tr.componentCommercialTypeRow").length;
    if (commercial != 0 || componentType == "ReferenceElectrode") {
        $('#' + componentType + ' .componentFooterButtons input.submitComponentCommercialBtn').removeClass('hidden');
    }
    else {
        $('#' + componentType + ' .componentFooterButtons input').removeClass('hidden');
        $('#' + componentType + ' .componentFooterButtons input.submitComponentCommercialBtn').addClass('hidden');
    }

    //$('#' + componentType + ' .componentFooterButtons .saved').remove();
    $('#' + componentType + ' .componentFooterButtons .componentSaved').addClass('hidden');

    //remove savedAsBatch class
    $('#' + componentType + " .row.step").removeClass('savedAsBatch');

    //$("#" + componentType + " .documentsButton").attr('disabled', 'true').attr('title', 'Adding documents is available after save!');
    $("#" + componentType + " .stepButtons .documentsButton").attr('disabled', 'true').attr('title', 'Adding step documents is available after save!');

    $('#' + componentType + ' .js-handle').removeClass('hidden');
    $('#' + componentType + ' .moveColumn').removeClass('hidden');

    $('#' + componentType + ' .viewMeasurementsContentLevelBtn').addClass('hidden');
    $('#' + componentType + ' .addMeasurementsContentLevelBtn').removeClass('hidden');

    $('#' + componentType + ' .viewMeasurementsStepLevelBtn').addClass('hidden');
    $('#' + componentType + ' .addMeasurementsStepLevelBtn').removeClass('hidden');

    $('#' + componentType + ' .viewMeasurementsComponentLevelBtn').addClass('hidden');
    $('#' + componentType + ' .addMeasurementsComponentLevelBtn').removeClass('hidden');

    //ENABLE MOVE SORTING
    if (viewMode == false) {
        //console.log('enabling sort for ' + componentType);
        var materialsTable = $('#' + componentType + ' .materialsAndBatchesTable tbody ');
        var processesTable = $('#' + componentType + ' .processesTable tbody ');
        materialsTable.each(function (i, e) {
            if (e.children.length > 1)
                sortable(e, 'enable');
        });
        processesTable.each(function (i, e) {
            if (e.children.length > 1)
                sortable(e, 'enable');
        });
    }

    var batteryComponentStepsNumber = $('#' + componentType + ' .step').length;
    if (commercial == 0 && batteryComponentStepsNumber == 0)
        StartComponentOver(componentType);
}
function SetComponentsToViewMode() {
    $('div' + ".batteryComponent").each(function () {
        if ($(this).hasClass('collapsed-box')) {
            $(this).find(".btn-box-tool").trigger('click');
        }
    });

    $("div.batteryComponent" + ' .step .saveAsBatchBtn').remove();
    $("div.batteryComponent" + ' .step .stepContentButtons').remove();

    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable .table-actions-col').remove();

    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody tr').filter("th:last").remove();
    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody tr th').last().remove();
    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody').remove();



    //trgni site kopcinja
    //action th
    //process view attributes
    //input disable


}

function FinishExperimentCreation(experimentId, editing) {
    $("#finishExperimentBtn").attr('disabled', 'true');
    var componentTypes = [];
    var componentCommercialTypes = [];

    var componentNumberOfSteps = 0;
    var componentCommercialTypeSelected = 0;

    //obligatory
    componentNumberOfSteps = $('#Anode ' + ' .box-body').find(".step").length;
    componentCommercialTypeSelected = $('#Anode ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("Anode");
    }
    else {
        componentTypes.push("Anode");
    }
    //obligatory
    componentNumberOfSteps = $('#Cathode ' + ' .box-body').find(".step").length;
    componentCommercialTypeSelected = $('#Cathode ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("Cathode");
    }
    //if (componentNumberOfSteps != 0) {
    else {
        componentTypes.push("Cathode");
    }
    //non-obligatory
    componentNumberOfSteps = $('#Separator ' + ' .box-body').find(".step").length;
    componentCommercialTypeSelected = $('#Separator ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("Separator");
    }
    else {
        componentTypes.push("Separator");
    }
    //obligatory
    componentNumberOfSteps = $('#Electrolyte ' + ' .box-body').find(".step").length;
    componentCommercialTypeSelected = $('#Electrolyte ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("Electrolyte");
    }
    else {
        componentTypes.push("Electrolyte");
    }
    //only-prefab type
    componentCommercialTypeSelected = $('#ReferenceElectrode ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("ReferenceElectrode");
    }
    else {
        componentTypes.push("ReferenceElectrode");
    }
    //obligatory
    componentNumberOfSteps = $('#Casing ' + ' .box-body').find(".step").length;
    componentCommercialTypeSelected = $('#Casing ' + ' .box-body').find("table.commercialComponentTable").length;
    if (componentCommercialTypeSelected != 0) {
        componentCommercialTypes.push("Casing");
    }
    else {
        componentTypes.push("Casing");
    }
    //console.log("Components Submitted: " + componentTypes);
    //console.log("Commercial Components Submitted: " + componentCommercialTypes);

    var results = [];

    $.each(componentTypes, function (index, componentType) {
        //UnlockComponentEditing(componentType);
        results.push(SubmitComponent(componentType, experimentId));
    });
    $.each(componentCommercialTypes, function (index, componentType) {
        //UnlockComponentEditing(componentType);
        results.push(SubmitComponentCommercalType(componentType));
    });

    $.when.apply(this, results).done(function () {
        //when all components inserting is done

        if (!ajaxResultsValidForSubmit) {
            ajaxResultsValidForSubmit = true;
            $("#finishExperimentBtn").removeAttr('disabled');
            return;
        }

        //create json of all manually entered components that need to go to stock
        var jsonRequestObject = {};

        var jsonObjectGeneralData = CreateExperimentGeneralDataJsonRequestObject();
        var jsonObjectComponents = CreateComponentsJsonRequestObject(componentTypes);

        jsonRequestObject["experimentInfo"] = jsonObjectGeneralData.experimentInfo;
        jsonRequestObject["batteryComponents"] = jsonObjectComponents.batteryComponents;


        //console.log(JSON.stringify(jsonRequestObject));

        //SubmitAllComponentsToStock(componentTypes);
        //return;
        reqUrl = "/Helpers/WebMethods.asmx/FinishExperimentCreation";
        var RequestDataString = JSON.stringify({
            formData: JSON.stringify(jsonRequestObject),
            experimentId: experimentId,
            editing: editing
        });

        var ajaxData = $.ajax({
            type: "POST",
            url: reqUrl,
            data: RequestDataString,
            //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var jsonResult = JSON.parse(result.d);
                //console.log(jsonResult);
                //ENABLE THE FINISH BUTTON BACK
                if (jsonResult.status != "ok")
                    $("#finishExperimentBtn").removeAttr('disabled');

                if (jsonResult.status == "ok") {
                    //notify("Experiment saved", "success");
                    //window.location.replace("/Experiments/");
                    window.location.replace("/Results/Experiments/Insert/" + experimentId);
                }
                else if (jsonResult.status == "incompleteComponents") {
                    //console.log(jsonResult.response);
                    $.each(jsonResult.response, function (id, value) {
                        MarkComponentInvalid(value);
                    });
                    notify(jsonResult.message, "danger");
                }
                else if (jsonResult.status == "invalidStock") {
                    //console.log(jsonResult.response);
                    $.each(jsonResult.response, function (id, value) {
                        MarkComponentInvalid(value);
                    });
                    notify(jsonResult.message, "warning");
                }
                else {
                    notify(jsonResult.message, "warning");
                    //notify("Error saving experiment", "warning");
                }
            },
            error: function (p1, p2, p3) {
                alert(p1.status.toString() + " " + p3.toString());
                //ENABLE THE FINISH BUTTON BACK
                $("#finishExperimentBtn").removeAttr('disabled');
            }
        }).done(function (data) {
            //$("#finishExperimentBtn").removeAttr('disabled');
        });
    });

}
function MarkComponentInvalid(componentType) {
    $("#" + componentType).removeClass("box-success");
    $("#" + componentType).removeClass("box-warning");
    $("#" + componentType).removeClass("box-default");

    $("#" + componentType).addClass("box-danger");
}

function SaveStepAsBatch(componentType, stepId, table, button) {
    //alert('clicked');

    //console.log(JSON.stringify(button));
    //$(button).data("item").batchPersonalLabel = $('#batchPersonalLabel').val();   

    var data = $(button).data("item");
    //console.log("element data: " + data);
    //console.log("element data: " + JSON.stringify($(button).data("item")));

    var jsonRequestObject = {};

    var batchInfo = {};

    var html = '<div class="row"><div class="col-md-12">';

    html += '<fieldset class="form-horizontal">';

    html += '<div class="form-group">';
    html += '<label for="batchPersonalLabel" class="col-sm-2 control-label">Personal Label</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="batchPersonalLabel" type="text" id="batchPersonalLabel" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="batchDescription" class="col-sm-2 control-label">Description</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="batchDescription" type="text" id="batchDescription" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="batchChemicalFormula" class="col-sm-2 control-label">Chemical Formula</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="batchChemicalFormula" type="text" id="batchChemicalFormula" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="selectMeasurementUnit" class="col-sm-2 control-label">Measurement Unit</label>';
    html += '<div class="col-sm-10">';
    html += '<select id="selectMeasurementUnit" class="measurement-unit-data-ajax">    </select>';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="totalBatchOutput" class="col-sm-2 control-label">Total Batch Output</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="totalBatchOutput" type="number" id="totalBatchOutput" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="batchOutput" class="col-sm-2 control-label">Usable Batch Output</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="batchOutput" type="number" id="batchOutput" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="wasteAmount" class="col-sm-2 control-label">Waste Amount</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="wasteAmount" type="number" id="wasteAmount" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="releasedAs" class="col-sm-2 control-label">Released as liquid, solid or gas</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="releasedAs" type="text" id="releasedAs" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="wasteChemicalComposition" class="col-sm-2 control-label">Waste chemical composition</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="wasteChemicalComposition" type="text" id="wasteChemicalComposition" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="wasteComment" class="col-sm-2 control-label">Waste Comment</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="wasteComment" type="text" id="wasteComment" class="form-control">';
    html += '</div>';
    html += '</div>';
    html += '<div class="form-group">';
    html += '<label for="selectMaterialType" class="col-sm-2 control-label">Material Type</label>';
    html += '<div class="col-sm-10">';
    html += '<select id="selectMaterialType" class="material-type-data-ajax">    </select>';
    html += '</div>';
    html += '</div>';

    html += '</fieldset>';

    html += '</div></div>';

    var batchPersonalLabel;
    var batchDescription;
    var batchChemicalFormula;
    var batchOutput;
    var measurementUnit;
    var materialType;

    var dialog = bootbox.dialog({
        title: 'Add batch general info',
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
                    //var selectedData = $('#selectMaterial').select2('data')[0].data;

                    //var row = GenerateMaterialHtml(selectedData);
                    //AppendRow($('#materialsAndBatchesTable'), row);                    

                    //var batchLabel = $('#<%= TxtLabel.ClientID %>').val();

                    batchPersonalLabel = $('#batchPersonalLabel').val();
                    batchDescription = $('#batchDescription').val();
                    batchChemicalFormula = $('#batchChemicalFormula').val();
                    batchOutput = $('#batchOutput').val();
                    measurementUnit = $('#selectMeasurementUnit').val();
                    materialType = $('#selectMaterialType').val();
                    totalBatchOutput = $('#totalBatchOutput').val();
                    wasteAmount = $('#wasteAmount').val();
                    releasedAs = $('#releasedAs').val();
                    wasteChemicalComposition = $('#wasteChemicalComposition').val();
                    wasteComment = $('#wasteComment').val();

                    batchInfo["batchPersonalLabel"] = batchPersonalLabel;
                    batchInfo["description"] = batchDescription;
                    batchInfo["chemicalFormula"] = batchChemicalFormula;
                    batchInfo["batchOutput"] = batchOutput;
                    batchInfo["fkMeasurementUnit"] = measurementUnit;
                    batchInfo["fkMaterialType"] = materialType;                  
                    batchInfo["totalBatchOutput"] = totalBatchOutput;
                    batchInfo["wasteAmount"] = wasteAmount;
                    batchInfo["releasedAs"] = releasedAs;
                    batchInfo["wasteChemicalComposition"] = wasteChemicalComposition;
                    batchInfo["wasteComment"] = wasteComment;

                    //FOR KEEPING SELECTED DATA IN THE DIALOG
                    batchInfo.measurementUnitText = $('#selectMeasurementUnit').text().trim();
                    batchInfo.materialTypeText = $('#selectMaterialType').text().trim();


                    var batchContentList = [];
                    var batchProcessList = [];
                    var step = 1;

                    $("#materialsAndBatchesTable" + componentType + "Step" + stepId + " tbody tr.experimentContentRow").each(function (id) {
                        //var quantity = parseFloat($(this).find("input[name='quantity']").val());
                        var quantity = $(this).find("input[name='quantity']").val().replace(',', '.');

                        var row = $(this).data("item");
                        var itemType = row.type;
                        var item = row.item;

                        var measurements = $(this).find(".addMeasurementsContentLevelBtn").data("item");
                        var measurementsItem = measurements.item;

                        var batchContent = {};
                        batchContent["step"] = step;
                        if (itemType == "material") {
                            var attr = 'materialId';
                            if (attr in item) {
                                batchContent["fkStepMaterial"] = item.materialId;
                            }
                            else batchContent["fkStepMaterial"] = item.fkStepMaterial;
                            batchContent["materialName"] = item.materialName;
                        }
                        else if (itemType == "batch") {
                            var attr = 'batchId';
                            if (attr in item) {
                                batchContent["fkStepBatch"] = item.batchId;
                            }
                            else batchContent["fkStepBatch"] = item.fkStepBatch;

                            batchContent["batchSystemLabel"] = item.batchSystemLabel;
                            batchContent["batchPersonalLabel"] = item.batchPersonalLabel;
                        }
                        batchContent["weight"] = quantity;
                        batchContent["fkFunction"] = item.fkFunction;
                        var percentageOfActive = item.percentageOfActive;
                        if (percentageOfActive != null)
                            percentageOfActive = percentageOfActive.toString().replace(',', '.');
                        batchContent["percentageOfActive"] = percentageOfActive;
                        //batchContent["fkStoredInType"] = 1;
                        batchContent["orderInStep"] = id + 1;

                        batchContent["measurements"] = measurementsItem;

                        batchContentList.push(batchContent);
                    });
                    //console.log(batchContentList);
                    //console.log(JSON.stringify(batchContentList));

                    $("#processesTable" + componentType + "Step" + stepId + " tbody tr.experimentProcessRow").each(function (id) {
                        var row = $(this).data("item");
                        var itemType = row.type;
                        var item = row.item;

                        var processRequest = {};
                        var batchProcess = {};
                        batchProcess["step"] = step;
                        batchProcess["fkProcessType"] = row.data.fkProcessType;
                        batchProcess["equipmentModelId"] = row.data.equipmentModelId;
                        batchProcess["fkEquipment"] = row.data.fkEquipment;
                        batchProcess["processOrderInStep"] = id + 1;

                        processRequest["batchProcess"] = batchProcess;
                        processRequest["processAttributes"] = item;

                        var equipmentSettings = $(this).find(".editSelectedProcessButton").data("item");
                        var equipmentSettingsItem = equipmentSettings.item;
                        processRequest["equipmentSettings"] = equipmentSettingsItem;

                        batchProcessList.push(processRequest);
                    });
                    //console.log(batchProcessList);
                    //console.log(JSON.stringify(batchProcessList));
                    jsonRequestObject["batchInfo"] = batchInfo;
                    jsonRequestObject["batchContent"] = batchContentList;
                    jsonRequestObject["batchProcesses"] = batchProcessList;
                    jsonRequestObject["fkExperiment"] = experimentId;

                    var step = $("#" + componentType + "Step" + stepId)
                    var measurements = step.find(".addMeasurementsStepLevelBtn").data("item");
                    var measurementsItem = measurements.item;
                    //console.log("batch level measurements: " + JSON.stringify(measurementsItem));
                    jsonRequestObject["measurements"] = measurementsItem;

                    //console.log(jsonRequestObject);
                    //jsonString = JSON.stringify(jsonRequestObject);

                    //console.log(JSON.stringify(jsonRequestObject));
                    //return;

                    reqUrl = "/Helpers/WebMethods.asmx/SubmitBatchWithContent";
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
                                notify("Batch successfully created", "success");

                                //console.log(JSON.stringify(jsonResult));

                                var batchData = jsonResult.response;
                                //console.log(batchData);
                                var row = GenerateBatchHtml(batchData);
                                ClearStepContent(componentType, stepId);
                                AppendRow($('#' + table), row);

                                //$('#' + componentType + "Step" + stepId).addClass('savedAsBatch');

                                //$("#" + componentType + "Step" + stepId + " .stepFooterButtons input").replaceWith("Saved as batch");
                                $('#' + componentType + "Step" + stepId + ' .box').addClass('box-success');
                            }
                            else {
                                notify(jsonResult.message, "warning");
                                //$(button).data("item").item = '{ "batchPersonalLabel":' + batchPersonalLabel + ', "batchDescription":' + batchDescription + ', "batchChemicalFormula":' + batchChemicalFormula + ', "batchOutput":' + batchOutput + ', "measurementUnit":' + measurementUnit + ', "materialType":' + materialType + ' }';
                                $(button).data("item").item = batchInfo;
                                return false;
                            }
                            //if (result.d == 0) {
                            //    //ClearModalData();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
                            //    //GetActorData();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
                            //    //$('#newModal').modal('hide');                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
                            //}
                            //else if (result.d == 2) {
                            //    //$('#newModal').modal('hide');                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
                            //    //bootbox.alert("<%= DbRes.T("There is data for the selected date! Edit the existing data with the new values!", "Actors") %>", function(){                                                                                                                                                                                                                                                                                                                                                                                                                                     
                            //    //    setTimeout(function () { $('#newModal').modal('show'); }, 500);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                            //    //});                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
                            //} else {
                            //    //alert(result.d);
                            //}
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
        var batchData = data.item;
        if (batchData != "") {
            $('#batchPersonalLabel').val(batchData.batchPersonalLabel);
            $('#batchDescription').val(batchData.description);
            $('#batchChemicalFormula').val(batchData.chemicalFormula);
            $('#batchOutput').val(batchData.batchOutput);
            $('#totalBatchOutput').val(batchData.totalBatchOutput);
            $('#wasteAmount').val(batchData.wasteAmount);
            $('#releasedAs').val(batchData.releasedAs);
            $('#wasteChemicalComposition').val(batchData.wasteChemicalComposition);
            $('#wasteComment').val(batchData.wasteComment);

            if (batchData.fkMeasurementUnit) {
                var text = batchData.measurementUnitText;
                var id = batchData.fkMeasurementUnit;
                var option = new Option(text, id, true, true);
                $('#selectMeasurementUnit').append(option).trigger('change');
            }
            if (batchData.fkMaterialType) {
                var text = batchData.materialTypeText;
                var id = batchData.fkMaterialType;
                var option = new Option(text, id, true, true);
                $('#selectMaterialType').append(option).trigger('change');
            }
        }

        $("#batchPersonalLabel").focus();
    });

    var reqUrlMaterialTypes = "/Helpers/WebMethods.asmx/GetMaterialTypes";
    $('#selectMaterialType').select2({
        ajax: {
            type: "POST",
            url: reqUrlMaterialTypes,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }
                return JSON.stringify({ search: term, type: 'public' });
            },
            processResults: function (data, params) {
                //console.log(JSON.parse(data.d).results);
                return {
                    //results: JSON.parse(data.d).results
                    results: $.map(JSON.parse(data.d), function (item) {
                        return {
                            id: item.materialTypeId,
                            text: item.materialType
                        };
                    })
                };
            },
            //cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Search for material type',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        templateResult: formatRepo,
        //templateSelection: formatRepoSelection
    });
    var reqUrlMeasurementUnits = "/Helpers/WebMethods.asmx/GetMeasurementUnits";
    $('#selectMeasurementUnit').select2({
        ajax: {
            type: "POST",
            url: reqUrlMeasurementUnits,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }
                return JSON.stringify({ search: term, type: 'public' });
            },
            processResults: function (data, params) {
                return {
                    results: $.map(JSON.parse(data.d), function (item) {
                        return {
                            id: item.measurementUnitId,
                            text: item.measurementUnitSymbol + " | " + item.measurementUnitName,
                            measurementUnitSymbol: item.measurementUnitSymbol
                        };
                    })
                };
            },
            //cache: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Select measurement unit',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatMeasurementUnit,
        //templateSelection: formatMeasurementUnit
    });


    //$('#selectMeasurementUnit').val("2").trigger("change");
    //$('#selectMaterialType').val("2").trigger("change");

}
function SaveStepAsSequence(componentType, stepId, table, button) {


    var data = $(button).data("item");

    var jsonRequestObject = {};

    var sequenceInfo = {};

    var html = '<div class="row"><div class="col-md-12">';

    html += '<fieldset class="form-horizontal">';

    html += '<div class="form-group">';
    html += '<label for="sequencePersonalLabel" class="col-sm-2 control-label">Personal Label</label>';
    html += '<div class="col-sm-10">';
    html += '<input name="sequencePersonalLabel" type="text" id="sequencePersonalLabel" class="form-control">';
    html += '</div>';
    html += '</div>';

    html += '</fieldset>';

    html += '</div></div>';

    var sequencePersonalLabel;

    var dialog = bootbox.dialog({
        title: 'Add sequence general info',
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

                    sequencePersonalLabel = $('#sequencePersonalLabel').val();

                    sequenceInfo["label"] = sequencePersonalLabel;

                    /*//FOR KEEPING SELECTED DATA IN THE DIALOG
                    batchInfo.measurementUnitText = $('#selectMeasurementUnit').text().trim();
                    batchInfo.materialTypeText = $('#selectMaterialType').text().trim();*/

                    var sequenceProcessList = [];
                    var step = 1;

                    $("#processesTable" + componentType + "Step" + stepId + " tbody tr.experimentProcessRow").each(function (id) {
                        var row = $(this).data("item");
                        var processLabel = row.type;
                        var item = row.item;

                        var processRequest = {};
                        //var sequenceProcess = {};
                        processRequest["step"] = step;
                        processRequest["processTypeId"] = row.data.fkProcessType;
                        processRequest["equipmentModelId"] = row.data.equipmentModelId;
                        processRequest["equipmentId"] = row.data.fkEquipment;
                        processRequest["processLabel"] = row.label;
                        processRequest["processOrderInStep"] = id + 1;

                        processRequest["processAttributes"] = item;

                        var equipmentSettings = $(this).find(".editSelectedProcessButton").data("item");
                        var equipmentSettingsItem = equipmentSettings.item;
                        //processRequest["equipmentSettings"] = equipmentSettingsItem;

                        sequenceProcessList.push(processRequest);
                    });

                    jsonRequestObject["sequenceInfo"] = sequenceInfo;
                    jsonRequestObject["sequenceProcesses"] = sequenceProcessList;

                    /*  var step = $("#" + componentType + "Step" + stepId)
                      var measurements = step.find(".addMeasurementsStepLevelBtn").data("item");
                      var measurementsItem = measurements.item;
                      //console.log("batch level measurements: " + JSON.stringify(measurementsItem));
                      jsonRequestObject["measurements"] = measurementsItem;*/

                    //console.log(jsonRequestObject);
                    //jsonString = JSON.stringify(jsonRequestObject);

                    //console.log(JSON.stringify(jsonRequestObject));
                    //return;

                    reqUrl = "/Helpers/WebMethods.asmx/SubmitProcessSequence";
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
                                notify("Sequence successfully created", "success");

                                //console.log(JSON.stringify(jsonResult));

                                var batchData = jsonResult.response;
                                //console.log(batchData);
                                // var row = GenerateBatchHtml(batchData);
                                //ClearStepContent(componentType, stepId);
                                //AppendRow($('#' + table), row);
                                // $('#' + componentType + "Step" + stepId + ' .box').addClass('box-success');
                            }
                            else {
                                notify(jsonResult.message, "warning");
                                //$(button).data("item").item = '{ "batchPersonalLabel":' + batchPersonalLabel + ', "batchDescription":' + batchDescription + ', "batchChemicalFormula":' + batchChemicalFormula + ', "batchOutput":' + batchOutput + ', "measurementUnit":' + measurementUnit + ', "materialType":' + materialType + ' }';
                                $(button).data("item").item = sequenceInfo;
                                return false;
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
        var sequenceData = data.item;
        if (sequenceData != "") {
            $('#sequencePersonalLabel').val(sequenceData.label);
            /*if (sequenceData.fkMeasurementUnit) {
                var text = sequenceData.measurementUnitText;
                var id = sequenceData.fkMeasurementUnit;
                var option = new Option(text, id, true, true);
                $('#selectMeasurementUnit').append(option).trigger('change');
            }*/
            /*if (sequenceData.fkMaterialType) {
                var text = batchData.materialTypeText;
                var id = batchData.fkMaterialType;
                var option = new Option(text, id, true, true);
                $('#selectMaterialType').append(option).trigger('change');
            }*/
        }

        $("#sequencePersonalLabel").focus();
    });

}


function formatRepo(repo) {
    var markup = repo.text;
    return markup;
}



function ShowExperiment(experiment, viewMode) {

    if (viewMode == true) {
        var experimentGeneralInfoHtml = GenerateExperimentInfoViewHtml(experiment.experimentInfo);
        $('#experimentGeneralInfo tbody').append(experimentGeneralInfoHtml);
    }
    else {
        //Fill general info fields
        $('#projectName').val(experiment.experimentInfo.projectAcronym);
        $('#experimentSystemLabel').val(experiment.experimentInfo.experimentSystemLabel);
        var dateCreated = ToLocalDate(experiment.experimentInfo.dateCreated);
        $('#experimentDateCreated').val(dateCreated);
        $('#experimentCreatedBy').val(experiment.experimentInfo.operatorUsername);
        $('#experimentPersonalLabel').val(experiment.experimentInfo.experimentPersonalLabel);
        $('#experimentDescription').val(experiment.experimentInfo.experimentDescription);
    }

    var anodeHtml = "";
    var cathodeHtml = "";
    var separatorHtml = "";
    var electrolyteHtml = "";
    var referenceElectrodeHtml = "";
    var casingHtml = "";

    //$.each(experiment.batteryComponents, function (id, componentSteps) {
    //    //console.log(key + ": " + value);
    //    //if (value.fkStepMaterial != null) {
    //        html += GenerateStepMaterialViewHtml(value);
    //    //}
    //    //else {
    //    //    html += GenerateStepBatchViewHtml(value);
    //    //}
    //});


    //var experimentContentHtml = GenerateBatchContentViewHtml(batch.batchContentList);
    //$('#materialsAndBatchesTable tbody').append(batchContentHtml);
    //var experimentProcessHtml = GenerateBatchProcessViewHtml(batch.batchProcessList);
    //$('#processesTable tbody').append(batchProcessHtml);

    RegenerateCompletedContents(experiment, viewMode);
}


function ShowExperimentGeneralInfo(experiment) {
    //var experimentGeneralInfoHtml = GenerateExperimentInfoViewHtml(experiment.experimentInfo);
    var experimentGeneralInfoHtml = GenerateExperimentInfoViewHtml(experiment);
    $('#experimentGeneralInfo tbody').append(experimentGeneralInfoHtml);
}
function ShowExperimentCalculations(calculations) {
    var experimentCalculationsHtml = GenerateExperimentCalculationsHtml(calculations);
    $('#experimentGeneralInfo tbody').append(experimentCalculationsHtml);
}

function ShowExperimentSummary(allContent, allProcesses, experiment) {
    //console.log(allContent);
    var experimentMaterialsHtml = GenerateExperimentMaterialsHtml(allContent);
    $('#experimentSummaryMaterials tbody').append(experimentMaterialsHtml);

    var experimentProcessesHtml = GenerateExperimentProcessesHtml(allProcesses);
    $('#experimentSummaryProcesses tbody').append(experimentProcessesHtml);
}

function GenerateExperimentInfoViewHtml(experiment) {
    var html = "";

    html += '<tr>' +
        '<td>Project</td>' +
        '<td>' + experiment.projectAcronym + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>System Label</td>' +
        '<td>' + experiment.experimentSystemLabel + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Personal Label</td>' +
        '<td>' + experiment.experimentPersonalLabel + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Description</td>' +
        '<td>' + experiment.experimentDescription + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Created by</td>' +
        '<td>' + experiment.operatorUsername + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Date created</td>' +
        '<td>' + ToLocalDate(experiment.dateCreated) + '</td>' +
        '</tr>';

    return html;
}
function GenerateExperimentCalculationsHtml(calculations) {
    var html = "";

    if (calculations.anode.componentEmpty == false) {
        if ('commercialType' in calculations.anode) {
            html += '<tr>' +
                '<td>Anode: A.M. </td>' +
                '<td> / </td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Anode: A.M. </td>' +
                '<td>' + calculations.anode.totalActiveMaterials.toPrecision(4) + " g" + " : " + calculations.anode.activeMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.cathode.componentEmpty == false) {
        if ('commercialType' in calculations.cathode) {
            html += '<tr>' +
                '<td>Cathode: A.M </td>' +
                '<td> / </td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Cathode: A.M </td>' +
                '<td>' + calculations.cathode.totalActiveMaterials.toPrecision(4) + " g" + " : " + calculations.cathode.activeMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }

    if (calculations.anode.componentEmpty == false) {
        if ('commercialType' in calculations.anode) {
            html += '<tr>' +
                '<td>Anode: </td>' +
                '<td>' + calculations.anode.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Anode: </td>' +
                '<td>' + 'Total weight: ' + calculations.anode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.anode.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.cathode.componentEmpty == false) {
        if ('commercialType' in calculations.cathode) {
            html += '<tr>' +
                '<td>Cathode: </td>' +
                '<td>' + calculations.cathode.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Cathode: </td>' +
                '<td>' + 'Total weight: ' + calculations.cathode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.cathode.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.separator.componentEmpty == false) {
        if ('commercialType' in calculations.separator) {
            html += '<tr>' +
                '<td>Separator: </td>' +
                '<td>' + calculations.separator.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Separator: </td>' +
                '<td>' + 'Total weight: ' + calculations.separator.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.separator.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.electrolyte.componentEmpty == false) {
        if ('commercialType' in calculations.electrolyte) {
            html += '<tr>' +
                '<td>Electrolyte: </td>' +
                '<td>' + calculations.electrolyte.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Electrolyte: </td>' +
                '<td>' + 'Total weight: ' + calculations.electrolyte.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.electrolyte.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.referenceElectrode.componentEmpty == false) {
        if ('commercialType' in calculations.referenceElectrode) {
            html += '<tr>' +
                '<td>Reference Electrode: </td>' +
                '<td>' + calculations.referenceElectrode.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Reference Electrode: </td>' +
                '<td>' + 'Total weight: ' + calculations.referenceElectrode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.referenceElectrode.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }
    if (calculations.casing.componentEmpty == false) {
        if ('commercialType' in calculations.casing) {
            html += '<tr>' +
                '<td>Casing: </td>' +
                '<td>' + calculations.casing.commercialType.commercialType + '</td>' +
                '</tr>';
        }
        else {
            html += '<tr>' +
                '<td>Casing: </td>' +
                '<td>' + 'Total weight: ' + calculations.casing.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.casing.labeledMaterialsPercentages + " %" + '</td>' +
                '</tr>';
        }
    }


    return html;
}

function GenerateExperimentMaterialsHtml(allContent) {
    var html = "";

    $(allContent).each(function (index, value) {
        //console.log(value)
        var item = value.content;
        var batteryComponentType = "";
        var name = "";
        var functionInExperiment = "";
        var percentageOfActive = "";

        if (item.fkCommercialType == null) {
            if (item.batteryComponentId != null) {

            }

            functionInExperiment = item.materialFunction;

            if (item.fkFunction != 1) {
                percentageOfActive = item.percentageOfActive != null ? item.percentageOfActive : "\\";
            }
            else {
                percentageOfActive = item.percentageOfActive != null ? item.percentageOfActive : "";
            }

            batteryComponentType = value.batteryComponentType;
            if ('batteryComponentId' in item) {

                //batteryComponentType = item.batteryComponentType;
            }

            //only if it is a material and not a batch
            if (item.fkStepMaterial != null) {
                //console.log('mat ' + JSON.stringify(material));
                //name = '<a href="">' + item.materialName + '</a>';
                name = '<a href=\"/Materials/View/' + item.fkStepMaterial + '\" target="_blank">' + item.materialName + '</a>'
            }
            else {
                //name = item.batchSystemLabel;
                name = '<a href=\"/Batches/View/' + item.fkStepBatch + '\" target="_blank">' + item.batchSystemLabel + '</a>'
            }

            var measurements = item.measurements;

            var measuredTime = "";
            var measuredWidth = "";
            var measuredLength = "";
            var measuredConductivity = "";
            var measuredThickness = "";
            var measuredWeight = "";

            if (measurements != null) {
                measuredTime = measurements.measuredTime != null ? measurements.measuredTime : "";
                measuredWidth = measurements.measuredWidth != null ? measurements.measuredWidth : "";
                measuredLength = measurements.measuredLength != null ? measurements.measuredLength : "";
                measuredConductivity = measurements.measuredConductivity != null ? measurements.measuredConductivity : "";
                measuredThickness = measurements.measuredThickness != null ? measurements.measuredThickness : "";
                measuredWeight = measurements.measuredWeight != null ? measurements.measuredWeight : "";

                //measuredTime = measurements.measuredTime;
                //measuredWidth = measurements.measuredWidth;
                //measuredLength = measurements.measuredLength;
                //measuredConductivity = measurements.measuredConductivity;
                //measuredThickness = measurements.measuredThickness;
                //measuredWeight = measurements.measuredWeight;
            }
            html += '<tr>' +
                '<td>' + batteryComponentType + '</td>' +
                '<td>' + name + '</td>' +
                '<td>' + item.chemicalFormula + '</td>' +
                '<td>' + item.weight.toFixed(5) + '</td>' +
                '<td>' + item.measurementUnitSymbol + '</td>' +
                '<td>' + functionInExperiment + '</td>' +
                '<td>' + percentageOfActive + '</td>' +
                '<td>' + measuredTime + '</td>' +
                '<td>' + measuredWidth + '</td>' +
                '<td>' + measuredLength + '</td>' +
                '<td>' + measuredConductivity + '</td>' +
                '<td>' + measuredThickness + '</td>' +
                '<td>' + measuredWeight + '</td>' +
                '</tr>';
        }




    });

    //console.log('val ' + JSON.stringify(value.batteryComponentSteps));


    return html;

}

function GenerateExperimentProcessesHtml(allProcesses) {
    //console.log(batteryComponents);

    var html = "";

    $(allProcesses).each(function (index, value) {

        var item = value;
        var process = "";

        var batteryComponentType = "";

        if ('stepProcess' in item) {
            process = item.stepProcess;
            batteryComponentType = process.batteryComponentType;
        }
        else if ('batchProcess' in item) {
            process = item.batchProcess;
            batteryComponentType = item.batteryComponentType;
        }
        var processAttributes = item.processAttributes;
        var equipmentSettings = item.equipmentSettings;



        var equipmentName = "";
        var equipmentModelName = "";
        var modelBrand = "";

        if (!$.isEmptyObject(process))
            equipmentName = process.equipmentName != null ? process.equipmentName : "";
        if (!$.isEmptyObject(process))
            equipmentModelName = process.equipmentModelName != null ? process.equipmentModelName : "";
        if (!$.isEmptyObject(process))
            modelBrand = process.modelBrand != null ? process.modelBrand : "";

        //if ('stepProcess' in item) {
        //    batteryComponentType = process.batteryComponentType;
        //}


        html += '<tr>' +
            '<td>' + batteryComponentType + '</td>' +
            '<td>' + process.processType + '</td>' +
            //'<td>' + process.subcategory + '</td>' +
            '<td>' + equipmentName + '</td>' +
            '<td>' + equipmentModelName + '</td>' +
            '<td>' + modelBrand + '</td>' +
            '</tr>';

        //console.log('val ' + JSON.stringify(value.batteryComponentSteps));

    });

    return html;
}


function GenerateExperimentMaterialsHtmlOld(experiment) {
    var batteryComponents = experiment.batteryComponents;
    //console.log(batteryComponents);

    var html = "";

    $(batteryComponents).each(function (index, value) {
        var batteryComponentType = value.componentType;

        $(value.batteryComponentSteps).each(function (index, value) {
            $(value.stepContent).each(function (index, value) {
                var material = value;
                //only if it is a material and not a batch
                if (material.fkStepMaterial != null) {
                    //console.log('mat ' + JSON.stringify(material));

                    var measurements = material.measurements;

                    var measuredTime = "";
                    var measuredWidth = "";
                    var measuredLength = "";
                    var measuredConductivity = "";
                    var measuredThickness = "";
                    var measuredWeight = "";

                    if (measurements != null) {
                        measuredTime = measurements.measuredTime != null ? measurements.measuredTime : "";
                        measuredWidth = measurements.measuredWidth != null ? measurements.measuredWidth : "";
                        measuredLength = measurements.measuredLength != null ? measurements.measuredLength : "";
                        measuredConductivity = measurements.measuredConductivity != null ? measurements.measuredConductivity : "";
                        measuredThickness = measurements.measuredThickness != null ? measurements.measuredThickness : "";
                        measuredWeight = measurements.measuredWeight != null ? measurements.measuredWeight : "";

                        //measuredTime = measurements.measuredTime;
                        //measuredWidth = measurements.measuredWidth;
                        //measuredLength = measurements.measuredLength;
                        //measuredConductivity = measurements.measuredConductivity;
                        //measuredThickness = measurements.measuredThickness;
                        //measuredWeight = measurements.measuredWeight;
                    }
                    html += '<tr>' +
                        '<td>' + batteryComponentType + '</td>' +
                        '<td>' + material.materialName + '</td>' +
                        '<td>' + material.chemicalFormula + '</td>' +
                        '<td>' + material.weight + '</td>' +
                        '<td>' + material.measurementUnitSymbol + '</td>' +
                        '<td>' + measuredTime + '</td>' +
                        '<td>' + measuredWidth + '</td>' +
                        '<td>' + measuredLength + '</td>' +
                        '<td>' + measuredConductivity + '</td>' +
                        '<td>' + measuredThickness + '</td>' +
                        '<td>' + measuredWeight + '</td>' +
                        '</tr>';
                }

            });
        });

        //console.log('val ' + JSON.stringify(value.batteryComponentSteps));
    });

    return html;
}

function GenerateExperimentProcessesHtmlOld(experiment) {
    var batteryComponents = experiment.batteryComponents;
    //console.log(batteryComponents);

    var html = "";

    $(batteryComponents).each(function (index, value) {
        var batteryComponentType = value.componentType;
        $(value.batteryComponentSteps).each(function (index, value) {
            $(value.stepProcesses).each(function (index, value) {
                var item = value;

                //console.log('process ' + JSON.stringify(item.processAttributes.equipmentName));

                html += '<tr>' +
                    '<td>' + batteryComponentType + '</td>' +
                    '<td>' + item.stepProcess.processType + '</td>' +
                    '<td>' + item.processAttributes.equipmentName + '</td>' +
                    '</tr>';

            });
        });

        //console.log('val ' + JSON.stringify(value.batteryComponentSteps));

    });

    return html;
}


/*
*
*
EXPERIMENT FILE ATTACHMENTS
*
*/
function getBase64(file, returnHere, where) {
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = (function (rh, where) {
        return function (e) {
            //console.log(reader.result);
            if (where == "image")
                returnHere.image = reader.result;
            else if (where == "audio")
                returnHere.audio = reader.result;
        }
    })(returnHere, where);
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}

text_truncate = function (str, length, ending) {
    if (length == null) {
        length = 100;
    }
    if (ending == null) {
        ending = '...';
    }
    if (str.length > length) {
        return str.substring(0, length - ending.length) + ending;
    } else {
        return str;
    }
};

function DocTypesOnShow() {
    var reqUrlDocTypes = "/Helpers/WebMethods.asmx/GetDocumentTypes";
    $('#selectDocumentType').select2({
        ajax: {
            type: "POST",
            url: reqUrlDocTypes,
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            delay: 250,
            data: function (params) {
                var term = "";
                if (params.term) {
                    var term = params.term;
                }

                return JSON.stringify({ search: term, type: 'public' });
            },
            processResults: function (data, params) {
                return {
                    results: $.map(JSON.parse(data.d), function (item) {
                        return {
                            id: item.documentTypeId,
                            text: item.documentTypeName,
                            data: item
                        };
                    })
                };
            },
            cache: true,
            required: true
        },
        dropdownParent: dialog,
        width: "100%",
        //theme: "bootstrap",
        placeholder: 'Search for a document type',
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });
}
function ShowFileAttachmentsExperiment(elementName, experimentId, componentTypeId, stepId) {
    //stepId = stepId || null;

    //var fileTypeHtml = '<div class="row"><div class="col-md-12">';
    //fileTypeHtml += '<select id="selectFileType" class="batch-data-ajax">    </select>';
    //fileTypeHtml += '</div></div>';

    var fileTypeHtml = '<div class="form-group">';
    fileTypeHtml += '<label for="file-attachment-document-type">Document type</label>';
    fileTypeHtml += '<select class="form-control input-sm" id="selectDocumentType" class="document-type-data-ajax">    </select>';
    fileTypeHtml += '</div>';



    var html = '<div class="row"><div class="col-md-12">';

    html += '<div class="table-responsive">' +
        '<table class="table table-condensed">' +
        '<thead>' +
        '<th>Filename</th>' +
        '<th>Document type</th>' +
        '<th>Description</th>' +
        '<th>Type</th>' +
        '<th>Date Added</th>' +
        '<th>Action</th>' +
        '</thead>' +
        '<tbody>';

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetFileAttachmentsExperimentByStep",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{elementName:"' + elementName + '", experimentId:"' + experimentId + '", componentTypeId:"' + componentTypeId + '", stepId:"' + stepId + '"}',
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            if (jsonResult.status == "ok") {

                $(jsonResult.response).each(function () {
                    html += '<tr>';
                    html += '<td><a href="#" onclick="DownloadFileAttachment(' + this.fileAttachmentId + ')" title="' + this.filename + '">' + text_truncate(this.filename, 30, "...") + '</span></td>';
                    html += '<td>' + this.documentTypeName + '</td>';
                    html += '<td><span title="' + this.description + '">' + text_truncate(this.description, 40, "...") + '</td>';
                    html += '<td>' + this.extension + '</td>';
                    html += '<td>' + this.createdOn + '</td>';
                    if (sharedViewMode == false)
                        html += '<td><div title="Delete File" class="btn btn-xs btn-danger" onclick="DeleteFileAttachmentExperiment(' + this.fileAttachmentId + ', \'' + elementName + '\', ' + experimentId + '\, ' + componentTypeId + '\, ' + stepId + ')">Delete</div>' + '</td>';
                    else
                        html += '<td> </td>';
                    html += '</tr>';
                });

                if (jsonResult.response == null) {
                    html += '<tr><td colspan="5" class="text-center">There are no files</td></tr>';
                }

            } else {
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status);
            alert(p3);
        }
    });

    html += '</tbody></table></div></div></div>';

    if (sharedViewMode == false) {
        html += '<div class="row">' +
            '<div class="col-md-12"><h4>Upload a document</h4><hr></div>' +
            '<div class="col-md-5">' +
            '<div class="form-group">' +
            '<input type="file" id="file-attachment-file">' +
            '<p class="help-block">Supported formats: jpg, png, bmp, doc/docx, xls/xlsx, pdf.</p>' +
            '</div>' +
            '</div>' +
            '<div class="col-md-5">' +
            //'<div class="form-group">' +
            //    '<label for="file-attachment-document-type">Document type</label>' +
            //    '<input type="text" class="form-control input-sm" id="file-attachment-document-type">' +
            //'</div>' +
            fileTypeHtml +
            '</div>' +
            '<div class="col-md-2">' +
            '<br/>' +
            '<a class=" btn btn-default" target="_blank" href="/DocumentTypes/Insert">Create doc type</a>' +
            '</div>' +
            '</div>' +
            //'<div class="col-md-2">' +
            //    '<br/>' +
            //    '<div class="btn btn-default" onclick="UploadFileAttachmentExperiment(\'' + elementName + '\', ' + elementId + ')">Upload File</div>' +
            //'</div>' +
            '</div>';
        html += '<div class="row">' +
            '<div class="col-md-10">' +
            '<div class="form-group">' +
            '<label for="file-attachment-description">Description (optional)</label>' +
            '<input type="text" class="form-control input-sm" id="file-attachment-description">' +
            '</div>' +
            '</div>' +
            '<div class="col-md-2">' +
            '<br/>' +
            '<div class="btn btn-default" onclick="UploadFileAttachmentExperiment(\'' + elementName + '\',  ' + experimentId + ',  ' + componentTypeId + ',  ' + stepId + ')">Upload File</div>' +
            '</div>' +
            '</div>';
    }


    dialog = bootbox.dialog({
        title: 'Documents',
        message: html,
        closeButton: true,
        onEscape: true,
        size: "large"
    }).on('shown.bs.modal', function (e) {
        DocTypesOnShow();
    });
}

function UploadFileAttachmentExperiment(elementName, experimentId, componentTypeId, stepId) {
    var file = document.querySelector('#file-attachment-file').files[0];
    var desc = $("#file-attachment-description").val();



    if (file != null) {
        var docTypeId = $("#selectDocumentType").val();
        if (docTypeId == null) {
            //$("#selectDocumentType").addClass("danger");
            //$("label[for='file-attachment-document-type']").css('color', 'red');
            notify("You must select the document type", "warning");
        }
        else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = (function () {
                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/SubmitFileAttachmentExperiment",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"fileBase64":"' + reader.result + '", "filename":"' + file.name + '","description":"' + desc + '", "elementName":"' + elementName + '", "experimentId":' + experimentId + ', "componentTypeId":' + componentTypeId + ', "stepId":' + stepId + ', "documentTypeId":' + docTypeId + '}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {
                            notify("Success!", "success");
                            bootbox.hideAll();
                            ShowFileAttachmentsExperiment(elementName, experimentId, componentTypeId, stepId);
                        } else {
                            notify(jsonResult.message, "warning");
                        }
                    },
                    error: function (p1, p2, p3) {
                        notify(p2 + " - " + p3, "danger");
                    }
                });
            });
            reader.onerror = function (error) {
                console.log('Error: ', error);
            };
        }
    }
}

function DownloadFileAttachment(fileAttachmentId) {
    //var win = window.open('/Helpers/DownloadFile.ashx?fileAttachmentId=' + fileAttachmentId + '&folder=' + folder, '_blank');
    var win = window.open('/Helpers/DownloadFileExperiment.ashx?fileAttachmentId=' + fileAttachmentId, '_blank');
}

function DeleteFileAttachmentExperiment(fileAttachmentId, elementName, experimentId, componentTypeId, stepId) {
    bootbox.confirm({
        title: "Delete document",
        message: "Are you sure you want to delete the document?",
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
                    url: "/Helpers/WebMethods.asmx/DeleteFileAttachmentExperiment",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"fileAttachmentId":' + fileAttachmentId + '}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {
                            notify("Success!", "success");
                            bootbox.hideAll();
                            ShowFileAttachmentsExperiment(elementName, experimentId, componentTypeId, stepId);
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

function GenerateStepDocumentsTableHtml(data, elementName, experimentId, componentTypeId, componentName) {
    var resultHtml = "";

    var tableHtml = '';

    var previousStepId = 0;
    var lastDocumentIndex = data.length - 1;
    $(data).each(function (index, value, data) {
        var stepId = this.stepId;

        if (previousStepId == 0) {
            previousStepId = stepId;
        }


        var rowHtml = "";
        rowHtml += '<tr>';
        rowHtml += '<td><a href="#" onclick="DownloadFileAttachment(' + this.fileAttachmentId + ')" title="' + this.filename + '">' + text_truncate(this.filename, 30, "...") + '</span></td>';
        rowHtml += '<td>' + this.documentTypeName + '</td>';
        rowHtml += '<td><span title="' + this.description + '">' + text_truncate(this.description, 40, "...") + '</td>';
        rowHtml += '<td>' + this.extension + '</td>';
        rowHtml += '<td>' + this.createdOn + '</td>';
        rowHtml += '<td><div title="Delete File" class="btn btn-xs btn-danger" onclick="DeleteFileAttachmentExperiment(' + this.fileAttachmentId + ', \'' + elementName + '\', ' + experimentId + '\, ' + componentTypeId + '\, ' + 1 + ')">Delete</div>' + '</td>';
        rowHtml += '</tr>';

        if (previousStepId == stepId) {
            if (tableHtml != '') {
                tableHtml += rowHtml;
            }
            else {
                tableHtml = "Step " + this.stepId;
                //tableHtml = "Step " + this.componentTypeId;
                tableHtml += '<div class="row"><div class="col-md-12">';

                tableHtml += '<div class="table-responsive">' +
                    '<table class="table table-condensed">' +
                    '<thead>' +
                    '<th>Filename</th>' +
                    '<th>Document type</th>' +
                    '<th>Description</th>' +
                    '<th>Type</th>' +
                    '<th>Date Added</th>' +
                    '<th>Action</th>' +
                    '</thead>' +
                    '<tbody>';
                tableHtml += rowHtml;
                rowHtml = "";
            }
        }
        else {
            tableHtml += '</tbody></table></div></div></div>';
            resultHtml += tableHtml;

            //tableHtml = "";
            tableHtml = "Step " + this.stepId;
            //tableHtml = "Step " + this.componentTypeId;
            tableHtml += '<div class="row"><div class="col-md-12">';

            tableHtml += '<div class="table-responsive">' +
                '<table class="table table-condensed">' +
                '<thead>' +
                '<th>Filename</th>' +
                '<th>Document type</th>' +
                '<th>Description</th>' +
                '<th>Type</th>' +
                '<th>Date Added</th>' +
                '<th>Action</th>' +
                '</thead>' +
                '<tbody>';

            tableHtml += rowHtml;
        }

        previousStepId = stepId;

        if (index == lastDocumentIndex) {
            tableHtml += '</tbody></table></div></div></div>';
            resultHtml += tableHtml;
        }

    });

    return resultHtml;
}

function ShowFileAttachmentsForAllSteps(elementName, experimentId, componentTypeId, componentName) {


    var fileTypeHtml = '<div class="form-group">';
    fileTypeHtml += '<label for="file-attachment-document-type">Document type</label>';
    fileTypeHtml += '<select class="form-control input-sm" id="selectDocumentType" class="document-type-data-ajax">    </select>';
    fileTypeHtml += '</div>';


    var html = "";
    var html = '<div class="row"><div class="col-md-12">';

    //html += '<div class="table-responsive">' +
    //        '<table class="table table-condensed">' +
    //            '<thead>' +
    //                '<th>Filename</th>' +
    //                '<th>Document type</th>' +
    //                '<th>Description</th>' +
    //                '<th>Type</th>' +
    //                '<th>Date Added</th>' +
    //                '<th>Action</th>' +
    //            '</thead>' +
    //            '<tbody>';

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetFileAttachmentsExperimentAllSteps",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{elementName:"' + elementName + '", experimentId:"' + experimentId + '", componentTypeId:"' + componentTypeId + '"}',
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            if (jsonResult.status == "ok") {

                if (jsonResult.response == null) {
                    html += '<tr><td colspan="5" class="text-center">There are no files</td></tr>';
                }
                else {
                    var allTablesHtml = GenerateStepDocumentsTableHtml(jsonResult.response, elementName, experimentId, componentTypeId, componentName);

                    html += allTablesHtml;
                }

            } else {
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status);
            alert(p3);
        }
    });

    //html += '<div class="row">' +
    //            '<div class="col-md-12"><h4>Upload a document</h4><hr></div>' +
    //            '<div class="col-md-5">' +
    //                '<div class="form-group">' +
    //                    '<input type="file" id="file-attachment-file">' +
    //                    '<p class="help-block">Supported formats: jpg, png, bmp, doc/docx, xls/xlsx, pdf.</p>' +
    //                '</div>' +
    //            '</div>' +
    //            '<div class="col-md-5">' +                    
    //                fileTypeHtml +
    //            '</div>' +
    //            '<div class="col-md-2">' +
    //                   '<br/>' +
    //                   '<a class=" btn btn-default" target="_blank" href="/DocumentTypes/Insert">Create doc type</a>' +
    //                '</div>' +
    //            '</div>' +
    //            //'<div class="col-md-2">' +
    //            //    '<br/>' +
    //            //    '<div class="btn btn-default" onclick="UploadFileAttachmentExperiment(\'' + elementName + '\', ' + elementId + ')">Upload File</div>' +
    //            //'</div>' +
    //        '</div>';
    //html += '<div class="row">' +
    //            '<div class="col-md-10">' +
    //                '<div class="form-group">' +
    //                    '<label for="file-attachment-description">Description (optional)</label>' +
    //                    '<input type="text" class="form-control input-sm" id="file-attachment-description">' +
    //                '</div>' +
    //            '</div>' +
    //            '<div class="col-md-2">' +
    //                '<br/>' +
    //                '<div class="btn btn-default" onclick="UploadFileAttachmentExperiment(\'' + elementName + '\',  ' + experimentId + ',  ' + componentTypeId + ',  ' + 1 + ')">Upload File</div>' +
    //            '</div>' +
    //        '</div>';

    dialog = bootbox.dialog({
        title: '' + componentName + ' Documents',
        message: html,
        closeButton: true,
        onEscape: true,
        size: "large"
    }).on('shown.bs.modal', function (e) {
        DocTypesOnShow();
    });
}
function DuplicateChildNodes(fkAttribute) {
    let count = $('[id^=pchild-' + fkAttribute + ']').length;
    var parent = document.getElementById(fkAttribute);
    var child = document.getElementById('pchild-' + fkAttribute + '-1');

    count = count + 1;
    var cln = child.cloneNode(true);
    cln.id = 'pchild-' + fkAttribute + '-' + count + '';
    parent.appendChild(cln);
    $('#pchild-' + fkAttribute + '-' + count + '').find('input').val('');
    var html = "";
    html += '<div class="btn btn-xs removeChild" onclick="RemoveParentNodes(this)"><i class="fa fa-minus"></i></div>';
    $('#pchild-' + fkAttribute + '-' + count + ' .childNodes').append(html);

    AddAutoHideEvent();
};

function RemoveParentNodes(e) {
    $(e).parent('div').remove();
}

$(function () {

    //$(document).bind("hashchange", function () {
    //    console.log('e');
    //});


});

function AddAutoHideEvent() {
    $(".removeChild").addClass("hidden");
    $(".childNodesWrapper").on({
        "mouseover": function (event) {
            //$(".removeChild").removeClass("hidden");
            $(this).find(".removeChild").removeClass("hidden");
        }
        , "mouseout": function (event) {
            $(this).find(".removeChild").addClass("hidden");
        }
    });

    $(".duplicateChildBtn").addClass("hidden");
    $(".parentChildWrapper").on({
        "mouseover": function (event) {
            //$(".removeChild").removeClass("hidden");
            $(this).find(".duplicateChildBtn").removeClass("hidden");
        }
        , "mouseout": function (event) {
            $(this).find(".duplicateChildBtn").addClass("hidden");
        }
    });
}