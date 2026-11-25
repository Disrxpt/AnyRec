using AnyRec.Server;
using AnyRec.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AnyRec
{
    internal class BuildLibrary
    {
        public static int BuildId;
        public static int LatestSupported = 76;
        public static void Load()
        {
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine("AnyRec - Build Library");
            var choices = new List<string> { "Return to Main Menu" };
            foreach (var build in VersionHandler.BuildList)
            {
                if (Directory.Exists($"Builds/{build.Id}/"))
                {
                    choices.Add($"{build.Id} - {build.Date.ToString()} | {build.UpdateName} [INSTALLED]");
                }
                else if (build.Id > BuildLibrary.LatestSupported && !Config.GetBool("DeveloperMode"))
                {
                    continue;
                }
                else
                {
                    choices.Add($"{build.Id} - {build.Date.ToString()} | {build.UpdateName}");
                }
            }
            string choice = UI.ShowList(choices, 15);
            if (choice == "Return to Main Menu")
            {
                Program.Main(new string[0]);
            }
            BuildLibrary.ShowBuildPage(VersionHandler.BuildList.FirstOrDefault(x => x.Id.ToString() == choice.Split(' ')[0]));
        }
        public static void ShowBuildPage(BuildInformationJson buildinformation)
        {
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine($"AnyRec - {buildinformation.Date.ToString()}\n");
            Console.WriteLine($"BuildID: {buildinformation.Id}\nManifestID: {buildinformation.ManifestId}\nUpdate Name: {buildinformation.UpdateName}\nAdditionalInformation: {buildinformation.Description}\nRequired Mods: ");
            foreach (string l in buildinformation.ModsToDownload)
            {
                Console.Write("\"" + l + "\" ");
            }
            Console.WriteLine($"");
            var choices = new List<string>();
            if (Directory.Exists($"Builds/{buildinformation.Id}/"))
            {
                choices.Add("Uninstall Build");
                choices.Add("Run Game");
            }
            else
            {
                choices.Add("Install Game Files");
            }
            choices.Add("Return to Library");
            string choice = UI.ShowList(choices);
            if (choice == "Install Game Files")
            {
                BuildLibrary.DownloadVersion(buildinformation);
            }
            else if (choice == "Uninstall Build")
            {
                string directory = Path.GetFullPath($"Builds/{buildinformation.Id}/");
                Directory.Delete(directory, true);
                BuildLibrary.ShowBuildPage(buildinformation);
            }
            else if (choice == "Return to Library")
            {
                BuildLibrary.Load();
            }
            else if (choice == "Run Game")
            {
                Console.Clear();
                Process p = new Process();
                string directory = Path.GetFullPath($"Builds/{buildinformation.Id}/");
                string exe = "";
                if (File.Exists(Path.Combine(directory, "RecRoom.exe")))
                {
                    exe = Path.Combine(directory, "RecRoom.exe");
                }
                else
                {
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        if (Path.GetFileName(file).ToLower().Contains("recroom") && file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && file.ToLower().Contains("release"))
                        {
                            exe = file;
                            break;
                        }
                    }
                }
                if (string.IsNullOrEmpty(exe))
                {
                    Console.WriteLine("[!] Rec Room executable not found!");
                    return;
                }
                p.StartInfo.FileName = exe;
                p.StartInfo.WorkingDirectory = directory;
                p.StartInfo.UseShellExecute = false;
                Dictionary<string, string> photonsettings = JWT.Decode(Config.GetString("Server"));
                p.StartInfo.ArgumentList.Add("--photon-appid");
                p.StartInfo.ArgumentList.Add(photonsettings["RealtimeId"]);
                p.StartInfo.ArgumentList.Add("--photon-voice-appid");
                p.StartInfo.ArgumentList.Add(photonsettings["VoiceId"]);
                UI.WriteTitle();
                Console.WriteLine("What mode would you like to run the game in?");
                string r = UI.ShowList(new List<string> { "Screen Mode", "VR Mode", "Nevermind!" });
                if (r == "Nevermind!")
                {
                    BuildLibrary.ShowBuildPage(buildinformation);
                    return;
                }
                else if (r == "Screen Mode")
                {
                    p.StartInfo.ArgumentList.Add("+forcemode:screen");
                }
                else
                {
                    p.StartInfo.ArgumentList.Add("+forcemode:vr");
                }
                p.Start();
                if (buildinformation.Id < 33)
                {
                    Program.Main(new string[0]);
                }
                else
                {
                    BuildId = buildinformation.Id;
                    ServerStart.StartServers(p, BuildId);
                }
            }
        }
        public static string BepInExMono = "https://github.com/BepInEx/BepInEx/releases/download/v5.4.22/BepInEx_x64_5.4.22.0.zip";
        public static string BepInExIl2cpp = "https://github.com/BepInEx/BepInEx/releases/download/v6.0.0-pre.2/BepInEx-Unity.IL2CPP-win-x64-6.0.0-pre.2.zip";
        public static WebClient WebClient = new WebClient();
        public static void DownloadVersion(BuildInformationJson buildinformation)
        {
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine($"AnyRec - Downloader\n");
            Console.WriteLine("[>] Preparing download...");
            string directory = $"Builds/{buildinformation.Id}/";
            Directory.CreateDirectory(directory);
            Console.WriteLine("[>] Downloading .zip from archive...");
            WebClient.DownloadFile(buildinformation.DownloadUrl, directory + "temp.zip");
            Console.WriteLine("[>] Unzipping...");
            ZipFile.ExtractToDirectory(directory + "temp.zip", directory);
            File.Delete(directory + "temp.zip");
            Console.WriteLine("[>] Deleting \".DepotDownloader/\"...");
            Directory.Delete(directory + ".DepotDownloader/", true);
            Console.WriteLine("[>] Creating \"steam_appid.txt\"...");
            if (Config.GetBool("AppId480"))
                File.WriteAllText(directory + "steam_appid.txt", "480");
            else
                File.WriteAllText(directory + "steam_appid.txt", "471710");
            if (buildinformation.Il2cpp)
            {
                Console.WriteLine("[>] Downloading Bepinex 6 (Il2cpp)...");
                WebClient.DownloadFile(BepInExIl2cpp, directory + "temp2.zip");
                ZipFile.ExtractToDirectory(directory + "temp2.zip", directory);
                File.Delete(directory + "temp2.zip");
            }
            else
            {
                Console.WriteLine("[>] Downloading Bepinex 5.4 (Mono)...");
                WebClient.DownloadFile(BepInExMono, directory + "temp2.zip");
                ZipFile.ExtractToDirectory(directory + "temp2.zip", directory);
                File.Delete(directory + "temp2.zip");
            }
            Console.WriteLine("[>] Preparing Bepinex...");
            Directory.CreateDirectory(directory + "BepInEx/plugins/");
            BuildLibrary.ConfigureBepInEx(directory, buildinformation);
            BuildLibrary.ConfigureUnityLogging(directory);
            Console.WriteLine("[>] Downloading required mods...");
            foreach (string mod in buildinformation.ModsToDownload)
            {
                File.WriteAllBytes(directory + "BepInEx/plugins/" + mod, Resources.GetResourceBytes("AnyRec.Resources.Mods." + mod));
            }
            Console.WriteLine("[>] Done!");
            BuildLibrary.ShowBuildPage(buildinformation);
        }
        public static void ConfigureBepInEx(string directory, BuildInformationJson buildinformation)
        {
            if (buildinformation.Il2cpp || buildinformation.Id > 88)
                return;
            string configPath = Path.Combine(directory, "BepInEx", "config", "BepInEx.cfg");
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            string config = File.Exists(configPath) ? File.ReadAllText(configPath) : "";
            string replacement = "[Preloader.Entrypoint]\nAssembly = UnityEngine.dll\nType = Camera\nMethod = .cctor\n";
            string pattern = @"(?ms)^\[Preloader\.Entrypoint\][\s\S]*?(?=^\[|\z)";
            if (Regex.IsMatch(config, pattern))
            {
                config = Regex.Replace(config, pattern, _ => replacement);
            }
            else
            {
                config += Environment.NewLine + replacement;
            }
            File.WriteAllText(configPath, config);
        }
        public static void ConfigureUnityLogging(string directory)
        {
            string configPath = Path.Combine(directory, "BepInEx", "config", "BepInEx.cfg");
            Directory.CreateDirectory(Path.GetDirectoryName(configPath));
            string config = File.Exists(configPath) ? File.ReadAllText(configPath) : "";
            string section = "[Logging.Console]";
            string pattern = @"(?m)^\[Logging\.Console\]\s*$";
            if (!Regex.IsMatch(config, pattern))
                config += Environment.NewLine + section + Environment.NewLine;
            string sectionPattern = @"(?ms)^\[Logging\.Console\][^\r\n]*\r?\n.*?(?=^\[|\z)";
            config = Regex.Replace(config, sectionPattern, match =>
            {
                string content = match.Value;
                if (Regex.IsMatch(content, @"(?m)^\s*Enabled\s*="))
                    content = Regex.Replace(content, @"(?m)^(\s*Enabled\s*=\s*)\S+", "${1}true");
                else
                    content += "Enabled = true" + Environment.NewLine;
                return content;
            });
            File.WriteAllText(configPath, config);
        }
    }
}
