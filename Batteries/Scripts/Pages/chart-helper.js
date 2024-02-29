//const { ajax } = require("jquery");

var globalChartSettings = {
    //chartsList: []
};

var isBatch = false;

var experimentGeneralDataList;
var batchGeneralDataList;
var experimentGeneralData;
var batchGeneralData;

var selectedExperiments = [];
var selectedBatches = [];
var selectedSubjects = []; //general list that may contain selected batches or selected experiments

var massSelection = 1;
var maxCycleIndex = 0;
var selectedMassValue;

var shouldGroupLegend = false;
var shouldShowExperimentId = false;
//var shouldShowBatchId = false;

var color;

var chartsColorArray = ['rgb(248, 118, 109)',
    'rgb(0, 176, 246)',
    'rgb(216, 144, 0)',
    'rgb(163, 165, 0)',
    'rgb(57, 182, 0)',
    'rgb(0, 191, 125)',
    'rgb(0, 191, 196)',

    'rgb(149, 144, 255)',
    'rgb(231, 107, 243)',
    'rgb(255, 98, 188)'
];

function GetRandomColor() {
    opacity = 1;
    var randomColor = '#' + ('000000' + Math.floor(Math.random() * 16777215).toString(16)).slice(-6);
    return randomColor;
    //return 'rgba(' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ',' + (opacity || '.3') + ')';
}

function GetRandomLightColor() {
    var letters = 'BCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * letters.length)];
    }
    return color;
}

function LightenDarkenColor(col, amt) {

    var usePound = false;

    if (col[0] == "#") {
        col = col.slice(1);
        usePound = true;
    }

    var num = parseInt(col, 16);

    var r = (num >> 16) + amt;

    if (r > 255) r = 255;
    else if (r < 0) r = 0;

    var b = ((num >> 8) & 0x00FF) + amt;

    if (b > 255) b = 255;
    else if (b < 0) b = 0;

    var g = (num & 0x0000FF) + amt;

    if (g > 255) g = 255;
    else if (g < 0) g = 0;

    return (usePound ? "#" : "") + (g | (b << 8) | (r << 16)).toString(16);

}
function ColorLuminance(hex, lum) {
    // validate hex string
    hex = String(hex).replace(/[^0-9a-f]/gi, '');
    if (hex.length < 6) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    lum = lum || 0;
    // convert to decimal and change luminosity
    var rgb = "#", c, i;
    for (i = 0; i < 3; i++) {
        c = parseInt(hex.substr(i * 2, 2), 16);
        c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
        rgb += ("00" + c).substr(c.length);
    }
    return rgb;
}


