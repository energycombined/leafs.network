<%@ Page Title="New Batch" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Batches.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="modal modal-primary fade" id="modal-primary">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">×</span></button>
                        <h4 class="modal-title">Primary Modal</h4>
                    </div>
                    <div class="modal-body">
                        <p>One fine body…</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-outline pull-left" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-outline">Save changes</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">New Batch</h3>
                        <%--<legend>Batch General Info</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                             <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlProject" CssClass="col-sm-2 control-label" Text="Project"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfProjectSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlProject" runat="server" ItemType="" CssClass="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlProject"
                                        CssClass="text-danger" ErrorMessage="Please assign batch to a project." Display="Dynamic" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="BtnNewProject" CausesValidation="false" Text="New Project" target="_blank" runat="server" CssClass="btn btn-default" href="/Projects/Insert" />
                                </div>
                            </div> 
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTemplate" CssClass="col-sm-2 control-label" Text="Template"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfDdlTemplateSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlTemplate" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <a id="viewBatchLink" target="_blank" class="btn btn-default disabled" href="<%= Page.ResolveUrl("~/Batches/View/") %>">View Details</a>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />
                            <div class="pull-right">                                
                                <asp:Button runat="server" ID="BtnStart" Text="Start Batch" CssClass="btn btn-lg btn-primary" OnClick="BtnStart_Click" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>

        <div id="batchContent">

        </div>
        
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/Forms/form-helpers.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script>
            formsFolderURL = "<%= ResolveClientUrl("~/Forms")%>"; //experiment.js
        </script>
        <script type="text/javascript">
                                    
            var reqUrlBatches = "/Helpers/WebMethods.asmx/GetBatches";
            $('#<%= DdlTemplate.ClientID %>').select2({
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
                width: "100%",
                //theme: "bootstrap",
                allowClear: true,
                placeholder: 'No template selected',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                //templateResult: formatBatch,
                //templateSelection: formatRepoSelection
            });

            $('#<%= DdlTemplate.ClientID %>').change(function () {
                var selected = $('#<%= DdlTemplate.ClientID %>').val();
                $('#<%=HfDdlTemplateSelectedValue.ClientID%>').val($(this).val());

                if (selected != null) {
                    $("#viewBatchLink").attr("href", "<%= Page.ResolveUrl("~/Batches/View/") %>" + selected);
                    $("#viewBatchLink").removeClass("disabled");
                }
                else {
                    $("#viewBatchLink").addClass("disabled");
                }

            });

            $(function () {

                
                //$('.select2').select2({
                //    width: "100%",
                //    placeholder: 'Select',
                //    allowClear: true
                //    //theme: "bootstrap"
                //});
            });

            reqUrl = "/Helpers/WebMethods.asmx/GetProjectsForResearchGroup";
            $('#<%= DdlProject.ClientID %>').select2({
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
                                    id: item.projectId,
                                    text: item.projectAcronym,
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
                placeholder: 'Select project',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                //templateSelection: formatRepoSelection
            });

            function formatRepo(repo) {
                var markup = "" + repo.text;
                //var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }

            $('#<%= DdlProject.ClientID %>').on('change', function () {
                  $('#<%=HfProjectSelectedValue.ClientID%>').val($(this).val());
              });
        </script>


    </section>
</asp:Content>
