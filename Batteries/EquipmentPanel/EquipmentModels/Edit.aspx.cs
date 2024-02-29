using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models.Responses;
using Batteries.Forms.DataSources;
using Batteries.Models;
using Batteries.Helpers;
using Batteries.Dal;


namespace Batteries.EquipmentPanel.EquipmentModels
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadEquipment();
            var equipmentModel = GetEquipmentModel(GetEquipmentModelIdFromUrl());
            Fill(equipmentModel);
        }
        private void LoadEquipment()
        {
            List<EquipmentExt> equipmentList = Dal.EquipmentDa.GetAllEquipment();

            int index = 0;

            foreach (EquipmentExt equipment in equipmentList)
            {
                DdlEquipment.Items.Insert(index, new ListItem(equipment.equipmentName, equipment.equipmentId.ToString()));
                index++;
            }

            DdlEquipment.Items.Insert(0, new ListItem("", ""));
        }
        private int GetEquipmentModelIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private EquipmentModel GetEquipmentModel(int equipmentModelId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var equipmentModel = EquipmentModelDa.GetAllEquipmentModels(null, equipmentModelId);
            return equipmentModel[0];
        }
        private void Fill(EquipmentModel equipmentModel)
        {
            DdlEquipment.SelectedValue = equipmentModel.fkEquipment != (int?)null ? equipmentModel.fkEquipment.ToString() : "";
            TxtEquipmentModelName.Text = equipmentModel.equipmentModelName;
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var equipmentModel = new EquipmentModel
                {
                    equipmentModelId = GetEquipmentModelIdFromUrl(),
                    equipmentModelName = TxtEquipmentModelName.Text,
                    fkEquipment = DdlEquipment.SelectedValue != "" ? int.Parse(DdlEquipment.SelectedValue) : (int?)null,

                };
                var result = EquipmentModelDa.UpdateEquipmentModel(equipmentModel);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Equipment model info not updated", NotifyHelper.NotifyType.danger, "");
            }
            catch (System.Threading.ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            }
        }
        protected void CancelButton_OnClick(object sender, EventArgs e)
        {
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}