/**
 * @namespace Chart.helpers
 */
var helpers = {
    /**
	 * An empty function that can be used, for example, for optional callback.
	 */
    noop: function () { },

    /**
	 * Returns a unique id, sequentially generated from a global variable.
	 * @returns {Number}
	 * @function
	 */
    uid: (function () {
        var id = 0;
        return function () {
            return id++;
        };
    }()),

    /**
	 * Returns true if `value` is neither null nor undefined, else returns false.
	 * @param {*} value - The value to test.
	 * @returns {Boolean}
	 * @since 2.7.0
	 */
    isNullOrUndef: function (value) {
        return value === null || typeof value === 'undefined';
    },

    /**
	 * Returns true if `value` is an array, else returns false.
	 * @param {*} value - The value to test.
	 * @returns {Boolean}
	 * @function
	 */
    isArray: Array.isArray ? Array.isArray : function (value) {
        return Object.prototype.toString.call(value) === '[object Array]';
    },

    /**
	 * Returns true if `value` is an object (excluding null), else returns false.
	 * @param {*} value - The value to test.
	 * @returns {Boolean}
	 * @since 2.7.0
	 */
    isObject: function (value) {
        return value !== null && Object.prototype.toString.call(value) === '[object Object]';
    },

    /**
	 * Returns `value` if defined, else returns `defaultValue`.
	 * @param {*} value - The value to return if defined.
	 * @param {*} defaultValue - The value to return if `value` is undefined.
	 * @returns {*}
	 */
    valueOrDefault: function (value, defaultValue) {
        return typeof value === 'undefined' ? defaultValue : value;
    },

    /**
	 * Returns value at the given `index` in array if defined, else returns `defaultValue`.
	 * @param {Array} value - The array to lookup for value at `index`.
	 * @param {Number} index - The index in `value` to lookup for value.
	 * @param {*} defaultValue - The value to return if `value[index]` is undefined.
	 * @returns {*}
	 */
    valueAtIndexOrDefault: function (value, index, defaultValue) {
        return helpers.valueOrDefault(helpers.isArray(value) ? value[index] : value, defaultValue);
    },

    /**
	 * Calls `fn` with the given `args` in the scope defined by `thisArg` and returns the
	 * value returned by `fn`. If `fn` is not a function, this method returns undefined.
	 * @param {Function} fn - The function to call.
	 * @param {Array|undefined|null} args - The arguments with which `fn` should be called.
	 * @param {Object} [thisArg] - The value of `this` provided for the call to `fn`.
	 * @returns {*}
	 */
    callback: function (fn, args, thisArg) {
        if (fn && typeof fn.call === 'function') {
            return fn.apply(thisArg, args);
        }
    },

    /**
	 * Note(SB) for performance sake, this method should only be used when loopable type
	 * is unknown or in none intensive code (not called often and small loopable). Else
	 * it's preferable to use a regular for() loop and save extra function calls.
	 * @param {Object|Array} loopable - The object or array to be iterated.
	 * @param {Function} fn - The function to call for each item.
	 * @param {Object} [thisArg] - The value of `this` provided for the call to `fn`.
	 * @param {Boolean} [reverse] - If true, iterates backward on the loopable.
	 */
    each: function (loopable, fn, thisArg, reverse) {
        var i, len, keys;
        if (helpers.isArray(loopable)) {
            len = loopable.length;
            if (reverse) {
                for (i = len - 1; i >= 0; i--) {
                    fn.call(thisArg, loopable[i], i);
                }
            } else {
                for (i = 0; i < len; i++) {
                    fn.call(thisArg, loopable[i], i);
                }
            }
        } else if (helpers.isObject(loopable)) {
            keys = Object.keys(loopable);
            len = keys.length;
            for (i = 0; i < len; i++) {
                fn.call(thisArg, loopable[keys[i]], keys[i]);
            }
        }
    },

    /**
	 * Returns true if the `a0` and `a1` arrays have the same content, else returns false.
	 * @see http://stackoverflow.com/a/14853974
	 * @param {Array} a0 - The array to compare
	 * @param {Array} a1 - The array to compare
	 * @returns {Boolean}
	 */
    arrayEquals: function (a0, a1) {
        var i, ilen, v0, v1;

        if (!a0 || !a1 || a0.length !== a1.length) {
            return false;
        }

        for (i = 0, ilen = a0.length; i < ilen; ++i) {
            v0 = a0[i];
            v1 = a1[i];

            if (v0 instanceof Array && v1 instanceof Array) {
                if (!helpers.arrayEquals(v0, v1)) {
                    return false;
                }
            } else if (v0 !== v1) {
                // NOTE: two different object instances will never be equal: {x:20} != {x:20}
                return false;
            }
        }

        return true;
    },

    /**
	 * Returns a deep copy of `source` without keeping references on objects and arrays.
	 * @param {*} source - The value to clone.
	 * @returns {*}
	 */
    clone: function (source) {
        if (helpers.isArray(source)) {
            return source.map(helpers.clone);
        }

        if (helpers.isObject(source)) {
            var target = {};
            var keys = Object.keys(source);
            var klen = keys.length;
            var k = 0;

            for (; k < klen; ++k) {
                target[keys[k]] = helpers.clone(source[keys[k]]);
            }

            return target;
        }

        return source;
    },

    /**
	 * The default merger when Chart.helpers.merge is called without merger option.
	 * Note(SB): this method is also used by configMerge and scaleMerge as fallback.
	 * @private
	 */
    _merger: function (key, target, source, options) {
        var tval = target[key];
        var sval = source[key];

        if (helpers.isObject(tval) && helpers.isObject(sval)) {
            helpers.merge(tval, sval, options);
        } else {
            target[key] = helpers.clone(sval);
        }
    },

    /**
	 * Merges source[key] in target[key] only if target[key] is undefined.
	 * @private
	 */
    _mergerIf: function (key, target, source) {
        var tval = target[key];
        var sval = source[key];

        if (helpers.isObject(tval) && helpers.isObject(sval)) {
            helpers.mergeIf(tval, sval);
        } else if (!target.hasOwnProperty(key)) {
            target[key] = helpers.clone(sval);
        }
    },

    /**
	 * Recursively deep copies `source` properties into `target` with the given `options`.
	 * IMPORTANT: `target` is not cloned and will be updated with `source` properties.
	 * @param {Object} target - The target object in which all sources are merged into.
	 * @param {Object|Array(Object)} source - Object(s) to merge into `target`.
	 * @param {Object} [options] - Merging options:
	 * @param {Function} [options.merger] - The merge method (key, target, source, options)
	 * @returns {Object} The `target` object.
	 */
    merge: function (target, source, options) {
        var sources = helpers.isArray(source) ? source : [source];
        var ilen = sources.length;
        var merge, i, keys, klen, k;

        if (!helpers.isObject(target)) {
            return target;
        }

        options = options || {};
        merge = options.merger || helpers._merger;

        for (i = 0; i < ilen; ++i) {
            source = sources[i];
            if (!helpers.isObject(source)) {
                continue;
            }

            keys = Object.keys(source);
            for (k = 0, klen = keys.length; k < klen; ++k) {
                merge(keys[k], target, source, options);
            }
        }

        return target;
    },

    /**
	 * Recursively deep copies `source` properties into `target` *only* if not defined in target.
	 * IMPORTANT: `target` is not cloned and will be updated with `source` properties.
	 * @param {Object} target - The target object in which all sources are merged into.
	 * @param {Object|Array(Object)} source - Object(s) to merge into `target`.
	 * @returns {Object} The `target` object.
	 */
    mergeIf: function (target, source) {
        return helpers.merge(target, source, { merger: helpers._mergerIf });
    },

    /**
	 * Applies the contents of two or more objects together into the first object.
	 * @param {Object} target - The target object in which all objects are merged into.
	 * @param {Object} arg1 - Object containing additional properties to merge in target.
	 * @param {Object} argN - Additional objects containing properties to merge in target.
	 * @returns {Object} The `target` object.
	 */
    extend: function (target) {
        var setFn = function (value, key) {
            target[key] = value;
        };
        for (var i = 1, ilen = arguments.length; i < ilen; ++i) {
            helpers.each(arguments[i], setFn);
        }
        return target;
    },

    /**
	 * Basic javascript inheritance based on the model created in Backbone.js
	 */
    inherits: function (extensions) {
        var me = this;
        var ChartElement = (extensions && extensions.hasOwnProperty('constructor')) ? extensions.constructor : function () {
            return me.apply(this, arguments);
        };

        var Surrogate = function () {
            this.constructor = ChartElement;
        };

        Surrogate.prototype = me.prototype;
        ChartElement.prototype = new Surrogate();
        ChartElement.extend = helpers.inherits;

        if (extensions) {
            helpers.extend(ChartElement.prototype, extensions);
        }

        ChartElement.__super__ = me.prototype;
        return ChartElement;
    }
};














