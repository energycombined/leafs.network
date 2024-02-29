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

namespace Batteries.MeasurementUnits
{
    public partial class Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var measurementUnit = GetMeasurementUnit(GetMeasurementUnitIdFromUrl());
            Fill(measurementUnit);
        }
        private int GetMeasurementUnitIdFromUrl()
        {
            IList<string> segments = Request.GetFriendlyUrlSegments();
            int pId = -1;
            if (segments.Count != 0)
                int.TryParse(segments[0], out pId);
            return pId;
        }
        private MeasurementUnit GetMeasurementUnit(int measurementUnitId)
        {
            var measurementUnit = MeasurementUnitDa.GetAllMeasurementUnits(measurementUnitId);
            return measurementUnit[0];
        }
        private void Fill(MeasurementUnit measurementUnit)
        {
            TxtMeasurementUnitName.Text = measurementUnit.measurementUnitName;
            TxtMeasurementUnitSymbol.Text = measurementUnit.measurementUnitSymbol;
        }
        protected void UpdateButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                var measurementUnit = new MeasurementUnit
                {
                    measurementUnitId = GetMeasurementUnitIdFromUrl(),
                    measurementUnitName = TxtMeasurementUnitName.Text,
                    measurementUnitSymbol = TxtMeasurementUnitSymbol.Text
                };
                var result = MeasurementUnitDa.UpdateMeasurementUnit(measurementUnit);
                if (result == 0)
                {
                    NotifyHelper.Notify("Success", NotifyHelper.NotifyType.success, "");
                    RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
                }
                else
                    NotifyHelper.Notify("MeasurementUnit info not updated", NotifyHelper.NotifyType.danger, "");
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