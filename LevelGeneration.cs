using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

	public GameObject[] GameObjectsByTypes;
	public GameObject Manager;
	public Material[] skyboxTypes;
	//Загружаемая информация.
	private int[,] blocks = new int[80,80];
	private GameObject[,] Cubes = new GameObject[80,80];
	public CubeCheck[,] CubeCheckScripts = new CubeCheck[80, 80];
	public bool[,] isNotEmpty = new bool[80,80];
	private int fruitAmount;
	private int skyboxType;

	public void GenerateLevel(string levelName, bool isLevelCustom) {
			LevelInformation levelInfo;

		if (isLevelCustom)
			levelInfo = SaveSystem.LoadLevel (levelName);
		else
			levelInfo = SaveSystem.LoadLevelOfficial (levelName);

		if (levelInfo != null) {
			blocks = levelInfo.blocks;
			fruitAmount = levelInfo.fruitAmount;
			skyboxType = levelInfo.skyboxType;
		}

		//TODO: сделать обработку исключения, если уровень НЕ загрузился.

		GameObject levelManager = Instantiate (Manager, Vector3.zero, Quaternion.Euler (0, 0, 0));
		levelManager.transform.SetParent (GameObject.Find("GameMain").transform);

		Level levelManagerScript = levelManager.GetComponent<Level> ();
		levelManagerScript.defaultFruitAmount = fruitAmount;
		levelManagerScript.levelName = levelName;
		levelManagerScript.isLevelCustom = isLevelCustom;

		int i;
		if (skyboxType == 0)
			i = Random.Range (0, 3);
		else
			i = skyboxType - 1;
		RenderSettings.skybox = skyboxTypes [i];

		for (int x = 0; x < 80; x++)
			for (int y = 0; y < 80; y++) {
				int CubeType = blocks [x, y] / 100;
				int CubeData = blocks [x, y] % 100;

				GameObject obj = FindBlockByID (CubeType, CubeData);
				if (obj) {
					if(CubeType == 99)
						Cubes[x, y] = Instantiate (obj, new Vector3 (x * 3 + 1.5f, y * 3, 0), obj.transform.rotation, levelManager.transform);
					else
						Cubes[x, y] = Instantiate (obj, new Vector3 (x * 3, y * 3, 0), obj.transform.rotation, levelManager.transform);

					if (CubeType == 1) {
						levelManagerScript.PlayerStartingPosition = new Vector3 (x * 3, y * 3);
						levelManagerScript.PlayerPrefab = obj;
					} else if (CubeType < 50) {
						isNotEmpty [x, y] = true;
						CubeCheckScripts [x, y] = Cubes [x, y].GetComponent<CubeCheck> ();
					}
				}
			}
		levelManagerScript.Cubes = Cubes;
		levelManagerScript.CubeCheckScripts = CubeCheckScripts;
		levelManagerScript.defIsNotEmpty = isNotEmpty.Clone() as bool[,];
		Destroy (this.gameObject);
	}

	public GameObject FindBlockByID (int CubeType, int CubeData) {
		if (CubeType == 0)
			return null;
		else if (CubeType == 1) {
			if (CubeData == 0)
				return GameObjectsByTypes [0];
			else if (CubeData == 1)
				return GameObjectsByTypes [1];
			else if (CubeData == 2)
				return GameObjectsByTypes [2];
			else if (CubeData == 3)
				return GameObjectsByTypes [3];
			else if (CubeData == 4)
				return GameObjectsByTypes [4];
			else
				return GameObjectsByTypes [5];
		} else if (CubeType == 2) {
			if (CubeData == 0)
				return GameObjectsByTypes [6];
			else if (CubeData == 1)
				return GameObjectsByTypes [7];
			else if (CubeData == 2)
				return GameObjectsByTypes [8];
			else if (CubeData == 3)
				return GameObjectsByTypes [9];
			else if (CubeData == 4)
				return GameObjectsByTypes [10];
			else if (CubeData == 5)
				return GameObjectsByTypes [11];
			else
				return GameObjectsByTypes [12];
		} else if (CubeType == 3) {
			if (CubeData == 0)
				return GameObjectsByTypes [13];
			else if (CubeData == 1)
				return GameObjectsByTypes [14];
			else if (CubeData == 2)
				return GameObjectsByTypes [15];
			else if (CubeData == 3)
				return GameObjectsByTypes [16];
			else if (CubeData == 4)
				return GameObjectsByTypes [17];
			else if (CubeData == 5)
				return GameObjectsByTypes [18];
			else
				return GameObjectsByTypes [19];
		} else if (CubeType == 4) {
			if (CubeData == 0)
				return GameObjectsByTypes [20];
			else if (CubeData == 1)
				return GameObjectsByTypes [21];
			else if (CubeData == 2)
				return GameObjectsByTypes [22];
			else if (CubeData == 3)
				return GameObjectsByTypes [23];
			else if (CubeData == 4)
				return GameObjectsByTypes [24];
			else
				return GameObjectsByTypes [25];
		} else if (CubeType == 5) {
			if (CubeData == 0)
				return GameObjectsByTypes [26];
			else if (CubeData == 1)
				return GameObjectsByTypes [27];
			else if (CubeData == 2)
				return GameObjectsByTypes [28];
			else if (CubeData == 3)
				return GameObjectsByTypes [29];
			else if (CubeData == 4)
				return GameObjectsByTypes [30];
			else
				return GameObjectsByTypes [31];
		} else if (CubeType == 6) {
			if (CubeData == 0)
				return GameObjectsByTypes [32];
			else if (CubeData == 1)
				return GameObjectsByTypes [33];
			else if (CubeData == 2)
				return GameObjectsByTypes [34];
			else if (CubeData == 3)
				return GameObjectsByTypes [35];
			else if (CubeData == 4)
				return GameObjectsByTypes [36];
			else
				return GameObjectsByTypes [37];
		} else if (CubeType == 7)
			return GameObjectsByTypes [38];
		else if (CubeType == 8)
			return GameObjectsByTypes [43];
		else if (CubeType == 9)
			return GameObjectsByTypes [44];
		else if (CubeType == 10)
			return GameObjectsByTypes [45];
		else if (CubeType == 11)
			return GameObjectsByTypes [46];
		else if (CubeType == 12)
			return GameObjectsByTypes [47];
		else if (CubeType == 98) {
			if (CubeData == 0)
				return GameObjectsByTypes [41];
			else
				return GameObjectsByTypes [42];
		}
		else if (CubeType == 99) {
			if (CubeData == 0)
				return GameObjectsByTypes [39];
			else
				return GameObjectsByTypes [40];
		}
		return null;
	}
}
