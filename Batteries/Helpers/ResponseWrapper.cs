using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    public class ResponseWrapper
    {
        public object response { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }
}