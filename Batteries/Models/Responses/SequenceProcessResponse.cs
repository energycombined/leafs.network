using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class SequenceProcessResponse
    { 
        public ProcessSequenceContentExt sequenceContent { get; set; }
        public dynamic processAttributes { get; set; }
    }
}