using AnyRec.Tools;
using System;

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
                string r = UI.ShowList(new List<string> { "Use default server"});
                if (r == "Use default server")
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

            }
            else if (choice == "Settings")
            {

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
            Config.SetDefault("PlayerId", 1);
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

            //Settings (Launcher)
            Config.SetDefault("DeveloperMode", false);
            Config.SetDefault("ForceSandboxMode", false);
            Config.SetDefault("MaxDownloads", 16);
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
    }
}