var selectedExperiments = [];
var chartCD1;
var chartCD2;
var chartCD3;
var chartCD4;
var chartCD5;

var color;

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



var massSelection = 1;
var maxCycleIndex = 0;

function GetRandomColor() {
    opacity = 1;
    var randomColor = '#' + ('000000' + Math.floor(Math.random() * 16777215).toString(16)).slice(-6);
    return randomColor;
    //return 'rgba(' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ',' + Math.round(Math.random() * 255) + ',' + (opacity || '.3') + ')';
}
//function GetRandomHSLColor() {
//    color = "hsl(" + Math.random() * 360 + ", " + Math.random() * 360 + "%, 85%)";
//    return color;
//}

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

function ClearCharts() {
    chartCD1.data.datasets = [];
    chartCD1.update();
    chartCD2.data.datasets = [];
    chartCD2.update();
    chartCD3.data.datasets = [];
    chartCD3.update();
    chartCD4.data.datasets = [];
    chartCD4.update();
    chartCD5.data.datasets = [];
    chartCD5.update();
}
function ResetZoom(chartNumber) {

    switch (chartNumber) {
        case 1:
            chartCD1.resetZoom();
            break;
        case 2:
            chartCD2.resetZoom();
            break;
        case 3:
            chartCD3.resetZoom();
            break;
        case 4:
            chartCD4.resetZoom();
            break;
        case 5:
            chartCD5.resetZoom();
            break;
    }
}
function ShowPlots() {
    //var experimentId = 10;
    //var massSelection = 1;

    //console.log("calling show plots");

    //var massCalculations = GetExperimentCalculations(experimentId);
    //var testData = GetChargeDischargeTestDataForExperiment(experimentId);

    //chartCD1.clear();

    ClearCharts();

    $.each(selectedExperiments, function (index, experimentId) {
        //console.log(experimentId);
        //console.log('eid: ' + experimentId);
        //console.log('eid: ' + parseInt(experimentId));
        GetChargeDischargeTestDataForExperiment(parseInt(experimentId));
    });

    //console.log(testData);
}

