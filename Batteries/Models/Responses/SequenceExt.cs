using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class SequenceExt : ProcessSequence
    {
        public string username { get; set; }
        public string researchGroupName { get; set; }
        public SequenceExt(ProcessSequence e = null)
        {
            if (e != null)
            {
                this.processSequenceId = e.processSequenceId;
                this.label = e.label;
                this.fkUser = e.fkUser;
                this.fkResearchGroup = e.fkResearchGroup;
                this.dateCreated = e.dateCreated;
            }
        }
    }
}