[System.Serializable]
public class Progress {
	public int levelAvailable;
	public float[] bestTimeOnLevel;
	public int[] deathOnLevel;

	public Progress (int newLevelAvailable, float[] newBestTimeOnLevel, int[] newDeathOnLevel) {
		levelAvailable = newLevelAvailable;
		bestTimeOnLevel = newBestTimeOnLevel;
		deathOnLevel = newDeathOnLevel;
	}
}