function GetExperimentCalculations(experimentId) {

}

function GetChargeDischargeTestDataForExperiment(experimentId) {
    var RequestDataString = JSON.stringify({ experimentId: experimentId });

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetChargeDischargeTestDataForGraphs",
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

                //console.log("before call");                
                MakeCharts(experimentId, jsonResult.response);

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



function MakeCharts(experimentId, response) {
    var selectedMassValue = response.massCalculations.mass1;

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
    }

    color = GetRandomColor();

    MakeChartCD1(experimentId, response, selectedMassValue, chartCD1);
    MakeChartCD2(experimentId, response, selectedMassValue, chartCD2);
    MakeChartCD3(experimentId, response, selectedMassValue, chartCD3);
    MakeChartCD4(experimentId, response, selectedMassValue, chartCD4);
    MakeChartCD5(experimentId, response, selectedMassValue, chartCD5);

    AddChartCD3Control();
}
function AddChartCD3Control() {
    var html = "";
    html += '<div class="form-group">';
    html += '<div class="col-sm-12">';
    html += '<select id="selectCycleIndex" class="" name="cycleIndexes[]" multiple="multiple">';

    //maxCycleIndex
    for (i = 1; i <= maxCycleIndex; i++) {
        if (i > 3)
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
    });

    $('#selectCycleIndex').on('change', function () {

        //console.log('selected cycle indexes: ' + $(this).val());
        //selectedExperiments = $(this).val();
        //UpdateChartData();
        var selectedIndexes = $(this).val();
        //console.log(selectedIndexes);

        var datasets = chartCD3.data.datasets;
        $.each(datasets, function (id, dataset) {
            //myChart.data.datasets.push(dataset);
            //console.log(dataset.cycleIndex);

            //console.log($.inArray(dataset.cycleIndex.toString(), selectedIndexes) != -1);
            
            if ($.inArray(dataset.cycleIndex.toString(), selectedIndexes) != -1)
                dataset.hidden = false;
            else
                dataset.hidden = true;
            //console.log('value : ' + dataset);
        });
        chartCD3.update();



    });
}


function MakeChartCD1(experimentId, response, selectedMassValue, myChart) {

    var localData = GetDataForChartCD1(experimentId, response);

    //console.log("mkchart1");

    //var localData = {
    //    label: 'exp_003',
    //    backgroundColor: '#074F9F',
    //    borderColor: '#074F9F',
    //    data: [{
    //        x: 3,
    //        y: 9
    //    }, {
    //        x: 2,
    //        y: 7
    //    },
    //    {
    //        x: 3,
    //        y: 1
    //    }
    //    ]
    //};

    //myChart.data.datasets[0].data = localData;
    myChart.data.datasets.push(localData);
    //console.log(myChart.data.datasets);
    //myChart.config.centerText.text = localDataBefDl.reduce((a, b) => a + b, 0);
    myChart.update();



    //    $.ajax({
    //        type: "POST",
    //        url: "/Helpers/WebMethods.asmx/GetChartCD1",
    //    async: true,
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    data: '{mun: "' + mun + '"}',
    //    success: function (result) {
    //        var jsonResult = JSON.parse(result.d);
    //        var localData = [];

    //        var item = itemExists(jsonResult, 'Batelco');
    //        if (item != null) {
    //            localData.push(item.Count);
    //        } else {
    //            localData.push(0);
    //        }


    //    localData ={
    //        label: 'exp_003',
    //        backgroundColor: '#074F9F',
    //        borderColor: '#074F9F',
    //        data: [{
    //            x: 3,
    //            y: 9
    //        }, {
    //            x: 2,
    //            y: 7
    //        },
    //        {
    //            x: 3,
    //            y: 1
    //        }
    //        ]
    //    };

    //        myChart.data.datasets[0].data = localData;
    //        //myChart.config.centerText.text = localDataBefDl.reduce((a, b) => a + b, 0);
    //        myChart.update();
    //    },
    //    error: function (p1, p2, p3) {
    //        alert(p1.status);
    //        alert(p3);
    //    }
    //});
}
function MakeChartCD2(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD2(experimentId, response, selectedMassValue);
    myChart.data.datasets.push(localData);
    myChart.update();
}
function MakeChartCD3(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD3(experimentId, response, selectedMassValue);

    $.each(localData, function (id, value) {
        myChart.data.datasets.push(value);

    });
    //console.log('local data: ' + localData);

    //myChart.data.datasets.push(localData);
    //console.log(myChart.data.datasets);
    myChart.update();
}

function MakeChartCD4(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD4(experimentId, response, selectedMassValue);

    $.each(localData, function (id, value) {
        myChart.data.datasets.push(value);
        //console.log('value : ' + value);

    });
    //console.log('local data: ' + localData);

    //myChart.data.datasets.push(localData);
    //console.log(myChart.data.datasets);
    myChart.update();
}

function MakeChartCD5(experimentId, response, selectedMassValue, myChart) {
    var localData = GetDataForChartCD5(experimentId, response, selectedMassValue);

    $.each(localData, function (id, value) {
        myChart.data.datasets.push(value);

    });
    //console.log('local data: ' + localData);

    //myChart.data.datasets.push(localData);
    //console.log(myChart.data.datasets);
    //var legend = myChart.generateLegend();
    //$("#chartCD5Legend").html(legend);
    
    //myChart.canvas.before.html(legend);
    myChart.update();
}


