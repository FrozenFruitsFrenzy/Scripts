using UnityEngine;
using System.Collections;

public class BlurControl : MonoBehaviour {
	
	private float[] typeValue = { 0.0f, 3.5f, 6.0f };
	private float actualValue = 6.0f;

	[Header ("Other Elements")]
	public GameObject BlurEffect;
	private Material BlurMaterial;

	[Header ("Public Variables")]
	public int currentType = 2;
	public int level;
	public bool blurEnabled;


    void Start() {
		BlurMaterial = BlurEffect.GetComponent<MeshRenderer> ().material;
    }

	private IEnumerator SetNewValue() {
		while(Mathf.Abs(actualValue - typeValue [currentType]) > 0.1f) {
			actualValue = Mathf.Lerp(actualValue, typeValue [currentType], 2.0f * Time.unscaledDeltaTime);
			BlurMaterial.SetFloat ("_Size", actualValue);
			yield return null;
		}
		actualValue = typeValue [currentType];
		BlurMaterial.SetFloat ("_Size", actualValue);
		if (currentType == 0)
			BlurEffect.SetActive (false);
	}

	public void UpdateLevel(int newLevel) {
		if (!blurEnabled)
			return;
		
		BlurEffect.SetActive (true);
		
		level = newLevel;

		if (level == 1)
			currentType = 2;
		else
			currentType = 0;

		StartCoroutine (SetNewValue ());
	}
	public void Pause() {
		if (!blurEnabled)
			return;
		
		BlurEffect.SetActive (true);
		currentType = 1;
		StartCoroutine (SetNewValue ());
	}

	public void Unpause() {
		if (!blurEnabled)
			return;
		
		currentType = 0;
		StartCoroutine (SetNewValue ());
	}

	public void EnableBlur() {
		blurEnabled = true;
		BlurEffect.SetActive (true);
	}

	public void DisableBlur() {
		blurEnabled = false;
		BlurEffect.SetActive (false);
	}
}
