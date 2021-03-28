using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParts : MonoBehaviour {
	[Header ("Ball Components")]
	public Rigidbody RBody;
	public MeshRenderer MRenderer;
	public SphereCollider SphCollider;
	[Header ("Parts Components")]
	public GameObject[] ballPartObjects;
	public MeshRenderer[] ballPartMeshRenderers;

	public void EnableParts(Material partMaterial) {
		SwitchBall (false);
		for (int i = 0; i < ballPartObjects.Length; i++) {
			ballPartObjects [i].gameObject.SetActive (true);
			ballPartMeshRenderers [i].material = partMaterial;
		}
	}

	public void DefaultObject() {
		for (int i = 0; i < ballPartObjects.Length; i++) {
			ballPartObjects [i].gameObject.SetActive (false);
			ballPartObjects [i].transform.position = transform.TransformPoint(Vector3.zero);
			ballPartObjects [i].transform.rotation = Quaternion.identity;
		}
		SwitchBall (true);
	}

	void SwitchBall (bool enable) {
		SphCollider.enabled = enable;
		MRenderer.enabled = enable;
		RBody.useGravity = enable;
	}
}
