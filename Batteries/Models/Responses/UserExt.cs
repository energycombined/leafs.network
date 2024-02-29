using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class UserExt : User
    {
        public string researchGroupName { get; set; }
        public string researchGroupAcronym { get; set; }
        public UserExt(User e = null)
        {
            if (e != null)
            {
                this.userId = e.userId;
                this.userName = e.userName;
                this.firstName = e.firstName;
                this.lastName = e.lastName;
                this.phone = e.phone;
                this.active = e.active;
                this.email = e.email;
                this.userRole = e.userRole;
                this.fkResearchGroup = e.fkResearchGroup;
            }
        }
    }
}