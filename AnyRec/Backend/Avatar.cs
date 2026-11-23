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
        public static void SetAvatar(string postdata)
        {
            AvatarModel data = JsonConvert.DeserializeObject<AvatarModel>(postdata);
            Config.Set("HairColor", data.HairColor);
            Config.Set("AvatarSelections", data.OutfitSelections);
            Config.Set("SkinColor", data.SkinColor);
        }
    }
}
