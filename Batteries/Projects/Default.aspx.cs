using Batteries.Dal;
using Batteries.Helpers;
using Batteries.Models;
using Batteries.Models.Responses;
using Microsoft.AspNet.FriendlyUrls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.Projects
{
    public partial class Default : System.Web.UI.Page
    {
        public static int currentUserId;
        public int currentRG;
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentUser = UserHelper.GetCurrentUser();
            currentUserId = UserHelper.GetCurrentUser().userId;
            currentRG = (int)currentUser.fkResearchGroup;
        }

       
        [WebMethod]
        public static string DeleteProject(int projectId)
        {

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd.MM.yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = ProjectDa.DeleteProject(projectId);
            }
            catch (Exception ex)
            {
                resp.status = "error";
                resp.message = ex.Message;
                return JsonConvert.SerializeObject(resp);
            }
            return JsonConvert.SerializeObject(resp, jsonSettings);
        }
    }
}