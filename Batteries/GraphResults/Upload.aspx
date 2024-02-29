<%@ Page Title="Upload test document" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Upload.aspx.cs" Inherits="Batteries.GraphResults.Upload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box" id="fieldSet" runat="server">
                    <div class="box-header with-border">
                        <h3 class="box-title">Upload test results document</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <input type="text" style="position: absolute; top: -1000px;" />
                                <asp:Label Style="text-align: right; font-size: 14px; margin-bottom: 5px;" runat="server" AssociatedControlID="TestResultsFiles" CssClass="col-sm-2 control-label" Text="Test Results file(s)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:FileUpload ID="TestResultsFiles" AllowMultiple="true" runat="server"></asp:FileUpload>
                                    <asp:RequiredFieldValidator CssClass="text-danger" ErrorMessage="Document upload is required" ControlToValidate="TestResultsFiles" runat="server" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-sm-2 control-label" style="text-align: right; font-size: 14px;">Test Procedure file</label>
                                <div class="col-sm-10">
                                    <asp:FileUpload ID="TestProcedureFile" AllowMultiple="false" runat="server"></asp:FileUpload>
                                </div>
                            </div>
                            <% if (bool.Parse(ConfigurationManager.AppSettings["uploadJsonDirectlyOption"]))
                                { %>
                            <div class="form-group">
                                <div class="col-sm-2" style="text-align: right; font-size: 14px;">
                                    <label></label>
                                </div>
                                <div class="col-sm-10">
                                    <asp:CheckBox ID="CbJsonDirectly" runat="server"
                                        AutoPostBack="False"
                                        Text=" Uploading json directly"
                                        TextAlign="Right" />
                                </div>
                            </div>
                            <% } %>
                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <label id="lblExperiment">
                                        <input type="radio" name="rb" value="experiment" id="rbExperiment">
                                        Experiment</label>
                                    &nbsp
                                    <label id="lblBatch">
                                        <input type="radio" name="rb" value="batch" id="rbBatch">
                                        Batch</label>
                                    &nbsp
                                    <label id="lblMaterial">
                                        <input type="radio" name="rb" value="material" id="rbMaterial">
                                        Material</label>
                                </div>
                            </div>
                            <div class="form-group hidden" id="divExperiment">
                                <asp:Label runat="server" AssociatedControlID="DdlExperiment" CssClass="col-sm-2 control-label" Text="Experiment"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfExperimentSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlExperiment" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlExperiment"
                                        CssClass="text-danger" ErrorMessage="Please select experiment." Display="Dynamic" /> --%>
                                </div>
                            </div>
                            <div class="form-group hidden" id="divBatch">
                                <asp:Label runat="server" AssociatedControlID="DdlBatch" CssClass="col-sm-2 control-label" Text="Batch"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfBatchSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlBatch" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="rfvBatch" runat="server" ControlToValidate="DdlBatch"
                                        CssClass="text-danger" ErrorMessage="Please select batch." Display="Dynamic"/> --%>
                                </div>
                            </div>
                            <div class="form-group hidden" id="divMaterial">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterial" CssClass="col-sm-2 control-label" Text="Material"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfMaterialSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlMaterial" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <%-- <asp:RequiredFieldValidator ID="rfvBatch" runat="server" ControlToValidate="DdlBatch"
                                        CssClass="text-danger" ErrorMessage="Please select batch." Display="Dynamic"/> --%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTestType" CssClass="col-sm-2 control-label" Text="Test Type"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfTestTypeSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlTestType" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DdlTestType"
                                        CssClass="text-danger" ErrorMessage="Please select test type." Display="Dynamic" />
                                    <label id="txtMessage" style="margin-top: 10px;" class="text-danger hidden">The viewer doesn’t support these file formats yet. But you can upload the raw data and later download them in the experiments list</label>

                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTestEquipmentModel" CssClass="col-sm-2 control-label" Text="Test Equipment Model"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfTesEquipmentModelSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlTestEquipmentModel" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlTestEquipmentModel"
                                        CssClass="text-danger" ErrorMessage="Please select test equipment model." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtTestLabel" CssClass="col-sm-2 control-label" Text="Test Label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtTestLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtComment" CssClass="col-sm-2 control-label" Text="Comment"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtComment" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="BtnInsert" Text="Upload" CssClass="btn btn-primary" OnClientClick="return ConfirmUploadDialog(this)" OnClick="BtnInsert_Click" />
                                <%--<asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>
        <script src="<%= ResolveUrl("~/Scripts/sweeralert2.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/files.js")%>"></script>
        <script>

            $(function () {
                FillPreselectedExperiment();
            })
            reqUrl = "/Helpers/WebMethods.asmx/GetCompleteExperimentsPaged";
            $('#<%= DdlExperiment.ClientID %>').select2({
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
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.experimentId,
                                    text: item.experimentSystemLabel + " | " + item.experimentPersonalLabel,
                                    /* goal: item.projectDescription,*/
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
                //dropdownParent: dialog,
                width: "100%",
                //theme: "bootstrap",
                placeholder: 'Search for experiment',
                allowClear: true,
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                //templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });

            /* function formatRepo(repo) {
                 var markup = "" + repo.text + ' | ' + repo.goal;
                 return markup;
             }*/

            /*  function formatRepoSelection(repo) {
                  return repo.text;
              }*/

            $('#<%= DdlExperiment.ClientID %>').on('change', function () {
                $('#<%=HfExperimentSelectedValue.ClientID%>').val($(this).val());
            });

            reqUrl = "/Helpers/WebMethods.asmx/GetBatchForMeasurementsDropdown";
            $('#<%= DdlBatch.ClientID %>').select2({
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
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.batchId,
                                    text: item.batchSystemLabel + " | " + item.batchPersonalLabel,
                                    /* goal: item.projectDescription,*/
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
                //dropdownParent: dialog,
                width: "100%",
                //theme: "bootstrap",
                placeholder: 'Search for batch',
                allowClear: true,
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                //templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });


            /* function formatRepo(repo) {
                 var markup = "" + repo.text + ' | ' + repo.goal;
                 return markup;
             }*/

            /*  function formatRepoSelection(repo) {
                  return repo.text;
              }*/

            $('#<%= DdlBatch.ClientID %>').on('change', function () {
                $('#<%=HfBatchSelectedValue.ClientID%>').val($(this).val());
            });

            reqUrl = "/Helpers/WebMethods.asmx/GetMaterials";
            $('#<%= DdlMaterial.ClientID %>').select2({
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
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.materialId,
                                    text: item.materialName + " | " + item.materialLabel,
                                    data: item
                                };
                            }),
                            pagination: {
                                "more": response.pagination.more
                            }
                        };
                    },
                },
                width: "100%",
                placeholder: 'Search for material',
                allowClear: true,
                escapeMarkup: function (markup) { return markup; }
            });

            $('#<%= DdlMaterial.ClientID %>').on('change', function () {
                $('#<%=HfMaterialSelectedValue.ClientID%>').val($(this).val());
            });


            var reqUrlTestTypes = "/Helpers/WebMethods.asmx/GetTestTypes";
            $('#<%= DdlTestType.ClientID %>').select2({
                ajax: {
                    type: "POST",
                    url: reqUrlTestTypes,
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
                                    id: item.testTypeId,
                                    text: item.testType + " | " + item.testTypeSubcategory,
                                    data: item
                                };
                            })
                        };
                    },
                    //cache: true
                },
                // dropdownParent: dialog,
                width: "100%",
                //theme: "bootstrap",
                allowClear: true,
                placeholder: 'Search for test type',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            });

            $('#<%= DdlTestType.ClientID %>').on('change', function () {
                $('#<%=HfTestTypeSelectedValue.ClientID%>').val($(this).val());
                if ($(this).val() != null) {
                    var supportsGraphing = $('#<%=DdlTestType.ClientID%>').select2('data')[0].data.supportsGraphing;
                    if (supportsGraphing == false) {
                        $('#txtMessage').removeClass("hidden");
                    }
                    else {
                        $('#txtMessage').addClass("hidden");
                    }
                }
            });

            var reqUrlTestEquipmentModels = "/Helpers/WebMethods.asmx/GetTestEquipmentModels";
            $('#<%= DdlTestEquipmentModel.ClientID %>').select2({
                ajax: {
                    type: "POST",
                    url: reqUrlTestEquipmentModels,
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
                            type: 'public',
                            page: params.page || 1
                        });
                    },
                    processResults: function (data, params) {
                        //console.log(JSON.parse(data.d).results);
                        var response = JSON.parse(data.d);
                        return {
                            results: $.map(response.results, function (item) {
                                return {
                                    id: item.testEquipmentModelId,
                                    text: item.brandName + " | " + item.testEquipmentModelName
                                };
                            }),
                            pagination: {
                                "more": response.pagination.more
                            }
                        };
                    },
                    //cache: true
                },
                // dropdownParent: dialog,
                width: "100%",
                allowClear: true,
                //theme: "bootstrap",
                placeholder: 'Search for test equipment model',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            });
            $('#<%= DdlTestEquipmentModel.ClientID %>').on('change', function () {
                $('#<%=HfTesEquipmentModelSelectedValue.ClientID%>').val($(this).val());
            });

            $(function () {
                /*    $('#divBatch').addClass("hidden");*/
                $('input[name="rb"]').on('change', function () {
                    if ($(this).val() == 'experiment') {
                        $('#divBatch').addClass("hidden");
                        $('#divExperiment').removeClass("hidden");
                        $('#divMaterial').addClass("hidden");
                    }
                    else if ($(this).val() == 'batch') {
                        $('#divBatch').removeClass("hidden");
                        $('#divExperiment').addClass("hidden");
                        $('#divMaterial').addClass("hidden");
                    }
                    else if ($(this).val() == 'material') {
                        $('#divBatch').addClass("hidden");
                        $('#divExperiment').addClass("hidden");
                        $('#divMaterial').removeClass("hidden");
                    }
                });
                $('input[name="rb"]').on('change', function () {
                    $('#<%= DdlExperiment.ClientID %>').val('').trigger('change');
                    $('#<%= DdlBatch.ClientID %>').val('').trigger('change');
                    $('#<%= DdlMaterial.ClientID %>').val('').trigger('change');
                });
            });

            function getUrlVars() {
                var vars = {};
                var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value.replace('#', '');
                });
                return vars;
            }

            function FillPreselectedExperiment() {

                var experimentId = getUrlVars()["exp"];
                var batchId = getUrlVars()["btc"];
                var materialId = getUrlVars()["mat"];

                if (experimentId != undefined) {
                    $('#divExperiment').removeClass("hidden");
                    $('#lblBatch').addClass("hidden");
                    $('#lblMaterial').addClass("hidden");
                    //$('#divBatch').addClass("hidden");
                    //$('#divExperiment').removeClass("hidden");

                    $("input[type='radio'][value='experiment']").prop('checked', true).trigger('change');
                    var jsonRequestObject = new Object();
                    jsonRequestObject.experimentId = experimentId;

                    var RequestDataString = JSON.stringify(jsonRequestObject);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetExperimentById",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //console.log(result.d)
                            var res = JSON.parse(result.d);
                            if (res != null) {
                                var item = res;
                                var text = item.experimentSystemLabel + " | " + item.experimentPersonalLabel;
                                var id = item.experimentId;
                                //var option = new Option("bla", 2, true, true);
                                var option = new Option(text, id, true, true);
                                $('#<%= DdlExperiment.ClientID%>').append(option).trigger('change');
                            }
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }
                else if (batchId != undefined) {
                    $('#divBatch').removeClass("hidden");
                    $('#lblExperiment').addClass("hidden");
                    $('#lblMaterial').addClass("hidden");
                    $("input[type='radio'][value='batch']").prop('checked', true).trigger('change');
                    //$("input[type='radio'][value='batch']").click();
                    var jsonRequestObject = new Object();
                    jsonRequestObject.batchId = batchId;

                    var RequestDataString = JSON.stringify(jsonRequestObject);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetBatchById",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //console.log(result.d)
                            var res = JSON.parse(result.d);
                            if (res != null) {
                                var item = res;
                                var text = item.batchSystemLabel + " | " + item.batchPersonalLabel;
                                var id = item.batchId;
                                //var option = new Option("bla", 2, true, true);
                                var option = new Option(text, id, true, true);
                                $('#<%= DdlBatch.ClientID%>').append(option).trigger('change');
                            }
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }
                else if (materialId != undefined) {
                    $('#divMaterial').removeClass("hidden");
                    $('#divBatch').addClass("hidden");
                    $('#lblExperiment').addClass("hidden");
                    $('#lblBatch').addClass("hidden");
                    $("input[type='radio'][value='material']").prop('checked', true).trigger('change');
                    //$("input[type='radio'][value='batch']").click();
                    var jsonRequestObject = new Object();
                    jsonRequestObject.materialId = materialId;

                    var RequestDataString = JSON.stringify(jsonRequestObject);

                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetMaterialById",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //console.log(result.d)
                            var res = JSON.parse(result.d);
                            var item = res;
                            var text = item.materialName + " | " + item.materialLabel;
                            var id = item.materialId;
                            //var option = new Option("bla", 2, true, true);
                            var option = new Option(text, id, true, true);
                            $('#<%= DdlMaterial.ClientID%>').append(option).trigger('change');
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }
                else {
                    $("input[type='radio'][value='experiment']").prop('checked', true).trigger('change');
                    $('#divExperiment').removeClass("hidden");
                    $('#divMaterial').addClass("hidden");
                    $('#divBatch').addClass("hidden");
                    $('#lblExperiment').removeClass("hidden");
                    $('#lblBatch').removeClass("hidden");
                }
            };

            function successMessage() {
                Swal.fire({
                    icon: 'success',
                    title: 'Successful import',
                    showConfirmButton: true,
                    //timer: 2500
                })
            }
        </script>
        <script type="text/javascript">
            function RefreshParent() {
                if (window.opener != null && !window.opener.closed) {
                    window.opener.location.reload();
                }
            }
            //window.onbeforeunload = RefreshParent;
        </script>

        <script type="text/javascript">
            function ConfirmUploadDialog(sender) {
                Page_ClientValidate();
                if (Page_IsValid) {
                    //if (!ValidateForm()) return false;
                    if ($(sender).attr("confirmed") == "true") { return true; }

                    if ($('#<%=HfTestTypeSelectedValue.ClientID%>').val() != "") {
                        var jsonRequestObject = {};
                        jsonRequestObject["testTypeId"] = $('#<%=HfTestTypeSelectedValue.ClientID%>').val();
                        jsonRequestObject["experimentId"] = $('#<%=HfExperimentSelectedValue.ClientID%>').val();
                        jsonRequestObject["batchId"] = $('#<%=HfBatchSelectedValue.ClientID%>').val();
                        jsonRequestObject["materialId"] = $('#<%=HfMaterialSelectedValue.ClientID%>').val();
                        var RequestDataString = JSON.stringify(jsonRequestObject);

                        $.ajax({
                            type: "POST",
                            url: "<%= Page.ResolveUrl("~/Helpers/WebMethods.asmx/GetFileAttachmentsMeasurements")%>",
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: RequestDataString,
                            success: function (result) {
                                var jsonResult = JSON.parse(result.d);
                                if (jsonResult.status == "ok") {
                                    if (jsonResult.response == null) {
                                        $(sender).attr("confirmed", true);
                                        sender.click();
                                        return true;
                                    }

                                    html = "";
                                    html += '<h4>The following files for this test type will be removed and replaced with the new ones: </h4>';
                                    html += "<br />";

                                    //html += '<div class="table-responsive">';
                                    html += '<div class="">' +
                                        '<table class="table table-condensed table-responsive table-striped table-bordered table-hover text-center" style="width:100%">' +
                                        '<thead>' +
                                        '<th>Filename</th>' +
                                        '<th>Document type</th>' +
                                        '<th>Test type</th>' +
                                        //'<th>Description</th>' +
                                        '<th>Extension</th>' +
                                        '<th>Date Added</th>' +
                                        '</thead>' +
                                        '<tbody>';

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

                                    html += '</tbody></table></div></div></div>';

                                    html += '<h4>Are you sure you want to proceed?</h4>';

                                    bootbox.confirm({
                                        title: "Confirm",
                                        message: html,
                                        buttons: {
                                            cancel: {
                                                label: 'No'
                                            },
                                            confirm: {
                                                label: 'Yes'
                                            }
                                        },
                                        callback: function (result) {
                                            if (result) {
                                                $(sender).attr("confirmed", true);
                                                sender.click();
                                            }
                                        }
                                    });


                                } else {
                                    notify(escapeHtml(result.d.message), "warning");
                                }
                            },
                            error: function (p1, p2, p3) {
                                alert(p1.status);
                                alert(p3);
                            }
                        });
                    }



                }
                return false;
            }
        </script>
    </section>
</asp:Content>
