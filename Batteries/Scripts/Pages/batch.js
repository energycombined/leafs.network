var formsFolderURL;
var alpacaForm;
var dialog;

var viewMode = false;
var sharedViewMode = false;

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

function MakeTablesSortable(id) {
    //console.log('make sortable first time for ' + id);
    sortable('#' + id + ' .js-sortable-table', {
        items: "tr.js-sortable-tr",
        handle: '.js-handle',
        placeholder: "<tr><td colspan=\"3\"><span class=\"center\">Place here</span></td></tr>",
        forcePlaceholderSize: false,
        //draggingClass: "sortable-dragging",
    });
}

function CreateBatchJsonRequestObject() {
    var jsonRequestObject = {};

    var batchInfoObject = CreateBatchGeneralDataJsonRequestObject();
    var batchContentObject = CreateBatchContentJsonRequestObject();

    jsonRequestObject["batchInfo"] = batchInfoObject.batchInfo;
    jsonRequestObject["batchContent"] = batchContentObject.batchContent;
    jsonRequestObject["batchProcesses"] = batchContentObject.batchProcesses;

    jsonRequestObject["measurements"] = batchContentObject.measurements;

    return jsonRequestObject;
}

function CreateBatchGeneralDataJsonRequestObject() {
    var jsonRequestObject = {};

    var batchInfo = {};
    var batchPersonalLabel = $('#batchPersonalLabel').val();
    var batchDescription = $('#batchDescription').val();
    var batchChemicalFormula = $('#batchChemicalFormula').val();
    var batchOutput = $('#batchOutput').val();
    var totalBatchOutput = $('#totalBatchOutput').val();
    var wasteAmount = $('#wasteAmount').val();
    var releasedAs = $('#releasedAs').val();
    var wasteChemicalComposition = $('#wasteChemicalComposition').val();
    var wasteComment = $('#wasteComment').val();

    var measurementUnit = $('#selectMeasurementUnit').val();
    var materialType = $('#selectMaterialType').val();
    batchInfo["batchPersonalLabel"] = batchPersonalLabel;
    batchInfo["description"] = batchDescription;
    batchInfo["chemicalFormula"] = batchChemicalFormula;
    batchInfo["totalBatchOutput"] = totalBatchOutput;
    batchInfo["batchOutput"] = batchOutput;
    batchInfo["wasteAmount"] = wasteAmount;
    batchInfo["releasedAs"] = releasedAs;
    batchInfo["wasteChemicalComposition"] = wasteChemicalComposition;
    batchInfo["wasteComment"] = wasteComment;
    batchInfo["fkMeasurementUnit"] = measurementUnit;
    batchInfo["fkMaterialType"] = materialType;

    var batchContentList = [];
    var batchProcessList = [];

    jsonRequestObject["batchInfo"] = batchInfo;
    jsonRequestObject["batchContent"] = batchContentList;
    jsonRequestObject["batchProcesses"] = batchProcessList;
    //console.log(jsonObject);
    //jsonString = JSON.stringify(jsonRequestObject);


    //var measurements = $("#Batch .addMeasurementsBatchLevelBtn").data("item");
    //var measurementsItem = measurements.item;
    ////console.log("batch level measurements: " + JSON.stringify(measurementsItem));
    //jsonRequestObject["measurements"] = measurementsItem;

    return jsonRequestObject;
}

function CreateBatchContentJsonRequestObject() {
    var jsonRequestObject = {};

    var batchInfo = {};
    var batchContentList = [];
    var batchProcessList = [];

    var step = 1;

    $("#materialsAndBatchesTable tbody tr.batchContentRow").each(function (id) {
        //var quantity = parseFloat($(this).find("input[name='quantity']").val());
        var quantity = $(this).find("input[name='quantity']").val().replace(',', '.');

        var row = $(this).data("item");
        var itemType = row.type;
        var item = row.item;

        var measurements = $(this).find(".addMeasurementsBatchContentLevelBtn").data("item");
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
        batchContent["orderInStep"] = id + 1;

        batchContent["measurements"] = measurementsItem;

        batchContentList.push(batchContent);
    });

    $("#processesTable tbody tr.batchProcessRow").each(function (id) {
        var row = $(this).data("item");
        var itemType = row.type;
        var item = row.item;

        var processRequest = {};
        var batchProcess = {};
        batchProcess["step"] = step;
        batchProcess["fkProcessType"] = row.data.fkProcessType;
        batchProcess["processType"] = row.data.processType;
        batchProcess["fkEquipment"] = row.data.fkEquipment;
        batchProcess["equipmentModelId"] = row.data.equipmentModelId;
        batchProcess["label"] = row.label;
        batchProcess["processOrderInStep"] = id + 1;
        processRequest["batchProcess"] = batchProcess;
        processRequest["processAttributes"] = item;

        var equipmentSettings = $(this).find(".editSelectedProcessButton").data("item");
        var equipmentSettingsItem = equipmentSettings.item;
        processRequest["equipmentSettings"] = equipmentSettingsItem;

        batchProcessList.push(processRequest);
    });

    jsonRequestObject["batchInfo"] = batchInfo;
    jsonRequestObject["batchContent"] = batchContentList;
    jsonRequestObject["batchProcesses"] = batchProcessList;

    var measurements = $("#Batch .addMeasurementsBatchLevelBtn").data("item");
    var measurementsItem = measurements.item;

    jsonRequestObject["measurements"] = measurementsItem;

    return jsonRequestObject;
}

