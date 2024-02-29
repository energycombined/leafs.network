using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProcessSequence
    {
        public int processSequenceId { get; set; }
        public string label { get; set; }
        public int? fkUser { get; set; }
        public int? fkResearchGroup { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}