
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AnyRec.Tools
{
    // Credit to whoswhip (https://whoswhip.dev) for creating the build archive.
    // All builds are downloaded directly from https://archive.recagain.site/.
    // This removes the account information requirement to use DepotDownloader.
    internal class VersionHandler
    {
        public static List<BuildInformationJson> BuildList = JsonConvert.DeserializeObject<List<BuildInformationJson>>(Resources.GetResource("AnyRec.Resources.BuildInformation.json")) ?? new List<BuildInformationJson>();
        public static int LastFullSupportedId = 1;
        public static void AdjustBuildInformation()
        {
            BuildList = JsonConvert.DeserializeObject<List<BuildInformationJson>>(Resources.GetResource("AnyRec.Resources.BuildInformation.json")) ?? new List<BuildInformationJson>();
            foreach (var vers in BuildList)
            {
                if (vers.Id <= 54)
                {
                    vers.ModsToDownload.Add("2016PhotonPatch.dll");
                }
                else if (vers.Id >= 55 && vers.Id <= 62)
                {
                    vers.ModsToDownload.Add("RecNetObfuscatedPhoton.dll");
                }
                else if (vers.Id >= 63 && vers.Id <= 76)
                {
                    vers.ModsToDownload.Add("MonoPhotonPatch3.dll");
                }
                if (vers.Id >= 33 && vers.Id <= 38)
                {
                    vers.ModsToDownload.Add("OldRecNetPatch.dll");
                }
                else if (vers.Id >= 39 && vers.Id <= 41)
                {
                    vers.ModsToDownload.Add("OldRecNetPatch2.dll");
                }
                else if (vers.Id >= 42 && vers.Id <= 54)
                {
                    vers.ModsToDownload.Add("RecNetMonoPatch.dll");
                }
                else if (vers.Id >= 55 && vers.Id <= 62)
                {
                    vers.ModsToDownload.Add("MonoObfuscatedRecnetPatch.dll");
                }
                else if (vers.Id >= 63 && vers.Id <= 76)
                {
                    vers.ModsToDownload.Add("RecNetMonoPatch3.dll");
                }
            }
        }
    }
    public class BuildInformationJson
    {
        [JsonProperty("manifestId")]
        public string ManifestId { get; set; }
        [JsonProperty("updateName")]
        public string UpdateName { get; set; }
        [JsonProperty("date")]
        [JsonConverter(typeof(BuildDateConverter))]
        public DateTime Date { get; set; }
        [JsonProperty("il2cpp")]
        public bool Il2cpp { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; } = "";
        [JsonProperty("modsToDownload")]
        public List<string> ModsToDownload { get; set; } = new List<string>();
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonIgnore]
        public string DownloadUrl => $"https://archive.recagain.site/download/{Date.ToUniversalTime():yyyy-MM-ddTHH-mm-ssZ}";
    }
    public class BuildDateConverter : JsonConverter<DateTime>
    {
        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string value = reader.Value?.ToString();
            if (reader.TokenType == JsonToken.Date && reader.Value is DateTime date)
            {
                return date.ToUniversalTime();
            }
            return DateTime.ParseExact(value, new[] { "yyyy-MM-dd'T'HH-mm-ss'Z'", "yyyy-MM-dd'T'HH:mm:ss'Z'" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        }
        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));
        }
    }
}