function GetDataForChartCD1(experimentId, response) {
    var data = response.testData;

    var dataArray = [];
    //dataArray.push({
    //    x: 3,
    //    y: 9
    //});
    //dataArray.push({
    //    x: 3,
    //    y: 9
    //});

    //dataArray.push(newRequestObj);

    //console.log(dataArray);

    $.each(data, function (id, value) {

        //console.log('value : ' + value.voltage);
        //testTime
        var obj = new Object();
        obj.x = value.testTime / 3600;
        obj.y = value.voltage;
        obj.cycleIndex = value.cycleIndex;

        dataArray.push(obj);
    });


    //console.log('aaa : ' + data);
    //var color = GetRandomColor();

    var result = {
        label: experimentId,
        //backgroundColor: '#074F9F',
        //borderColor: '#074F9F',
        backgroundColor: color,
        borderColor: color,
        //borderColor: [
        //                    '#00b0f0',
        //                    '#808080',
        //                    '#ffff00',
        //                    '#ff0000',
        //                    '#d883ff',
        //                    '#00b050',
        //                    '#c86e0a'
        //],
        data: dataArray,
        experimentId: experimentId,
    };


    return result;
}

function GetDataForChartCD2(experimentId, response, selectedMassValue) {
    var data = response.testData;

    var dataArray = [];
    //var massValue = response.massCalculations.mass1;

    //switch mass selection napuni masa so koja ke se deli
    //console.log("mass selection: "+ massSelection);
    //switch (massSelection) {
    //    case 1:
    //        massValue = response.massCalculations.mass1;
    //        break;
    //    case 2:
    //        massValue = response.massCalculations.mass2;
    //        break;
    //    case 3:
    //        massValue = response.massCalculations.mass3;
    //        break;
    //    case 4:
    //        massValue = response.massCalculations.mass4;
    //        break;
    //    case 5:
    //        massValue = response.massCalculations.mass5;
    //        break;
    //    case 6:
    //        massValue = response.massCalculations.mass6;
    //        break;
    //}

    console.log("massSelection: " + massSelection);
    console.log("selected mass value: " + selectedMassValue);

    $.each(data, function (id, value) {
        var obj = new Object();
        obj.x = value.testTime / 3600;
        obj.y = value.current / selectedMassValue;
        obj.cycleIndex = value.cycleIndex;
        //console.log("object ok: " + JSON.stringify(obj));
        dataArray.push(obj);
    });

    //var color = GetRandomColor();
    var result = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        data: dataArray,
        experimentId: experimentId
    };

    return result;
}

function GetDataForChartCD3(experimentId, response, selectedMassValue) {
    var data = response.testData;
    //console.log('data ' + JSON.stringify(data));

    var result = [];

    var dataArrayD = [];
    var dataArrayC = [];

    //console.log("selected mass value: " + selectedMassValue);

    var length = data.length;

    //first cycle index
    var previousCycleIndex = data[0].cycleIndex;
    //console.log('len : ' + data.length);

    //var color = GetRandomColor();
    var color = GetRandomLightColor();
    var numOfCycleIndexes = data[length - 1].cycleIndex;
    var colorStep = -1 / numOfCycleIndexes;

    if (maxCycleIndex < numOfCycleIndexes)
        maxCycleIndex = numOfCycleIndexes;

    $.each(data, function (id, value) {
        var objD = new Object();
        var objC = new Object();

        objD.x = value.dischargeCapacity / selectedMassValue * 1000;
        objD.y = value.voltage;
        objD.cycleIndex = value.cycleIndex;

        objC.x = value.chargeCapacity / selectedMassValue * 1000;
        objC.y = value.voltage;
        objC.cycleIndex = value.cycleIndex;
        //obj.index = value.cycleIndex;

        if (value.cycleIndex == previousCycleIndex) {
            if (value.current < 0)
                dataArrayD.push(objD);
            else
                dataArrayC.push(objC);
            //console.log(previousCycleIndex);
        }
        else {
            if (dataArrayD.length != 0) {
                //result.push(dataArray);
                var lineD = {
                    label: experimentId,
                    //backgroundColor: chartCD3ColorArray[value.cycleIndex - 1],
                    backgroundColor: color,
                    borderColor: color,
                    borderWidth: 1,
                    data: dataArrayD,
                    experimentId: experimentId,
                    cycleIndex: previousCycleIndex,
                    //hidden: true,
                };
                var lineC = {
                    label: experimentId,
                    backgroundColor: color,
                    borderColor: color,
                    borderWidth: 1,
                    data: dataArrayC,
                    experimentId: experimentId,
                    cycleIndex: previousCycleIndex,
                };
                if (value.cycleIndex > 3) {
                    lineD.hidden = true;
                    lineC.hidden = true;
                }
                

                result.push(lineD);
                result.push(lineC);
                color = ColorLuminance(color, colorStep);

                dataArrayD = [];
                dataArrayC = [];
                if (value.current < 0)
                    dataArrayD.push(objD);
                else
                    dataArrayC.push(objC);
            }
        }
        if (id == length - 1) {

            var lineD = {
                label: experimentId,
                backgroundColor: color,
                borderColor: color,
                borderWidth: 1,
                data: dataArrayD,
                experimentId: experimentId,
                cycleIndex: previousCycleIndex,
            };
            var lineC = {
                label: experimentId,
                backgroundColor: color,
                borderColor: color,
                borderWidth: 1,
                data: dataArrayC,
                experimentId: experimentId,
                cycleIndex: previousCycleIndex,
            };
            if (value.cycleIndex > 3) {
                lineD.hidden = true;
                lineC.hidden = true;
            }

            result.push(lineD);
            result.push(lineC);

            color = ColorLuminance(color, colorStep);
            //if (value.cycleIndex == previousCycleIndex) {
            //    result.push(dataArray);
            //}
            //else {
            //    result.push(dataArray);
            //}

        }

        //console.log('resulttttt: ' + JSON.stringify(result));
        //console.log('resulttttt: ' + result.length);
        previousCycleIndex = value.cycleIndex;


    });

    //console.log('resulttttt: ' + JSON.stringify(result));


    //var line1 = {
    //    label: experimentId,
    //    backgroundColor: '#00bfff',
    //    borderColor: '#00bfff',
    //    data: dataArray1
    //};
    //var line2 = {
    //    label: experimentId,
    //    backgroundColor: '#000000',
    //    borderColor: '#000000',
    //    data: dataArray2
    //};
    //var line3 = {
    //    label: experimentId,
    //    backgroundColor: '#000000',
    //    borderColor: '#000000',
    //    borderDash: [5, 5],
    //    pointStyle: 'triangle',
    //    fill: false,
    //    pointRadius: 5,
    //    pointHoverRadius: 7,
    //    data: dataArray3
    //};
    //var line4 = {
    //    label: experimentId,
    //    backgroundColor: '#ff0000',
    //    borderColor: '#ff0000',
    //    data: dataArray4
    //};
    //var line5 = {
    //    label: experimentId,
    //    backgroundColor: '#ff0000',
    //    borderColor: '#ff0000',
    //    borderDash: [5, 5],
    //    pointStyle: 'triangle',
    //    fill: false,
    //    pointRadius: 5,
    //    pointHoverRadius: 7,
    //    data: dataArray5
    //};

    //result.push(line1);
    //result.push(line2);
    //result.push(line3);
    //result.push(line4);
    //result.push(line5);

    return result;
}

