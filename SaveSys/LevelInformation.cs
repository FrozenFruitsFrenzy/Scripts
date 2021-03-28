[System.Serializable]
public class LevelInformation {
	public int[,] blocks;
	public int fruitAmount;
	public int skyboxType;

	public LevelInformation (Editor editor) {
		blocks = editor.blocks;
		fruitAmount = editor.fruitAmount;
		skyboxType = editor.skyboxType;
	}
}