function SubmitBatchGeneralData(batchId) {
    jsonRequestObject = CreateBatchGeneralDataJsonRequestObject();
    //if (setComplete == true)
    //    jsonRequestObject.batchInfo.isComplete = true;

    reqUrl = "/Helpers/WebMethods.asmx/UpdateBatchGeneralData";

    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject), batchId: batchId });

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
                var batchData = jsonResult.response;

                $('#' + 'Batch' + ' .box').removeClass('box-warning');
                $('#' + 'Batch' + ' .box').addClass('box-success');

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
function SubmitBatchGeneralDataAndFinish(batchId) {
    jsonRequestObject = CreateBatchGeneralDataJsonRequestObject();
    jsonRequestObject.batchInfo.isComplete = true;

    reqUrl = "/Helpers/WebMethods.asmx/UpdateBatchGeneralDataAndFinish";

    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject), batchId: batchId });

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

                //console.log(JSON.stringify(jsonResult));
                var batchData = jsonResult.response;

                $('#' + 'Batch' + ' .box').removeClass('box-warning');
                $('#' + 'Batch' + ' .box').addClass('box-success');

                window.location.replace("/Batches/");
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
function SubmitBatchContent(batchId) {
    jsonRequestObject = CreateBatchContentJsonRequestObject();

    reqUrl = "/Helpers/WebMethods.asmx/UpdateBatchContent";

    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject), batchId: batchId });

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
                notify("Batch content saved", "success");

                //console.log(JSON.stringify(jsonResult));
                var batchData = jsonResult.response;

                $('#' + 'Batch' + ' .box').removeClass('box-warning');
                $('#' + 'Batch' + ' .box').addClass('box-success');

                if (jsonResult.message != null) {
                    //$('#' + 'Batch' + ' .box-footer .lastSave').removeClass('hidden');
                    $('#' + 'Batch' + ' .box-footer .lastSave').css("display", "none");
                    $('#' + 'Batch' + ' .box-footer .lastSave i').html(jsonResult.message);
                    $('#' + 'Batch' + ' .box-footer .lastSave').fadeToggle("slow", "linear");
                }

                //.animate({
                //    "height": totalHeight
                //});
            }
            else {
                $('#' + 'Batch' + ' .box-footer .lastSave').css("display", "none");
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}

//function SubmitBatchEdit(batchId) {

//    jsonRequestObject = CreateBatchJsonRequestObject();

//    reqUrl = "/Helpers/WebMethods.asmx/SubmitBatchEditWithContent";

//    var RequestDataString = JSON.stringify({ formData: JSON.stringify(jsonRequestObject), batchId: batchId });

//    $.ajax({
//        type: "POST",
//        url: reqUrl,
//        data: RequestDataString,
//        //async: true,                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            var jsonResult = JSON.parse(result.d);
//            if (jsonResult.status == "ok") {
//                notify("Batch successfully saved", "success");

//                console.log(JSON.stringify(jsonResult));
//                var batchData = jsonResult.response;

//                $('#' + 'Batch' + ' .box').removeClass('box-warning');
//                $('#' + 'Batch' + ' .box').addClass('box-success');

//                window.location.replace("/Batches/");
//            }
//            else {
//                notify(jsonResult.message, "warning");
//            }
//        },
//        error: function (p1, p2, p3) {
//            alert(p1.status.toString() + " " + p3.toString());
//        }
//    });

//}
function FinishBatchCreation(batchId, editing) {
    $("#finishBatchBtn").attr('disabled', 'true');

    jsonRequestObject = CreateBatchJsonRequestObject();

    reqUrl = "/Helpers/WebMethods.asmx/FinishBatchCreation";

    var RequestDataString = JSON.stringify({
        formData: JSON.stringify(jsonRequestObject),
        batchId: batchId,
        editing: editing
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
                //notify("Batch successfully saved", "success");

                //console.log(JSON.stringify(jsonResult));
                var batchData = jsonResult.response;

                $('#' + 'Batch' + ' .box').removeClass('box-warning');
                $('#' + 'Batch' + ' .box').addClass('box-success');

                window.location.replace("/Batches/");
            }
            else {
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    }).done(function (data) {
        $("#finishBatchBtn").removeAttr('disabled');
    });

}

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

