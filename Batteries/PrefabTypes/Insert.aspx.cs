using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.PrefabTypes
{
    public partial class Insert : System.Web.UI.Page
    {
        public string componentType = "";
        public int componentTypeId = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            componentType = Request.QueryString["componentType"];
            switch (componentType)
            {
                case "Anode":
                    componentTypeId = 1;
                    break;
                case "Cathode":
                    componentTypeId = 2;
                    break;
                case "Separator":
                    componentTypeId = 3;
                    break;
                case "Electrolyte":
                    componentTypeId = 4;
                    break;
                case "ReferenceElectrode":
                    componentTypeId = 5;
                    break;
                case "Casing":
                    componentTypeId = 6;
                    break;
            }
        }
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var BatteryComponentCommercialType = new BatteryComponentCommercialType
                {
                    batteryComponentCommercialType = TxtName.Text,
                    model = TxtModel.Text,
                    fkBatteryComponentType = componentTypeId,
                    fkResearchGroup = (int)UserHelper.GetCurrentUser().fkResearchGroup
                };
                var result = BatteryComponentCommercialTypeDa.AddBatteryComponentCommercialType(BatteryComponentCommercialType);
                if (result != 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    if (sender == BtnInsert)
                    {
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/PrefabTypes/Default.aspx?componentType=" + componentType), Response);
                    }
                    else if (sender == BtnSaveAndEdit)
                    {
                        RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/PrefabTypes/Edit/" + result), Response);
                    }
                    
                }
                else
                    NotifyHelper.Notify("Prefab type not inserted", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PrefabTypes/Default.aspx?componentType=" + componentType);
        }
    }
}