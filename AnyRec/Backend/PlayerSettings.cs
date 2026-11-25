using AnyRec.Models;
using AnyRec.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class PlayerSettings
    {
        public static List<Setting> GetPlayerSettings()
        {
            return JsonConvert.DeserializeObject<List<Setting>>(Config.GetString("Settings"));
        }
        public static void SetSettings(Setting setting)
        {
            var settings = PlayerSettings.GetPlayerSettings();
            if (settings.FirstOrDefault(x => x.Key == setting.Key) == null)
            {
                settings.Add(setting);
            }
            else
            {
                settings.FirstOrDefault(x => x.Key == setting.Key).Value = setting.Value;
            }
            Config.Set("Settings", JsonConvert.SerializeObject(settings));
        }
    }
}
