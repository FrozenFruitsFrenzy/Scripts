using UnityEngine;

public class CameraControl: MonoBehaviour {
	public GameObject Player;
	public enum CameraTypes {Smooth, Vanilla, Centered};
	public CameraTypes CameraType;
	public Camera myCamera;
	public Camera SkyboxCamera;
	public GameObject AudioListenerGO;
	private Vector3 delta;
	[Range(25,35)]
	public int CameraDistance = 30;
	public bool Orthographic;

	private int level;

	void FixedUpdate () {
		if (level != 3)
			return;
		
		if (CameraType == CameraTypes.Vanilla) {
			delta = new Vector3 (transform.position.x, transform.position.y, -CameraDistance);
			transform.position = delta;
			delta = Vector3.zero;
			float dx = Player.transform.position.x - transform.position.x;
			if (dx > 6 || dx < -6)
				delta.x = (transform.position.x < Player.transform.position.x) ? dx - 6 : dx + 6;
			float dy = Player.transform.position.y - transform.position.y;
			if (dy > 6 || dy < -6)
				delta.y = (transform.position.y < Player.transform.position.y) ? dy - 6 : dy + 6;
			transform.position = transform.position + delta;
		}

		if (CameraType == CameraTypes.Smooth) {
			delta = new Vector3 (0, 0, -CameraDistance);
			transform.position = Vector3.Lerp (transform.position, Player.transform.position + delta, 6.0f * Time.deltaTime);
		}

		if (CameraType == CameraTypes.Centered) {
			delta = new Vector3 (Player.transform.position.x, Player.transform.position.y, -CameraDistance);
			transform.position = delta;
		}
	}

	public void UpdateLevel(int newLevel) {
		level = newLevel;

		myCamera.enabled = !(level == 2);
		SkyboxCamera.enabled = !(level == 2);


		if (level == 3) {
			FindPlayer(GameObject.FindGameObjectWithTag ("Player"));
			delta = new Vector3 (Player.transform.position.x, Player.transform.position.y);
			transform.position = delta;

			AudioListenerGO.transform.position = new Vector3 (AudioListenerGO.transform.position.x, AudioListenerGO.transform.position.y, CameraDistance);
		}
	}

	public void FindPlayer(GameObject newPlayer) {
		Player = newPlayer;
	}

	public void RenderDetails(bool value) {
		if (value)
			myCamera.cullingMask = -1;
		else
			myCamera.cullingMask = 32567;
	}
}