using Batteries.Dal;
using Batteries.Models;
using Batteries.Models.Responses;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Batteries.GraphResults
{
    public partial class DefaultOld : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadExperiments();
            FillMassDropdown();
            LoadUsers();
        }

        private void LoadUsers()
        {
            List<UserExt> usersList = UserDa.GetUsers();
            int index = 0;
            if (usersList != null)
            {
                foreach (User user in usersList)
                {
                    DdlOperator.Items.Insert(index, new ListItem(user.userName, user.userId.ToString()));
                    index++;
                }
            }
            DdlOperator.Items.Insert(0, new ListItem("", ""));
        }
        private void LoadExperiments()
        {
            //List<ExperimentExt> experimentsList = ExperimentDa.GetAllExperimentsList();
            List<ExperimentExt> experimentsList = null;
            int index = 0;
            if (experimentsList != null)
            {
                foreach (Experiment experiment in experimentsList)
                {
                    DdlExperiments.Items.Insert(index, new ListItem(experiment.experimentSystemLabel + " | " + experiment.experimentPersonalLabel + " | " + ((DateTime)experiment.dateCreated).ToString(ConfigurationManager.AppSettings["dateFormat"]), experiment.experimentId.ToString()));
                    index++;
                }
            }
            DdlExperiments.Items.Insert(0, new ListItem("", ""));
        }
        private void FillMassDropdown()
        {
            DdlMassType.Items.Insert(0, new ListItem("1: Active material in anode", "1"));
            DdlMassType.Items.Insert(1, new ListItem("2: Total weight of materials in anode", "2"));
            DdlMassType.Items.Insert(2, new ListItem("3: Active material in cathode", "3"));
            DdlMassType.Items.Insert(3, new ListItem("4: Total weight of materials in cathode", "4"));
            DdlMassType.Items.Insert(4, new ListItem("5: Total weight of materials in anode, cathode and electrolyte", "5"));
            DdlMassType.Items.Insert(5, new ListItem("6: Total weight of materials in anode, cathode, electrolyte, casing", "6"));
            //DdlMassType.DataBind();
        }
    }
}

