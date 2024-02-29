using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models.Responses
{
    public class ProcessResponse
    {
        public ExperimentProcessExt stepProcess { get; set; }
        public dynamic processAttributes { get; set; }
        public dynamic equipmentSettings { get; set; }
    }
}