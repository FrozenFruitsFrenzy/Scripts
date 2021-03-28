using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class FPSCounter : MonoBehaviour {
	public bool isOn;
	[Range (0,10)] 
	public int Delay = 10; 
	public TMP_Text Counter;
	public GameObject CounterGameObject;

	void Start () {
		StartCoroutine (UpdateCounter ());
	}

	public void EnableCounter () {
		CounterGameObject.SetActive (true);
		isOn = true;
	}

	public void DisableCounter () {
		CounterGameObject.SetActive (false);
		isOn = false;
	}

	private IEnumerator UpdateCounter() {
		while (true) {
			if (isOn) {
				yield return new WaitForSecondsRealtime (((float)Delay) / 10);
				Counter.text = "FPS: " + ((int)(1f / Time.unscaledDeltaTime)).ToString ();
			} else
				yield return null;
		}
	}
}