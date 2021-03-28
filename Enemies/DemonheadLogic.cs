using UnityEngine;
public class DemonheadLogic : Enemy {

	[Header ("Fast Access")]
	public Animation anim;

	[Header ("Current Variables")]
	public GameObject Ground;
	public bool isRight;
	public float HorizontalSpeed;
	public float VerticalSpeed;

	[Header ("Default Variables")]
	bool defRight;

	public override void Start () {
		base.Start();
		defRight = isRight;
	}

	void Update () {
		if (isDead)
			return;
		
		if (Ground && Ground.activeSelf == false)
			Ground = null;

		if (Ground)
			rb.velocity = new Vector3 (isRight ? HorizontalSpeed : -HorizontalSpeed, 0f);
		else
			rb.velocity = new Vector3 (0, -VerticalSpeed);
	}

	public override void OnCollisionEnter(Collision col) {
		base.OnCollisionEnter (col);
		if (col.collider.CompareTag ("Cube")) {
			Vector3 normal = col.GetContact (0).normal;
			CubeCheck script = col.gameObject.GetComponent<CubeCheck> ();
			int type = script.CubeType;
			if (Mathf.Abs (normal.x) > Mathf.Abs (normal.y))
				ChangeDirection (col, type, script);
			else {
				if (transform.position.y > col.gameObject.transform.position.y) {
					Ground = col.gameObject;
					if (type >= 7 && type <= 13)
						DestroyObject ();
				}
			}
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.gameObject == Ground)
			Ground = null;
	}

	void OnCollisionStay(Collision col) {
		Vector3 normal = col.GetContact (0).normal;
		CubeCheck script = col.gameObject.GetComponent<CubeCheck> ();
		int type = script.CubeType;
		if (Mathf.Abs (normal.x) > Mathf.Abs (normal.y))
			ChangeDirection (col, type, script);
		else if (transform.position.y > col.gameObject.transform.position.y)
			Ground = col.gameObject;
	}

	void ChangeDirection(Collision col, int type, CubeCheck script) {
		if (!Ground)
			return;

		bool newIsRight = transform.position.x > col.gameObject.transform.position.x;
		anim ["DemonheadRotate"].speed = isRight ? -1.0f : 1.0f;
		if (isRight != newIsRight)
			if (type >= 0 || type == -3)
				script.DeleteCube ();
		isRight = newIsRight;
	}

	public override void DefaultObject () {
		base.DefaultObject ();
		Ground = null;
		isRight = defRight;
		anim.Stop ();
		anim ["DemonheadRotate"].speed = isRight ? -1.0f : 1.0f;
		anim.Play("DemonheadRotate");
	}
}