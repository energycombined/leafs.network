using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProjectResearchGroup
    {
        public int projectResearchGroupId { get; set; }
        public int? fkProject { get; set; }
        public int? fkResearchGroup { get; set; }
        public int? fkUser { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}