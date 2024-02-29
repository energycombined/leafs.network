var chartEIS1;

function ClearEISCharts() {
    var allTraces;
    var allIndicesArray = [];

    allTraces = chartEIS1.data;
    $.each(allTraces, function (index, value) {
        allIndicesArray.push(index);
    });
    Plotly.deleteTraces(chartEIS1, allIndicesArray);
    allIndicesArray = [];
}

function GetEISTestData(testTypeId, index, experimentId, batchId) {
    var RequestDataString = JSON.stringify({ testTypeId: testTypeId, experimentId: experimentId, batchId: batchId });

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetTestDataForCharts",
        async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: RequestDataString,
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            var localData = [];
            if (jsonResult.status == "ok") {
                var id;
                var systemLabel = "";
                if (experimentId != null) {
                    id = experimentId;
                    systemLabel = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId)).experimentInfo.experimentSystemLabel;
                }
                else {
                    id = batchId;
                    systemLabel = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(batchId)).batchInfo.batchSystemLabel;
                }
                if (jsonResult.response.test != null && jsonResult.response.test.testData != null)
                    MakeChartsEIS(index, id, jsonResult.response);
                else {
                    notify("No test data found for " + systemLabel);
                }
            }
            else return null;
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}

function ShowEISPlots(testTypeId) {
    ClearEISCharts();

    if (selectedExperiments.length > 0) {
        $.each(selectedExperiments, function (index, experimentId) {
            GetEISTestData(testTypeId, index, parseInt(experimentId), null);
        });
    }
    else if (selectedBatches.length > 0) {
        $.each(selectedBatches, function (index, batchId) {
            GetEISTestData(testTypeId, index, null, parseInt(batchId));
        });
    }
}
function MakeChartsEIS(index, id, response) {
    if (index >= chartsColorArray.length)
        color = GetRandomColor();
    else
        color = chartsColorArray[index];

    MakeChartEIS1(id, response, selectedMassValue, chartEIS1);
}
function MakeChartEIS1(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartEIS1(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}

function GetDataForChartEIS1(experimentId, response) {
    var test = response.test;
    var experimentData = JSON.parse(test.testData.data).experiment_data;
    var data = experimentData.data;

    if (isBatch)
        batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(experimentId));
    else
        experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));

    var dataArrayX = [];
    var dataArrayY = [];
    var hoverTextArray = [];

    $.each(data, function (id, valuesArray) {

        var x = GetValueForColumnName("Re(Z)/Ohm", valuesArray, experimentData);
        var y = GetValueForColumnName("-Im(Z)/Ohm", valuesArray, experimentData);
        var hoverText = "";
        hoverText += "x: " + x + "<br />";
        hoverText += "y: " + y + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText += "" + test.testLabel + "<br />";

        dataArrayX.push(x);
        dataArrayY.push(y);
        hoverTextArray.push(hoverText);
    });

    var traceName = "";
    if (isBatch)
        traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
    else
        traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

    var resultData = [];
    var trace = {
        x: dataArrayX,
        y: dataArrayY,
        mode: 'lines+markers',
        name: traceName,
        hoverinfo: 'text',
        //hovertext:'a',
        text: hoverTextArray,
        marker: {
            color: color,
            size: 7,
            symbol: "circle-closed"
        },
        line: {
            color: color,
        },
        experimentId: experimentId,
    };

    resultData.push(trace);
    return resultData;
}

$(function () {

    //layout
    var chartEIS1Layout = {
        title: 'EIS results',
        xaxis: {
            title: 'Re(Z)/Ohm',
            //showgrid: false,
            //zeroline: false
        },
        yaxis: {
            title: '-Im(Z)/Ohm',
            //showline: false
        },
        legend: {
            //y: 0.5,
            //traceorder: 'reversed',
            font: {
                //size: 16
            }
        },
        hovermode: 'closest',
    };

    chartEIS1 = document.getElementById("chartEIS1");
    var chartEIS1Data = [];
    Plotly.plot(chartEIS1, chartEIS1Data, chartEIS1Layout);
    //initially hide
    HideAllCharts();
});