function formatMeasurementUnit(repo) {
    var markup = repo.measurementUnitSymbol + ' | ' + repo.text;
    return markup;
}


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
function formatRepo(repo) {
    var markup = repo.text;
    return markup;
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

function RecreateBatch(batchId) {
    //NE SE KORISTI
    reqUrl = "/Helpers/WebMethods.asmx/RecreateBatch";
    var RequestDataString = JSON.stringify({ batchId: batchId });
    //console.log(RequestDataString);
    $('#recreateBatchBtn').attr('disabled', 'disabled');
    $.ajax({
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
                //batch = GetBatchWithContent(batchId);
                //notify("Batch quantity successfully recreated", "success");
                location.reload();
            }
            else {
                notify(jsonResult.message, "warning");
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    }).done(function () {
        //$(this).addClass("done");
        $('#recreateBatchBtn').removeAttr('disabled');
    });
}

function GenerateBatchStartHtml() {
    var html = "                   \
    <div class=\"row batchContentsRow\">  \
            <div class=\"col-md-10 col-lg-8 col-lg-offset-2\">                    \
                <div id=\"Batch\" class=\"box box-warning  batteryComponent\">  \
                    <div class=\"box-header with-border\">    \
                        <h3 class=\"box-title\">Batch Contents</h3>   \
                        <div class=\"box-tools pull-right\">  \
                            <button type=\"button\" class=\"btn btn-xs btn-default startBatchOverButton hidden\" onclick=\"StartBatchOver()\">    \
                                <i class=\"fa fa-refresh\"></i>Start Over \
                            </button>   \
                            <button type=\"button\" class=\"btn btn-box-tool component-box-tool\" data-widget=\"collapse\">   \
                                <i class=\"fa fa-plus\"></i>  \
                            </button>   \
                        </div>  \
    \
                    </div>  \
                    <!-- /.box-header -->   \
    \
                    <!-- form start --> \
                    <fieldset class=\"form-horizontal\">  \
                        <div class=\"box-body\">  \
                                \
                        </div>  \
                        <!-- /.box-body --> \
                        <div class=\"box-footer\">    \
                            <div class=\"pull-right batchFooterButtons\"> \
    \
                            </div>  \
                        </div>  \
                        <!-- /.box-footer -->   \
                    </fieldset> \
                </div>  \
                <!-- /.box -->  \
            </div>  \
        </div>  \
    \
        <div class=\"row\">   \
            <div class=\"col-md-10 col-lg-8 col-lg-offset-2\">    \
                <div class=\"box box-solid\"> \
    \
                    <!-- form start --> \
                    <fieldset class=\"form-horizontal\">  \
    \
                        <!-- /.box-body --> \
                        <div class=\"box-footer no-border\">  \
                            <div class=\"pull-right\">    \
                                <asp:Button runat=\"server\" ID=\"cancelBatchBtn\" Text=\"Cancel\" CssClass=\"btn btn-default\" OnClick=\"BtnCancel_Click\" />    \
                                <input type=\"button\" id=\"submitBatchBtn\" value=\"Submit batch\" class=\"btn btn-success\" onclick=\"SubmitBatch()\" />    \
                            </div>  \
                        </div>  \
                        <!-- /.box-footer -->   \
                    </fieldset> \
                </div>  \
                <!-- /.box -->  \
            </div>  \
        </div>  \
";

    return html;
}
function AddBatchContentHtml() {
    var startHtml = GenerateBatchStartHtml();
    $('div#' + 'batchContent').html(startHtml);
}

function StartBatchBlank() {
    //==AddNewStep

    var html = '';
    var measurements = "";
    html = GenerateBatchContentBlankHtml(measurements);
    //==GenerateNewStepHtml
    //$('#' + 'Batch' + " .btn-box-tool").trigger('click');

    //console.log(html);

    $('#' + 'Batch' + ' .box-body:first').append(html);
    $('#' + 'Batch' + ' .startBatchOverButton').removeClass('hidden');
}

function GenerateBatchContentBlankHtml(measurements, viewMode) {

    var html = "";
    var materialsAndBatchesTable = 'materialsAndBatchesTable';
    var processesTable = 'processesTable';

    var measurementsData = JSON.stringify({ "type": "measurements", "item": measurements });
    if (viewMode) {
        var measurementsButton = GenerateViewMeasurementsButton(measurementsData, 5);
    }
    else {
        var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 5);
    }

    $("#" + "Batch" + " .boxButtons").prepend(measurementsButton);
    //var documentsButton = '<input type="button" class="btn btn-xs btn-default documentsButton" onclick="ShowFileAttachments(\'' + "Step" + '\',  ' + experimentId + ',  ' + componentTypeId + ',  ' + stepNumber + ')" disabled="true" data-toggle="tooltip" title="Adding documents is available after save!" value="Documents" /> ';

    html += '<div class="row "' + ' id=' + 'Batch' + '>';

    html += '<div class="col-md-12 col-lg-12 col-lg-offset-0">' +
        '<div class="col-sm-12">' +
        '<div class="pull-right batchContentButtons">' +
        '<input type="button" onclick=" ' + 'NewMaterialPopup(\'' + materialsAndBatchesTable + '\')" value="Add material" class="newMaterialBtn btn bg-olive" /> ' +
        '<input type="button" onclick=" ' + 'NewBatchPopup(\'' + materialsAndBatchesTable + '\')" value="Add batch" class="newBatchBtn btn btn-warning" /> ' +
        '<input type="button" onclick=" ' + 'NewProcessPopup(\'' + processesTable + '\')" value="Add process" class="newProcessBtn btn btn-default" /> ' +
        '<input type="button" onclick=" ' + 'NewProcessSequencePopup(\'' + processesTable + '\')" value="Add sequence" class="newSequenceBtn btn btn-default" /> ' +
        '</div>' +
        '</div>' +

        '<div class="col-sm-12">' +
        '<div class="table-responsive">' +
        '<table id="materialsAndBatchesTable' + '" class="table table-condensed table-hover materialsAndBatchesTable">' +
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
        '</div>' +

        '<div class="col-sm-12">' +
        '<div class="table-responsive">' +
        '<table id="processesTable' + '" class="table table-condensed table-hover processesTable">' +
        '<tbody class="js-sortable-table">' +
        '<tr>' +
        '<th class="moveColumn"></th>' +
        '<th></th>' +
        '<th>Process</th>' +
        '<th>Equipment</th>' +
        '<th>Label</th>' +
        '<th></th>' +
        '<th>Process Attributes</th>' +
        '</tr>' +
        '</tbody>' +
        '</table>' +
        '</div>' +
        '</div>' +
        '<div class="box-footer">';
    html += '<div class="pull-right stepFooterButtons">';

    //html += '<a class="btn btn-primary btn-xs addMeasurementsStepLevelBtn" data-item=\'' +
    //    measurementsData + '\' onclick="AddMeasurements(this, 2)" >' + ' Add measurements</a> ';


    html += '<input type="button" value="Save as sequence"' + ' data-item=\'' + JSON.stringify({ "type": "sequenceData", "item": "" }) + '\'' + ' onclick="SaveStepAsSequence(\'' + materialsAndBatchesTable + '\', this' + ')" class="saveAsBatchBtn btn btn-default hidden">' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';

    //console.log(html);
    return html;
}


