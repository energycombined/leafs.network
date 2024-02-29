using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.EquipmentPanel.EquipmentModels
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadEquipments();
        }
        private void LoadEquipments()
        {
            List<EquipmentExt> equipmentList = Dal.EquipmentDa.GetAllEquipment();

            int index = 0;

            foreach (EquipmentExt equipment in equipmentList)
            {
                DdlEquipment.Items.Insert(index, new ListItem(equipment.processType + ": " + equipment.equipmentName, equipment.equipmentId.ToString()));
                index++;
            }

            DdlEquipment.Items.Insert(0, new ListItem("", ""));
        }
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var equipmentModel = new EquipmentModel
                {
                    equipmentModelName = TxtEquipmentModelName.Text,
                    fkEquipment = DdlEquipment.SelectedValue != "" ? int.Parse(DdlEquipment.SelectedValue) : (int?)null,
                };
                var result = EquipmentModelDa.AddEquipmentModel(equipmentModel);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Equipment model not inserted", NotifyHelper.NotifyType.danger, "");
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
            RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
        }
    }
}