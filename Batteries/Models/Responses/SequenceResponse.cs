using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class SequenceResponse
    {
       public SequenceExt sequenceInfo { get; set; }
       public List<SequenceProcessResponse> processesList { get; set; }
    }
}