function GenerateViewMeasurementsButton(measurementsData, measurementsLevelType) {
    var html = "";
    if (measurementsLevelType == 4) {
        html += '<a class="btn btn-primary btn-xs addMeasurementsContentLevelBtn" data-item=\''
            + measurementsData + '\' onclick="ViewMeasurements(this, 4)" >' + ' View measurements</a> ';
    }
    else if (measurementsLevelType == 5) {
        html += '<a class="btn btn-primary btn-xs addMeasurementsStepLevelBtn" data-item=\''
            + measurementsData + '\' onclick="ViewMeasurements(this, 5)" >' + ' Batch measurements</a> ';
    }
    return html;
}

function GenerateAddMeasurementsButton(measurementsData, measurementsLevelType) {
    var html = "";

    if (measurementsLevelType == 4) {
        html += '<a class="btn btn-primary btn-xs addMeasurementsBatchContentLevelBtn" data-item=\''
            + measurementsData + '\' onclick="AddMeasurements(this, 4)" >' + ' Add measurements</a> ';
    }
    else if (measurementsLevelType == 5) {
        html += '<a class="btn btn-primary btn-xs addMeasurementsBatchLevelBtn" data-item=\''
            + measurementsData + '\' onclick="AddMeasurements(this, 5)" >' + ' Batch measurements</a> ';
    }

    return html;
}

function StartBatchOver() {
    $('#' + 'Batch' + ' .box-body').html("");
    $("#" + "Batch" + " .boxButtons .addMeasurementsBatchLevelBtn").remove();
    StartBatchBlank();
}

function LockBatchEditing() {
    //$('#' + 'Batch' + ' .editBatchButton').removeClass('hidden');
    $('#' + 'Batch' + ' .startBatchOverButton').addClass('hidden');

    //var savedHtml = "<span class = \"saved\"><i class=\"fa fa-fw fa-check componentSaved\"></i> Saved</span>";

    //"input[name='quantity']"
    $('#' + 'Batch').removeClass('box-warning');
    $('#' + 'Batch').addClass('box-success');

    $('#' + 'Batch' + ' .removeRow').addClass('hidden');
    $('#' + 'Batch' + ' input[name="quantity"]').attr('disabled', 'disabled');
    $('#' + 'Batch' + ' .batchProcessRow .editSelectedProcessButton').addClass('hidden');
    $('#' + 'Batch' + ' .batchProcessRow .viewSelectedProcessButton').removeClass('hidden');

    $('#' + 'Batch' + ' .batchProcessRow .addEquipmentSettingsButton').addClass('hidden');
    $('#' + 'Batch' + ' .batchProcessRow .viewEquipmentSettingsButton').removeClass('hidden');


    //$('#' + 'Batch' + ' .componentFooterButtons input').addClass('hidden');
    //$('#submit' + componentType + 'Button').parent().append(savedHtml);

    //$('#' + componentType + ' .componentFooterButtons').append(savedHtml);
    //$('#' + 'Batch' + ' .batchFooterButtons .batchSaved').removeClass('hidden');

    //$("#" + componentType + " .box-tools .documentsButton").removeAttr('disabled').removeAttr("title");
    $("#" + 'Batch' + " .documentsButton").removeAttr('disabled').removeAttr("title");

    $('#' + 'Batch' + ' .js-handle').addClass('hidden');
    $('#' + 'Batch' + ' .moveColumn').addClass('hidden');
    //DISABLE MOVE SORTING
    if (viewMode == false) {
        //console.log('disabling sort for ' + componentType);
        var materialsTable = $('#' + 'Batch' + ' .materialsAndBatchesTable tbody ');
        var processesTable = $('#' + 'Batch' + ' .processesTable tbody ');
        sortable(materialsTable, 'disable');
        sortable(processesTable, 'disable');
    }

}
function SaveStepAsSequence(table, button) {


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
                    var componentType = "Anode";
                    var stepId = 1;

                    $("#processesTable tbody tr.batchProcessRow").each(function (id) {
                        var row = $(this).data("item");
                        var itemType = row.type;
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
                    var closePopup = true;
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
                                closePopup = true;
                            }
                            else {
                                notify(jsonResult.message, "warning");
                                //$(button).data("item").item = '{ "batchPersonalLabel":' + batchPersonalLabel + ', "batchDescription":' + batchDescription + ', "batchChemicalFormula":' + batchChemicalFormula + ', "batchOutput":' + batchOutput + ', "measurementUnit":' + measurementUnit + ', "materialType":' + materialType + ' }';
                                $(button).data("item").item = sequenceInfo;
                                closePopup = false;
                            }
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status.toString() + " " + p3.toString());
                        }
                    });
                    return closePopup;
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
function UnlockBatchEditing() { }

function ExpandBatchBox() {
    var batch = $('#Batch');
    if ($(batch).hasClass('collapsed-box')) {
        $(batch).find(".btn-box-tool").trigger('click');
    }
}
function SetBatchToViewMode() {
    var batch = $('#Batch');
    ExpandBatchBox();

    //$("div.batteryComponent" + ' .step .saveAsBatchBtn').remove();
    $("#Batch" + ' .batchContentButtons').remove();

    $('#' + 'Batch').removeClass('box-warning');
    $('#' + 'Batch').addClass('box-default');

    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable .table-actions-col').remove();

    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody tr').filter("th:last").remove();
    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody tr th').last().remove();
    //$("div.batteryComponent" + ' .step .materialsAndBatchesTable tbody').remove();



    //trgni site kopcinja
    //action th
    //process view attributes
    //input disable


}







