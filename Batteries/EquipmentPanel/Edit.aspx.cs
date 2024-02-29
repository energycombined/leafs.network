using Batteries.Models;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Helpers;
using Batteries.Dal;

namespace Batteries.EquipmentPanel
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProcessTypes();
            var equipment = GetEquipment(GetEquipmentIdFromUrl());
            Fill(equipment);
        }
        private void LoadProcessTypes()
        {
            List<ProcessTypeExt> processTypeList = Dal.ProcessTypeDa.GetAllProcessTypes();

            int index = 0;

            foreach (ProcessType processType in processTypeList)
            {
                DdlProcessType.Items.Insert(index, new ListItem(processType.processType, processType.processTypeId.ToString()));
                index++;
            }

            DdlProcessType.Items.Insert(0, new ListItem("", ""));
        }
        private int GetEquipmentIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private Equipment GetEquipment(int equipmentId)
        {
            var currentUser = UserHelper.GetCurrentUser();
            var equipment = EquipmentDa.GetAllEquipment(null, equipmentId);
            return equipment[0];
        }
        private void Fill(Equipment equipment)
        {
            DdlProcessType.SelectedValue = equipment.fkProcessType != (int?)null ? equipment.fkProcessType.ToString() : "";
            TxtName.Text = equipment.equipmentName;
            TxtLabel.Text = equipment.equipmentLabel;
        }

        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var equipment = new Equipment
                {
                    equipmentId = GetEquipmentIdFromUrl(),
                    equipmentName = TxtName.Text,
                    equipmentLabel = TxtLabel.Text,
                    fkProcessType = DdlProcessType.SelectedValue != "" ? int.Parse(DdlProcessType.SelectedValue) : (int?)null,
                    
                };
                var result = EquipmentDa.UpdateEquipment(equipment);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Equipment info not updated", NotifyHelper.NotifyType.danger, "");
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