<%@ Page Title="Test Results Graphs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefaultOld.aspx.cs" Inherits="Batteries.GraphResults.DefaultOld" %>

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


                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTestGroup" CssClass="col-sm-2 control-label" Text="Test Group"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlTestGroup" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlOperator" CssClass="col-sm-2 control-label" Text="Created by"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlOperator" runat="server" CssClass="form-control"></asp:DropDownList>
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
                <div class="box box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Graph 1</h3>
                        <div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(1)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <canvas id="chartCD1" height="23vh" width="50vw"></canvas>
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
                        <div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(2)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <canvas id="chartCD2" height="23vh" width="50vw"></canvas>
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
                        <div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(3)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>
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
                                    <canvas id="chartCD3" height="30vh" width="20vw"></canvas>
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
                        <div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(4)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <canvas id="chartCD4" height="23vh" width="50vw"></canvas>
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
                        <div class="pull-right">
                            <button type="button" class="btn btn-xs btn-default" title="Reset zoom" onclick="ResetZoom(5)">
                                <i class="fa fa-arrows-alt"></i>
                            </button>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <!-- /.box-body -->


                        <div class="box-body">
                            <div class="row">
                                <div class="col-sm-12">
                                    <div id="chartCD5Legend"></div>
                                    <canvas id="chartCD5" height="23vh" width="50vw"></canvas>
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

        <!-- ChartJS -->
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/chartjs/Chart.js")%>"></script>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/hammerjs/hammer.min.js")%>"></script>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/chartjs/chartjs-plugin-zoom.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/Pages/graphsOld.js")%>"></script>

        <script>
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

                        return JSON.stringify({ search: term });
                    },
                    processResults: function (data, params) {
                        // parse the results into the format expected by Select2
                        // since we are using custom formatting functions we do not need to
                        // alter the remote JSON data, except to indicate that infinite
                        // scrolling can be used

                        return {
                            results: $.map(JSON.parse(data.d), function (item) {
                                return {
                                    id: item.testGroupId,
                                    text: item.testGroupName,
                                    goal: item.testGroupGoal,
                                    data: item
                                };
                            })
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
                templateResult: formatRepo,
                allowClear: true,
                //templateSelection: formatRepoSelection
            });

            function formatRepo(repo) {
                var markup = "" + repo.text + ' | ' + repo.goal;
                return markup;
            }

            function formatRepoSelection(repo) {
                return repo.text;
            }

            
            $(function () {

                //var testGroupId = 3;
                var researchGroupId = null;
                var operatorId = null;

                reqUrlExperiments = "/Helpers/WebMethods.asmx/GetExperimentsByTestGroupAndOperator";
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
                            return JSON.stringify({ search: term, testGroupId: $('#<%= DdlTestGroup.ClientID %>').val(), operatorId: $('#<%= DdlOperator.ClientID %>').val(), researchGroupId: researchGroupId, type: 'public' });
                        },
                        processResults: function (data, params) {

                            return {
                                results: $.map(JSON.parse(data.d), function (item) {
                                    return {
                                        id: item.experimentId,
                                        text: item.experimentSystemLabel + " | " + item.experimentPersonalLabel,
                                        data: item
                                    };
                                })
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
                    //templateSelection: formatRepoSelection
                });
                function formatExperiment(e) {
                    //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                    var markup = e.text;
                    return markup;
                }




                <%--$('#<%= DdlExperiments.ClientID%>').select2({
                    //allowClear: true,
                    placeholder: 'Select experiments',
                    width: "100%",

                });--%>
                $('#<%= DdlExperiments.ClientID%>').on('change', function () {

                    //console.log($(this).val());
                    selectedExperiments = $(this).val();
                    //console.log(selectedExperiments);
                });

                $('#<%= DdlOperator.ClientID%>').select2({
                    allowClear: true,
                    placeholder: 'Select operator',
                    width: "100%",
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

                <%--experimentId = <%= experimentId %>;
                experiment = <%= experimentContent %>;
                RegenerateCompletedContents(experiment, false);--%>
                            

                <%--function makeChartCD1(mass, myChart) {
                    $.ajax({
                        type: "POST",
                        url: "<%= Page.ResolveUrl("~/TRA/Dashboards/StrategicTra.aspx/GetDashboard1")%>",
                        async: true,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: '{mun: "' + mun + '"}',
                        success: function (result) {
                            var jsonResult = JSON.parse(result.d);
                            var localData = [];

                            var item = itemExists(jsonResult, 'Batelco');
                            if (item != null) {
                                localData.push(item.Count);
                            } else {
                                localData.push(0);
                            }

                            item = itemExists(jsonResult, 'Mena Telecom');
                            if (item != null) {
                                localData.push(item.Count);
                            } else {
                                localData.push(0);
                            }

                            item = itemExists(jsonResult, 'Viva');
                            if (item != null) {
                                localData.push(item.Count);
                            } else {
                                localData.push(0);
                            }

                            item = itemExists(jsonResult, 'Zain');
                            if (item != null) {
                                localData.push(item.Count);
                            } else {
                                localData.push(0);
                            }


                            myChart.data.datasets[0].data = localData;
                            //myChart.config.centerText.text = localDataBefDl.reduce((a, b) => a + b, 0);
                            myChart.update();
                        },
                        error: function (p1, p2, p3) {
                            alert(p1.status);
                            alert(p3);
                        }
                    });
                }--%>







                // Load data for the charts
                //makeChartCD1('', chartCD1);


                //na klik na show  graphs
                //povikaj web metodi za vrakanje podatoci
                //napraj presmetka so podatocite funkcija
                //makeChart





            });
        </script>
    </section>
</asp:Content>
