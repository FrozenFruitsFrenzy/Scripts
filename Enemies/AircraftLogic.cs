using UnityEngine;

public class AircraftLogic : Enemy {

	[Header ("Fast Access")]
	public Animation anim;

	[Header ("Current Variables")]
	public bool isRight;

	[Header ("Default Variables")]
	Quaternion defRotation;
	bool defIsRight;

	public override void Start () {
		base.Start();
		defIsRight = isRight;
		defRotation = isRight ? transform.rotation : Quaternion.Euler(new Vector3 (transform.rotation.x, 180, transform.rotation.z));
	}

	void FixedUpdate () {
		if (isDead)
			return;
		
		if (isRight)
			rb.MovePosition (transform.position + new Vector3 (0.117586f, 0, 0));
		else 
			rb.MovePosition (transform.position + new Vector3 (-0.117586f, 0, 0));
	}

	public override void OnCollisionEnter(Collision col){
		base.OnCollisionEnter (col);
		if (col.collider.CompareTag ("Cube")) {
			isRight = !isRight;
			if (isRight)
				anim.Play ("AircraftRotateRight");
			else
				anim.Play ("AircraftRotateLeft");
		}
	}

	public override void DefaultObject () {
		base.DefaultObject ();
		isRight = defIsRight;
		anim.Stop ();
		transform.rotation = defRotation;
	}
}