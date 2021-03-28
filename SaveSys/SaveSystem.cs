using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
	
	public static void CheckDirectory() {
		if(!Directory.Exists(Application.dataPath + "/Saves/"))
			Directory.CreateDirectory (Application.dataPath + "/Saves/");
	}

	public static void CheckDirectoryLevels() {
		if(!Directory.Exists(Application.dataPath + "/Levels/"))
			Directory.CreateDirectory (Application.dataPath + "/Levels/");
	}

	public static void SaveProgress (Progress data) {
		CheckDirectory ();
		BinaryFormatter formatter = new BinaryFormatter ();
		string path = Application.dataPath + "/Saves/player.save";
		FileStream stream = new FileStream (path, FileMode.Create);
		formatter.Serialize (stream, data);
		stream.Close ();
	}

	public static void Delete (string pathPart) {
		string path = Application.dataPath + "/Saves/" + pathPart + ".save";
		File.Delete(path);
	}

	public static Progress LoadProgress() {
		string path = Application.dataPath + "/Saves/player.save";
		if (File.Exists (path)) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream stream = new FileStream (path, FileMode.Open);
			Progress data = formatter.Deserialize (stream) as Progress;
			stream.Close ();
			return data;
		} else {
			Debug.LogError ("Save file for progress not found in " + path);
			return null;
		}
	}

	public static void SaveSettings (Menu menu) {
		CheckDirectory ();
		BinaryFormatter formatter = new BinaryFormatter ();
		string path = Application.dataPath + "/Saves/settings.save";
		FileStream stream = new FileStream (path, FileMode.Create);

		Settings data = new Settings (menu);
		formatter.Serialize (stream, data);
		stream.Close ();
	}

	public static Settings LoadSettings () {
		string path = Application.dataPath + "/Saves/settings.save";
		if (File.Exists (path)) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream stream = new FileStream (path, FileMode.Open);
			Settings data = formatter.Deserialize (stream) as Settings;
			stream.Close ();
			return data;
		} else {
			Debug.LogError ("Save file for settings not found in " + path);
			return null;
		}
	}

	public static void SaveLevel (Editor editor, string name) {
		CheckDirectoryLevels ();
		BinaryFormatter formatter = new BinaryFormatter ();
		string path = Application.dataPath + "/Levels/" + name + ".lvl";
		FileStream stream = new FileStream (path, FileMode.Create);

		LevelInformation data = new LevelInformation (editor);
		formatter.Serialize (stream, data);
		stream.Close ();
	}

	public static LevelInformation LoadLevel (string name) {
		string path = Application.dataPath + "/Levels/" + name + ".lvl";
		if (File.Exists (path)) {
			BinaryFormatter formatter = new BinaryFormatter ();
			FileStream stream = new FileStream (path, FileMode.Open);
			LevelInformation data = formatter.Deserialize (stream) as LevelInformation;
			stream.Close ();
			return data;
		} else {
			Debug.LogError ("No level found at " + path);
			return null;
		}
	}


	public static LevelInformation LoadLevelOfficial (string name) {
		TextAsset levelFile = Resources.Load<TextAsset> ("Levels/" + name);
		if (levelFile) {
			BinaryFormatter formatter = new BinaryFormatter ();
			MemoryStream stream = new MemoryStream (levelFile.bytes);
			LevelInformation data = (LevelInformation)formatter.Deserialize (stream);
			stream.Close ();
			return data;
		} else {
			Debug.LogError ("No official level called " + name + "can be found in the game files!");
			return null;
		}
	}
}