function GetBatchWithContent(batchId) {
    //NE SE KORISTI
    var newRequestObj = new Object();
    //var batchId = 1;
    //reqUrl = "../Edit.aspx/AddData";
    reqUrl = "/Helpers/WebMethods.asmx/GetBatchWithContent";

    //var RequestDataString = JSON.stringify(newRequestObj);
    var RequestDataString = JSON.stringify({ batchId: batchId });
    //console.log(RequestDataString);

    $.ajax({
        type: "POST",
        url: reqUrl,
        data: RequestDataString,
        //async: true,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            //ako e ok ShowBatch(batch-result);
            if (result.d == 0) {

            }
            else if (result.d == 2) {

            } else {
                //alert(result.d);
                //console.log(result.d);
            }
        },
        error: function (p1, p2, p3) {
            alert(p1.status.toString() + " " + p3.toString());
        }
    });
}
function ShowBatch(batch, viewMode) {
    //console.log(batch);
    if (viewMode == true) {
        var batchGeneralInfoHtml = GenerateBatchInfoViewHtml(batch.batchInfo);
        $('#batchGeneralInfo tbody').append(batchGeneralInfoHtml);
    }
    else {
        //batchSystemLabel        
        $('#batchSystemLabel').val(batch.batchInfo.batchSystemLabel);
        $('#batchDateCreated').val(ToLocalDate(batch.batchInfo.dateCreated));
        $('#batchPersonalLabel').val(batch.batchInfo.batchPersonalLabel);
        $('#batchDescription').val(batch.batchInfo.description);
        $('#batchChemicalFormula').val(batch.batchInfo.chemicalFormula);
        $('#batchOutput').val(batch.batchInfo.batchOutput);
        $('#totalBatchOutput').val(batch.batchInfo.totalBatchOutput);
        $('#wasteAmount').val(batch.batchInfo.wasteAmount);
        $('#releasedAs').val(batch.batchInfo.releasedAs);
        $('#wasteChemicalComposition').val(batch.batchInfo.wasteChemicalComposition);
        $('#wasteComment').val(batch.batchInfo.wasteComment);

        if (batch.batchInfo.fkMeasurementUnit) {
            var text = batch.batchInfo.measurementUnitSymbol + " | " + batch.batchInfo.measurementUnitName;
            var id = batch.batchInfo.fkMeasurementUnit;
            var option = new Option(text, id, true, true);
            $('#selectMeasurementUnit').append(option).trigger('change');
        }
        if (batch.batchInfo.fkMaterialType) {
            var text = batch.batchInfo.materialType;
            var id = batch.batchInfo.fkMaterialType;
            var option = new Option(text, id, true, true);
            $('#selectMaterialType').append(option).trigger('change');
        }
    }

    //var batchContentHtml = GenerateBatchContentViewHtml(batch.batchContentList);
    //$('#materialsAndBatchesTable tbody').append(batchContentHtml);
    //var batchProcessHtml = GenerateBatchProcessViewHtml(batch.batchProcessList);
    //$('#processesTable tbody').append(batchProcessHtml);

    RegenerateBatchContents(batch, viewMode);
    if (viewMode == true) {
        LockBatchEditing();
        SetBatchToViewMode();
    }

}
function RegenerateBatchContents(batch, viewMode) {
    var measurements = "";
    var attr = 'measurements';
    if (attr in batch) {
        if (batch.measurements != null) {
            measurements = batch.measurements;
            //console.log("batch Measurement: " + measurements);
        }
    }

    html = GenerateBatchContentBlankHtml(measurements, viewMode);

    $('#' + 'Batch' + ' .box-body:first').append(html);
    $('#' + 'Batch' + ' .startBatchOverButton').removeClass('hidden');


    var materialsBatchesTable = "materialsAndBatchesTable";

    $.each(batch.batchContent, function (key, batchContent) {
        //console.log(key + ": " + value);
        //if (batchContent.fkStepMaterial != null) {
        //    html += GenerateStepMaterialViewHtml(value);
        //}
        //else {
        //    html += GenerateStepBatchViewHtml(value);
        //}
        if (batchContent.fkStepMaterial != null) {
            var row = GenerateMaterialHtml(batchContent, viewMode);
            AppendRow($('#' + materialsBatchesTable), row);
        }
        else if (batchContent.fkStepBatch != null) {
            var row = GenerateBatchHtml(batchContent, viewMode);
            AppendRow($('#' + materialsBatchesTable), row);
        }
    });

    var processesTable = "processesTable";
    $.each(batch.batchProcesses, function (key, batchProcess) {
        //var row = GenerateProcessHtml(stepProcess);
        var batchProcessItem = batchProcess.batchProcess;
        var processAttributes = batchProcess.processAttributes;
        var equipmentSettings = batchProcess.equipmentSettings;
        var row = GenerateProcessHtmlFromDatabase(batchProcessItem, processAttributes, equipmentSettings, viewMode);
        AppendRow($('#' + processesTable), row);
        //console.log(stepProcess);
    });
}
function GenerateBatchContentViewHtml(batchContentList) {
    var html = "";
    //console.log(batchContentList);
    //foreach(batchContent in batchContentList){
    //    html += GenerateMaterialViewHtml(batchContent);
    //}
    $.each(batchContentList, function (key, value) {
        //console.log(key + ": " + value);
        if (value.fkStepMaterial != null) {
            html += GenerateStepMaterialViewHtml(value);
        }
        else {
            html += GenerateStepBatchViewHtml(value);
        }
    });
    //console.log(html);
    return html;
}
//function GenerateBatchProcessViewHtml(batchProcessList) {
//    var html = "";
//    $.each(batchProcessList, function (key, value) {
//        html += GenerateProcessViewHtml(value);
//    });
//    //console.log(html);
//    return html;
//}
//function GenerateStepMaterialViewHtml(data) {
//    //console.log(data);
//    var rowId = guid('short');
//    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchContentRow\'' + ' data-item=\'' + JSON.stringify({ "type": "material", "item": data }) + '\'' + '>';
//    //id=\'material' + guid('short')
//    //res += '<td>';
//    //res += '</td>';

