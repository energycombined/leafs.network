<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditPTG.aspx.cs" Inherits="Batteries.Projects.EditPTG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/chosen.min.css") %>" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <section class="content">



        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Add Test groups to a Project</h3>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlProject" CssClass="col-sm-2 control-label" Text="Project"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HfProjectSelectedValue" runat="server" />
                                    <asp:DropDownList ID="DdlProject" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlProject"
                                        CssClass="text-danger" ErrorMessage="Please assign a project to Test group." Display="Dynamic" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="LinkButton1" CausesValidation="false" Text="New Project" target="_blank" runat="server" CssClass="btn btn-default" href="/Projects/Insert" />
                                </div>
                            </div>
                            <div class="form-group">
                                 <label class="col-sm-2 control-label">Test group</label>
                                <div class="col-sm-7">
                                    <select id="DdlTestGroup" class="form-control" name="DdlTestGroup">
                                    </select>                                 
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="BtnNewTestGroup" CausesValidation="false" Text="New Test Group" target="_blank" runat="server" CssClass="btn btn-default" href="/TestGroups/Insert" />
                                </div>
                            </div>
                              <%--<div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlTGroup" CssClass="col-sm-2 control-label" Text="Test Group"></asp:Label>
                                <div class="col-sm-7">
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <asp:DropDownList ID="DdlTGroup" runat="server" ItemType="" multiple="true" CssClass="form-control chosen-select">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlTGroup"
                                        CssClass="text-danger" ErrorMessage="Please assign a project Test group." Display="Dynamic" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="LinkButton2" CausesValidation="false" Text="New Test group" target="_blank" runat="server" CssClass="btn btn-default" href="/ResearchGroups/Insert" />
                                </div>
                            </div>--%>
                        </div>
                    </fieldset>
                </div>
                <!-- /.box-body -->
                <div class="box-footer">
                    <div class="pull-right">
                        <asp:Button runat="server" ID="UpdateButton" Text="Save" CssClass="btn btn-primary" OnClick="UpdateButton_OnClick" />
                        <asp:Button runat="server" ID="CancelButton" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="CancelButton_OnClick" />
                    </div>
                </div>
                <!-- /.box-footer -->

            </div>
            <!-- /.box -->
        </div>    
         <script src="<%= ResolveUrl("~/Scripts/Pages/experiment.js")%>"></script>
         
           
         <script>
             $(function () {
                 $('.select2').select2({
                     width: "100%",
                     placeholder: 'Select',
                     allowClear: true
                     //theme: "bootstrap"
                 });
           

             var reqUrl = "/Helpers/WebMethods.asmx/GetTestGroupsFromOwnRG";
         <%--$('#<%= DdlTestGroup.ClientID %>').select2({--%>
             $('#DdlTestGroup').select2({
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
                 //templateSelection: formatRepoSelection
                 allowClear: true
             });

             });        
         </script>




       <%-- <script src="<%= ResolveUrl("~/Scripts/chosen.jquery.js")%>"></script>
        <script>
            $(function () {
                $('.chosen-select').chosen();
                $('.chosen-select-deselect').chosen({ allow_single_deselect: true });
                $('.chosen-select').chosen(function () {
                    var selectedList = <%=DdlTGroup.ClientID%> ;

                    var comma1 = ", ";
                    var manl1 = "";
                    for (var i1 = 0; i1 < selectedList.options.length; i1++) {
                        if (selectedList.options[i1].selected) {
                            var man1 = selectedList.options[i1].value;
                            var xz1 = man1.trim();
                            if (xz1 != "") {
                                manl1 = manl1 + man1 + comma1;
                            }
                        }
                    }
                    <%=HiddenField1.ClientID%>.value = manl1;
                });
            })

            $('.select2').select2({
                width: "100%",
                placeholder: 'Select',
                allowClear: true
                //theme: "bootstrap"
            });
        </script>     --%>
       

         

    </section>
</asp:Content>
