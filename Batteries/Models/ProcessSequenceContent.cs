using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class ProcessSequenceContent
    {
        public int processSequenceContentId { get; set; }
        public int? order { get; set; }
        public int? fkProcessSequence { get; set; }
        public int? fkProcess { get; set; }
        public string processLabel { get; set; }
        public DateTime? dateCreated { get; set; }
    }
}