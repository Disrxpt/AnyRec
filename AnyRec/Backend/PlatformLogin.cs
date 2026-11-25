using AnyRec.Models;
using AnyRec.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class PlatformLogin
    {
        public static AnyRec.Models.PlatformLogin TokenLogin()
        {
            return new Models.PlatformLogin
            {
                PlayerId = Config.GetInt("PlayerId"),
                Token = Guid.NewGuid().ToString()
            };
        }
    }
}
