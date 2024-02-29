using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;

namespace Batteries.PrefabTypes
{
    public partial class View : System.Web.UI.Page
    {
        public int commercialComponentId = 0;
        public int componentTypeId = 0;
        public string componentType = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            commercialComponentId = GetBatteryComponentCommercialTypeIdFromUrl();
            var batteryComponentCommercialType = GetBatteryComponentCommercialType(commercialComponentId);

            componentTypeId = (int)batteryComponentCommercialType.fkBatteryComponentType;
            switch (componentTypeId)
            {
                case 1:
                    componentType = "Anode";
                    break;
                case 2:
                    componentType = "Cathode";
                    break;
                case 3:
                    componentType = "Separator";
                    break;
                case 4:
                    componentType = "Electrolyte";
                    break;
                case 5:
                    componentType = "ReferenceElectrode";
                    break;
                case 6:
                    componentType = "Casing";
                    break;
            }
            if (batteryComponentCommercialType.fkResearchGroup != currentUser.fkResearchGroup)
            {
                RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/PrefabTypes/Default.aspx?componentType=" + componentType), Response);

            }

            if (IsPostBack) return;
            Fill(batteryComponentCommercialType);
        }
        private int GetBatteryComponentCommercialTypeIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private BatteryComponentCommercialType GetBatteryComponentCommercialType(int batteryComponentCommercialTypeId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var batteryComponentCommercialType = BatteryComponentCommercialTypeDa.GetBatteryComponentCommercialTypes(batteryComponentCommercialTypeId);
            return batteryComponentCommercialType[0];
        }
        private void Fill(BatteryComponentCommercialType batteryComponentCommercialType)
        {
            LblComponentType.Text = componentType;
            LblName.Text = batteryComponentCommercialType.batteryComponentCommercialType;
            LblModel.Text = batteryComponentCommercialType.model;
        }

        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("~/PrefabTypes/Default.aspx?componentType=" + componentType), Response);
        }
    }
}