function GetDataForChartCD4(experimentId, response, selectedMassValue) {
    var data = response.capacityData;
    //console.log('data ' + JSON.stringify(data));
    var dataArrayEfc = [];
    var dataArrayEfe = [];

    //console.log("selected mass value: " + selectedMassValue);

    //var color = GetRandomColor();
    $.each(data, function (id, value) {
        var objEfc = new Object();


        var efficiency1 = (value.maxDischargeCapacity / selectedMassValue) / (value.maxChargeCapacity / selectedMassValue);
        var efficiency2 = (value.maxDischargeEnergy / selectedMassValue) / (value.maxChargeEnergy / selectedMassValue);

        //console.log('ef1 ' + efficiency1);

        objEfc.x = value.cycleIndex;
        objEfc.y = efficiency1;
        dataArrayEfc.push(objEfc);

        //console.log("object: " + JSON.stringify(objEfc.x));

        var objEfe = new Object();
        objEfe.x = value.cycleIndex;
        objEfe.y = efficiency2;
        dataArrayEfe.push(objEfe);
    });

    //console.log('data array 1 ' + JSON.stringify(dataArrayEfc));
    //console.log('data array 2 ' + JSON.stringify(dataArrayEfe));
    //console.log(dataArrayEf2);

    //#00bfff

    var line1 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        data: dataArrayEfc
    };
    var line2 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        pointStyle: 'triangle',
        pointRadius: 4,
        pointHoverRadius: 6,
        data: dataArrayEfe
    };
    var result = [];
    result.push(line1);
    result.push(line2);

    return result;
}

function GetDataForChartCD5(experimentId, response, selectedMassValue) {
    var data = response.capacityData;
    //console.log('data ' + JSON.stringify(data));
    var dataArray1 = [];
    var dataArray2 = [];
    var dataArray3 = [];
    var dataArray4 = [];
    var dataArray5 = [];

    //console.log("selected mass value: " + selectedMassValue);

    //var color = GetRandomColor();
    $.each(data, function (id, value) {
        var obj1 = new Object();
        var obj2 = new Object();
        var obj3 = new Object();
        var obj4 = new Object();
        var obj5 = new Object();

        obj1.x = value.cycleIndex;
        obj1.y = value.maxChargeCapacity / response.massCalculations.mass1;
        dataArray1.push(obj1);

        obj2.x = value.cycleIndex;
        obj2.y = value.maxDischargeCapacity / response.massCalculations.mass1;
        dataArray2.push(obj2);

        obj3.x = value.cycleIndex;
        obj3.y = value.maxDischargeCapacity / response.massCalculations.mass2;
        dataArray3.push(obj3);

        obj4.x = value.cycleIndex;
        obj4.y = value.maxDischargeCapacity / response.massCalculations.mass3;
        dataArray4.push(obj4);

        obj5.x = value.cycleIndex;
        obj5.y = value.maxDischargeCapacity / response.massCalculations.mass4;
        dataArray5.push(obj5);

    });
    //#00bfff
    //#000000
    //#ff0000

    var line1 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        data: dataArray1,
        experimentId: experimentId
    };
    var line2 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        data: dataArray2,
        experimentId: experimentId
    };
    var line3 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        borderDash: [5, 5],
        pointStyle: color,
        fill: false,
        pointRadius: 3,
        pointHoverRadius: 6,
        data: dataArray3,
        experimentId: experimentId
    };
    var line4 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        data: dataArray4,
        experimentId: experimentId
    };
    var line5 = {
        label: experimentId,
        backgroundColor: color,
        borderColor: color,
        borderDash: [5, 5],
        pointStyle: 'triangle',
        fill: false,
        pointRadius: 4,
        pointHoverRadius: 6,
        data: dataArray5,
        experimentId: experimentId
    };
    var result = [];
    result.push(line1);
    result.push(line2);
    result.push(line3);
    result.push(line4);
    result.push(line5);

    return result;
}




