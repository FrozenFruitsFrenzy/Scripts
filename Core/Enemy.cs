using UnityEngine;

public abstract class Enemy : MonoBehaviour {
	[Header ("BASE Fast Access")]
	public Level LevelScript;
	public MeshRenderer mRenderer;
	public Collider mCollider;
	public Rigidbody rb;

	[Header ("BASE Current Variables")]
	public bool isDead;

	[Header ("BASE Default Variables")]
	public Vector3 defPosition;

	public virtual void Start() {
		LevelScript = transform.parent.GetComponent<Level>();
		LevelScript.AddToEnemies (this.gameObject);

		defPosition = transform.position;
	}

	public virtual void OnCollisionEnter(Collision col) {
		if (col.collider.CompareTag ("Player")) {
			rb.velocity = Vector3.zero;
			Death ();
		}
	}

	public virtual void Death() {
		LevelScript.Death ();
	}
   
	public virtual void DestroyObject () {
		isDead = true;
		mRenderer.enabled = false;
		mCollider.enabled = false;
	}

	public virtual void DefaultObject () {
		isDead = false;
		mRenderer.enabled = true;
		mCollider.enabled = true;
		transform.position = defPosition;
	}
}