using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Models
{
    internal class PlatformLogin
    {
        public string Token { get; set; }
        public int PlayerId { get; set; }
        public string Error { get; set; }
    }
}
