var dialog;
var sharedViewMode = false;

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

function ShowFileAttachments(elementName, elementId) {

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
        url: "/Helpers/WebMethods.asmx/GetFileAttachments",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{elementName:"' + elementName + '", id:"' + elementId + '"}',
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
                    if (sharedViewMode)
                        html += '<td> </td>';
                    else
                        html += '<td><div title="Delete File" class="btn btn-xs btn-danger" onclick="DeleteFileAttachment(' + this.fileAttachmentId + ', \'' + elementName + '\', ' + elementId + ')">Delete</div>' + '</td>';
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
            //    '<div class="btn btn-default" onclick="UploadFileAttachment(\'' + elementName + '\', ' + elementId + ')">Upload File</div>' +
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
            '<div class="btn btn-default" onclick="UploadFileAttachment(\'' + elementName + '\', ' + elementId + ')">Upload File</div>' +
            '</div>' +
            '</div>';
    }

    dialog = bootbox.dialog({
        title: 'Documents',
        message: html,
        closeButton: true,
        onEscape: true,
        size: "large"
    });
    dialog.on('shown.bs.modal', function () {
        DocTypesOnShow();
    });


}
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

function UploadFileAttachment(elementName, elementId) {
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
                    url: "/Helpers/WebMethods.asmx/SubmitFileAttachment",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"fileBase64":"' + reader.result + '", "filename":"' + file.name + '","description":"' + desc + '", "elementName":"' + elementName + '", "elementId":' + elementId + ', "documentTypeId":' + docTypeId + '}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {
                            notify("Success!", "success");
                            bootbox.hideAll();
                            ShowFileAttachments(elementName, elementId);

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
    var win = window.open('/Helpers/DownloadFile.ashx?fileAttachmentId=' + fileAttachmentId, '_blank');
}

function DeleteFileAttachment(fileAttachmentId, elementName, elementId) {
    bootbox.confirm({
        title: "Delete file",
        message: "Are you sure you want to delete the file?",
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
                    url: "/Helpers/WebMethods.asmx/DeleteFileAttachment",
                    async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: '{"fileAttachmentId":' + fileAttachmentId + '}',
                    success: function (result) {
                        var jsonResult = JSON.parse(result.d);
                        if (jsonResult.status == "ok") {
                            notify("Success!", "success");
                            bootbox.hideAll();
                            ShowFileAttachments(elementName, elementId);
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


function ShowFileAttachmentsMeasurements(testTypeId, experimentId, batchId, materialId) {
    if (testTypeId == undefined || testTypeId == null) {
        testTypeId = null;
    }    

    if (experimentId == undefined || experimentId == null) {
        experimentId = null;
    }
    if (batchId == undefined || batchId == null) {
        batchId = null;
    }
    if (materialId == undefined || materialId == null) {
        materialId = null;
    }

    var html = '<div class="row"><div class="col-md-12">';

    html += '<div class="table-responsive">' +
        '<table class="table table-bordered table-hover table-striped text-center" style="width:100%">' +
        '<thead>' +
        '<th>Filename</th>' +
        '<th>Document type</th>' +
        '<th>Test type</th>' +
        //'<th>Description</th>' +
        '<th>Extension</th>' +
        '<th>Date Added</th>' +
        '</thead>' +
        '<tbody>';

    var newRequestObj = new Object();
    newRequestObj.testTypeId = testTypeId;
    newRequestObj.experimentId = experimentId;
    newRequestObj.batchId = batchId;
    newRequestObj.materialId = materialId;
    var RequestDataString = JSON.stringify(newRequestObj);

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetFileAttachmentsMeasurements",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //data: '{testTypeId: ' + testTypeId + ', experimentId:"' + experimentId + '", batchId:"' + batchId + '", materialId:"' + materialId + '"}',
        data: RequestDataString,
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            if (jsonResult.status == "ok") {

                $(jsonResult.response).each(function () {
                    html += '<tr>';
                    html += '<td><a href="#" onclick="DownloadFileAttachment(' + this.fileAttachmentId + ')" title="' + this.filename + '">' + text_truncate(this.filename, 30, "...") + '</span></td>';
                    html += '<td>' + this.documentTypeName + '</td>';
                    html += '<td>' + this.testType;
                    if (this.testTypeSubcategory != '')
                        html += ' | ' + this.testTypeSubcategory
                    html += '</td>';
                    //html += '<td><span title="' + this.description + '">' + text_truncate(this.description, 40, "...") + '</td>';
                    html += '<td>' + this.extension + '</td>';
                    html += '<td>' + this.createdOn + '</td>';
                });

                if (jsonResult.response == null) {
                    html += '<tr><td colspan="6" class="text-center">There are no files</td></tr>';
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


    dialog = bootbox.dialog({
        title: 'Test results documents',
        message: html,
        closeButton: true,
        onEscape: true,
        size: "large"
    }).on('shown.bs.modal', function (e) {
        /*DocTypesOnShow();*/
    });
}

function ShowFileAttachmentsMeasurementsLowLevel(testTypeId, experimentId, componentTypeId, stepId) {
    if (testTypeId == undefined || testTypeId == null) {
        testTypeId = null;
    }
    if (componentTypeId == undefined || componentTypeId == null) {
        componentTypeId = null;
    }
    if (stepId == undefined || stepId == null) {
        stepId = null;
    }

    var html = '<div class="row"><div class="col-md-12">';

    html += '<div class="table-responsive">' +
        '<table class="table table-bordered table-hover table-striped text-center" style="width:100%">' +
        '<thead>' +
        '<th>Filename</th>' +
        '<th>Document type</th>' +
        '<th>Test type</th>' +
        //'<th>Description</th>' +
        '<th>Extension</th>' +
        '<th>Date Added</th>' +
        '</thead>' +
        '<tbody>';

    var newRequestObj = new Object();
    newRequestObj.testTypeId = testTypeId;
    newRequestObj.experimentId = experimentId;
    newRequestObj.componentTypeId = componentTypeId;
    newRequestObj.stepId = stepId;
    var RequestDataString = JSON.stringify(newRequestObj);

    $.ajax({
        type: "POST",
        url: "/Helpers/WebMethods.asmx/GetFileAttachmentsMeasurementsLowLevel",
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: RequestDataString,
        success: function (result) {
            var jsonResult = JSON.parse(result.d);
            if (jsonResult.status == "ok") {

                $(jsonResult.response).each(function () {
                    html += '<tr>';
                    html += '<td><a href="#" onclick="DownloadFileAttachment(' + this.fileAttachmentId + ')" title="' + this.filename + '">' + text_truncate(this.filename, 30, "...") + '</span></td>';
                    html += '<td>' + this.documentTypeName + '</td>';
                    html += '<td>' + this.testType;
                    if (this.testTypeSubcategory != '')
                        html += ' | ' + this.testTypeSubcategory
                    html += '</td>';
                    //html += '<td><span title="' + this.description + '">' + text_truncate(this.description, 40, "...") + '</td>';
                    html += '<td>' + this.extension + '</td>';
                    html += '<td>' + this.createdOn + '</td>';
                });

                if (jsonResult.response == null) {
                    html += '<tr><td colspan="6" class="text-center">There are no files</td></tr>';
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


    dialog = bootbox.dialog({
        title: 'Test results documents',
        message: html,
        closeButton: true,
        onEscape: true,
        size: "large"
    }).on('shown.bs.modal', function (e) {
        /*DocTypesOnShow();*/
    });
}

$(function () {


});