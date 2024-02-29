using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ResearchGroup
    {
        public int researchGroupId { get; set; }
        public string researchGroupName { get; set; }
        public string acronym { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? lastChange { get; set; }
        public int? fkOperator { get; set; }
        public int? lastExperimentNumber { get; set; }
        public int? lastBatchNumber { get; set; }
    }
}