using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Models
{
    public class Recovery
    {
        public int RecoveryId { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ValidThrough { get; set; }
    }
}