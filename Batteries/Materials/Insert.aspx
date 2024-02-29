<%@ Page Title="New Material" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Insert.aspx.cs" Inherits="Batteries.Materials.Insert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-10 col-lg-8 col-lg-offset-2">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Insert New Material</h3>
                        <%--<legend>New Material</legend>--%>
                    </div>
                    <!-- /.box-header -->
                    <!-- form start -->
                    <fieldset class="form-horizontal">
                        <div class="box-body">
                            <div class="form-group">
                                <label class="col-sm-2 control-label">Select existing</label>
                                <div class="col-sm-10">
                                    <select id="DdlMaterial" class="form-control select2" name="DdlMaterial">
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtName" CssClass="col-sm-2 control-label" Text="Material Name (in text)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtName"
                                        CssClass="text-danger" ErrorMessage="Material name is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtChemicalFormula" CssClass="col-sm-2 control-label" Text="Chemical Formula"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtChemicalFormula" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtChemicalFormula"
                                        CssClass="text-danger" ErrorMessage="Material chemical formula is required." Display="Dynamic" />--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtMaterialLabel" CssClass="col-sm-2 control-label" Text="Label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtMaterialLabel" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterialType" CssClass="col-sm-2 control-label" Text="Material Type"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMaterialType" runat="server" ItemType="Batteries.Models.MaterialType" DataTextField="MaterialType" DataValueField="MaterialTypeId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialType"
                                        CssClass="text-danger" ErrorMessage="Material type is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlStoredInType" CssClass="col-sm-2 control-label" Text="Stored In"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlStoredInType" runat="server" ItemType="Batteries.Models.StoredInType" DataTextField="storedInType" DataValueField="storedInTypeId" AppendDataBoundItems="true" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlStoredInType"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMaterialFunction" CssClass="col-sm-2 control-label" Text="Function in experiments"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMaterialFunction" runat="server"  AppendDataBoundItems="true" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMaterialFunction"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPercentageOfActive" CssClass="col-sm-2 control-label" Text="Percentage of active"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPercentageOfActive" runat="server" CssClass="form-control" type="text" disabled="true"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPercentageOfActive"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtPercentageOfActive"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                    <asp:RangeValidator ControlToValidate="TxtPercentageOfActive" MinimumValue="0" MaximumValue="100" Type="Double" CssClass="text-danger" ErrorMessage="The value must be from 0 to 100." runat="server"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtDescription" CssClass="col-sm-2 control-label" Text="Description"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDescription" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtCasNumber" CssClass="col-sm-2 control-label" Text="CAS Number"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtCasNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtLotNumber" CssClass="col-sm-2 control-label" Text="LOT Number"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtLotNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="box-header no-border">
                                <h3 class="box-title">Stock</h3>
                                <%--<legend class="box-title">Stock</legend>--%>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtAmount" CssClass="col-sm-2 control-label" Text="Stock Amount"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtAmount" runat="server" CssClass="form-control" type="text"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtAmount"
                                        CssClass="text-danger" ErrorMessage="Stock amount is required." Display="Dynamic" />
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtAmount"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                    <%--"^-?(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$"--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="DdlMeasurementUnit" CssClass="col-sm-2 control-label" Text="Measurement Unit"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:DropDownList ID="DdlMeasurementUnit" runat="server" ItemType="" DataTextField="measurementUnitName" DataValueField="measurementUnitId" CssClass="form-control select2">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DdlMeasurementUnit"
                                        CssClass="text-danger" ErrorMessage="Measurement unit is required." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtPrice" CssClass="col-sm-2 control-label" Text="Price (€) per unit"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtPrice" runat="server" CssClass="form-control" type="text"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtPrice"
                                        CssClass="text-danger" ErrorMessage="Price is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtPrice"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="DdlVendor" CssClass="col-sm-2 control-label" Text="Vendor"></asp:Label>--%>
                                <label class="col-sm-2 control-label">Vendor</label>
                                <div class="col-sm-8">
                                    <%--<asp:DropDownList ID="DdlVendor" runat="server" CssClass="form-control select2">
                                    </asp:DropDownList>--%>
                                    <select id="DdlVendor" class="form-control select2" name="DdlVendor">
                                    </select>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="DdlVendor"
                                        CssClass="text-danger" ErrorMessage="Field is required." Display="Dynamic" />--%>
                                </div>
                                <div class="col-sm-2">
                                    <asp:LinkButton ID="BtnNewVendor" CausesValidation="false" Text="Add new" target="_blank" runat="server" CssClass="btn btn-default" href="/Vendors/Insert"/>
                                </div>
                            </div>
                            <div class="form-group">
                                <%--<asp:Label runat="server" AssociatedControlID="TxtDateBought" CssClass="col-sm-2 control-label" Text="Date Bought"></asp:Label>--%>
                                <label class="col-sm-2 control-label">Date Bought</label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtDateBought" runat="server" CssClass="form-control datepicker"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TxtDateBought"
                                        CssClass="text-danger" ErrorMessage="Date Bought is required." Display="Dynamic" />
                                    <%--<input ID="DateBought" type="text" class="form-control datepicker">--%>
                                    <%--<asp:RangeValidator runat="server" ControlToValidate="TxtDateBought" ErrorMessage="Invalid Date" Type="Date" MinimumValue="01/01/1900" MaximumValue="<%= DateTime.Now.ToShortDateString() %>" Display="Dynamic"></asp:RangeValidator>--%>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtBulkPrice" CssClass="col-sm-2 control-label" Text="Bulk Price (€/kg)"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtBulkPrice" runat="server" CssClass="form-control" type="text"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="TxtBulkPrice"
                                        CssClass="text-danger" ErrorMessage="Bulk price is required." Display="Dynamic" />--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        CssClass="text-danger" ErrorMessage="You have to enter a positive number. For decimal please use a dot."
                                        ControlToValidate="TxtBulkPrice"
                                        ValidationExpression="^(([1-9][0-9]*)|(0))(?:\.[0-9]+)?$" Display="Dynamic" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" AssociatedControlID="TxtReference" CssClass="col-sm-2 control-label" Text="Reference"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="TxtReference" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>


                            <div class="form-group">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <div class="btn btn-xs btn-default" disabled="true" data-toggle="tooltip" title="Adding documents is available after save!">Documents</div>
                                </div>
                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="pull-right">
                                <asp:Button runat="server" ID="BtnSaveAndEdit" Text="Save and Edit" data-toggle="tooltip" title="Save and edit immediately" CssClass="btn btn-info" OnClick="BtnInsert_Click" />
                                <asp:Button runat="server" ID="BtnInsert" Text="Save" data-toggle="tooltip" title="Save and go to list" CssClass="btn btn-primary" OnClick="BtnInsert_Click" />
                                <asp:Button runat="server" ID="BtnCancel" Text="Cancel" CausesValidation="false" CssClass="btn btn-default" OnClick="BtnCancel_Click" />
                            </div>
                        </div>
                        <!-- /.box-footer -->
                    </fieldset>
                </div>
                <!-- /.box -->

            </div>
        </div>
        <script>
            $(function () {
                $('#<%= DdlMaterialFunction.ClientID %>').change(function () {
                    var selectedFunctionId = $('#<%= DdlMaterialFunction.ClientID %>').val();

                    //console.log(selectedFunctionId == "1");
                    if (selectedFunctionId == "1" || selectedFunctionId == "5")
                         $('#<%= TxtPercentageOfActive.ClientID %>').removeAttr("disabled");
                    else {
                        $('#<%= TxtPercentageOfActive.ClientID %>').val("");
                        $('#<%= TxtPercentageOfActive.ClientID %>').attr("disabled", "true");
                    }
                        

                 });

                $('#<%= DdlMeasurementUnit.ClientID %>').change(function () {
                    var selectedUnitId = $('#<%= DdlMeasurementUnit.ClientID %>').val();
                    var text = "";
                    switch (selectedUnitId) {
                        case "1":
                            text = "Price per kilo";
                            break;
                        case "2":
                            text = "Price per liter";
                            break;
                        case "3":
                            text = "Price 1000 pieces";
                            break;
                    }
                    $('#<%= TxtBulkPrice.ClientID %>').attr("placeholder", text);
                });

                $('.select2').select2({
                    width: "100%",
                    placeholder: 'Select',
                    allowClear: true
                    //theme: "bootstrap"
                });

                $('.datepicker').datepicker({
                    format: 'dd/mm/yyyy'
                });



                var reqUrl = "/Helpers/WebMethods.asmx/GetVendors";
                //$('#<= DdlVendor.ClientID %>').select2({
                    $('#DdlVendor').select2({
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

                            return JSON.stringify({ search: term, type: 'public' });
                        },
                        processResults: function (data, params) {

                            return {
                                results: $.map(JSON.parse(data.d), function (item) {
                                    return {
                                        id: item.vendorId,
                                        text: item.vendorName,
                                        vendorSite: item.vendorSite
                                    };
                                })
                            };
                        },
                        //cache: true
                    },
                    //dropdownParent: dialog,
                    width: "100%",
                    //theme: "bootstrap",
                    placeholder: 'Select vendor',
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    //minimumInputLength: 1,
                    templateResult: formatRepo,
                    //templateSelection: formatRepoSelection,
                    allowClear: true
                });

                function formatRepo(repo) {
                    var markup = "" + repo.text + " | " + repo.vendorSite;
                    return markup;
                }

                function formatRepoSelection(repo) {
                    return repo.text + " | " + repo.vendorSite;
                }


                var reqUrlMaterials = "/Helpers/WebMethods.asmx/GetMaterials";
                $('#DdlMaterial').select2({
                    ajax: {
                        type: "POST",
                        url: reqUrlMaterials,
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

                            //console.log(JSON.parse(data.d).results);

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
                    //dropdownParent: dialog,
                    width: "100%",
                    //theme: "bootstrap",
                    placeholder: 'Search for material',
                    escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
                    //minimumInputLength: 1,
                    templateResult: formatRepoM,
                    allowClear: true,
                    //templateSelection: formatRepoSelection
                });

                function formatRepoM(repo) {
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

                $('#DdlMaterial').change(function () {
                    var selectedMaterial = $('#DdlMaterial').val();                   

                    if (selectedMaterial) {
                        selectedMaterialItem = $('#DdlMaterial').select2('data')[0].data;
                        FillMaterialInfo(selectedMaterialItem);
                    }
                    else {
                        ClearMaterialInfo();
                    }
                });
                

            });


            function FillMaterialInfo(material) {
                $('#<%= TxtName.ClientID %>').val(material.materialName);
                $('#<%= TxtChemicalFormula.ClientID %>').val(material.chemicalFormula);
                $('#<%= TxtMaterialLabel.ClientID %>').val(material.materialLabel);
                $('#<%= DdlMaterialType.ClientID %>').val(material.materialName);
                $('#<%= DdlMaterialType.ClientID %>').val(material.fkMaterialType).trigger('change');
                $('#<%= DdlStoredInType.ClientID %>').val(material.fkStoredInType).trigger('change');
                $('#<%= DdlMaterialFunction.ClientID %>').val(material.fkFunction).trigger('change');
                $('#<%= TxtPercentageOfActive.ClientID %>').val(material.percentageOfActive);
                $('#<%= TxtDescription.ClientID %>').val(material.description);
                $('#<%= DdlMeasurementUnit.ClientID %>').val(material.fkMeasurementUnit).trigger('change');
                $('#<%= TxtPrice.ClientID %>').val(material.price);
                $('#<%= TxtBulkPrice.ClientID %>').val(material.bulkPrice);
                $('#<%= TxtDateBought.ClientID %>').val(material.dateBought);
                //$('#DdlVendor').val(material.fkVendor).trigger('change');
                $('#<%= TxtReference.ClientID %>').val(material.reference);
                $('#<%= TxtCasNumber.ClientID %>').val(material.casNumber);
                $('#<%= TxtLotNumber.ClientID %>').val(material.lotNumber);
                
            }
            function ClearMaterialInfo() {
                $('#<%= TxtName.ClientID %>').val("");
                $('#<%= TxtChemicalFormula.ClientID %>').val("");
                $('#<%= TxtMaterialLabel.ClientID %>').val("");
                $('#<%= DdlMaterialType.ClientID %>').val("");
                $('#<%= DdlMaterialType.ClientID %>').val("").trigger('change');
                $('#<%= DdlStoredInType.ClientID %>').val("").trigger('change');
                $('#<%= DdlMaterialFunction.ClientID %>').val("").trigger('change');
                $('#<%= TxtPercentageOfActive.ClientID %>').val("");
                $('#<%= TxtDescription.ClientID %>').val("");
                $('#<%= DdlMeasurementUnit.ClientID %>').val("").trigger('change');
                $('#<%= TxtPrice.ClientID %>').val("");
                $('#<%= TxtBulkPrice.ClientID %>').val("");
                $('#<%= TxtDateBought.ClientID %>').val("");
                //$('#DdlVendor').val("") material.fkVendor).trigger('change');
                $('#<%= TxtReference.ClientID %>').val("");
                $('#<%= TxtCasNumber.ClientID %>').val("");
                $('#<%= TxtLotNumber.ClientID %>').val("");
            }

            
        </script>
    </section>
</asp:Content>
