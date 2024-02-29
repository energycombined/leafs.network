using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class TestGroup
    {
        public int testGroupId { get; set; }
        public string testGroupGoal { get; set; }
        public string testGroupName { get; set; }
        public int? fkUser { get; set; }
        public int? fkResearchGroup { get; set; }
        public int? fkProject { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastChange { get; set; }
    }
}