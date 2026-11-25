using AnyRec.Tools;
using System;
using System.ComponentModel.Design;

namespace AnyRec
{
    public class Program
    {
        //Starting point for the program.
        public static void Main(string[] args)
        {
            //Identify the version of the program.
            string versionidentifier = "1.0.0";

            //Set the color of text to white.
            Console.ForegroundColor = ConsoleColor.White;

            //Setting the name of the console window.
            Console.Title = $"AnyRec [v{versionidentifier}]";

            //Hide the cursor.
            Console.CursorVisible = false;

            //Write the "AnyRec" text in the console.
            UI.WriteTitle();
            Console.WriteLine("[>] Loading...");

            //Load the configs and create
            Config.Load();
            Program.ValidateDirectories();
            Program.SetupConfigs();

            //Validate versions
            VersionHandler.AdjustBuildInformation();

            //Check if this is this users first time running.
            if (Config.GetBool("FirstRun") == true)
            {
                bool TutorialEnabled = false;
                if (TutorialEnabled)
                {
                    Console.Clear();
                    UI.WriteTitle();
                    Console.WriteLine("AnyRec - Welcome\n\nHello! Welcome to AnyRec!");
                    Console.WriteLine("AnyRec is an opensource project to create servers for EVERY Rec Room Version!");
                    Console.WriteLine("As updates release, more and more versions will continue to be supported.");
                    Console.WriteLine("Features to other versions will be updated as well!");
                    Console.WriteLine("Lets get started, what is your desired display name? ");
                    string username = Console.ReadLine();
                    if (username.Length > 35)
                    {
                        username = username.Remove(35);
                    }
                    Config.Set("Username", username);
                    Console.WriteLine("Amazing name!");
                    Console.WriteLine("Before we get started, you'll need a server to connect to.");
                    string r = UI.ShowList(new List<string> { "Use default server" });
                    if (r == "Use default server")
                    {
                        Config.Set("Server", JWT.Create(new Dictionary<string, string>
                        {
                            { "RealtimeId", "6fc4a5fd-f8fd-46a2-a34a-c55d0e34e7ec" },
                            { "VoiceId", "d4664e7b-0a04-49b8-8212-c6d9cb1da233" },
                            { "Name", "AnyRec Default" }
                        }));
                    }
                }
                else
                {
                    Config.Set("Server", JWT.Create(new Dictionary<string, string>
                    {
                        { "RealtimeId", "6fc4a5fd-f8fd-46a2-a34a-c55d0e34e7ec" },
                        { "VoiceId", "d4664e7b-0a04-49b8-8212-c6d9cb1da233" },
                        { "Name", "AnyRec Default" }
                    }));
                }
                Config.Set("FirstRun", false);
            }
            //Start menu
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine($"\nAnyRec - RecNet emulator to make any Rec Room build easily playable again!\nCreated by ImagineHacking (version v{versionidentifier})");
            var choices = new List<string> { "What's Cooking?", "Edit Profile", "Settings", "Build Library"};
            string choice = UI.ShowList(choices);
            if (choice == "Edit Profile")
            {
                Program.ProfileEditor();
            }
            else if (choice == "What's Cooking?")
            {
                Console.ForegroundColor = ConsoleColor.White;
                Program.Changelog(versionidentifier);
                Console.WriteLine("\nPress any key to return to menu.");
                Console.ReadKey();
                Program.Main(new string[0]);
            }
            else if (choice == "Settings")
            {
                Program.Settings();
            }
            else if (choice == "Build Library")
            {
                BuildLibrary.Load();
            }
        }
        public static void SetupConfigs()
        {
            Config.SetDefault("FirstRun", true);

            //Player profile configs
            Config.SetDefault("PlayerId", new Random().Next(1000, 100000));
            Config.SetDefault("Username", "Default");
            Config.SetDefault("Level", 1);

            //Avatar
            Config.SetDefault("AvatarSelections", "");
            Config.SetDefault("FaceFeatures", "");
            Config.SetDefault("HairColor", "");
            Config.SetDefault("SkinColor", "");

            //Wallet
            Config.SetDefault("Tokens", 0);
            Config.SetDefault("Gold", 0);
            Config.SetDefault("Silver", 0);
            Config.SetDefault("Tickets", 0);

            //Settings
            Config.SetDefault("Settings", "[]");

            //Settings (Launcher)
            Config.SetDefault("DeveloperMode", false);
            Config.SetDefault("AppId480", false);
            Config.SetDefault("ForceSandboxMode", false);
            Config.SetDefault("Server", "");
        }
        public static void ValidateDirectories()
        {
            if (!Directory.Exists("Prerequisites/"))
            {
                Directory.CreateDirectory("Prerequisites/");
            }
            if (!Directory.Exists("Builds/"))
            {
                Directory.CreateDirectory("Builds/");
            }
        }
        public static void Changelog(string vers)
        {
            Console.Clear();
            Console.WriteLine($"AnyRec Changelog (Version {vers})\n");
            Console.WriteLine(Logs);
        }
        public static string Logs = "THIS IS A VERY EARLY VERSION OF ANYREC!\nCurrently all 2016 builds are tested and supported.\nWhen updates are made, they will be here\nHave fun!";
        public static void Settings()
        {
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine("AnyRec - Settings");
            string op = UI.ShowList(new List<string> { "Go Back", $"Toggle Developer mode [Current: {Config.GetBool("DeveloperMode")}]", $"Toggle AppId Fix [Current: {Config.GetBool("AppId480")}]" });
            if (op == "Go Back")
            {
                Program.Main(new string[0]);
            }
            else if (op.StartsWith("Toggle Developer mode"))
            {
                bool developermode = Config.GetBool("DeveloperMode");
                if (developermode)
                {
                    Config.Set("DeveloperMode", false);
                }
                else
                {
                    Config.Set("DeveloperMode", true);
                }
            }
            else if (op.StartsWith("Toggle AppId Fix"))
            {
                bool fix = Config.GetBool("AppId480");
                if (fix)
                {
                    Config.Set("AppId480", false);
                }
                else
                {
                    Console.Clear();
                    UI.WriteTitle();
                    Console.WriteLine("This is a setting will allow you to play AnyRec builds without owning Rec Room on steam.\nEnabling this WILL cause some builds to stop working entirely\nThis is only recommended if you don't own Rec Room on steam.");
                    string yn = UI.ShowList(new List<string> { "Enable it!", "Nevermind!" });
                    if (yn == "Enable it!")
                        Config.Set("AppId480", true);
                }
            }
            Program.Settings();
        }
        public static void ProfileEditor()
        {
            Console.Clear();
            UI.WriteTitle();
            Console.WriteLine("AnyRec - Profile Editor");
            string name = Config.GetString("Username");
            if (name.Length > 35)
            {
                name = name.Remove(35);
                Config.Set("Username", name);
            }
            string op = UI.ShowList(new List<string> { "Go Back", $"Edit Username [Current: {name}]", $"Edit Level [Current: {Config.GetInt("Level")}]" });
            if (op == "Go Back")
            {
                Program.Main(new string[0]);
            }
            else if (op.StartsWith("Edit Username"))
            {
                Console.Clear();
                UI.WriteTitle();
                Console.Write("\nNew username: ");
                string str1 = Console.ReadLine();
                if (str1.Length > 35)
                {
                    str1 = str1.Remove(35);
                    Config.Set("Username", str1);
                }
                else
                {
                    Config.Set("Username", str1);
                }
                Program.ProfileEditor();
            }
            else if (op.StartsWith("Edit Level"))
            {
                Console.Clear();
                UI.WriteTitle();
                Console.Write("\nNew Level: ");
                string str1 = Console.ReadLine();
                if (int.TryParse(str1, out int lvl))
                {
                    Config.Set("Level", lvl);
                }
                Program.ProfileEditor();
            }
        }
    }
}