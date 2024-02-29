using Batteries.Bll;
using Batteries.Models;
using Batteries.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;
using Batteries.Models.Responses;
using Batteries.Helpers;
using Newtonsoft.Json.Converters;

namespace Batteries.Admin.UsersPanel
{
    public partial class Default : System.Web.UI.Page
    {
        public static int currentUserId;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack) return;
            //GetUsers();
            currentUserId = UserHelper.GetCurrentUser().userId;
            LoadRoles();
            GetResearchGroups();
        }
        private void LoadRoles()
        {
            List<Role> rolesList = Dal.RoleDa.GetAllRoles();

            int index = 0;

            foreach (Role role in rolesList)
            {
                ddlRole.Items.Insert(index, new ListItem(role.roleName, role.roleId.ToString()));
                index++;
            }

            ddlRole.Items.Insert(0, new ListItem("", ""));

            //var roles = Bl.GetAllRoles();
            //DdlRoles.DataSource = roles;
            //DdlRoles.DataBind();
        }
        private void GetResearchGroups()
        {
            List<ResearchGroupExt> researchGroupsList = ResearchGroupDa.GetAllResearchGroups();
            int index = 0;
            if (researchGroupsList != null)
            {
                foreach (ResearchGroup researchGroup in researchGroupsList)
                {
                    ddlResearchGroup.Items.Insert(index, new ListItem(researchGroup.acronym + " - " + researchGroup.researchGroupName, researchGroup.researchGroupId.ToString()));
                    index++;
                }
            }
            ddlResearchGroup.Items.Insert(0, new ListItem("", ""));
        }
        [WebMethod]
        public static string FilterList(int? roleid = null, int? researchGroupId = null)
        {
            try
            {
                List<UserExt> users = UserDa.GetUsers(null, roleid, researchGroupId);
                return JsonConvert.SerializeObject(users);
            }
            catch (Exception e)
            {
                return "Error! " + e.Message;
            }
        }

        private void GetUsers()
        {
            //var users = Bl.GetAllUsers();
            //ViewState["listUsers"] = users;
            //BindListView();
        }

        //protected void ListUsers_OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    //set current page startindex, max rows and rebind to false
        //    DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

        //    //rebind List View
        //    BindListView();
        //}

        //private void BindListView()
        //{
        //    ListUsers.DataSource = ViewState["listUsers"];
        //    ListUsers.DataBind();
        //}

        [WebMethod]
        public static string DeleteUser(int userId)
        {
            //try
            //{
            //    var result = MaterialDa.DeleteMaterial(materialId);
            //    if (result == 0)
            //    {
            //        NotifyHelper.Notify("Successfully deleted Material", NotifyHelper.NotifyType.success, "");
            //        //RedirectHelper.RedirectToReturnUrl(ResolveUrl("Default.aspx"), Response);
            //        //Response.Redirect("~/Materials/Default");
            //    }
            //    else
            //        NotifyHelper.Notify("Error deleting Material", NotifyHelper.NotifyType.danger, "");
            //}
            //catch (System.Threading.ThreadAbortException)
            //{
            //}
            //catch (Exception ex)
            //{
            //    NotifyHelper.Notify(ex.Message, NotifyHelper.NotifyType.danger, "");
            //}

            var resp = new ResponseWrapper { status = "ok", response = null };
            var dateFormat = "dd/MM/yyyy"; // your datetime format
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = dateFormat };

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = dateFormat;

            try
            {
                var result = UserDa.Delete(userId);
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