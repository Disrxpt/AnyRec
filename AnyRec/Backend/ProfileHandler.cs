using AnyRec.Models;
using AnyRec.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class ProfileHandler
    {
        public static GetOrCreateModel Get2017Profile()
        {
            return new GetOrCreateModel
            {
                Id = Config.GetInt("PlayerId"),
                Username = Config.GetString("Username"),
                DisplayName = Config.GetString("Username"),
                Level = 1,
                XP = 0,
                ProfileImageName = "ProfileImage",
                Reputation = 1,
                PhoneLastFour = "1111",
                HasEmail = true,
                CanReceiveInvites = false,
                Developer = true,
                Verified = true,
            };
        }
    }
}
