var chartCD1;
var chartCD2;
var chartCD3;
var chartCD4;
var chartCD5;

var chartCD1El;
var chartCD2El;
var chartCD3El;
var chartCD4El;
var chartCD5El;

var chartCD3ColorArray = ['#0000FD',
    '#0014D7',
    '#002EA3',
    '#004476',
    '#005653',
    '#006732',
    '#00790C',
    '#256D00',
    '#4D5A00',
    '#7C4100',
    '#9B3200',
    '#C81C00',
    '#EB0A00',
    '#FD0000'];

function ClearCharts() {
    var allTraces;
    var allIndicesArray = [];

    allTraces = chartCD1.data;
    $.each(allTraces, function (index, value) {
        allIndicesArray.push(index);
    });
    Plotly.deleteTraces(chartCD1, allIndicesArray);
    allIndicesArray = [];

    //allTraces = chartCD2.data;
    //$.each(allTraces, function (index, value) {
    //    allIndicesArray.push(index);
    //});
    //Plotly.deleteTraces(chartCD2, allIndicesArray);
    //allIndicesArray = [];

    allTraces = chartCD3.data;
    $.each(allTraces, function (index, value) {
        allIndicesArray.push(index);
    });
    Plotly.deleteTraces(chartCD3, allIndicesArray);
    allIndicesArray = [];

    allTraces = chartCD4.data;
    $.each(allTraces, function (index, value) {
        allIndicesArray.push(index);
    });
    Plotly.deleteTraces(chartCD4, allIndicesArray);
    allIndicesArray = [];

    //allTraces = chartCD5.data;
    //$.each(allTraces, function (index, value) {
    //    allIndicesArray.push(index);
    //});
    //Plotly.deleteTraces(chartCD5, allIndicesArray);
    //allIndicesArray = [];

}


function GetChargeDischargeTestDataForExperiment(testTypeId, index, experimentId, batchId) {
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
            //console.log(jsonResult.status);
            if (jsonResult.status == "ok") {
                //console.log(jsonResult.response);
                //return jsonResult.response;

                //console.log('GetChargeDischargeTestDataForExperiment done for ' + experimentId);
                //console.log("before call");  
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

                if (jsonResult.response.test != null && jsonResult.response.test.testData != null) {
                    MakeCharts(index, id, jsonResult.response);
                }
                else {
                    notify("No test data found for " + systemLabel);
                }

            }
            else return null;

            //var item = itemExists(jsonResult, 'Batelco');
            //if (item != null) {
            //    localData.push(item.Count);
            //} else {
            //    localData.push(0);
            //}

            //myChart.data.datasets[0].data = localData;
            ////myChart.config.centerText.text = localDataBefDl.reduce((a, b) => a + b, 0);
            //myChart.update();
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}
function MakeCharts(index, id, response) {

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
    }

    //color = GetRandomColor();
    if (index >= chartsColorArray.length)
        color = GetRandomColor();
    else
        color = chartsColorArray[index];

    MakeChartCD4(id, response, selectedMassValue, chartCD4);
    //MakeChartCD5(id, response, selectedMassValue, chartCD5);
    MakeChartCD5(id, response, selectedMassValue, chartCD4);


    MakeChartCD1(id, response, selectedMassValue, chartCD1);
    //MakeChartCD2(id, response, selectedMassValue, chartCD2);
    MakeChartCD2(id, response, selectedMassValue, chartCD1);
    MakeChartCD3(id, response, selectedMassValue, chartCD3);

    AddChartCD3Control();
}

