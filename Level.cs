using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	[Header ("General Level Information")]
	public bool isLevelCustom;
	public string levelName;
	public int defaultFruitAmount;
	public GameObject PlayerPrefab;
	public Vector3 PlayerStartingPosition;
	public bool[,] defIsNotEmpty = new bool[80, 80];
	[Header ("Playthrough Info")]
	public bool Dead;
	public bool VictoryAchieved;

	public GameObject[] Enemies = new GameObject[6400];
	public int lastEnemyIndex;
	public Queue<GameObject> DestroyedGameObjects = new Queue<GameObject>();
	public Queue<GameObject> ObjectsToBeDefaulted = new Queue<GameObject>();

	public static bool fiveMinsOn;
	public float timeSpentOnLevel;
	public int AmountOfDeaths;
	public int currentFruitAmount;
	public bool[,] isNotEmpty = new bool[80, 80];
	[Header ("Level Elements")]
	public GameObject Player;
	public GameObject[,] Cubes = new GameObject[80, 80];
	public CubeCheck[,] CubeCheckScripts = new CubeCheck[80, 80];
	public GameObject myCamera;
	public LevelChanger LevelChangerScript;
	public CameraControl CameraControlScript;
	public TimerCounter TimerCounterScript;

	void Awake() {
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		Cursor.visible = false;
		fiveMinsOn = false;
	}

    void Start() {
		Player = GameObject.FindGameObjectWithTag ("Player");
		GameObject myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		CameraControlScript = myCamera.GetComponent<CameraControl> ();
		LevelChangerScript = myCamera.GetComponent<LevelChanger> ();
		TimerCounterScript = myCamera.GetComponent<TimerCounter> ();
		currentFruitAmount = defaultFruitAmount;
		isNotEmpty = defIsNotEmpty.Clone() as bool[,];
    }

	void Update() {
		if(!Dead)
			timeSpentOnLevel += Time.deltaTime;

		if (!Dead && Input.GetKeyDown(KeyCode.R)) {
			Death ();
		} else if (Dead && Input.anyKeyDown) {
			RestoreLevel ();
		}
	}

	public void Death () {
		Player.GetComponent<AudioSource> ().Play ();

		myCamera.GetComponent<SoundManager> ().SetDead (true);
		Dead = true; 
		Player.transform.Find ("Light").GetComponent<Light> ().enabled = false;

		Player.SendMessage("DestroyObject");
		Material partMaterial = Player.GetComponent<BallColor> ().SphereMaterial;
		Player.GetComponent<BallParts> ().EnableParts (partMaterial);

		if (!isLevelCustom) {
			SaveGame.SaveDeaths (Convert.ToInt32 (levelName));
			SaveGame.SaveProgress ();
		}
		TimerCounterScript.SwitchDead (true);
	}

	public void Victory () {
		if (Dead)
			return;
		
		Destroy (this.gameObject);

		if (levelName == "EditorTest") {
			LevelChangerScript.ActivateEditor (true);
			return;
		}
	
		if (fiveMinsOn)
			AchievementActivation.UnlockAchievement ("ImStillWorthy");
			
		if (!isLevelCustom) {
			int levelNumber = Convert.ToInt32 (levelName);
			if (SaveGame.levelAvailable < levelNumber)
				SaveGame.levelAvailable = levelNumber; 
			SaveGame.SaveTime (timeSpentOnLevel, levelNumber);
		}
		LevelChangerScript.ActivateMenu ();
	}

	public void Restart () {
		if (!isLevelCustom) {
			SaveGame.SaveDeaths (Convert.ToInt32 (levelName));
			SaveGame.SaveProgress ();
		}
		TimerCounterScript.ResetTime ();
		timeSpentOnLevel = 0;

		RestoreCubes ();
		DefaultCubes ();
		DefaultEnemies ();

		isNotEmpty = defIsNotEmpty.Clone() as bool[,];
		currentFruitAmount = defaultFruitAmount;

		Player.SendMessage("DefaultObject");
	}

	public void AddToDestroyed(GameObject DestroyedGameObject) {
		DestroyedGameObjects.Enqueue (DestroyedGameObject);
	}

	public void AddToDefaulted(GameObject ObjectToBeDefaulted) {
		ObjectsToBeDefaulted.Enqueue (ObjectToBeDefaulted);
	}

	public void AddToEnemies(GameObject Enemy) {
		Enemies [lastEnemyIndex] = Enemy;
		lastEnemyIndex++;
	}

	void RestoreLevel() {
		Dead = false; 
		timeSpentOnLevel = 0;

		RestoreCubes ();
		DefaultCubes ();
		DefaultEnemies ();

		isNotEmpty = defIsNotEmpty.Clone() as bool[,];
		currentFruitAmount = defaultFruitAmount;

		Player.SendMessage("DefaultObject");

		myCamera.GetComponent<SoundManager> ().SetDead (false);
		TimerCounterScript.SwitchDead (false);
	}

	void RestoreCubes() {
		int count = DestroyedGameObjects.Count;
		for (int i = 0; i < count; i++) {
			GameObject go = DestroyedGameObjects.Dequeue ();
			go.SetActive (true);
		}
	}

	void DefaultCubes() {
		int count = ObjectsToBeDefaulted.Count;
		for (int i = 0; i < count; i++) {
			GameObject go = ObjectsToBeDefaulted.Dequeue ();
			go.GetComponent<CubeCheck> ().DefaultObject ();
		}
	}

	void DefaultEnemies() {
		for (int i = 0; i < 6400; i++)
			if (Enemies [i] != null)
				Enemies [i].GetComponent<Enemy> ().DefaultObject ();
			else
				return;
	}
}