using AnyRec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Backend
{
    internal class Configs
    {
        public static ConfigV2 GetConfigSettings()
        {
            return new ConfigV2
            {
                CdnBaseUri = "https://localhost:3000/",
                MatchmakingParams = new MatchmakingParams
                {
                    PreferEmptyRoomsFrequency = 0.0f,
                    PreferFullRoomsFrequency = 0.0f
                },
                LevelProgressionMaps = new List<LevelProgMaps>
                {
                    new LevelProgMaps
                    {
                        Level = 1,
                        RequiredXp = 1000
                    }
                },
                PhotonConfig = new PhotonSettings
                {
                    CloudRegion = "us",
                    EnableServerTracingAfterDisconnect = false,
                    CrcCheckEnabled = false
                },
                DailyObjectives = Objectives.LoadObjectives(),
                MessageOfTheDay = "Welcome to AnyRec by ImagineHacking.",
                ConfigTable = new List<ConfigTable>
                {
                    new ConfigTable
                    {
                        Key = "Gift.DropChance",
                        Value = "0.5"
                    },
                    new ConfigTable
                    {
                        Key = "Gift.XP",
                        Value = "10"
                    },
                }
            };
        }
    }
}
