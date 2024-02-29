using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProjectTestGroup
    {
        public int projectTestGroupId { get; set; }
        public int? fkProject { get; set; }
        public int? fkTestGroup { get; set; }
        public int? fkUser { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}