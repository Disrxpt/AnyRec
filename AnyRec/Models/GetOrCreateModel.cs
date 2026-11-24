using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Models
{
    internal class GetOrCreateModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Reputation { get; set; }
        public bool Verified { get; set; }
    }
}