function ShowSummary(experiments) {
    CreateSummaryTable();
    AppendExperimentsSummary(experiments);
}
function ShowBatchSummary(batches) {
    CreateBatchSummaryTable();
    AppendBatchSummary(batches);
}
function CreateSummaryTable() {

    //<tr>    \
    //    <th></th>   \
    //</tr>   \
    var html = "<thead> \
                                        </thead>    \
                                        <tbody> \
                                            <tr id='systemLabel'>    \
                                                <th>System Label</th>   \
                                            </tr>   \
                                            <tr id='personalLabel'>    \
                                                <th>Personal Label</th> \
                                            </tr>   \
                                            <tr id='description'>    \
                                                <th>Description</th>    \
                                            </tr>   \
                                            <tr id='createdBy'>    \
                                                <th>Created by</th> \
                                            </tr>   \
                                            <tr id='dateCreated'>    \
                                                <th>Date created</th>   \
                                            </tr>   \
                                            <tr id='anodeActiveMat'>    \
                                                <th>Anode: A.M. </th>   \
                                            </tr>   \
                                            <tr id='cathodeActiveMat'>    \
                                                <th>Cathode: A.M </th>  \
                                            </tr>   \
                                            <tr id='anode'>    \
                                                <th>Anode: </th>    \
                                            </tr>   \
                                            <tr id='cathode'>    \
                                                <th>Cathode: </th>  \
                                            </tr>   \
                                            <tr id='separator'>    \
                                                <th>Separator: </th>    \
                                            </tr>   \
                                            <tr id='electrolyte'>    \
                                                <th>Electrolyte: </th>  \
                                            </tr>   \
                                            <tr id='referenceElectrode'>    \
                                                <th>Reference Electrode: </th>  \
                                            </tr>   \
                                            <tr id='casing'>    \
                                                <th>Casing: </th>   \
                                            </tr>   \
                                        </tbody>    \
    ";

    $('#summaryTable').html(html);
}
function AppendExperimentsSummary(experiments) {
    $.each(experiments, function (index, experiment) {
        var url = "/Experiments/View/" + experiment.experimentInfo.experimentId;
        var link = '<a  title="View" href="' + url + '" target="_blank">' + experiment.experimentInfo.experimentSystemLabel + '</a> ';

        //$('#summaryTable thead tr').append("<th>" + experiment.experimentInfo.experimentId + "</th>");
        $('#summaryTable tbody tr#systemLabel').append("<td>" + link + "</td>");
        $('#summaryTable tbody tr#personalLabel').append("<td>" + experiment.experimentInfo.experimentPersonalLabel + "</td>");
        $('#summaryTable tbody tr#description').append("<td>" + experiment.experimentInfo.experimentDescription + "</td>");
        $('#summaryTable tbody tr#createdBy').append("<td>" + experiment.experimentInfo.operatorUsername + "</td>");
        $('#summaryTable tbody tr#dateCreated').append("<td>" + experiment.experimentInfo.dateCreated + "</td>");

        var calculations = experiment.calculations;
        var html = "";
        if (calculations.anode.componentEmpty == true || (calculations.anode.fkCommercialType != null)) {
            html = '<td> / </td>';
        }
        else {
            html = '<td>' + calculations.anode.totalActiveMaterials.toPrecision(4) + " g" + " : " + calculations.anode.activeMaterials + " = " + calculations.anode.activePercentages + " %" + '</td>';
        }
        $('#summaryTable tbody tr#anodeActiveMat').append(html);


        if (calculations.cathode.componentEmpty == true || (calculations.cathode.fkCommercialType != null)) {
            html = '<td> / </td>';
        }
        else {
            html = '<td>' + calculations.cathode.totalActiveMaterials.toPrecision(4) + " g" + " : " + calculations.cathode.activeMaterials + " = " + calculations.cathode.activePercentages + " %" + '</td>';

        }
        $('#summaryTable tbody tr#cathodeActiveMat').append(html);


        if (calculations.anode.componentEmpty == false) {
            if (calculations.anode.fkCommercialType != null) {
                html = '<td>' + calculations.anode.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.anode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.anode.labeledMaterials + " = " + calculations.anode.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#anode').append(html);

        if (calculations.cathode.componentEmpty == false) {
            if (calculations.cathode.fkCommercialType != null) {
                html = '<td>' + calculations.cathode.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.cathode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.cathode.labeledMaterials + " = " + calculations.cathode.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#cathode').append(html);

        if (calculations.separator.componentEmpty == false) {
            if (calculations.separator.fkCommercialType != null) {
                html = '<td>' + calculations.separator.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.separator.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.separator.labeledMaterials + " = " + calculations.separator.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#separator').append(html);

        if (calculations.electrolyte.componentEmpty == false) {
            if (calculations.electrolyte.fkCommercialType != null) {
                html = '<td>' + calculations.electrolyte.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.electrolyte.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.electrolyte.labeledMaterials + " = " + calculations.electrolyte.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#electrolyte').append(html);

        if (calculations.referenceElectrode.componentEmpty == false) {
            if (calculations.referenceElectrode.fkCommercialType != null) {
                html = '<td>' + calculations.referenceElectrode.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.referenceElectrode.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.referenceElectrode.labeledMaterials + " = " + calculations.referenceElectrode.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#referenceElectrode').append(html);


        if (calculations.casing.componentEmpty == false) {
            if (calculations.casing.fkCommercialType != null) {
                html = '<td>' + calculations.casing.commercialTypeName + '</td>';
            }
            else {
                html = '<td>' + 'Total weight: ' + calculations.casing.totalLabeledMaterials.toPrecision(4) + " g" + " : " + calculations.casing.labeledMaterials + " = " + calculations.casing.labeledPercentages + " %" + '</td>';
            }
        }
        else html = '<td> / </td>';
        $('#summaryTable tbody tr#casing').append(html);

    });



    $('div#summary').removeClass('hidden');
}

//<tr>    \
//        <th></th>   \
//    </tr>   \
function CreateBatchSummaryTable() {
    var html = "<thead> \
                                        </thead>    \
                                        <tbody> \
                                            <tr id='systemLabel'>    \
                                                <th>System Label</th>   \
                                            </tr>   \
                                            <tr id='personalLabel'>    \
                                                <th>Personal Label</th> \
                                            </tr>   \
                                            <tr id='description'>    \
                                                <th>Description</th>    \
                                            </tr>   \
                                            <tr id='createdBy'>    \
                                                <th>Created by</th> \
                                            </tr>   \
                                            <tr id='dateCreated'>    \
                                                <th>Date created</th>   \
                                            </tr>   </tbody >    \
    ";

    $('#summaryTable').html(html);
}
function AppendBatchSummary(batches) {
    $.each(batches, function (index, batch) {
        var url = "/Batches/View/" + batch.batchInfo.batchId;
        var link = '<a  title="View" href="' + url + '" target="_blank">' + batch.batchInfo.batchSystemLabel + '</a> ';
        //$('#summaryTable thead tr').append("<th>" + batch.batchInfo.batchId + "</th>");

        $('#summaryTable tbody tr#systemLabel').append("<td>" + link + "</td>");
        $('#summaryTable tbody tr#personalLabel').append("<td>" + batch.batchInfo.batchPersonalLabel + "</td>");
        $('#summaryTable tbody tr#description').append("<td>" + batch.batchInfo.description + "</td>");
        $('#summaryTable tbody tr#createdBy').append("<td>" + batch.batchInfo.operatorUsername + "</td>");
        $('#summaryTable tbody tr#dateCreated').append("<td>" + batch.batchInfo.dateCreated + "</td>");

    });

    $('div#summary').removeClass('hidden');
}

function GetExperimentsSummaryList(selectedExperiments) {
    var RequestDataString = JSON.stringify({ experimentIds: selectedExperiments });

    var ajaxData = $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetExperimentsSummaryList",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: RequestDataString,
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            //console.log('GetExperimentsSummaryList done for ' + selectedExperiments);
            //console.log(jsonResult);
            experimentGeneralDataList = jsonResult;
            ShowSummary(jsonResult);

            //console.log(jsonResult.status);
            //if (jsonResult.status == "ok") {
            //    //      
            //    //
            //}
            //else return null;
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
    return ajaxData;
}
function GetSummary(testTypeId, testType, experiments, batches) {
    $('div#summary').addClass('hidden');
    if (selectedExperiments.length > 0) {
        $.when(
            GetExperimentsSummaryList(selectedExperiments)
        ).done(function (x) {
            DrawCharts(testTypeId, testType, experiments, batches);
        });
    }
    if (selectedBatches.length > 0) {
        //GetBatchSummaryList(selectedBatches);
        $.when(
            GetBatchSummaryList(selectedBatches)
        ).done(function (x) {
            DrawCharts(testTypeId, testType, experiments, batches);
        });
    }
}

function GetBatchSummaryList(selectedBatches) {
    var RequestDataString = JSON.stringify({ batchIds: selectedBatches });

    var ajaxData = $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetBatchSummaryList",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: RequestDataString,
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            //console.log('GetExperimentsSummaryList done for ' + selectedExperiments);
            //console.log(jsonResult);
            batchGeneralDataList = jsonResult;
            ShowBatchSummary(jsonResult);

            //console.log(jsonResult.status);            
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });

    return ajaxData;
}

function DrawSummaryAndCharts(testTypeId, testType, experiments, batches) {
    GetSummary(testTypeId, testType, experiments, batches);
}

function DrawCharts(testTypeId, testType, experiments, batches) {
    HideAllCharts();

    //clean any previous general data
    experimentGeneralData = undefined;
    batchGeneralData = undefined;

    if (experiments.length > 0) {
        isBatch = false;
    }
    else if (batches.length > 0) {
        isBatch = true;
    }

    if (selectedExperiments.length > 1 || selectedBatches.length > 1) {
        shouldGroupLegend = true;
        shouldShowExperimentId = true;
    }
    else {
        shouldGroupLegend = false;
        shouldShowExperimentId = false;
    }

    if (isBatch) {
        selectedSubjects = selectedBatches;
    }
    else {
        selectedSubjects = selectedExperiments;
    }

    if (testType == "Charge-discharge") {
        document.getElementById("wrapperChargeDischarge").removeAttribute("hidden");
        if (experiments.length != 0) {
            //document.getElementById("wrapperChargeDischargeBatch").removeAttribute("hidden")
        }
        else {
            // document.getElementById("divBatchGraph").setAttribute("hidden");
        }
        ShowChargeDischargePlots(testTypeId);
    }
    else if (testType == "XRD") {
        document.getElementById("wrapperXRD").removeAttribute("hidden");
        ShowtXRDPlots(testTypeId);
    }
    else if (testType == "Voltammetry") {
        document.getElementById("wrapperVoltammetry").removeAttribute("hidden");
        ShowVoltammetryPlots(testTypeId);
    }
    else if (testType == "Electrical Impedance Spectroscopy (EIS)") {
        document.getElementById("wrapperEIS").removeAttribute("hidden");
        ShowEISPlots(testTypeId);
    }
}

function HideAllCharts() {
    document.getElementById("wrapperChargeDischarge").setAttribute("hidden", "");
    document.getElementById("wrapperVoltammetry").setAttribute("hidden", "");
    document.getElementById("wrapperXRD").setAttribute("hidden", "");
    document.getElementById("wrapperEIS").setAttribute("hidden", "");
}