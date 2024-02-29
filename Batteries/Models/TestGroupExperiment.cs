using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class TestGroupExperiment
    {
        public long testGroupExperimentId { get; set; }
        public int? fkTestGroup { get; set; }
        public int? fkExperiment { get; set; }
        public int? fkProject { get; set; }
        public int? fkUser { get; set; }
        public string experimentHypothesis { get; set; }
        public string conclusion { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}