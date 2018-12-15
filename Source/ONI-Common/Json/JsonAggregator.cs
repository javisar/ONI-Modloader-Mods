using System.Collections.Generic;
using ONI_Common.Json;
using System.IO;

namespace ONI_Common.Json
{
	public class JsonAggregator
	{
		public bool Enabled { get; set; } = true;

		public bool Debug { get; set; } = false;

		public Dictionary<string, Dictionary<string, object>> Modifiers { get; set; } = new Dictionary<string, Dictionary<string, object>>();
		string filetext = "";
		List<string> directorys = new List<string>();
		string basepath;// = ONI_Common.Paths.GetStatePath("BuildingModifierMod");
		string filename= "__Temp.json";
		void parsedirectory(string dir)
		{
			foreach (string dir2 in Directory.GetDirectories(dir)) {
				directorys.Add(dir2);
				parsedirectory(dir2);
			}
		}
		public string Aggregate(string basepathin)
		{
			basepath = basepathin;
			parsedirectory(basepath);
			foreach (string file2 in directorys) addpath(file2);
			try {
				filetext = File.ReadAllText(basepath + Path.DirectorySeparatorChar + "Opening.json") + System.Environment.NewLine + filetext + System.Environment.NewLine + File.ReadAllText(basepath + Path.DirectorySeparatorChar + "Closing.json");
			}
			catch { }
			File.WriteAllText(basepath + Path.DirectorySeparatorChar + filename, filetext);
			return filename;
		}
		List<string> duplicatepurge = new List<string>();
		void addpath(string toadd)
		{
			foreach (string file in Directory.GetFiles(toadd)) {
				if (!file.EndsWith(".json")) continue;
				if (file.Contains(filename)) continue;
				if (duplicatepurge.Contains(System.IO.Path.GetFileName(file))) continue;
				if (file.Contains("Opening.json")) continue;
				if (file.Contains("Closing.json")) continue;
				filetext += File.ReadAllText(file) + System.Environment.NewLine;
				duplicatepurge.Add(System.IO.Path.GetFileName(file));
			}
		}
	}
}
