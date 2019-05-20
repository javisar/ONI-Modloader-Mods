using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

class ConfigUtils<T>
{

	public static bool DoReloadConfig = false;

	protected static FileSystemWatcher fileSystemWatcher;

	protected static T _Instance;


	public static T LoadConfig(string filePath, string fileName, bool force)
	{
		if (_Instance != null && !DoReloadConfig && !force)
			return _Instance;


		string _path = Path.Combine(GetModPath(), filePath + Path.DirectorySeparatorChar + fileName);

		if (!DoReloadConfig)
		{
			Debug.Log("Loading Config from: " + _path);
			AddFileWatcher(filePath, fileName);
		}
		else
		{
			Debug.Log("Reloading Config from: " + _path);
		}

		DoReloadConfig = false;

		T result;
		try
		{
			JsonSerializer serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings { Formatting = Formatting.Indented, ObjectCreationHandling = ObjectCreationHandling.Replace });

			using (StreamReader streamReader = new StreamReader(_path))
			{
				using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
				{
					result = serializer.Deserialize<T>(jsonReader);
					jsonReader.Close();
				}
				streamReader.Close();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
			result = (T)Activator.CreateInstance(typeof(T));
		}

		_Instance = result;
		return result;
	}

	public static void AddFileWatcher(string filePath, string fileName)
	{
		if (fileSystemWatcher != null) return;

		fileSystemWatcher = new FileSystemWatcher(Path.Combine(GetModPath(), filePath));
		fileSystemWatcher.Changed += delegate (object sender, FileSystemEventArgs e)
		{
			if (Path.GetFileName(e.Name).Equals(fileName))
			{
				DoReloadConfig = true;
			}
		};
		fileSystemWatcher.EnableRaisingEvents = true;
	}

	protected static string GetModPath()
	{
		return Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(T)).Location);
	}
}

