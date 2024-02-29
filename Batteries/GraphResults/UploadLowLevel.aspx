<%@ Page Title="Upload test document" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="UploadLowLevel.aspx.cs" Inherits="Batteries.GraphResults.UploadLowLevel" %>

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

                            <asp:HiddenField ID="HfExperimentSelectedValue" runat="server" />
                            <asp:HiddenField ID="HfBatchSelectedValue" runat="server" />
                            <%--<div class="form-group" id="divExperiment">
                                <asp:Label runat="server" CssClass="col-sm-2 control-label" Text="Experiment"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfExperimentSelectedValue" runat="server" />

                                </div>
                            </div>
                            <div class="form-group" id="divBatch">
                                <asp:Label runat="server" CssClass="col-sm-2 control-label" Text="Batch"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfBatchSelectedValue" runat="server" />
                                </div>
                            </div>--%>
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
                //FillPreselectedExperiment();
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

            function getUrlVars() {
                var vars = {};
                var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
                    vars[key] = value;
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
                            var item = res;
                            var text = item.experimentSystemLabel + " | " + item.experimentPersonalLabel;
                            var id = item.experimentId;
                            //var option = new Option("bla", 2, true, true);
                            var option = new Option(text, id, true, true);

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
                            var item = res;
                            var text = item.batchSystemLabel + " | " + item.batchPersonalLabel;
                            var id = item.batchId;
                            //var option = new Option("bla", 2, true, true);
                            var option = new Option(text, id, true, true);
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }
                else {
                    //$("input[type='radio'][value='experiment']").prop('checked', true).trigger('change');
                    //$('#divExperiment').removeClass("hidden");
                    //$('#divMaterial').addClass("hidden");
                    //$('#divBatch').addClass("hidden");
                    //$('#lblExperiment').removeClass("hidden");
                    //$('#lblBatch').removeClass("hidden");
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
            function ConfirmUploadDialog(sender) {
                Page_ClientValidate();
                if (Page_IsValid) {
                    //if (!ValidateForm()) return false;
                    if ($(sender).attr("confirmed") == "true") { return true; }

                    if ($('#<%=HfTestTypeSelectedValue.ClientID%>').val() != "") {
                        var jsonRequestObject = {};
                        jsonRequestObject["testTypeId"] = $('#<%=HfTestTypeSelectedValue.ClientID%>').val();
                        jsonRequestObject["experimentId"] = <%= experimentId %>;
                        jsonRequestObject["componentTypeId"] = <%= componentTypeId %>;
                        jsonRequestObject["stepId"] = <%= stepId %>;
                        var RequestDataString = JSON.stringify(jsonRequestObject);

                        $.ajax({
                            type: "POST",
                            url: "<%= Page.ResolveUrl("~/Helpers/WebMethods.asmx/GetFileAttachmentsMeasurementsLowLevel")%>",
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
