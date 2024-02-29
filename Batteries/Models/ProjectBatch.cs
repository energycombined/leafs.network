using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProjectBatch
    {
        public int projectBatchId { get; set; }
        public int? fkProject { get; set; }
        public int? fkBatch { get; set; }
        public int? fkComingExperiment { get; set; }
        public int? fkComingBatch { get; set; }
        public DateTime? dateCreated { get; set; }
        public int? fkUser { get; set; }
        public bool addedManually { get; set; } = false;
    }
}