//    res += '<td>';
//    res += '<span class="badge bg-green">' + data.chemicalFormula + '</span>';
//    res += data.materialName;
//    res += '</td>';

//    res += '<td>' +
//    '<span>' + data.weight + '</span>' +
//    '</td>';

//    res += '<td>';
//    res += '<span class="badge bg-red">' + data.measurementUnitSymbol + '</span>';
//    res += '</td>';

//    res += '</tr>';
//    //console.log(res);

//    //$('#materialsTable tbody').append(res);
//    return res;
//}
//function GenerateStepBatchViewHtml(data) {
//    //console.log(data);
//    var rowId = guid('short');

//    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchContentRow\'' + ' data-item=\'' + JSON.stringify({ "type": "batch", "item": data }) + '\'' + '>';

//    //res += '<td>';
//    //res += '</td>';

//    res += '<td>';
//    res += '<span class="badge bg-blue">' + data.chemicalFormula + '</span>';
//    //res += data.batchLabel;
//    res += data.batchPersonalLabel;
//    //res += "Batch_" + data.batchSystemLabel; //treba da se polni vo Ext soodvetno od id
//    res += '</td>';

//    res += '<td>' +
//    '<span>' + data.weight + '</span>' +
//    '</td>';

//    res += '<td>';
//    res += '<span class="badge bg-red">' + data.measurementUnitSymbol + '</span>';
//    res += '</td>';

//    res += '</tr>';
//    //console.log(res);
//    return res;
//}
//function GenerateProcessViewHtml(data) {
//    var rowId = guid('short');

//    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchProcessRow\'' + ' data-item=\'' + JSON.stringify({ "type": "process", "item": data }) + '\'' + '>';

//    res += '<td>';
//    res += '<span class="badge bg-default">' + data.processType + '</span>';
//    res += '</td>';


//    res += '<td class="table-actions-col">' + '<a class="btn btn-primary btn-xs" data-item=\'' + JSON.stringify({ "type": "process", "item": data }) + '\' onclick="ViewProcessAttributes(this)" >' + ' View settings</a> ';
//    //res += '<td>';
//    //res += '</td>';

//    res += '</tr>';
//    return res;
//}
function GenerateBatchInfoViewHtml(batch) {
    var html = "";

    html += '<tr>' +
        '<td>System Label</td>' +
        '<td>' + batch.batchSystemLabel + '</td>' +
        //'<td>' + "Batch_" + batch.batchId + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Personal Label</td>' +
        '<td>' + batch.batchPersonalLabel + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Description</td>' +
        '<td>' + batch.description + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Chemical Formula</td>' +
        '<td>' + batch.chemicalFormula + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Measurement Unit</td>' +
        '<td>' + batch.measurementUnitName + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Batch Output</td>' +
        '<td>' + batch.totalBatchOutput + ' ' + batch.measurementUnitSymbol + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Usable Batch Output</td>' +
        '<td>' + batch.batchOutput + ' ' + batch.measurementUnitSymbol + '</td>' +
        '</tr>';
    var availableQuantity = batch.availableQuantity != null ? batch.availableQuantity : '0';
    html += '<tr>' +
        '<td>Available Quantity In Stock</td>' +
        '<td>' + availableQuantity + ' ' + batch.measurementUnitSymbol + '</td>' +
        '</tr>';
    if (batch.wasteAmount != null) {
        html += '<tr>' +
            '<td>Waste Amount</td>' +
            '<td>' + batch.wasteAmount + ' ' + batch.measurementUnitSymbol + '</td>' +
            '</tr>';
    }
    else {
        html += '<tr>' +
            '<td>Waste Amount</td>' +
            '<td>' + '0' + ' ' + batch.measurementUnitSymbol + '</td>' +
            '</tr>';
    }    
    html += '<tr>' +
        '<td>Released as liquid, solid or gas</td>' +
        '<td>' + batch.releasedAs + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Waste Chemical Composition</td>' +
        '<td>' + batch.wasteChemicalComposition + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Waste Comment</td>' +
        '<td>' + batch.wasteComment + '</td>' +
        '</tr>';
    html += '<tr>' +
        '<td>Material Type</td>' +
        '<td>' + batch.materialType + '</td>' +
        '</tr>';



    //html += '<div class="col-sm-12">' +
    //'<label class="col-sm-2">Description</label>' +
    //'<div class="col-sm-10">' +
    //'<span>' + batch.description + '</span>' +
    //'</div>' +
    //'</div>';

    //html += '<div class="col-sm-12">' +
    //'<label class="col-sm-2">Chemical Formula</label>' +
    //'<div class="col-sm-10">' +
    //'<span>' + batch.chemicalFormula + '</span>' +
    //'</div>' +
    //'</div>';

    //html += '<div class="col-sm-12">' +
    //'<label class="col-sm-2">Batch Output</label>' +
    //'<div class="col-sm-10">' +
    //'<span>' + batch.batchOutput + ' ' + batch.measurementUnitSymbol + '</span>' +
    //'</div>' +
    //'</div>';

    //html += '<div class="col-sm-12">' +
    //'<label class="col-sm-2">Measurement Unit</label>' +
    //'<div class="col-sm-10">' +
    //'<span>' + batch.measurementUnitName + '</span>' +
    //'</div>' +
    //'</div>';

    //html += '<div class="col-sm-12">' +
    //'<label class="col-sm-2">Material Type</label>' +
    //'<div class="col-sm-10">' +
    //'<span>' + batch.materialType + '</span>' +
    //'</div>' +
    //'</div>';




    //$('#batchGeneralInfo').html(html);
    //var batchGeneralInfoField;
    //console.log(html);
    return html;
}

