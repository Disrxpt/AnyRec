using System;
using System.Collections.Generic;
using System.Text;

namespace AnyRec.Models
{
    internal class ConfigV2
    {
        public string MessageOfTheDay { get; set; }
        public string CdnBaseUri { get; set; }
        public MatchmakingParams MatchmakingParams { get; set; }
        public Objective[][] DailyObjectives { get; set; }
        public List<ConfigTable> ConfigTable { get; set; }
        public PhotonSettings PhotonConfig { get; set; }
        public List<LevelProgMaps> LevelProgressionMaps { get; set; }
    }
    public class LevelProgMaps
    {
        public int Level { get; set; }
        public int RequiredXp { get; set; }
    }
    public class PhotonSettings
    {
        public string CloudRegion { get; set; }
        public bool EnableServerTracingAfterDisconnect { get; set; }
        public bool CrcCheckEnabled { get; set; }
    }
    public class ConfigTable
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class MatchmakingParams
    {
        public float PreferFullRoomsFrequency { get; set; }
        public float PreferEmptyRoomsFrequency { get; set; }
    }
}
