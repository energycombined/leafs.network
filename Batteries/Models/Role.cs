using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    [Serializable]
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
    }
}