function AppendRow(table, row) {
    //console.log(table.children('tbody'));
    table.children('tbody').append(row);

    var tableId = table.attr('id');
    if (viewMode == false)
        MakeTablesSortable(tableId);
}
function RemoveRow(rowId) {
    //console.log(rowId);
    $("#" + rowId).remove();
    //rowId.remove();
}
function ReplaceRow(oldRow, newRow) {
    var tableId = oldRow.parents('table').attr('id');
    oldRow.replaceWith(newRow);

    if (viewMode == false) {
        MakeTablesSortable(tableId);
    }
}



function GenerateMaterialHtml(data, viewMode) {
    var rowId = guid('short');
    var materialId = 0;
    var attr = 'materialId';
    if (attr in data) {
        materialId = data.materialId;
    }
    else materialId = data.fkStepMaterial;

    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchContentRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "material", "item": data }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
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
    //not used
    if (viewMode) {
        var measurementsButton = GenerateViewMeasurementsButton(measurementsData, 4);
    }
    else {
        var measurementsButton = GenerateAddMeasurementsButton(measurementsData, 4);
    }

    res += '<td class="table-actions-col">';
    if (sharedViewMode == false)
        res += '<a class="btn btn-primary btn-xs" target="_blank" href="/Materials/' + '"> Check Quantity</a> ';
    res += '<a class="btn btn-primary btn-xs addMeasurementsBatchContentLevelBtn" data-item=\'';
    if (viewMode) {
        res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="ViewMeasurements(this, 4)" >' + ' View measurements</a> ';
    }
    else {
        res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 4)" >' + ' Add measurements</a> ';
    }

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

    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchContentRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "batch", "item": data }) + '\'' + '>';

    res += '<td class="moveColumn">';
    res += '<span title="Move" class="js-handle">' + '<i class="fa fa-reorder"></i>' + '</span> ';
    res += '</td>';

    res += '<td>';
    res += '<div class="btn btn-danger btn-xs removeRow" onclick="RemoveRow(\'' + rowId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ';
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
    res += '<a class="btn btn-primary btn-xs addMeasurementsBatchContentLevelBtn" data-item=\'';
    //+ JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 4)" >' + ' Add measurements</a> ';
    if (viewMode) {
        res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="ViewMeasurements(this, 4)" >' + ' View measurements</a> ';
    }
    else {
        res += JSON.stringify({ "type": "measurements", "rowId": rowId, "item": measurements }) + '\' onclick="AddMeasurements(this, 4)" >' + ' Add measurements</a> ';
    }
    res += '</td>';

    res += '</tr>';
    return res;
}


function GenerateProcessHtml(label, selectedProcessData, arr) {
    /* alpacaFormData['processTypeId'] = selectedProcessData.processTypeId;
     alpacaFormData['processType'] = selectedProcessData.processType;
     alpacaFormData['processDatabaseType'] = selectedProcessData.processDatabaseType;*/
    var fkProcessType = selectedProcessData.fkProcessType;
    var processType = selectedProcessData.processType;
    var fkEquipment = selectedProcessData.fkEquipment;
    var equipmentName = selectedProcessData.equipmentName;
    var equipmentModelId = selectedProcessData.equipmentModelId;
    var equipmentModelName = selectedProcessData.equipmentModelName;
    var equipmentModelBrand = selectedProcessData.equipmentModelBrand;
    //var subcategory = selectedProcessData.subcategory;
    var rowId = guid('short');

    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchProcessRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "process", "item": arr, "data": selectedProcessData, "label": label }) + '\'' + '>';

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

    res = '<tr id=\'' + rowId + '\'' + ' class=\'batchProcessRow js-sortable-tr\'' + ' data-item=\'' + JSON.stringify({ "type": "process", "item": arr, "data": experimentProcessObj, "label": experimentProcess.label }) + '\'' + '>';

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
    res += '<a class="btn btn-primary btn-xs viewSelectedProcessButton hidden" data-item=\'';
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
    //var processTypeClassName = processType;
    //var processTypeClassName = processData.item.processDatabaseType;
    //if (processType == "Phase inversion") {
    //    processTypeClassName = "PhaseInversion";
    //}

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

    // alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson)
}

