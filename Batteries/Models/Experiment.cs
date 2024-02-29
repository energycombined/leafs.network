using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Experiment
    {
        public int experimentId { get; set; }
        public string experimentSystemLabel { get; set; }
        public string experimentPersonalLabel { get; set; }
        public int? fkUser { get; set; }
        public int? fkResearchGroup { get; set; }
        public string experimentDescription { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? dateModified { get; set; }
        public Boolean? isComplete { get; set; }
        public int? fkTemplate { get; set; }
        public Boolean? hasTestResultsDoc { get; set; }
        public int? fkEditedBy { get; set; }
        public int? fkProject { get; set; }
        public int? fkSharingType { get; set; }

    }
}