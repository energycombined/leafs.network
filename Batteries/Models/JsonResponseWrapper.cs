using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class JsonResponseWrapper
    {
        public object response { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }
}