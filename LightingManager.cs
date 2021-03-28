using UnityEngine;

public class LightingManager : MonoBehaviour {
	public bool lightingEnabled;
	public int level;

	public void UpdateLevel(int newLevel) {
		level = newLevel;

		if (level == 3 && !lightingEnabled)
			GameObject.FindWithTag ("Player").GetComponent<BallColor> ().DisableLighting ();
	}
}
