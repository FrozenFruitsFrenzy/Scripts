using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {
	private BlurControl Blur;
	private TimeManager TimeManage;
	private LevelChanger LevelChange;
	private GameObject LevelObject;
	private Level LevelScript;

	[Header ("Other Elements")]
	public GameObject PauseMenu;

	[Header ("General Variables")]
	public int level;
	public bool paused = false;

    void Start() {
		Blur = GetComponent<BlurControl> ();
		TimeManage = GetComponent<TimeManager> ();
		LevelChange = GetComponent<LevelChanger> ();
    }

    // Update is called once per frame
    void Update() {
		if ((Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.JoystickButton7)) && level == 3)
			if (paused)
				CloseMenu ();
			else
				OpenMenu ();
	}

	public void Exit() {
		CloseMenu ();
		Destroy(LevelObject);

		if (LevelChange.currentPlayableLevel == "EditorTest")
			LevelChange.ActivateEditor (false);
		else
			LevelChange.ActivateMenu ();
	}

	void OpenMenu() {
		paused = true;
		PauseMenu.SetActive (true);
		TimeManage.ChangeState (true);
		Blur.Pause ();
		Cursor.visible = true;
	}

	public void CloseMenu() {
		paused = false;
		PauseMenu.SetActive (false);
		TimeManage.ChangeState (false);
		Blur.Unpause ();
		if (level == 3)
			Cursor.visible = false;
	}

	public void Restart() {
		LevelScript.Restart ();
		CloseMenu ();
	}

	public void UpdateLevel(int newLevel) {
		level = newLevel;

		if (level == 3) {
			LevelObject = LevelChange.PlayObject.transform.Find ("Level(Clone)").gameObject;
			LevelScript = LevelObject.GetComponent<Level> ();
		}
		
		CloseMenu ();
	}
}