function AddChartCD3Control() {
    var html = "";
    html += '<div class="form-group">';
    html += '<div class="col-sm-12">';
    html += '<select id="selectCycleIndex" class="" name="cycleIndexes[]" multiple="multiple">';

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
    $("#chartCD3Control").html(html);
    $('#selectCycleIndex').select2({
        //allowClear: true,
        placeholder: 'Select cycle index',
        width: "100%",
        closeOnSelect: false
    });
    var scrollTop;
    $('#selectCycleIndex').on("select2:selecting", function (event) {
        var $pr = $('#' + event.params.args.data._resultId).parent();
        scrollTop = $pr.prop('scrollTop');
    });
    $('#selectCycleIndex').on("select2:select", function (event) {
        var $pr = $('#' + event.params.data._resultId).parent();
        $pr.prop('scrollTop', scrollTop);
    });
    $('#selectCycleIndex').on("select2:unselecting", function (event) {
        var $pr = $('#' + event.params.args.data._resultId).parent();
        scrollTop = $pr.prop('scrollTop');
    });
    $('#selectCycleIndex').on("select2:unselect", function (event) {
        var $pr = $('#' + event.params.data._resultId).parent();
        $pr.prop('scrollTop', scrollTop);
    });


    $('#selectCycleIndex').on('change', function () {

        //console.log('selected cycle indexes: ' + $(this).val());
        //selectedExperiments = $(this).val();
        //UpdateChartData();
        var selectedIndexes = $(this).val();
        //console.log(selectedIndexes);

        var datasets = chartCD3.data;
        $.each(datasets, function (id, dataset) {
            //myChart.data.datasets.push(dataset);
            //console.log(dataset.cycleIndex);

            //console.log($.inArray(dataset.cycleIndex.toString(), selectedIndexes) != -1);

            if ($.inArray(dataset.cycleIndex.toString(), selectedIndexes) != -1) {
                dataset.visible = true;
                //console.log(dataset.cycleIndex + " is in array");
            }
            else {
                dataset.visible = false;
                //console.log(dataset.cycleIndex + " is NOT in array");
            }
            //console.log('value : ' + dataset);
        });
        //chartCD3.update();

        //lineD.visible = "legendonly";
        //lineC.visible = "legendonly";
        //Plotly.addTraces(myChart, localData);
        if (selectedSubjects.length > 1) {
            //if only one experiment/batch is selected - every line is colored with a different color, otherwise we use opacity to the same color for the subject

            var visibleDatasets = chartCD3.data.filter(trace => trace.visible === true);
            selectedExperiments.forEach(function (experimentId) {
                //console.log('exp = ' + value + '; ');
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
        Plotly.react(chartCD3, datasets, chartCD3.layout);
    });
}

function MakeChartCD1(experimentId, response, selectedMassValue, myChart) {

    var localData = GetDataForChartCD1(experimentId, response);

    //myChart.data.push(localData);
    //var currentChartData = myChart.data;

    //var newChartData = currentChartData.push(localData);

    //console.log(myChart.data);

    //myChart.config.centerText.text = localDataBefDl.reduce((a, b) => a + b, 0);
    //myChart.update();

    //Plotly.react(myChart, newChartData, myChart.layout);
    Plotly.addTraces(myChart, localData);

}
function MakeChartCD2(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD2(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}
function MakeChartCD3(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD3(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}

function MakeChartCD4(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD4(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}
function MakeChartCD5(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD5(experimentId, response, selectedMassValue);
    Plotly.addTraces(myChart, localData);
}

function GetDataForChartCD1(experimentId, response) {
    //experimentId can be a batchId!
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

        var x = GetValueForColumnName("test_time", valuesArray, experimentData) / 3600;
        var y = GetValueForColumnName("voltage", valuesArray, experimentData);
        var hoverText = "";
        hoverText += "x: " + x + "<br />";
        hoverText += "y: " + y + "<br />";
        hoverText += "Cycle Index: " + GetValueForColumnName("cycle_index", valuesArray, experimentData) + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText += "" + test.testLabel + "<br />";
        //var hoverText = "tt: 1.751611e-05<br />voltage: 0.2938658<br />tt: 1.751611e-05<br />voltage: 0.2938658<br />cycle:  1";
        //obj.cycleIndex = valuesArray.cycleIndex;

        dataArrayX.push(x);
        dataArrayY.push(y);
        hoverTextArray.push(hoverText);
    });

    //"tt: 1.751611e-05<br />voltage: 0.2938658<br />tt: 1.751611e-05<br />voltage: 0.2938658<br />cycle:  1"

    //console.log('aaa : ' + data);
    //var color = GetRandomColor();

    var traceName = "";
    if (isBatch)
        traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
    else
        traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

    var resultData = [];
    var trace = {
        x: dataArrayX,
        y: dataArrayY,
        mode: 'lines',
        name: traceName,
        hoverinfo: 'text',
        //hovertext:'a',
        text: hoverTextArray,
        //marker: {
        //    color: color,
        //    size: 5
        //},
        line: {
            color: color,
        },
        experimentId: experimentId,
        legendgroup: experimentId,
        //showlegend: false
    };

    resultData.push(trace);
    //console.log(resultData);
    return resultData;
}

function GetDataForChartCD2(experimentId, response, selectedMassValue) {
    var test = response.test;
    var experimentData = JSON.parse(test.testData.data).experiment_data;
    var data = experimentData.data;

    if (isBatch)
        batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(experimentId));
    else
        experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));

    console.log("massSelection: " + massSelection);
    console.log("selected mass value: " + selectedMassValue);

    var dataArrayX = [];
    var dataArrayY = [];
    var hoverTextArray = [];

    $.each(data, function (id, valuesArray) {
        var x = GetValueForColumnName("test_time", valuesArray, experimentData) / 3600;
        if (selectedMassValue == 0 || GetValueForColumnName("current", valuesArray, experimentData) == 0)
            var y = 0;
        else
            var y = (GetValueForColumnName("current", valuesArray, experimentData) / selectedMassValue);
        var hoverText = "";
        hoverText += "x: " + x + "<br />";
        hoverText += "y: " + y + "<br />";
        hoverText += "Cycle Index: " + GetValueForColumnName("cycle_index", valuesArray, experimentData) + "<br />";
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
        xaxis: 'x',
        yaxis: 'y2',
        mode: 'lines',
        name: traceName,
        //showlegend: true,
        hoverinfo: 'text',
        text: hoverTextArray,
        //marker: {
        //    color: color,
        //    size: 5
        //},
        line: {
            color: color,
        },
        experimentId: experimentId,
        legendgroup: experimentId,
        showlegend: false
    };

    resultData.push(trace);
    return resultData;
}

function GetDataForChartCD3(experimentId, response, selectedMassValue) {
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
    var dataArrayCX = [];
    var dataArrayCY = [];


    var hoverTextDArray = [];
    var hoverTextCArray = [];

    var length = data.length;
    //first cycle index
    //var previousCycleIndex = data[0].cycleIndex;
    //var previousCycleIndex = GetCycleIndexByOrder(0, experimentData)
    var previousCycleIndex = GetValueForColumnName("cycle_index", data[0], experimentData);

    //var color = GetRandomLightColor();
    var colorCD3 = color;

    //var numOfCycleIndexes = data[length - 1].cycleIndex;
    //var numOfCycleIndexesOld = GetCycleIndexByOrder((length - 1), experimentData)
    var numOfCycleIndexes = GetValueForColumnName("cycle_index", data[length - 1], experimentData);

    var colorStep = -1 / numOfCycleIndexes;

    if (maxCycleIndex < numOfCycleIndexes)
        maxCycleIndex = numOfCycleIndexes;

    var lineCount = 0;

    $.each(data, function (id, valuesArray) {
        if (GetValueForColumnName("current", valuesArray, experimentData) == 0) {
            return true;
        }
        if (selectedMassValue == 0 || GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0)
            var xd = 0;
        else
            var xd = GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / selectedMassValue;
        var yd = GetValueForColumnName("voltage", valuesArray, experimentData);

        //var cycleIndex = GetCycleIndexByOrder(id, experimentData);
        var cycleIndex = GetValueForColumnName("cycle_index", valuesArray, experimentData);
        //dataArrayDX.push(xd);
        //dataArrayDY.push(yd);

        if (selectedMassValue == 0 || GetValueForColumnName("charge_capacity", valuesArray, experimentData) == 0)
            var xc = 0;
        else
            var xc = GetValueForColumnName("charge_capacity", valuesArray, experimentData) / selectedMassValue;
        var yc = GetValueForColumnName("voltage", valuesArray, experimentData);
        //dataArrayCX.push(xc);
        //dataArrayCY.push(yc);


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

        var hoverTextC = "";
        hoverTextC += "x: " + xc + "<br />";
        hoverTextC += "y: " + yc + "<br />";
        hoverTextC += "Cycle Index: " + cycleIndex + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverTextC += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverTextC += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverTextC += "" + test.testLabel + "<br />";

        var traceName = "";
        if (isBatch)
            traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
        else
            traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

        if (GetValueForColumnName("cycle_index", valuesArray, experimentData) == previousCycleIndex) {
            if (GetValueForColumnName("current", valuesArray, experimentData) < 0) {
                dataArrayDX.push(xd);
                dataArrayDY.push(yd);
                hoverTextDArray.push(hoverTextD);
            }
            else {
                dataArrayCX.push(xc);
                dataArrayCY.push(yc);
                hoverTextCArray.push(hoverTextC);
            }
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
                        color: colorCD3,
                        size: 5
                    },
                    line: {
                        color: colorCD3,
                    },
                    experimentId: experimentId,
                    legendgroup: experimentId + "c" + previousCycleIndex,
                    showlegend: true,
                    experimentId: experimentId,
                    cycleIndex: previousCycleIndex,
                    //visible: "legendonly",
                    startColor: colorCD3,
                };
                var lineC = {
                    x: dataArrayCX,
                    y: dataArrayCY,
                    mode: 'lines',
                    name: traceName + " - cycle: " + previousCycleIndex,
                    hoverinfo: 'text',
                    text: hoverTextCArray,
                    opacity: customOpacity,
                    marker: {
                        color: colorCD3,
                        size: 5
                    },
                    line: {
                        color: colorCD3,
                    },
                    experimentId: experimentId,
                    legendgroup: experimentId + "c" + previousCycleIndex,
                    showlegend: false,
                    experimentId: experimentId,
                    cycleIndex: previousCycleIndex,
                    startColor: colorCD3,
                };
                //if (lineCount > 2) {
                if (previousCycleIndex != 4) {
                    //lineD.visible = "legendonly"; //to show only the legend
                    lineD.visible = false;
                    lineC.visible = false;
                }


                resultData.push(lineD);
                resultData.push(lineC);
                lineCount = lineCount + 1;


                hoverTextDArray = [];
                hoverTextCArray = [];

                //colorCD3 = ColorLuminance(colorCD3, colorStep);
                if (selectedSubjects.length == 1) {
                    //if only one experiment/batch is selected - every line is colored with a different color, otherwise we use opacity to the same color for the subject

                    //colorCD3 = GetRandomColor();
                    var colorIndex = lineCount;
                    if (colorIndex >= chartsColorArray.length)
                        colorCD3 = GetRandomColor();
                    else
                        colorCD3 = chartsColorArray[colorIndex];
                }
                else {
                    //colorCD3 = ColorLuminance(colorCD3, colorStep);

                }


                dataArrayDX = [];
                dataArrayDY = [];
                dataArrayCX = [];
                dataArrayCY = [];

                if (GetValueForColumnName("current", valuesArray, experimentData) < 0) {
                    dataArrayDX.push(xd);
                    dataArrayDY.push(yd);
                    hoverTextDArray.push(hoverTextD);
                }
                else {
                    dataArrayCX.push(xc);
                    dataArrayCY.push(yc);
                    hoverTextCArray.push(hoverTextC);
                }

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
                    color: colorCD3,
                    size: 5
                },
                line: {
                    color: colorCD3,
                },
                experimentId: experimentId,
                legendgroup: experimentId + "c" + previousCycleIndex,
                showlegend: true,
                experimentId: experimentId,
                cycleIndex: previousCycleIndex,
                //visible: "legendonly",
                startColor: colorCD3,
            };
            var lineC = {
                x: dataArrayCX,
                y: dataArrayCY,
                mode: 'lines',
                name: traceName + " - cycle: " + previousCycleIndex,
                hoverinfo: 'text',
                text: hoverTextCArray,
                opacity: customOpacity,
                marker: {
                    color: colorCD3,
                    size: 5
                },
                line: {
                    color: colorCD3,
                },
                experimentId: experimentId,
                legendgroup: experimentId + "c" + previousCycleIndex,
                showlegend: false,
                experimentId: experimentId,
                cycleIndex: previousCycleIndex,
                startColor: colorCD3,
            };
            if (GetValueForColumnName("cycle_index", valuesArray, experimentData) != 4) {
                lineD.visible = false;
                lineC.visible = false;
            }

            resultData.push(lineD);
            resultData.push(lineC);
            lineCount = lineCount + 1;

            hoverTextDArray = [];
            hoverTextCArray = [];

            //colorCD3 = ColorLuminance(colorCD3, colorStep);
            if (selectedSubjects.length == 1) {
                //colorCD3 = GetRandomColor();
                var colorIndex = lineCount;
                if (colorIndex >= chartsColorArray.length)
                    colorCD3 = GetRandomColor();
                else {
                    //colorCD3 = chartsColorArray[colorIndex];

                }
            }
            else
                colorCD3 = ColorLuminance(colorCD3, colorStep);

        }
        previousCycleIndex = GetValueForColumnName("cycle_index", valuesArray, experimentData);

    });

    return resultData;
}


