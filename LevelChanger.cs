using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

public class LevelChanger : MonoBehaviour {

	public GameObject LevelGenerator;

	public AsyncOperation asyncLoad1;
	public AsyncOperation asyncLoad2;
	public AsyncOperation asyncLoad3;

	public GameObject MenuObject;
	public GameObject EditorObject;
	public GameObject PlayObject;

	public string currentPlayableLevel;
	private int levelNumber;

	public CameraControl CameraControlScript;
	public Pause PauseScript;
	public TimerCounter TimerCounterScript;
	public BlurControl BlurControlScript;
	public LightingManager LightingManagerScript;

	void Awake() {
		try {GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;} catch {}
		Physics.autoSimulation = false;
	}

	public void StartLoadingLevels() {
		StartCoroutine (LoadScenes ());
	}

	IEnumerator LoadScenes() {
		asyncLoad1 = SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);

		while (!asyncLoad1.isDone)
			yield return null;

		MenuObject = GameObject.Find ("MenuMain");
		MenuObject.SetActive (false);
		
		asyncLoad2 = SceneManager.LoadSceneAsync("Editor", LoadSceneMode.Additive);

		while (!asyncLoad2.isDone)
			yield return null;

		EditorObject = GameObject.Find ("EditorMain");
		EditorObject.SetActive (false);

		asyncLoad3 = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);

		while (!asyncLoad3.isDone)
			yield return null;
		
		PlayObject = GameObject.Find ("GameMain");
		PlayObject.SetActive (false);
	}

	public void ActivateMenu () {
		EditorObject.SetActive (false);
		PlayObject.SetActive (false);
		MenuObject.SetActive (true);

		FreeMemory ();

		Physics.autoSimulation = false;
		Cursor.visible = true;

		SceneManager.SetActiveScene (SceneManager.GetSceneByBuildIndex (1));

		UpdateLevel (1);
	}

	public void ActivateEditor (bool canLevelBeSaved) {
		PlayObject.SetActive (false);
		MenuObject.SetActive (false);
		EditorObject.SetActive (true);

		FreeMemory ();

		Physics.autoSimulation = false;
		Cursor.visible = true;

		SceneManager.SetActiveScene (SceneManager.GetSceneByBuildIndex (2));

		if (canLevelBeSaved)
			EditorObject.transform.Find ("Camera").GetComponent<Editor> ().canLevelBeSaved = true;
		
		UpdateLevel (2);
	}

	public void ActivateGame(string levelName) {
		MenuObject.SetActive (false);
		EditorObject.SetActive (false);
		PlayObject.SetActive (true);

		FreeMemory ();

		Physics.autoSimulation = true;
		Cursor.visible = false;

		SceneManager.SetActiveScene (SceneManager.GetSceneByBuildIndex (3));

		currentPlayableLevel = levelName;

		bool isLevelCustom = false;
		if (!Int32.TryParse (currentPlayableLevel, out levelNumber))
			isLevelCustom = true;
			
		GameObject Generator = Instantiate (LevelGenerator);
		Generator.GetComponent<LevelGeneration> ().GenerateLevel (levelName, isLevelCustom);

		UpdateLevel (3);
	}

	void UpdateLevel(int newLevel) {
		CameraControlScript.UpdateLevel (newLevel);
		PauseScript.UpdateLevel (newLevel);
		TimerCounterScript.UpdateLevel (newLevel);
		BlurControlScript.UpdateLevel (newLevel);
		LightingManagerScript.UpdateLevel (newLevel);
	}

	void FreeMemory() {
		try {
			GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
			GC.Collect ();
			GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
		} catch {}
	}
}
