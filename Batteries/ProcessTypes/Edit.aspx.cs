using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Batteries.Models;
using Batteries.Dal;
using Batteries.Helpers;

namespace Batteries.ProcessTypes
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var processType = GetProcessType(GetProcessTypeIdFromUrl());
            Fill(processType);
        }
        private int GetProcessTypeIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private ProcessType GetProcessType(int processTypeId)
        {
            var processType = ProcessTypeDa.GetAllProcessTypes(processTypeId);
            return processType[0];
        }
        private void Fill(ProcessType processType)
        {
            TxtProcessType.Text = processType.processType;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var processType = new ProcessType
                {
                    processTypeId = GetProcessTypeIdFromUrl(),
                    processType = TxtProcessType.Text
                };
                var result = ProcessTypeDa.UpdateProcessType(processType);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("Process type info not updated", NotifyHelper.NotifyType.danger, "");
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