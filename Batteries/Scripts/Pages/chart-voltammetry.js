var chartV1;

function ClearVCharts() {
    var allTraces;
    var allIndicesArray = [];

    allTraces = chartV1.data;
    $.each(allTraces, function (index, value) {
        allIndicesArray.push(index);
    });
    Plotly.deleteTraces(chartV1, allIndicesArray);
    allIndicesArray = [];
}

function GetVTestData(testTypeId, index, experimentId, batchId) {
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
                    MakeChartsV(index, id, jsonResult.response);
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

function ShowVoltammetryPlots(testTypeId) {
    ClearVCharts();

    if (selectedExperiments.length > 0) {
        $.each(selectedExperiments, function (index, experimentId) {
            GetVTestData(testTypeId, index, parseInt(experimentId), null);
        });
    }
    else if (selectedBatches.length > 0) {
        $.each(selectedBatches, function (index, batchId) {
            GetVTestData(testTypeId, index, null, parseInt(batchId));
        });
    }
}
function MakeChartsV(index, id, response) {
    if (isBatch) selectedMassValue = 1;

    else {
        selectedMassValue = response.massCalculations.mass1;

        switch (massSelection) {
            case "1":
                selectedMassValue = response.massCalculations.mass1;
                break;
            case "2":
                selectedMassValue = response.massCalculations.mass2;
                break;
            case "3":
                selectedMassValue = response.massCalculations.mass3;
                break;
            case "4":
                selectedMassValue = response.massCalculations.mass4;
                break;
            case "5":
                selectedMassValue = response.massCalculations.mass5;
                break;
            case "6":
                selectedMassValue = response.massCalculations.mass6;
                break;
            case "7":
                selectedMassValue = 1;
                break;
        }
        //if (selectedMassValue == 0) selectedMassValue = 1;
    }

    if (index >= chartsColorArray.length)
        color = GetRandomColor();
    else
        color = chartsColorArray[index];

    MakeChartV1(id, response, selectedMassValue, chartV1);

    AddChartV1Control();
}
function MakeChartV1(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartV1(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}

function GetDataForChartV1(experimentId, response) {
    var customOpacity = 1;
    var test = response.test;
    var experimentData = JSON.parse(test.testData.data).experiment_data;
    var data = experimentData.data;

    if (isBatch)
        batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(experimentId));
    else
        experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));

    var resultData = [];

    var dataArrayDX = [];
    var dataArrayDY = [];
    var hoverTextDArray = [];

    var length = data.length;
    //first cycle index
    var previousCycleIndex = GetValueForColumnName("cycle number", data[0], experimentData);

    //var color = GetRandomLightColor();
    var colorV1 = color;

    var numOfCycleIndexes = GetValueForColumnName("cycle number", data[length - 1], experimentData);

    var colorStep = -1 / numOfCycleIndexes;

    if (maxCycleIndex < numOfCycleIndexes)
        maxCycleIndex = numOfCycleIndexes;

    var lineCount = 0;

    $.each(data, function (id, valuesArray) {
        //voltage: "Ewe/V"  -X
        //current: "I/mA"   -Y

        //if (selectedMassValue == 0)
        //    var xd = 0;

        //if (GetValueForColumnName("I/mA", valuesArray, experimentData) == 0) {
        //    return true;
        //}

        var xd = GetValueForColumnName("Ewe/V", valuesArray, experimentData);

        if (selectedMassValue == 0 || GetValueForColumnName("I/mA", valuesArray, experimentData) == 0)
            var yd = 0;
        else
            var yd = (GetValueForColumnName("I/mA", valuesArray, experimentData) / selectedMassValue);
        //var yd = GetValueForColumnName("I/mA", valuesArray, experimentData);

        var cycleIndex = GetValueForColumnName("cycle number", valuesArray, experimentData);

        var hoverTextD = "";
        hoverTextD += "x: " + xd + "<br />";
        hoverTextD += "y: " + yd + "<br />";
        hoverTextD += "Cycle Index: " + cycleIndex + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverTextD += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverTextD += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverTextD += "" + test.testLabel + "<br />";

        var traceName = "";
        if (isBatch)
            traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
        else
            traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

        if (GetValueForColumnName("cycle number", valuesArray, experimentData) == previousCycleIndex) {
            dataArrayDX.push(xd);
            dataArrayDY.push(yd);
            hoverTextDArray.push(hoverTextD);
        }
        else {
            if (dataArrayDX.length != 0) {
                var lineD = {
                    x: dataArrayDX,
                    y: dataArrayDY,
                    mode: 'lines',
                    name: traceName + " - cycle: " + previousCycleIndex,
                    hoverinfo: 'text',
                    text: hoverTextDArray,
                    opacity: customOpacity,
                    marker: {
                        color: colorV1,
                        size: 5
                    },
                    line: {
                        color: colorV1,
                    },
                    experimentId: experimentId,
                    legendgroup: experimentId + "c" + previousCycleIndex,
                    showlegend: true,
                    experimentId: experimentId,
                    cycleIndex: previousCycleIndex,
                    //visible: "legendonly",
                    startColor: colorV1,
                };
                if (previousCycleIndex != 4) {
                    lineD.visible = false;
                }
                resultData.push(lineD);
                lineCount = lineCount + 1;

                hoverTextDArray = [];
                hoverTextCArray = [];

                if (selectedSubjects.length == 1) {
                    //if only one experiment/batch is selected - every line is colored with a different color, otherwise we use opacity to the same color for the subject

                    var colorIndex = lineCount;
                    if (colorIndex >= chartsColorArray.length)
                        colorV1 = GetRandomColor();
                    else
                        colorV1 = chartsColorArray[colorIndex];
                }
                //else {
                //    //colorV1 = ColorLuminance(colorV1, colorStep);

                //}

                dataArrayDX = [];
                dataArrayDY = [];

                dataArrayDX.push(xd);
                dataArrayDY.push(yd);
                hoverTextDArray.push(hoverTextD);
            }
        }
        if (id == length - 1) {

            var lineD = {
                x: dataArrayDX,
                y: dataArrayDY,
                mode: 'lines',
                name: traceName + " - cycle: " + previousCycleIndex,
                hoverinfo: 'text',
                text: hoverTextDArray,
                opacity: customOpacity,
                marker: {
                    color: colorV1,
                    size: 5
                },
                line: {
                    color: colorV1,
                },
                experimentId: experimentId,
                legendgroup: experimentId + "c" + previousCycleIndex,
                showlegend: true,
                experimentId: experimentId,
                cycleIndex: previousCycleIndex,
                //visible: "legendonly",
                startColor: colorV1,
            };
            if (GetValueForColumnName("cycle number", valuesArray, experimentData) != 4) {
                lineD.visible = false;
            }
            resultData.push(lineD);
            lineCount = lineCount + 1;

            hoverTextDArray = [];

            //colorV1 = ColorLuminance(colorV1, colorStep);
            if (selectedSubjects.length == 1) {
                //colorV1 = GetRandomColor();
                var colorIndex = lineCount;
                if (colorIndex >= chartsColorArray.length)
                    colorV1 = GetRandomColor();
                else {
                    //colorV1 = chartsColorArray[colorIndex];

                }
            }
            else
                colorV1 = ColorLuminance(colorV1, colorStep);

        }
        previousCycleIndex = GetValueForColumnName("cycle number", valuesArray, experimentData);

    });

    return resultData;
}
function AddChartV1Control() {
    var html = "";
    html += '<div class="form-group">';
    html += '<div class="col-sm-12">';
    html += '<select id="selectCycleIndexV1" class="" name="cycleIndexesV1[]" multiple="multiple">';

    //maxCycleIndex
    for (i = 1; i <= maxCycleIndex; i++) {
        if (i != 4)
            html += '<option value="' + i + '">' + i + '</option>';
        else
            html += '<option value="' + i + '" selected>' + i + '</option>';
    }
    //html += '<option value="1">1 </option>';
    //html += '<option value="2">2 </option>';

    html += '</select>';
    html += '</div>';
    html += '</div>';

    //myChart.canvas.before(html);
    $("#chartV1Control").html(html);
    $('#selectCycleIndexV1').select2({
        //allowClear: true,
        placeholder: 'Select cycle index',
        width: "100%",
        closeOnSelect: false
    });
    var scrollTop;
    $('#selectCycleIndexV1').on("select2:selecting", function (event) {
        var $pr = $('#' + event.params.args.data._resultId).parent();
        scrollTop = $pr.prop('scrollTop');
    });
    $('#selectCycleIndexV1').on("select2:select", function (event) {
        var $pr = $('#' + event.params.data._resultId).parent();
        $pr.prop('scrollTop', scrollTop);
    });
    $('#selectCycleIndexV1').on("select2:unselecting", function (event) {
        var $pr = $('#' + event.params.args.data._resultId).parent();
        scrollTop = $pr.prop('scrollTop');
    });
    $('#selectCycleIndexV1').on("select2:unselect", function (event) {
        var $pr = $('#' + event.params.data._resultId).parent();
        $pr.prop('scrollTop', scrollTop);
    });


    $('#selectCycleIndexV1').on('change', function () {
        var selectedIndexes = $(this).val();

        var datasets = chartV1.data;
        $.each(datasets, function (id, dataset) {

            if ($.inArray(dataset.cycleIndex.toString(), selectedIndexes) != -1) {
                dataset.visible = true;
            }
            else {
                dataset.visible = false;
            }
        });

        if (selectedSubjects.length > 1) {
            var visibleDatasets = chartV1.data.filter(trace => trace.visible === true);
            selectedSubjects.forEach(function (experimentId) {
                var visibleDatasetsForExperiment = visibleDatasets.filter(trace => trace.experimentId === parseInt(experimentId));
                var opacityStep = 1 / Math.round(visibleDatasetsForExperiment.length / 2);
                var opacity = 0;

                var cycleIndex = 0;
                $.each(visibleDatasetsForExperiment, function (id, dataset) {
                    if (cycleIndex != dataset.cycleIndex)
                        cycleIndex = dataset.cycleIndex;
                    else return;
                    opacity = opacity + opacityStep;
                    var visibleDatasetsForExperimentCycle = visibleDatasetsForExperiment.filter(trace => trace.cycleIndex === cycleIndex);
                    $.each(visibleDatasetsForExperimentCycle, function (id, dataset) {
                        dataset.opacity = opacity;
                    });
                });

            });
        }
        Plotly.react(chartV1, datasets, chartV1.layout);
    });
}

$(function () {

    //layout
    //chartV1
    var chartV1Data = [];
    var chartV1Layout = {
        //title: 'Line and Scatter Plot',
        autosize: true,
        xaxis: {
            title: 'Voltage (V)',
            autorange: true,
        },
        yaxis: {
            title: 'Current (mA)',
            autorange: true,
            //type: "linear",
        },
        legend: {
            //y: 0.5,
            font: {
                //size: 16
            }
        },
        hovermode: 'closest',
    };

    chartV1 = document.getElementById("chartV1");
    Plotly.plot(chartV1, chartV1Data, chartV1Layout);
    //initially hide
    HideAllCharts();
});


