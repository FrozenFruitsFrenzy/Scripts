using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour {
	public GameObject[] steps;
	public GameObject text;
	public GameObject theCamera;
	public GameObject loaderObjects;

	public Object[] textures;
	public Object[] sprites;
	public Object[] models;
	public Object[] materials;
	public Object[] audios;
	public Object[] animations;
	public Object[] shaders;
	public Object[] fonts;
	public Object[] levels;
	public Object[] scripts;

    void Start() {
		Physics.autoSimulation = false;
		StartCoroutine (Initialization ());
    }

	IEnumerator Initialization() {
		yield return new WaitForSeconds(0.5f);

		text.GetComponent<TMP_Text> ().text = "INITIALIZATION";

		GameObject MainCameraPrefab = Resources.Load("Prefabs/Main Camera") as GameObject;
		theCamera = Instantiate (MainCameraPrefab, Vector3.zero, Quaternion.Euler (Vector3.zero));

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadScenes ());
	}

	IEnumerator LoadScenes() {
		steps[0].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING SCENES";

		theCamera.GetComponent<LevelChanger> ().StartLoadingLevels ();

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadTextures ());
	}

	IEnumerator LoadTextures() {
		steps[1].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING TEXTURES & SPRITES";

		textures = Resources.LoadAll("Models/Textures", typeof(Texture2D));
		sprites = Resources.LoadAll("Sprites", typeof(Texture2D));

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadMeshes ());
	}

	IEnumerator LoadMeshes() {
		steps[2].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING MESHES & MATERIALS";

		materials = Resources.LoadAll("Models/Materials", typeof(Material));
		models = Resources.LoadAll("Models", typeof(Mesh));

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadAudio ());
	}

	IEnumerator LoadAudio() {
		steps[3].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING AUDIO";

		audios = Resources.LoadAll("Sounds", typeof(AudioClip));

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadAssets ());
	}

	IEnumerator LoadAssets() {
		steps[4].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING OTHER ASSETS";

		animations = Resources.LoadAll("Animations");
		shaders = Resources.LoadAll("Shaders");
		fonts = Resources.LoadAll("Fonts");
		levels = Resources.LoadAll("Levels");

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (LoadScripts ());
	}

	IEnumerator LoadScripts() {
		steps[5].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "LOADING SCRIPTS";

		scripts = Resources.LoadAll("Scripts");

		yield return new WaitForSeconds(0.0f);
		StartCoroutine (InitializeScripts ());
	}

	IEnumerator InitializeScripts() {
		steps[6].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "INITIALIZING SCRIPTS";

		theCamera.GetComponent<SaveGame> ().enabled = true;
		theCamera.GetComponent<SteamManager> ().enabled = true;
		theCamera.GetComponent<CameraControl> ().enabled = true;
		theCamera.GetComponent<FPSCounter> ().enabled = true;
		theCamera.GetComponent<TimerCounter> ().enabled = true;
		theCamera.GetComponent<TimeManager> ().enabled = true;
		theCamera.GetComponent<BlurControl> ().enabled = true;
		theCamera.GetComponent<Pause> ().enabled = true;
		theCamera.GetComponent<SoundManager> ().enabled = true;

		yield return new WaitForSeconds(0.1f);
		StartCoroutine (Done ());
	}

	IEnumerator Done() {
		steps[7].gameObject.SetActive(true);

		text.GetComponent<TMP_Text> ().text = "DONE";

		yield return new WaitForSeconds(0.4f);

		theCamera.GetComponent<LevelChanger> ().ActivateMenu ();
		Destroy (loaderObjects);
	}
}