<%@ Page Title="Edit Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Edit.aspx.cs" Inherits="Batteries.Projects.Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="container-fluid">

        <section class="content-header">
            <h1>Edit
                        <small>Edit/Preview of project</small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="<%= ResolveUrl("~/Projects/")%>"><i class="fa fa-building-o"></i>List of Projects</a></li>
                <li class="active">Edit</li>
            </ol>
        </section>

        <section class="col-lg-8 col-lg-offset-2 connectedSortable">
            <!-- Box (with bar chart) -->
            <div class="box box-solid box-danger collapsed-box" id="loading-example">
                <div class="box-header">
                    <!-- tools box -->
                    <div class="pull-right box-tools">
                        <%--<button class="btn btn-danger btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                        <button class="btn btn-danger btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-plus"></i></button>
                        <%--<button class="btn btn-danger btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                    </div>
                    <!-- /. tools -->
                    <i class="fa fa-building-o"></i>

                    <h3 class="box-title">Edit project</h3>
                </div>
                <!-- /.box-header -->
                <div class="box-body" style="padding: 50px;">
                    <div class="row">
                        <fieldset class="form-horizontal">
                            <div class="box-body">
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-4 control-label" Text="Project Title:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                            CssClass="text-danger" ErrorMessage="Project title is required." Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtAcronym" CssClass="col-sm-4 control-label" Text="Acronym:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtAcronym" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAcronym"
                                            CssClass="text-danger" ErrorMessage="Acronym is required." Display="Dynamic" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtAdminCoor" CssClass="col-sm-4 control-label" Text="Administrative Coordinator (Organisation):"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtAdminCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminCoor"
                                            CssClass="text-danger" ErrorMessage="Administrative Coordinator is required." Display="Dynamic" />--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtAdminContact" CssClass="col-sm-4 control-label" Text="Administrative Coordinator, contact person:"></asp:Label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TxtAdminContact" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminContact"
                                            CssClass="text-danger" ErrorMessage="Administrative Coordinator, contact person is required." Display="Dynamic" />--%>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                            <asp:TextBox ID="TxtAdminContactMail" type="email" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAdminContactMail"
                                                CssClass="text-danger" ErrorMessage="Administrative Coordinator, contact person email is required." Display="Dynamic" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtTechCoor" CssClass="col-sm-4 control-label" Text="Technical Coordinator (Organisation):"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtTechCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoor"
                                            CssClass="text-danger" ErrorMessage="Technical Coordinator is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtTechCoorContact" CssClass="col-sm-4 control-label" Text="Technical Coordinator, contact person:"></asp:Label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TxtTechCoorContact" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoorContact"
                                            CssClass="text-danger" ErrorMessage="Technical Coordinator, contact person is required." Display="Dynamic" />--%>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                            <asp:TextBox ID="TxtTechCoorMail" type="email" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtTechCoorMail"
                                                CssClass="text-danger" ErrorMessage="Technical Coordinator, contact person mail is required." Display="Dynamic" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtInnManager" CssClass="col-sm-4 control-label" Text="Innovation manager:"></asp:Label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TxtInnManager" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-phone-square"></i></span>
                                            <asp:TextBox ID="TxtInnManagerContact" runat="server" CssClass="form-control" placeholder="Contact person"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtDissCoor" CssClass="col-sm-4 control-label" Text="Dissemination Coordinator:"></asp:Label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="TxtDissCoor" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-phone-square"></i></span>
                                            <asp:TextBox ID="TxtDissCoorContact" runat="server" CssClass="form-control" placeholder="Contact person"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtGrantFund" CssClass="col-sm-4 control-label" Text="Grant funding organisation: "></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtGrantFund" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                            CssClass="text-danger" ErrorMessage="Grant funding organisation is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtFundProg" CssClass="col-sm-4 control-label" Text="Funding programme:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtFundProg" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                            CssClass="text-danger" ErrorMessage="Funding programme is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtCallIden" CssClass="col-sm-4 control-label" Text="Call identifier:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtCallIden" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCallIden"
                                            CssClass="text-danger" ErrorMessage="Call identifier is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtCallTop" CssClass="col-sm-4 control-label" Text="Call Topic:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtCallTop" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtCallTop"
                                            CssClass="text-danger" ErrorMessage="Call Topic is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="DdlFixedKey" CssClass="col-sm-4 control-label" Text="Fixed Keywords:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:DropDownList ID="DdlFixedKey" runat="server" CssClass="form-control select2">
                                            <asp:ListItem>Sodium-ion battery</asp:ListItem>
                                            <asp:ListItem>Lithium-ion battery</asp:ListItem>
                                            <asp:ListItem>Flow-battery</asp:ListItem>
                                            <asp:ListItem>PEM fuel cell</asp:ListItem>
                                            <asp:ListItem>Hydrogen storage </asp:ListItem>
                                        </asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlFixedKey"
                                            CssClass="text-danger" ErrorMessage="Fixed Keywords is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtFreeKey" CssClass="col-sm-4 control-label" Text="Free Keywords:"></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtFreeKey" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtFreeKey"
                                            CssClass="text-danger" ErrorMessage="Free Keywords is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Start of the project: </label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="TxtStartDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtStartDate"
                                                CssClass="text-danger" ErrorMessage="Start of the project is required." Display="Dynamic" />--%>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label">Duration-End Date:</label>
                                    <div class="col-sm-3">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="TxtEndDate" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                            <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtEndDate"
                                                CssClass="text-danger" ErrorMessage="End date of the project is required." Display="Dynamic" />--%>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" AssociatedControlID="TxtGoal" CssClass="col-sm-4 control-label" Text="Short summary "></asp:Label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="TxtGoal" MaxLength='1999' onkeyDown="checkTextAreaMaxLength(this,event,'1999');" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtGoal"
                                            CssClass="text-danger" ErrorMessage="Project Description is required." Display="Dynamic" />--%>
                                    </div>
                                </div>
                            </div>

                            <div class="pull-right">
                                <asp:Button runat="server" ID="UpdateButton" Text="Save" CssClass="btn btn-primary" OnClick="UpdateButton_OnClick" />
                                <asp:Button runat="server" ID="CancelButton" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="CancelButton_OnClick" />
                            </div>

                            <!-- /.box-body -->

                            <!-- /.box-footer -->
                        </fieldset>

                        <!-- /.col -->
                    </div>
                    <!-- /.row - inside box -->
                </div>
                <!-- /.box-body -->
            </div>
            <!-- /.box -->
        </section>

        <section class="col-lg-8 col-lg-offset-2 connectedSortable">
            <!-- Box (with bar chart) -->
            <div class="box  box-solid box-warning" id="loading-example2">
                <div class="box-header">
                    <!-- tools box -->
                    <div class="pull-right box-tools">
                        <%--<button class="btn btn-warning btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                        <button class="btn btn-warning btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-minus"></i></button>
                        <%--<button class="btn btn-warning btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                    </div>
                    <!-- /. tools -->
                    <i class="fa fa-flask"></i>

                    <h3 class="box-title">Experiment in project</h3>
                </div>
                <!-- /.box-header -->
                <div class="box-body" style="padding: 25px;">
                    <div class="row">
                        <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <div class="row">
                                    <div id="example1_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                        <div class="box-body">
                                            <div class="col-lg-12">
                                                <div class="table-responsive">
                                                    <table id="listDataTable" class="table text-center table-striped table-bordered table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>System Label
                                                                </th>
                                                                <th>Personal Label
                                                                </th>
                                                                <th>Created by
                                                                </th>
                                                                <th>Research group
                                                                </th>
                                                                <th>Date Created
                                                                </th>
                                                                <th>Action
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <%--  End row--%>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!-- /.col -->
                    </div>
                    <!-- /.row - inside box -->
                </div>
                <!-- /.box-body -->
                <%-- <div class="box-footer">
                    <div class="row">

                        <!-- ./col -->
                    </div>
                    <!-- /.row -->
                </div>--%>
                <!-- /.box-footer -->
            </div>
            <!-- /.box -->
        </section>

        <section class="col-lg-8 col-lg-offset-2 connectedSortable">
            <!-- Box (with bar chart) -->
            <div class="box box-solid box-primary" id="loading-example3">
                <div class="box-header">
                    <!-- tools box -->
                    <div class="pull-right box-tools">
                        <input type="button" id="addBatchBtn" value="Add batch to project" class="btn btn-warning" />
                        <%--<button class="btn btn-primary btn-xs refresh-btn" data-toggle="tooltip" title="Reload"><i class="fa fa-refresh"></i></button>--%>
                        <button class="btn btn-primary btn-xs" data-widget='collapse' data-toggle="tooltip"><i class="fa fa-minus"></i></button>
                        <%--<button class="btn btn-primary btn-xs" data-widget='remove' data-toggle="tooltip" title="Remove"><i class="fa fa-times"></i></button>--%>
                    </div>
                    <!-- /. tools -->
                    <i class="fa fa-tags"></i>
                    <h3 class="box-title">Batches in project</h3>
                </div>
                <!-- /.box-header -->
                <div class="box-body" style="padding: 25px;">
                    <div class="row">
                        <div id="collapseThree" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                            <div class="panel-body">
                                <div class="row">

                                    <div id="example2_wrapper" class="dataTables_wrapper form-inline dt-bootstrap">
                                        <div class="box-body">
                                            <div class="col-lg-12">
                                                <div class="table-responsive">
                                                    <table id="listDataTable2" class="table text-center table-striped table-bordered table-hover">
                                                        <thead>
                                                            <tr>
                                                                <%-- <th></th>--%>
                                                                <th>Batch ID</th>
                                                                <th>System label</th>
                                                                <th>Personal label</th>
                                                                <th>Chemical formula</th>
                                                                <%--<th>Incoming Batch ID
                                                                </th>--%>
                                                                <%--<th>Experiment ID
                                                                </th>
                                                                <th>Date Created
                                                                </th>--%>
                                                                <th>Action
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                        <%--  End row--%>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <!-- /.col -->
                    </div>
                    <!-- /.row - inside box -->
                </div>
                <!-- /.box-body -->
                <%--   <div class="box-footer">
                    <div class="row">
                      
                        <!-- ./col -->
                    </div>
                    <!-- /.row -->
                </div>--%>
                <!-- /.box-footer -->
            </div>
            <!-- /.box -->
        </section>


        <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
        <script src="<%= ResolveUrl("~/AdminLTE/plugins/datatables/datatables.min.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/jquery-ui-1.10.3.js")%>"></script>
        <script src="<%= ResolveUrl("~/Scripts/sorting.js")%>"></script>
        <script>
            $('.datepicker').datepicker({
                format: 'dd/mm/yyyy'
            });

            function checkTextAreaMaxLength(textBox, e, length) {

                var mLen = textBox["MaxLength"];
                if (null == mLen)
                    mLen = length;

                var maxLength = parseInt(mLen);
                if (!checkSpecialKeys(e)) {
                    if (textBox.value.length > maxLength - 1) {
                        if (window.event)//IE
                            e.returnValue = false;
                        else//Firefox
                            e.preventDefault();
                    }
                }
            }
            function checkSpecialKeys(e) {
                if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
                    return false;
                else
                    return true;
            }
        </script>

        <script type="text/javascript">

            var theTable2;

            function CreateBatchTable() {

                // DataTable
                theTable2 = $('#listDataTable2').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 4,
                            "orderable": false
                        },
                        //{ "type": "de_datetime", targets: [2] },
                    ],
                    //"order": [[2, "desc"]]
                    "order": []
                });
            }

            function RefreshTableData2(data) {
                var currentRG = <%= currentRG %>;
                var projectRgId = <%= projectRgId %>;
                theTable2.destroy();

                var res2 = JSON.parse(data);
                var output2 = "";


                for (var i in res2) {
                    //var commingExperiment = res2[i].fkComingExperiment ? res2[i].fkComingExperiment : '#';
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    /* var fkComingExperiment = res2[i].fkExperiment ? res2[i].fkExperiment : '#';*/
                    //if (currentRG == projectRgId)
                    //    var deleteBtn = '<a title="Remove" class="btn btn-danger btn-xs remove" onclick="OpenDeleteBatchInProjectForm(\'' + res2[i].projectBatchId + '\')">' + '<span class="fa fa-remove"></span>' + '</a> ';
                    //else
                    //        deleteBtn = '<a title="Remove" disabled="true" class="btn btn-danger btn-xs remove" onclick="OpenDeleteBatchInProjectForm(\'' + res2[i].projectBatchId + '\')">' + '<span class="fa fa-remove"></span>' + '</a> ';

                    output2 += '<tr ' + 'id="' + 'projectBatches' + res2[i].fkProject + '"' + '>' +
                        //'<td>' 
                        //output2 += deleteBtn + '</td>' +
                        '<td>' + res2[i].batchId + '</td>' +
                        '<td>' + res2[i].batchSystemLabel + '</td>' +
                        '<td>' + res2[i].batchPersonalLabel + '</td>' +
                        '<td>' + res2[i].chemicalFormula + '</td>' +
                        //'<td>' + res2[i].comingBatch + '</td>' +
                        //'<td>' + commingExperiment + '</td>' +
                        //'<td>' + res2[i].dateCreated + '</td>' +
                        //'<td>' + dateModified + '</td>' +
                        '<td class="table-actions-col">'
                    /* if (currentRG == projectRgId) */
                    output2 += '<a class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Batches/View/") %>' + res2[i].batchId + '"><i class="fa fa-eye"></i></a> ';
                         <%-- else 
                              output2 += '<a class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Batches/Shared/View/") %>' + res2[i].fkBatch + '">View</a> ';--%>
                    '</td>'
                          //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Batches/View/") %>' + res[i].experimentId + '">Edit</a> ' +
                          //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Batches/View/") %>' + res[i].experimentId + '">Delete</a> ' +
                    //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                    '</tr>';
                }
                $('#listDataTable2 tbody').html(output2);

                CreateBatchTable();
            }

            function GetBatchData(projectId) {
                var newRequestObj = new Object();
                newRequestObj.projectId = projectId;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllBatchesInProject",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTableData2(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }


            function refreshPage() {
                location.reload(true);
            }

            function OpenDeleteBatchInProjectForm(elementId) {

                var newRequestObj = new Object();
                newRequestObj.projectBatchId = elementId;
                var requestDataString = JSON.stringify(newRequestObj);

                bootbox.confirm({
                    title: "Remove batch from Project",
                    message: "Are you sure you want to remove batch from this Project?",
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
                                url: "/Helpers/WebMethods.asmx/DeleteBatchFromProject",
                                async: true,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: requestDataString,
                                success: function (result) {
                                    var jsonResult = JSON.parse(result.d);
                                    if (jsonResult.status == "ok") {
                                        setInterval('refreshPage()', 500);
                                        notify("Success!", "info");
                                        $("#projectBatches" + elementId).remove();
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


            $(function () {
                var projectId = <%= projectId %>;
                var researchGroupId = null;

                CreateBatchTable();
                GetBatchData(projectId);

                $('#addBatchBtn').click(function () {
                    var html = '<div class="form-horizontal">';
                    html += '<div class="form-group">';
                    html += '<div class="col-md-9">';
                    html += '<select id="selectBatch" class="batch-data-ajax">    </select>';
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
                                    var jsonRequestObject = {};
                                    jsonRequestObject["fkProject"] = projectId;
                                    jsonRequestObject["fkBatch"] = selectedData.batchId;

                                    reqUrl = "/Helpers/WebMethods.asmx/SubmitProjectBatch";
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
                                                GetBatchData(projectId);
                                                notify("Success", "info");
                                            }
                                            else {
                                                notify(jsonResult.message, "warning");
                                            }
                                        },
                                        error: function (p1, p2, p3) {
                                            alert(p1.status.toString() + " " + p3.toString());
                                        }
                                    });

                                    return true;

                                    //var materialFunctionId = $('#selectMaterialFunction').val();
                                    //var percentageOfActive = "";
                                    //if (materialFunctionId == 1) {
                                    //    percentageOfActive = $('#percentageOfActive').val();
                                    //}
                                    //selectedData.fkFunction = materialFunctionId;
                                    //selectedData.percentageOfActive = percentageOfActive;

                                    var row = GenerateBatchHtml(selectedData);
                                    if ($('#' + table).hasClass("hidden"))
                                        $('#' + table).removeClass("hidden");
                                    if ($('#' + table).parents(".box").find(".saveAsBatchBtn").hasClass("hidden"))
                                        $('#' + table).parents(".box").find(".saveAsBatchBtn").removeClass("hidden");

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

                    reqUrlBatches = "/Helpers/WebMethods.asmx/GetBatchesOutsideProject";
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
                                    projectId: <%= projectId %>,
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
                });
                function formatBatch(batch) {
                    //var markup = "" + repo.chemicalFormula + " | " + repo.text;
                    var markup = batch.text;
                    //var markup = "" + batch.id + " | " + batch.text;
                    return markup;
                }

            });
        </script>

        <script type="text/javascript">
            var theTable;

            function CreateTestGroupTable() {

                // DataTable
                theTable = $('#listDataTable').DataTable({
                    //"scrollX": true,
                    //"autoWidth": true,

                    "columnDefs": [
                        {
                            "targets": 5,
                            "orderable": false
                        },
                        { "type": "de_datetime", targets: [3, 4] },
                    ],
                    "order": [[5, "desc"]]
                });
            }

            function RefreshTableData(data) {

                theTable.destroy();

                var res = JSON.parse(data);
                var output = "";
                var currentRG = <%= currentRG %>;

                for (var i in res) {
                    //var availableQuantity = res[i].availableQuantity != null ? res[i].availableQuantity : '0';
                    /*var lastChange = res[i].lastChange ? res[i].lastChange : '#';*/

                    output += '<tr ' + 'id="' + 'projectExperiment' + res[i].ExperimentId + '"' + '>' +
                        // '<td>' +
                        ///* '<div title="Remove" class="btn btn-danger btn-xs remove" onclick="OpenDeleteTestGroupExperimentForm(\'' + res[i].projectExperimentId + '\')">' + '<span class="fa fa-remove"></span>' + '</div> ' +*/
                        // '</td>' +
                        '<td>' + res[i].experimentSystemLabel + '</td>' +
                        '<td>' + res[i].experimentPersonalLabel + '</td>' +
                        //'<td>' + res[i].experimentHypothesis + '</td>' +
                        //'<td>' + res[i].conclusion + '</td>' +
                        '<td>' + res[i].operatorUsername + '</td>' +
                        '<td>' + res[i].researchGroupName + '</td>' +
                        '<td>' + res[i].dateCreated + '</td>' +
                        //'<td>' + dateModified + '</td>' +
                        '<td class="table-actions-col">';
                    if (currentRG == res[i].fkResearchGroup)
                        output += '<a title="View" class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/View/") %>' + res[i].experimentId + '"><i class="fa fa-eye"></i></a> ';
                    else
                        output += '<a title="View" class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Shared/View/") %>' + res[i].experimentId + '"><i class="fa fa-eye"></i></a> ';
                      <%-- '<a class="btn btn-success btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Shared/View/") %>' + res[i].experimentId + '">View</a> ' +--%>
                                //'<a class="btn btn-primary btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Edit/") %>' + res[i].experimentId + '">Edit</a> ' +
                                //'<a class="btn btn-danger btn-xs" href="<%= Page.ResolveUrl("~/Experiments/Delete/") %>' + res[i].experimentId + '">Delete</a> ' +
                    //'<div class="btn btn-danger btn-xs" onclick="OpenDeleteForm(' + res[i].experimentId + ')' + '">Delete</div> ' +
                    '</tr>';
                }
                $('#listDataTable tbody').html(output);

                CreateTestGroupTable();
            }

            function GetTestGroupData(projectId) {
                var newRequestObj = new Object();
                newRequestObj.projectId = projectId;
                newRequestObj.researchGroupId = null;

                var RequestDataString = JSON.stringify(newRequestObj);

                $.ajax({
                    type: "POST",
                    url: "/Helpers/WebMethods.asmx/GetAllExperimentsInProject",
                    data: RequestDataString,
                    //async: true,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        RefreshTableData(result.d);
                    },
                    error: function (p1, p2, p3) {
                        alert(p1.status);
                        alert(p3);
                    }
                });
            }

            $(function () {
                var projectId = <%= projectId%>;
                var researchGroupId = null;

                CreateTestGroupTable();
                GetTestGroupData(projectId);

            });

        </script>

    </section>

</asp:Content>
