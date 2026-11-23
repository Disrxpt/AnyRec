using AnyRec.Models;
using AnyRec.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class Avatar
    {
        public static AvatarModel GetAvatar()
        {
            return new AvatarModel
            {
                HairColor = Config.GetString("HairColor"),
                OutfitSelections = Config.GetString("AvatarSelections"),
                SkinColor = Config.GetString("SkinColor"),
            };
        }
        public static void SetAvatar(AvatarModel updatedavatar)
        {
            Config.Set("HairColor", updatedavatar.HairColor);
            Config.Set("AvatarSelections", updatedavatar.OutfitSelections);
            Config.Set("SkinColor", updatedavatar.SkinColor);
        }
    }
}
