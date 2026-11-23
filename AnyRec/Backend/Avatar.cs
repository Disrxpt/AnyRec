using AnyRec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class Avatar
    {
        public static AvatarModel GetAvatar()
        {
            //NOT SETUP YET
            return new AvatarModel
            {
                HairColor = "",
                OutfitSelections = "",
                SkinColor = "",
            };
        }
    }
}
