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

namespace Batteries.EquipmentPanel
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProcessTypes();
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
        protected void BtnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                var equipment = new Equipment
                {
                    equipmentName = TxtName.Text,
                    equipmentLabel = TxtLabel.Text,
                    fkProcessType = DdlProcessType.SelectedValue != "" ? int.Parse(DdlProcessType.SelectedValue) : (int?)null,
                };
                var result = EquipmentDa.AddEquipment(equipment);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Equipment not inserted", NotifyHelper.NotifyType.danger, "");
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