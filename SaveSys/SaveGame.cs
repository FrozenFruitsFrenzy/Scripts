using UnityEngine;

public class SaveGame : MonoBehaviour {

	public static int levelAvailable = 0;
	public static float[] bestTimeOnLevel;
	public static int[] deathOnLevel;

	void Start () {
		LoadProgress ();
	}

	void LoadProgress () {
		Progress progress = SaveSystem.LoadProgress (); // загружаем прогресс игрока
		if (progress != null) {
			levelAvailable = progress.levelAvailable;
			bestTimeOnLevel = progress.bestTimeOnLevel;
			deathOnLevel = progress.deathOnLevel;
		}
	}

	public static void SaveTime (float time, int level) {
		if(time < bestTimeOnLevel[level] || bestTimeOnLevel[level] == 0.0f)
			bestTimeOnLevel [level] = time; 
	}

	public static void SaveDeaths (int level) {
		deathOnLevel [level] += 1;

		if (deathOnLevel [level] > 10)
			AchievementActivation.UnlockAchievement ("ThisIsPointless");
	}

	public static void SaveProgress () {
		SaveSystem.SaveProgress (new Progress(levelAvailable, bestTimeOnLevel, deathOnLevel));
	}

	public static void DeleteProgress () {
		SaveSystem.Delete ("player");
		levelAvailable = 0;
		bestTimeOnLevel = null;
		bestTimeOnLevel = new float[60];
		deathOnLevel = null;
		deathOnLevel = new int[60];
	}
}
