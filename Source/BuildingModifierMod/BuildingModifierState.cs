using System.Collections.Generic;
using ONI_Common.Json;
using System.IO;

namespace BuildingModifierMod
{
    public class BuildingModifierState
    {
        public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;

		public Dictionary<string, Dictionary<string, object>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, object>>();
        public static BaseStateManager<BuildingModifierState> StateManager;
        string filetext = "";
        List<string> directorys = new List<string>();
        string basepath = ONI_Common.Paths.GetStatePath("BuildingModifierMod");
        void parsedirectory(string dir)        {
            foreach (string dir2 in Directory.GetDirectories(dir))            {
                directorys.Add(dir2);
                parsedirectory(dir2);            }
         }
         void addpath(string toadd) { 
            foreach (string file in Directory.GetFiles(toadd)) {
                if (!file.EndsWith(".json")) continue;
                if (file.Contains("__Temp.json")) continue;
                if (file.Contains("Opening.json")) continue;
                if (file.Contains("Closing.json")) continue;
                filetext += File.ReadAllText(file)+System.Environment.NewLine;
            }}
        public BuildingModifierState()
        {
            parsedirectory(basepath);
            foreach(string file2 in directorys)  addpath(file2);
            try            {
                filetext = File.ReadAllText(basepath + Path.DirectorySeparatorChar + "Opening.json") + System.Environment.NewLine + filetext + System.Environment.NewLine + File.ReadAllText(basepath + Path.DirectorySeparatorChar + "Closing.json");
            }            catch { }
            File.WriteAllText(basepath + Path.DirectorySeparatorChar + "__Temp.json", filetext);
            StateManager = new BaseStateManager<BuildingModifierState>("__Temp");
        }
	}
}