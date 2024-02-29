<%@ Page Title="Test Results Graphs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefaultOldOld.aspx.cs" Inherits="Batteries.GraphResults.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-lg-10 col-lg-offset-1">
                <%--<asp:HyperLink runat="server" NavigateUrl="Insert" CssClass="btn btn-primary" Text="Show Plots" />--%>
                <%--<a href="#" class="btn btn-primary" onclick="ShowPlots()">Show Plots</a>--%>
            </div>
        </div>
        <br />

        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->

                        <%-- hidden field to take the default focus when opening page --%>
                        <%--<asp:TextBox ID="TxtHidden" runat="server" CssClass="form-control" Visible="true"></asp:TextBox>--%>
                        <input type="text" name="TxtHidden" value=" " style="position: relative; z-index: -1;" />

                        <div class="box-body">
                             <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlProject" CssClass="col-sm-2 control-label" Text="Project"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlProject" runat="server" ItemType="" CssClass="form-control"> </asp:DropDownList>                                   
                                </div>
                            </div>             


                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlResearchGroup" Visible="false" CssClass="col-sm-2 control-label" Text="Research Group"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlResearchGroup" runat="server" Visible="false" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTestGroup" Visible="false" CssClass="col-sm-2 control-label" Text="Test Group"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlTestGroup" runat="server" Visible="false" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlOperator" Visible="false" CssClass="col-sm-2 control-label" Text="Created by"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlOperator" runat="server" Visible="false" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlExperiments" CssClass="col-sm-2 control-label" Text="Experiments"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlExperiments" runat="server" CssClass="form-control" name="experiments[]" multiple="multiple"></asp:DropDownList>
                                </div>

                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMassType" CssClass="col-sm-2 control-label" Text="Mass type"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMassType" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div class="box-footer no-border">
                            <div class="pull-right">
                                <a href="#" class="btn btn-primary" onclick="ShowPlots()">Show Plots</a>
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div id="summary" class="box box-solid hidden">
                    <div class="box-header with-border">
                        <h3 class="box-title"></h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                                    <table id="summaryTable" class="table table-hover">
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="box-footer no-border">
                            <div class="pull-right">
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 1</h3>
                        <%--<div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(1)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->

                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD1" height="23vh" width="50vw"></div>
                                </div>
                            </div>

                        </div>



                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 2</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD2" height="23vh" width="50vw"></div>
                                </div>
                            </div>

                        </div>

                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 3</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD3Control"></div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD3" height="30vh" width="20vw"></div>
                                </div>
                            </div>

                        </div>

                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 4</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD4" height="23vh" width="50vw"></div>
                                </div>
                            </div>

                        </div>

                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-lg-10 col-lg-offset-1">
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 5</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <%--<div class="table-responsive">--%>
                                        <div id="chartCD5Legend"></div>
                                        <div id="chartCD5" height="23vh" width="50vw"></div>
                                    <%--</div>--%>
                                </div>
                            </div>

                        </div>

                        <div class="box-footer no-border">
                            <div class="">
                                <%--<input type="button" id="finishExperimentBtn" value="Finish" class="btn btn-block btn-default" onclick="" />--%>
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->
            </div>
        </div>

        <!-- Plot.ly JS -->
        <script src="<%= ResolveUrl("~/Scripts/plotly/plotly-latest.min.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/graphs.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>

                  

        <script>
           
            var reqUrl = "/Helpers/WebMethods.asmx/GetResearchGroups";
            $('#<%= DdlResearchGroup.ClientID %>').select2({
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
                                    id: item.researchGroupId,
                                    text: item.researchGroupName,
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
                placeholder: 'Select research group',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                //templateSelection: formatRepoSelection,
                allowClear: true
            });

            function formatRepo(repo) {
                var markup = "" + repo.text;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }

            reqUrl = "/Helpers/WebMethods.asmx/GetTestGroups";
            $('#<%= DdlTestGroup.ClientID %>').select2({
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
                            researchGroupId: $('#<%= DdlResearchGroup.ClientID %>').val(),
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
                                    id: item.testGroupId,
                                    text: item.testGroupName,
                                    goal: item.testGroupGoal,
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
                placeholder: 'Select test group',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatTestGroup,
                allowClear: true,
                //templateSelection: formatRepoSelection
            });

            function formatTestGroup(repo) {
                var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }


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
                                    text: item.projectName,
                                    goal: item.projectDescription,
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
                var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }







            reqUrl = "/Helpers/WebMethods.asmx/GetUsers";
            $('#<%= DdlOperator.ClientID %>').select2({
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
                            researchGroupId: $('#<%= DdlResearchGroup.ClientID %>').val(),
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
                                    id: item.userId,
                                    text: item.userName
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
                placeholder: 'Select operator',
                escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                //minimumInputLength: 1,
                templateResult: formatRepo,
                allowClear: true,
                //templateSelection: formatRepoSelection
            });

            $(function () {

                //var testGroupId = 3;
                var researchGroupId = null;
                var operatorId = null;

                reqUrlExperiments = "/Helpers/WebMethods.asmx/GetExperimentsByProjectForCharts";
                $('#<%= DdlExperiments.ClientID %>').select2({
                    ajax: {
                        type: "POST",
                        url: reqUrlExperiments,
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
                                projectId: $('#<%= DdlProject.ClientID %>').val(),
                              <%--  operatorId: $('#<%= DdlOperator.ClientID %>').val(),
                                researchGroupId: $('#<%= DdlResearchGroup.ClientID %>').val(),--%>
                                type: 'public',
                                page: params.page || 1
                            });
                        },
                        processResults: function (data, params) {
                            var response = JSON.parse(data.d);
                            return {
                                results: $.map(response.results, function (item) {
                                    return {
                                        id: item.experimentId,
                                        text: item.experimentSystemLabel + " | " + item.experimentPersonalLabel,
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
                    //dropdownParent: dialog,
                    width: "100%",
                    //theme: "bootstrap",
                    placeholder: 'Search through experiments',
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    //minimumInputLength: 1,
                    templateResult: formatExperiment,
                    //templateSelection: formatRepoSelection,
                    closeOnSelect: false,
                    allowClear: true
                });
                function formatExperiment(e) {
                    //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                    var markup = e.text;
                    return markup;
                }
                var scrollTop;
                $('#<%= DdlExperiments.ClientID %>').on("select2:selecting", function (event) {
                    var $pr = $(event.params.args.originalEvent.target).parent();
                    scrollTop = $pr.prop('scrollTop');
                });
                $('#<%= DdlExperiments.ClientID %>').on("select2:select", function (event) {
                    var $pr = $(event.params.originalEvent.target).parent();
                    $pr.prop('scrollTop', scrollTop);
                });
                $('#<%= DdlExperiments.ClientID %>').on("select2:unselecting", function (event) {
                    var $pr = $(event.params.args.originalEvent.target).parent();
                    scrollTop = $pr.prop('scrollTop');
                    var unselected_value = event.params.args.data.id;
                    var wanted_option = $('#<%= DdlExperiments.ClientID %>' + ' option[value="' + unselected_value + '"]');
                    wanted_option.prop('selected', false);
                    $('#<%= DdlExperiments.ClientID %>').trigger('change.select2');
                });
                $('#<%= DdlExperiments.ClientID %>').on("select2:unselect", function (event) {
                    var $pr = $(event.params.originalEvent.target).parent();
                    $pr.prop('scrollTop', scrollTop);
                    <%--var unselected_value = event.params.data.id;
                    var wanted_option = $('#<%= DdlExperiments.ClientID %>' + ' option[value="' + unselected_value + '"]');
                    wanted_option.prop('selected', false);
                    $('#<%= DdlExperiments.ClientID %>').trigger('change.select2');
                    console.log('unselected value : ' + unselected_value);--%>
                });

                $('#<%= DdlExperiments.ClientID%>').on('change', function () {

                    //console.log($(this).val());
                    selectedExperiments = $(this).val();
                    //console.log(selectedExperiments);
                });

                $('#<%= DdlMassType.ClientID%>').select2({
                    //allowClear: true,
                    placeholder: 'Select mass',
                    width: "100%",
                });
                $('#<%= DdlMassType.ClientID%>').on('change', function () {
                    //console.log($(this).val());
                    massSelection = $(this).val();
                });
                $('#<%= DdlResearchGroup.ClientID%>').on('change', function () {
                    $('#<%= DdlOperator.ClientID%>').val("").trigger('change');
                })




                //chartCD1
                //var chartCD1El = document.getElementById("chartCD1");
                //chartCD1 = new Chart(chartCD1El, {
                //    type: 'scatter',
                //    data: chartCD1Data,
                //    options: chartCD1Options
                //});

                //var trace1 = {
                //    x: [1, 2, 3, 4],
                //    y: [12, 9, 15, 12],
                //    mode: 'lines+markers',
                //    name: 'Scatter',
                //    marker: {
                //        color: 'rgb(219, 64, 82)',
                //        size: 12
                //    },
                //    line: {
                //        color: 'rgb(55, 128, 191)',
                //        width: 3
                //    }
                //};
                //var trace2 = {
                //    x: [2, 3, 4, 5],
                //    y: [16, 5, 11, 9],
                //    mode: 'lines+markers',
                //    name: 'name2'
                //};
                //var layout = {
                //    title: 'Line and Scatter Plot',
                //    xaxis: {
                //        title: 'GDP per Capita',
                //        //showgrid: false,
                //        //zeroline: false
                //    },
                //    yaxis: {
                //        title: 'Percent',
                //        //showline: false
                //    },
                //    legend: {
                //        //y: 0.5,
                //        //traceorder: 'reversed',
                //        font: {
                //            //size: 16
                //        }
                //    },
                //    hovermode: 'closest',
                //};

                //var data = [trace1, trace2];


                //var chartCD1 = Plotly.plot(chartCD1El, [{
                //    x: [1, 2, 3, 4, 5],
                //    y: [1, 2, 4, 8, 16]
                //}], {
                //    margin: { t: 0 }
                //});



                //var chartCD1 = Plotly.plot(chartCD1El, data, layout);


                FillPreselectedExperiments();
            });

            function FillPreselectedExperiments() {
                var urlParams = new URLSearchParams(window.location.search);

                var experimentIdsArray = [];

                if (urlParams.has('exp')) {
                    var ids = urlParams.get('exp');
                    if (ids != "")
                        experimentIdsArray = ids.split(",");
                    //console.log(experimentIdsArray);

                    var newRequestObj = new Object();
                    newRequestObj.experimentIdArray = experimentIdsArray;
                    newRequestObj.researchGroupId = null;
                    //research group id is taken inside the web method
                    var RequestDataString = JSON.stringify(newRequestObj);


                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetExperimentsByRGId",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //console.log(result.d)
                            var res = JSON.parse(result.d);
                            for (var i in res) {
                                var item = res[i];
                                var text = item.experimentSystemLabel + " | " + item.experimentPersonalLabel;
                                var id = item.experimentId;
                                //var option = new Option("bla", 2, true, true);
                                var option = new Option(text, id, true, true);
                                $('#<%= DdlExperiments.ClientID%>').append(option).trigger('change');

                                ShowPlots();
                            }

                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });

                     $.each(experimentIdsArray, function (i) {
                         console.log(experimentIdsArray[i]);
                     });
                 }
             }

           <%-- function FillPreselectedExperiments() {
                var urlParams = new URLSearchParams(window.location.search);

                var experimentIdsArray = [];

                if (urlParams.has('exp')) {
                    var ids = urlParams.get('exp');
                    if (ids != "")
                        experimentIdsArray = ids.split(",");
                    //console.log(experimentIdsArray);

                    var newRequestObj = new Object();
                    newRequestObj.experimentIdArray = experimentIdsArray;
                    newRequestObj.researchGroupId = null;
                    //research group id is taken inside the web method
                    var RequestDataString = JSON.stringify(newRequestObj);


                    $.ajax({
                        type: "POST",
                        url: "/Helpers/WebMethods.asmx/GetExperimentsById",
                        data: RequestDataString,
                        //async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            //console.log(result.d)
                            var res = JSON.parse(result.d);
                            for (var i in res) {
                                var item = res[i];
                                var text = item.experimentSystemLabel + " | " + item.experimentPersonalLabel;
                                var id = item.experimentId;
                                //var option = new Option("bla", 2, true, true);
                                var option = new Option(text, id, true, true);
                                $('#<%= DdlExperiments.ClientID%>').append(option).trigger('change');

                                ShowPlots();
                            }

                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });

                    $.each(experimentIdsArray, function (i) {
                        console.log(experimentIdsArray[i]);
                    });
                }
            }--%>
        </script>

    </section>
</asp:Content>
