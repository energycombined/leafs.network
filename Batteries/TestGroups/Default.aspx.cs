using Batteries.Dal;
using Batteries.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.TestGroups
{
    public partial class Default : System.Web.UI.Page
    {
        public int currentRG;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            currentRG = (int)currentUser.fkResearchGroup;
        }

        [WebMethod]
        public static string DeleteTestGroup(int testGroupId)
        {
            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;
            var currentUser = UserHelper.GetCurrentUser();
            var testGroup = TestGroupDa.GetAllTestGroups(testGroupId)[0];
            try
            {
                if (testGroup.fkResearchGroup != currentUser.fkResearchGroup)
                {
                    throw new Exception("No permission to do this action");
                }
                var result = TestGroupDa.DeleteTestGroup(testGroupId);
                /*
                if (result != 0)
                {
                    //NotifyHelper.Notify("Successfully deleted TestGroup", NotifyHelper.NotifyType.success, "");
                    //return "";
                    return "Error! Something went wrong";
                }
                //return "";
                 * */
            }
            catch (Exception ex)
            {
                //NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
                //return "Error! " + ex.Message;
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
    }
}