function AddEquipmentSettings(element, equipmentId) {

    var eqID = null;
    if (equipmentId != null) {
        eqID = equipmentId;
    }
    //console.log(eqID);

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

                    //console.log("alpaca form data: " + JSON.stringify(alpacaFormData));
                    //$('tr#' + measurementsData.rowId).data("item", alpacaFormDataJson)

                    //ne se gleda promenata vo DOM
                    $(element).data("item").item = alpacaFormData;
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
                formData["fkExperiment"] = 1; //proveri check
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
        formData["fkExperiment"] = 1; //proveri check
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
        //formData["fkExperiment"] = experimentId;
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


function AddMeasurements(element, measurementLevelTypeId) {
    var measurementsData = $(element).data("item");

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

                    //console.log("alpaca form data: " + JSON.stringify(alpacaFormData));
                    $(element).data("item").item = alpacaFormData;
                    //console.log("element data: " + JSON.stringify($(element).data("item")));

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
        var formData = {};

        formData = measurementsData.item;
        formData["form_action"] = 'Add';
        formData["form_element"] = "measurements";
        formData["fkExperiment"] = 1; //proveri check
        formData["fkMeasurementLevelType"] = measurementLevelTypeId;

        alpacaConfigJson.data = formData;

        //form-helpers.js
        setFormDatasourcePath(alpacaConfigJson, formsFolderURL + "/");
        setFormActionPath(alpacaConfigJson, formsFolderURL + "/");

        alpacaForm = $("#alpaca-universal-form").alpaca(alpacaConfigJson);
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
        formData["fkExperiment"] = 1; //proveri check
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



//Add new material popup
function NewMaterialPopup(table) {
    var selectedMaterialFunction = null;
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
                    selectedData.percentageOfActive = percentageOfActive;

                    var row = GenerateMaterialHtml(selectedData);
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
        placeholder: 'Select material function',
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
        if (selectedMaterialFunction == 1) {
            if (selectedMaterialItem && selectedMaterialItem.percentageOfActive != null) {
                $('#percentageOfActive').val(selectedMaterialItem.percentageOfActive);
            }
            $('#percentageOfActive').removeClass('hidden');
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

//Add new batch popup
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
        placeholder: 'Select material function',
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
    //console.log(this);
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

    html += '<div class="row"><div class="col-md-12">';
    html += '<form class="addAttributesForm">';
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
                    //check if there are no attributes for the process
                    if ($("form.addAttributesForm input").length == 0) {
                        notify("No attributes found!");
                        return false;
                    }
                    // Validate form
                    if ($('form.addAttributesForm').validate().form()) {
                        var selectedProcess = $('#selectProcess').val();
                        var selectedEquipmentSettings;
                        if (selectedProcess != null) {
                            selectedProcess = $('#selectProcess').select2('data')[0].data;
                            var label = $('#processLabel').val();
                            var selectedPreviousProcess = $('#selectPreviousProcess').val();
                            var selectedEquipmentModelId = $('#selectEquipmentModel').val();
                            var selectedEquipmentModel = null;
                            var equipmentModelName = null;
                            var modelBrand = null;

                            if (selectedPreviousProcess != null) {
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

    //$(".add-button").attr('disabled', 'disabled');
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
            $('#selectEquipment').append(equipment).trigger('change');
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
                        notify("No attributes found!");
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

        var equipmentId = $('#selectEquipment').select2("data")[0];
        var processId = $('#selectProcess').val();
        if (equipmentId != undefined) {
            var id;
            var eqdata = equipmentId.data;
            if (eqdata == undefined) {
                id = equipmentId.id;
            } else {
                id = equipmentId.data.equipmentId;
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
    $('#selectEquipmentModel').change(function () {
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
            //    $('#selectEquipment').prop('disabled', true);
            //}
        } else {
            $("#form").empty();
        }
    });
}
function formatPreviousProcessDropdown(item) {
    var markup = "";

    processId = item.experimentProcessId;
    markup += item.dateCreated + " | " + item.label + " | " + item.processType + " | " + item.equipmentName + ((item.equipmentModelName != "") ? (" | " + item.equipmentModelName) : "");
    return markup;
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

$(function () {

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
        escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
        //minimumInputLength: 1,
        //templateResult: formatProcess,
        //templateSelection: formatRepoSelection
    });


    //Add new process popup
    $('#newProcessBtn').click(function () {
        var html = '<div class="row"><div class="col-md-12">';
        html += '<select id="selectProcess" class="batch-data-ajax">    </select>';
        html += '</div></div>';

        html += '<div class="row"><div class="col-md-12">';
        html += '<div id="alpaca-universal-form"> </div>';
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

                        $("#alpaca-submit-button").prop("disabled", true);
                        //var alpacaFormData = this.getValue();
                        var alpacaFormData = alpacaForm.alpaca().getValue();
                        //console.log(alpacaFormData);
                        var alpacaFormDataJson = JSON.stringify(alpacaFormData);
                        //console.log(alpacaFormDataJson);
                        var selectedProcessData = $('#selectProcess').select2('data')[0].data;

                        var row = GenerateProcessHtml(selectedProcessData, alpacaFormData);
                        AppendRow($('#processesTable'), row);
                        //console.log(alpacaFormDataJson);

                        bootbox.hideAll();

                        return true;
                    }
                }
            },
            onEscape: true
        });
        //$("#alpaca-submit-button").attr('disabled', 'disabled');
        $(".add-button").attr('disabled', 'disabled');
        //var addButton = dialog.find('.submit-button');
        //addButton.attr('disabled', 'disabled');
        //console.log(addButton);
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
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            //minimumInputLength: 1,
            //templateResult: formatProcess,
            //templateSelection: formatRepoSelection
        });
    });
    function formatProcess(e) {
        //var markup = "" + repo.chemicalFormula + " | " + repo.text;
        var markup = "" + e.id + " | " + e.text;
        return markup;
    }

});
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