function GetDataForChartCD4(experimentId, response, selectedMassValue) {
    var test = response.test;
    var experimentData = JSON.parse(test.testData.data).experiment_summary; //the "max by cycle index" values
    //var data = response.capacityData;
    var data = experimentData.data;

    if (isBatch)
        batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(experimentId));
    else
        experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));

    var dataArrayEfcX = [];
    var dataArrayEfcY = [];

    var dataArrayEfeX = [];
    var dataArrayEfeY = [];


    var hoverText1Array = [];
    var hoverText2Array = [];

    $.each(data, function (id, valuesArray) {
        var efficiency1;
        var efficiency2;
        if (selectedMassValue == 0) {
            efficiency1 = 0;
            efficiency2 = 0;
        }
        else {
            if (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0 || GetValueForColumnName("charge_capacity", valuesArray, experimentData) == 0)
                efficiency1 = 0;
            else
                efficiency1 = (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / selectedMassValue) / (GetValueForColumnName("charge_capacity", valuesArray, experimentData) / selectedMassValue);

            if (GetValueForColumnName("discharge_energy", valuesArray, experimentData) == 0 || GetValueForColumnName("charge_energy", valuesArray, experimentData) == 0)
                efficiency2 = 0;
            else
                efficiency2 = (GetValueForColumnName("discharge_energy", valuesArray, experimentData) / selectedMassValue) / (GetValueForColumnName("charge_energy", valuesArray, experimentData) / selectedMassValue);
            //efficiency2 = (1 / selectedMassValue) / (1 / selectedMassValue); //energy missing
        }

        efficiency1 = efficiency1 * 100;
        efficiency2 = efficiency2 * 100;

        var x1 = GetCycleIndexByOrder(id, experimentData);
        var y1 = efficiency1;

        dataArrayEfcX.push(x1);
        dataArrayEfcY.push(y1);

        var x2 = GetCycleIndexByOrder(id, experimentData);
        var y2 = efficiency2;

        dataArrayEfeX.push(x2);
        dataArrayEfeY.push(y2);

        var hoverText1 = "";
        hoverText1 += "x: " + x1 + "<br />";
        hoverText1 += "y: " + y1 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText1 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText1 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText1 += "" + test.testLabel + "<br />";
        hoverText1Array.push(hoverText1);
        var hoverText2 = "";
        hoverText2 += "x: " + x2 + "<br />";
        hoverText2 += "y: " + y2 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText2 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText2 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText2 += "" + test.testLabel + "<br />";
        hoverText2Array.push(hoverText2);
    });

    var traceName = "";
    if (isBatch)
        traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
    else
        traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

    var resultData = [];
    var trace1 = {
        x: dataArrayEfcX,
        y: dataArrayEfcY,
        mode: 'lines+markers',
        name: traceName + " c",
        hoverinfo: 'text',
        text: hoverText1Array,
        marker: {
            color: color,
            size: 5
        },
        line: {
            color: color,
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        showlegend: true,
        //visible: "legendonly"
    };
    var trace2 = {
        x: dataArrayEfeX,
        y: dataArrayEfeY,
        mode: 'lines+markers',
        name: traceName + " e",
        hoverinfo: 'text',
        text: hoverText2Array,
        marker: {
            color: color,
            size: 5
        },
        line: {
            color: color,
            dash: "dot"
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        //showlegend: false,
    };

    resultData.push(trace1);
    resultData.push(trace2);
    return resultData;
}

function GetDataForChartCD5(experimentId, response, selectedMassValue) {
    //var data = response.capacityData;
    //summary contains only max values (previously max_charge_capacity, max_discharge_capacity...)
    var test = response.test;
    var experimentData = JSON.parse(test.testData.data).experiment_summary; //the "max by cycle index" values
    var data = experimentData.data;

    if (isBatch)
        batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(experimentId));
    else
        experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));

    var dataArray1X = [];
    var dataArray1Y = [];
    var dataArray2X = [];
    var dataArray2Y = [];
    var dataArray3X = [];
    var dataArray3Y = [];
    var dataArray4X = [];
    var dataArray4Y = [];
    var dataArray5X = [];
    var dataArray5Y = [];


    var hoverText1Array = [];
    var hoverText2Array = [];
    var hoverText3Array = [];
    var hoverText4Array = [];
    var hoverText5Array = [];

    $.each(data, function (id, valuesArray) {

        var x1;
        var y1;

        var x2;
        var y2;

        var x3;
        var y3;

        x1 = GetCycleIndexByOrder(id, experimentData);
        if (GetValueForColumnName("charge_capacity", valuesArray, experimentData) == 0)
            y1 = 0
        else
            y1 = GetValueForColumnName("charge_capacity", valuesArray, experimentData) / response.massCalculations.mass1; //mass 3 if cathode  //* 1000

        x2 = GetCycleIndexByOrder(id, experimentData);
        if (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0)
            y2 = 0
        else
            y2 = GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / response.massCalculations.mass1; //mass 3 if cathode

        x3 = GetCycleIndexByOrder(id, experimentData);
        if (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0)
            y3 = 0
        else
            y3 = GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / response.massCalculations.mass2; // mass 4 if cathode


        if (massSelection == "3" || massSelection == "4") {
            y1 = (GetValueForColumnName("charge_capacity", valuesArray, experimentData) == 0) ? 0 : GetValueForColumnName("charge_capacity", valuesArray, experimentData) / response.massCalculations.mass3;
            y2 = (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0) ? 0 : GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / response.massCalculations.mass3;
            y3 = (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0) ? 0 : GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / response.massCalculations.mass4;
        }

        else if (massSelection == "7") {
            y1 = GetValueForColumnName("charge_capacity", valuesArray, experimentData);
            y2 = GetValueForColumnName("discharge_capacity", valuesArray, experimentData);
            y3 = GetValueForColumnName("discharge_capacity", valuesArray, experimentData);
        }

        dataArray1X.push(x1);
        dataArray1Y.push(y1);

        dataArray2X.push(x2);
        dataArray2Y.push(y2);


        dataArray3X.push(x3);
        dataArray3Y.push(y3);

        var x4 = GetCycleIndexByOrder(id, experimentData);
        var y4 = (GetValueForColumnName("discharge_capacity", valuesArray, experimentData) == 0) ? 0 : GetValueForColumnName("discharge_capacity", valuesArray, experimentData) / response.massCalculations.mass5;
        dataArray4X.push(x4);
        dataArray4Y.push(y4);

        //var x5 = valuesArray.cycleIndex;
        //var y5 = valuesArray.maxDischargeCapacity / response.massCalculations.mass4 * 1000;
        //dataArray5X.push(x5);
        //dataArray5Y.push(y5);


        var hoverText1 = "";
        hoverText1 += "x: " + x1 + "<br />";
        hoverText1 += "y: " + y1 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText1 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText1 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText1 += "" + test.testLabel + "<br />";
        hoverText1Array.push(hoverText1);
        var hoverText2 = "";
        hoverText2 += "x: " + x2 + "<br />";
        hoverText2 += "y: " + y2 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText2 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText2 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText2 += "" + test.testLabel + "<br />";
        hoverText2Array.push(hoverText2);
        var hoverText3 = "";
        hoverText3 += "x: " + x3 + "<br />";
        hoverText3 += "y: " + y3 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText3 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText3 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText3 += "" + test.testLabel + "<br />";
        hoverText3Array.push(hoverText3);
        var hoverText4 = "";
        hoverText4 += "x: " + x4 + "<br />";
        hoverText4 += "y: " + y4 + "<br />";
        if (shouldShowExperimentId) {
            if (isBatch)
                hoverText4 += "Batch: " + batchGeneralData.batchInfo.batchSystemLabel + "<br />";
            else
                hoverText4 += "Experiment: " + experimentGeneralData.experimentInfo.experimentSystemLabel + "<br />";
        }
        hoverText4 += "" + test.testLabel + "<br />";
        hoverText4Array.push(hoverText4);
        //var hoverText5 = "";
        //hoverText5 += "x: " + x5 + "<br />";
        //hoverText5 += "y: " + y5 + "<br />";
        //hoverText5Array.push(hoverText5);
    });

    var traceName = "";
    if (isBatch)
        traceName += batchGeneralData.batchInfo.batchSystemLabel + " - " + test.testLabel;
    else
        traceName += experimentGeneralData.experimentInfo.experimentSystemLabel + " - " + test.testLabel;

    var resultData = [];
    var trace1 = {
        x: dataArray1X,
        y: dataArray1Y,
        mode: 'lines+markers',
        name: traceName + " cc",
        hoverinfo: 'text',
        text: hoverText1Array,
        marker: {
            color: color,
            size: 5
        },
        line: {
            color: color,
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        showlegend: true,
        //visible: "legendonly"
        xaxis: 'x',
        yaxis: 'y2',
    };
    var trace2 = {
        x: dataArray2X,
        y: dataArray2Y,
        mode: 'lines+markers',
        name: traceName + " dc",
        hoverinfo: 'text',
        text: hoverText2Array,
        marker: {
            color: color,
            size: 7,
            symbol: "circle-open"
        },
        line: {
            color: color,
            //dash: "dashdot"
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        //showlegend: false,
        xaxis: 'x',
        yaxis: 'y2',
    };
    var trace3 = {
        x: dataArray3X,
        y: dataArray3Y,
        mode: 'lines+markers',
        name: traceName + " dt",
        hoverinfo: 'text',
        text: hoverText3Array,
        marker: {
            color: color,
            size: 10,
            symbol: "triangle-down-open"
        },
        line: {
            color: color,
            dash: "dash"
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        //showlegend: false,
        xaxis: 'x',
        yaxis: 'y2',
    };
    var trace4 = {
        x: dataArray4X,
        y: dataArray4Y,
        mode: 'lines+markers',
        name: traceName + " dt_bat",
        hoverinfo: 'text',
        text: hoverText4Array,
        marker: {
            color: color,
            size: 5,
            //symbol: "triangle-down"
        },
        line: {
            color: color,
            dash: "dash",
        },
        experimentId: experimentId,
        legendgroup: shouldGroupLegend ? experimentId : "",
        //showlegend: false,
        xaxis: 'x',
        yaxis: 'y2',
    };
    //var trace5 = {
    //    x: dataArray5X,
    //    y: dataArray5Y,
    //    mode: 'lines+markers',
    //    name: traceName,
    //    hoverinfo: 'text',
    //    text: hoverText5Array,
    //    marker: {
    //        color: color,
    //        size: 10,
    //        symbol: "triangle-up-open"
    //    },
    //    line: {
    //        color: color,
    //        dash: "dot"
    //    },
    //    experimentId: experimentId,
    //    legendgroup: shouldGroupLegend ? experimentId : "",
    //    //showlegend: false,
    //};

    resultData.push(trace1);
    resultData.push(trace2);
    resultData.push(trace3);
    resultData.push(trace4);
    //resultData.push(trace5);
    return resultData;
}

function ShowChargeDischargePlots(testTypeId) {
    ClearCharts();
    //$('div#summary').addClass('hidden');    

    if (selectedExperiments.length > 0) {
        //GetExperimentsSummaryList(selectedExperiments);
        $.each(selectedExperiments, function (index, experimentId) {
            //not ok for this to be here because of asynchrous behaviour
            //experimentGeneralData = experimentGeneralDataList.find(x => x.experimentInfo.experimentId === parseInt(experimentId));
            GetChargeDischargeTestDataForExperiment(testTypeId, index, parseInt(experimentId), null);
        });
    }
    else if (selectedBatches.length > 0) {
        //GetBatchSummaryList(selectedBatches);
        $.each(selectedBatches, function (index, batchId) {
            //batchGeneralData = batchGeneralDataList.find(x => x.batchInfo.batchId === parseInt(batchId));
            GetChargeDischargeTestDataForExperiment(testTypeId, index, null, parseInt(batchId));
        });
    }
}

$(function () {

    //chartCD1
    /*
    var trace1 = {
        x: [1, 2, 3, 4],
        y: [12, 9, 15, 12],
        mode: 'lines+markers',
        name: 'Scatter',
        marker: {
            color: 'rgb(219, 64, 82)',
            size: 12
        },
        line: {
            color: 'rgb(55, 128, 191)',
            width: 3
        }
    };
    var trace2 = {
        x: [2, 3, 4],
        y: [16, 5, 11, 9],
        mode: 'lines+markers',
        name: 'name2',
        yaxis: 'x',
        yaxis: 'y2',
    };
    */

    var trace1 = {
        x: [1, 2, 3, 4],
        y: [12, 9, 15, 12],
        //type: 'scatter',
        mode: 'lines+markers',
        name: 'Scatter',
        marker: {
            color: 'rgb(219, 64, 82)',
            size: 12
        },
        line: {
            color: 'rgb(55, 128, 191)',
            width: 3
        },
        legendgroup: 'b',

    };

    var trace2 = {
        x: [2, 3, 4],
        y: [16, 5, 11, 9],
        xaxis: 'x',
        yaxis: 'y2',
        //type: 'scatter',
        mode: 'lines+markers',
        name: 'name2',
        legendgroup: 'b',
        showlegend: false
    };

    //var chartCD1Data = [];
    var chartCD1Data = [];

    //layout
    var chartCD1Layout = {
        grid: {
            rows: 2,
            columns: 1,
            subplots: [['xy'], ['xy2']],
            roworder: 'top to bottom',

        },
        xaxis: {
            title: 'Test Time [hours]',
            //showgrid: false,
            //zeroline: false
        },
        yaxis: {
            title: 'Voltage',
            //showline: false
        },
        yaxis2: {
            title: 'Current (mA/g_mass Type)',
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
        autosize: true
    };

    chartCD1 = document.getElementById("chartCD1");
    var data = [trace1, trace2];
    var config = { responsive: true }
    Plotly.plot(chartCD1, chartCD1Data, chartCD1Layout, config);
    //chartCD1 = Plotly.plot(chartCD1El, chartCD1Data, chartCD1Layout);
    //var a = "a";



    //chartCD2
    /*
    var chartCD2Data = [];
    var chartCD2Layout = {
        //title: 'Line and Scatter Plot',
        xaxis: {
            title: 'Test Time [hours]',
        },
        yaxis: {
            title: 'Current (mA/g_mass Type)',
        },
        legend: {
            //y: 0.5,
            font: {
                //size: 16
            }
        },
        hovermode: 'closest',
    };

    chartCD2 = document.getElementById("chartCD2");
    Plotly.plot(chartCD2, chartCD2Data, chartCD2Layout);
    */

    //chartCD3
    var chartCD3Data = [];
    var chartCD3Layout = {
        //title: 'Line and Scatter Plot',
        autosize: true,
        xaxis: {
            title: 'Discharge Capacity (mAh/g_Mass Type)',
            autorange: true,
        },
        yaxis: {
            title: 'Voltage (V)',
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

    chartCD3 = document.getElementById("chartCD3");
    Plotly.plot(chartCD3, chartCD3Data, chartCD3Layout);

    //chartCD4
    var chartCD4Data = [];
    var chartCD4Layout = {
        grid: {
            rows: 2,
            columns: 1,
            subplots: [['xy'], ['xy2']],
            roworder: 'top to bottom', //'bottom to top',

        },
        //title: 'Line and Scatter Plot',
        xaxis: {
            title: 'Cycles',
        },
        yaxis: {
            title: 'Efficiency',
        },
        yaxis2: {
            title: 'Capacity (mAh/g_Mass Type)',
        },
        legend: {
            //y: 0.5,
            font: {
                //size: 16
            }
        },
        hovermode: 'closest',
    };
    //var chartCD4Layout = {
    //    //title: 'Line and Scatter Plot',
    //    xaxis: {
    //        title: 'Cycles',
    //    },
    //    yaxis: {
    //        title: 'Efficiency',
    //    },
    //    legend: {
    //        //y: 0.5,
    //        font: {
    //            //size: 16
    //        }
    //    },
    //    hovermode: 'closest',
    //};

    chartCD4 = document.getElementById("chartCD4");
    //chartCD4Data
    Plotly.plot(chartCD4, data, chartCD4Layout);

    //chartCD5
    //var chartCD5Data = [];
    //var chartCD5Layout = {
    //    //title: 'Line and Scatter Plot',
    //    xaxis: {
    //        title: 'Cycles',
    //    },
    //    yaxis: {
    //        title: 'Capacity (mAh/g_Mass Type)',
    //    },
    //    legend: {
    //        //y: 0.5,
    //        font: {
    //            //size: 16
    //        }
    //    },
    //    hovermode: 'closest',
    //};

    //chartCD5 = document.getElementById("chartCD5");
    //Plotly.plot(chartCD5, chartCD5Data, chartCD5Layout);

    //initially hide
    HideAllCharts();
});