$(function () {
    //Chart.defaults.global.legendCallback = function (chart) {
    //    var text = [];
    //    text.push('<ul class="' + chart.id + '-legend">');
    //    var prevExpId = 0;
    //    for (var i = 0; i < chart.data.datasets.length; i++) {
    //        if (chart.data.datasets[i].experimentId != prevExpId) {
    //            text.push('<li><span style="background-color:' + chart.data.datasets[i].backgroundColor + '"></span>');
    //            if (chart.data.datasets[i].label) {
    //                text.push(chart.data.datasets[i].label + 'aman');
    //            }
    //            text.push('</li>');
    //        }
    //        prevExpId = chart.data.datasets[i].experimentId;
    //    }
    //    text.push('</ul>');
    //    return text.join('');
    //};

    //chartCD1
    var chartCD1Data = {
        datasets: [
        //    {
        //    //type: 'scatter',
        //    //showLine: true,
        //    label: 'exp_001',
        //    backgroundColor: '#C00000',
        //    borderColor: '#C00000',
        //    data: [
        //        {
        //            x: 10,
        //            y: 20
        //        }, {
        //            x: 15,
        //            y: 10
        //        },
        //    {
        //        x: 18,
        //        y: 24
        //    }
        //    ]
        //},
        //{
        //    type: 'line',
        //    label: 'exp_002',
        //    backgroundColor: '#07CF9F',
        //    borderColor: '#07CF9F',
        //    data: [{
        //        x: 13,
        //        y: 19
        //    }, {
        //        x: 22,
        //        y: 17
        //    },
        //    {
        //        x: 23,
        //        y: 1
        //    }
        //    ]
        //}
        ],
    };

    var chartCD1Options = {
        title: {
            display: false
        },
        legend: {
            display: true
        },
        tooltips: {
            //mode: 'point',
            intersect: false,
            callbacks: {
                title: function (item, data) {
                    // Pick first xLabel for now
                    //var title = '';
                    //console.log(data.datasets[item[0].datasetIndex]);
                    //title = data.datasets[item[0].datasetIndex].label || '';

                    //if (item.length > 0) {
                    //    if (item[0].yLabel) {
                    //        title += item[0].yLabel;
                    //    } else if (data.labels.length > 0 && item[0].index < data.labels.length) {
                    //        title += data.labels[item[0].index];
                    //    }
                    //}

                    //return title;
                },

                label: function (item, data) {
                    //var datasetLabel = data.datasets[item.datasetIndex].label || '';
                    //return datasetLabel + ':aa ' + item.xLabel;
                    var datasetLabel = data.datasets[item.datasetIndex].label;

                    //var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    //var tooltip = '';
                    //var x = 'x: ' + item.xLabel;
                    //var y = 'y: ' + item.yLabel;

                    return datasetLabel;
                },
                afterBody: function (tooltipItems, data) {
                    //var dataPoint = data.datasets[tooltipItems[0].datasetIndex].data[tooltipItems.index];
                    //console.log(dataPoint);

                    //var multistringText = [];

                    //var x = 'x: ' + data.datasets[tooltipItems[0].datasetIndex].xLabel;
                    //var y = 'y: ' + data.datasets[tooltipItems[0].datasetIndex].yLabel;
                    //var ci = 'Cycle Index: ' + data.datasets[tooltipItems[0].datasetIndex].cycleIndex;

                    //multistringText.push(x);
                    //multistringText.push(y);
                    //multistringText.push(ci);

                    //return multistringText;
                },

                afterLabel: function (item, data) {
                    var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    //console.log(dataPoint);
                    var multistringText = [];

                    var x = 'x: ' + dataPoint.x;
                    var y = 'y: ' + dataPoint.y;
                    var ci = 'Cycle Index: ' + dataPoint.cycleIndex;

                    multistringText.push(x);
                    multistringText.push(y);
                    multistringText.push(ci);

                    return multistringText;
                }
            }
        },
        responsive: true,
        showLines: true,
        scales: {
            xAxes: [{
                ticks: {
                    suggestedMin: 0,
                    suggestedMax: 100,
                    stepSize: 20
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Test Time [hours]'
                },
                type: 'linear',
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Voltage'
                }
            }]
        },
        elements: {
            line: {
                fill: false,
                borderWidth: 1,
                //tension: 0, // disables bezier curves

                //borderColor: [
                //            '#00b0f0',
                //            '#808080',
                //            '#ffff00',
                //            '#ff0000',
                //            '#d883ff',
                //            '#00b050',
                //            '#c86e0a'
                //],
                //borderColor: '#074F9F',
            },
            point: {
                radius: 1,
                hoverRadius: 3,
            }
        },
        animation: false,
        pan: {
            enabled: true,
            mode: 'xy'
        },
        zoom: {
            enabled: true,
            mode: 'xy',
            limits: {
                max: 10,
                min: 0.5
            }
        }

    };

    var chartCD1El = document.getElementById("chartCD1").getContext("2d");
    chartCD1 = new Chart(chartCD1El, {
        type: 'scatter',
        data: chartCD1Data,
        options: chartCD1Options
    });




    //chartCD2
    var chartCD2Data = {
        datasets: [
        //    {
        //    label: 'exp_001',
        //    backgroundColor: '#C00000',
        //    borderColor: '#C00000',            
        //    data: [
        //        {
        //            x: 10,
        //            y: 20
        //        }, {
        //            x: 15,
        //            y: 10
        //        },
        //    {
        //        x: 18,
        //        y: 24
        //    }
        //    ]
        //},
        //{
        //    label: 'exp_002',
        //    backgroundColor: '#07CF9F',
        //    borderColor: '#07CF9F',
        //    //pointBorderWidth: 1,
        //    data: [{
        //        x: 13,
        //        y: 19
        //    }, {
        //        x: 22,
        //        y: 17
        //    },
        //    {
        //        x: 23,
        //        y: 1
        //    }
        //    ]
        //}
        ],
    };
    var chartCD2Options = {
        title: {
            display: false
        },
        legend: {
            display: true
        },
        tooltips: {
            //mode: 'point',
            intersect: true,
            callbacks: {
                title: function (item, data) {
                },
                label: function (item, data) {
                    var datasetLabel = data.datasets[item.datasetIndex].label;
                    return datasetLabel;
                },
                afterLabel: function (item, data) {
                    var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    var multistringText = [];

                    var x = 'x: ' + dataPoint.x;
                    var y = 'y: ' + dataPoint.y;
                    var ci = 'Cycle Index: ' + dataPoint.cycleIndex;

                    multistringText.push(x);
                    multistringText.push(y);
                    multistringText.push(ci);

                    return multistringText;
                }
            }
        },
        responsive: true,
        showLines: true,
        scales: {
            xAxes: [{
                ticks: {
                    suggestedMin: 0,
                    suggestedMax: 100,
                    stepSize: 20
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Test Time [hours]'
                },
                type: 'linear',
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Current'
                }
            }]
        },
        elements: {
            line: {
                fill: false,
                borderWidth: 1,
                //pointBorderWidth: 1,
                tension: 0, // disables bezier curves

                //borderColor: [
                //            '#00b0f0',
                //            '#808080',
                //            '#ffff00',
                //            '#ff0000',
                //            '#d883ff',
                //            '#00b050',
                //            '#c86e0a'
                //],
                //borderColor: '#074F9F',
            },
            point: {
                radius: 1,
                hoverRadius: 3,
            }
        },
        animation: false,
        pan: {
            enabled: true,
            mode: 'xy'
        },
        zoom: {
            enabled: true,
            mode: 'xy',
            //limits: {
            //    max: 10,
            //    min: 0.5
            //}
        }

    };

    var chartCD2El = document.getElementById("chartCD2").getContext("2d");
    chartCD2 = new Chart(chartCD2El, {
        type: 'scatter',
        data: chartCD2Data,
        options: chartCD2Options,
    });

    //chartCD3
    var chartCD3Data = {
        datasets: [
        ],
    };
    var chartCD3Options = {
        title: {
            display: false
        },
        legend: {
            display: true
        },
        tooltips: {
            //mode: 'point',
            intersect: false,
            callbacks: {
                title: function (item, data) {
                },
                label: function (item, data) {
                    var datasetLabel = data.datasets[item.datasetIndex].label;
                    return datasetLabel;
                },
                afterLabel: function (item, data) {
                    var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    var multistringText = [];

                    var x = 'x: ' + dataPoint.x;
                    var y = 'y: ' + dataPoint.y;
                    var ci = 'Cycle Index: ' + dataPoint.cycleIndex;

                    multistringText.push(x);
                    multistringText.push(y);
                    multistringText.push(ci);

                    return multistringText;
                }
            }
        },
        responsive: true,
        showLines: true,
        scales: {
            xAxes: [{
                ticks: {
                    //suggestedMin: 0,
                    //suggestedMax: 15,
                    //stepSize: 5
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Discharge Capacity'
                },
                type: 'linear',
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Voltage (V)'
                }
            }]
        },
        elements: {
            line: {
                fill: false,
                borderWidth: 2,
                pointBorderWidth: 1,
                tension: 0,
            },
            point: {
                radius: 0
            }

        },
        animation: false,
        pan: {
            enabled: true,
            mode: 'xy'
        },
        zoom: {
            enabled: true,
            mode: 'xy',
            //limits: {
            //    max: 10,
            //    min: 0.5
            //}
        }

    };

    var chartCD3El = document.getElementById("chartCD3").getContext("2d");
    chartCD3 = new Chart(chartCD3El, {
        type: 'scatter',
        data: chartCD3Data,
        options: chartCD3Options,
    });

    //chartCD4
    var chartCD4Data = {
        datasets: [
        ],
    };
    var chartCD4Options = {
        title: {
            display: false
        },
        legend: {
            display: true
        },
        tooltips: {
            //mode: 'point',
            intersect: true,
            callbacks: {
                title: function (item, data) {
                },
                label: function (item, data) {
                    var datasetLabel = data.datasets[item.datasetIndex].label;
                    return datasetLabel;
                },
                afterLabel: function (item, data) {
                    var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    var multistringText = [];

                    var x = 'x: ' + dataPoint.x;
                    var y = 'y: ' + dataPoint.y;

                    multistringText.push(x);
                    multistringText.push(y);

                    return multistringText;
                }
            }
        },
        responsive: true,
        showLines: true,
        scales: {
            xAxes: [{
                ticks: {
                    suggestedMin: 0,
                    suggestedMax: 15,
                    stepSize: 5
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Cycles'
                },
                type: 'linear',
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Efficiency'
                }
            }]
        },
        elements: {
            line: {
                fill: false,
                borderWidth: 2,
                pointBorderWidth: 1,
                tension: 0,
            }
        },
        animation: false,
        pan: {
            enabled: true,
            mode: 'xy'
        },
        zoom: {
            enabled: true,
            mode: 'xy',
            limits: {
                max: 10,
                min: 0.5
            }
        }

    };

    var chartCD4El = document.getElementById("chartCD4").getContext("2d");
    chartCD4 = new Chart(chartCD4El, {
        type: 'scatter',
        data: chartCD4Data,
        options: chartCD4Options,
    });


    var defaultLegendClickHandler = Chart.defaults.global.legend.onClick;
    var newLegendClickHandler = function (e, legendItem) {
        var index = legendItem.datasetIndex;

        if (index > 1) {
            // Do the original logic
            defaultLegendClickHandler(e, legendItem);
        } else {
            let ci = this.chart;
            [ci.getDatasetMeta(0),
             ci.getDatasetMeta(1)].forEach(function (meta) {
                 meta.hidden = meta.hidden === null ? !ci.data.datasets[index].hidden : null;
             });
            ci.update();
        }
    };

    function customGenerateLegend(chart) {
        console.log(chart);
    }

    //chartCD5
    var chartCD5Data = {
        datasets: [
        ],
    };
    var chartCD5Options = {
        title: {
            display: false
        },
        //legend: false,
        legend: {
            display: true,
            //onClick: newLegendClickHandler,

            labels: {
                //generateLabels: function (chart) {
                //    var data = chart.data;
                //    var prevExpId = 0;
                //    return helpers.isArray(data.datasets) ? data.datasets.map(function (dataset, i) {
                //        if (dataset.experimentId != prevExpId) {
                //            prevExpId = dataset.experimentId;
                //            return {
                //                text: dataset.label + 'aaaaa',
                //                fillStyle: (!helpers.isArray(dataset.backgroundColor) ? dataset.backgroundColor : dataset.backgroundColor[0]),
                //                hidden: !chart.isDatasetVisible(i),
                //                lineCap: dataset.borderCapStyle,
                //                lineDash: dataset.borderDash,
                //                lineDashOffset: dataset.borderDashOffset,
                //                lineJoin: dataset.borderJoinStyle,
                //                lineWidth: dataset.borderWidth,
                //                strokeStyle: dataset.borderColor,
                //                pointStyle: dataset.pointStyle,

                //                // Below is extra data used for toggling the datasets
                //                datasetIndex: i
                //            };                            
                //        }
                //        prevExpId = dataset.experimentId;
                //    }, this) : [];
                //}
            }
        },
        tooltips: {
            //mode: 'point',
            intersect: true,
            callbacks: {
                title: function (item, data) {
                },
                label: function (item, data) {
                    var datasetLabel = data.datasets[item.datasetIndex].label;
                    return datasetLabel;
                },
                afterLabel: function (item, data) {
                    var dataPoint = data.datasets[item.datasetIndex].data[item.index];
                    var multistringText = [];

                    var x = 'x: ' + dataPoint.x;
                    var y = 'y: ' + dataPoint.y;

                    multistringText.push(x);
                    multistringText.push(y);

                    return multistringText;
                }
            }
        },
        responsive: true,
        showLines: true,
        scales: {
            xAxes: [{
                ticks: {
                    suggestedMin: 0,
                    suggestedMax: 15,
                    stepSize: 5
                },
                scaleLabel: {
                    display: true,
                    labelString: 'Cycles'
                },
                type: 'linear',
            }],
            yAxes: [{
                scaleLabel: {
                    display: true,
                    labelString: 'Capacity'
                }
            }]
        },
        elements: {
            line: {
                fill: false,
                borderWidth: 2,
                //pointBorderWidth: 1,
                tension: 0,
            }
        },
        animation: false,
        pan: {
            enabled: true,
            mode: 'xy'
        },
        zoom: {
            enabled: true,
            mode: 'xy',
            limits: {
                max: 10,
                min: 0.5
            }
        },
        legendCallback: function (chart) {
            var text = [];
            text.push('<ul class="' + chart.id + '-legend">');
            for (var i = 0; i < chart.data.datasets.length; i++) {
                text.push('<li><span style="background-color:' + chart.data.datasets[i].backgroundColor + '"></span>');
                if (chart.data.datasets[i].label) {
                    text.push(chart.data.datasets[i].label);
                }
                text.push('</li>');
            }
            text.push('</ul>');
            return text.join('');
        },        
    };

    var chartCD5El = document.getElementById("chartCD5").getContext("2d");
    chartCD5 = new Chart(chartCD5El, {
        type: 'scatter',
        data: chartCD5Data,
        options: chartCD5Options,
    });


    //MakeChartCD1(1, chartCD1);


});

