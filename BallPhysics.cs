using UnityEngine; 
using System.Collections;

public class BallPhysics : MonoBehaviour {
	[Header ("Fast Access")]
	public Level LevelScript;
	public bool[,] isNotEmpty;
	private Rigidbody rb;

	[Header ("Current Variables")]
	public bool up; 
	private float ballSpeed; 
	private Vector3 VerticalVelocity;
	private bool isRight; 
	private bool isUp; 
	private float timer1; 
	private Vector3 HorizontalVelocity;
	private Vector3 OverallVelocity;
	public bool iced;
	public bool rotated;
	public bool isDead; 
	private int lastHitX;
	private int lastHitY;

	[Header ("Default Variables")]
	public Vector3 defPosition;

	void Start () {
		defPosition = transform.position;
		LevelScript = transform.parent.GetComponent<Level>();
		isNotEmpty = LevelScript.isNotEmpty;
		rb = GetComponent<Rigidbody>();
	}

	void Update () {
		if (timer1 <= 0) {
			if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
				ballSpeed = 12;
			else
				ballSpeed = 6;
		}

		if (!isDead) {
			if (timer1 <= 0) {
				if (!iced) {
				
					if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A) || Input.GetAxis ("Horizontal") < 0)
						HorizontalVelocity = new Vector3 (-17f, 0f);
					else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D) || Input.GetAxis ("Horizontal") > 0)
						HorizontalVelocity = new Vector3 (17f, 0f);
					else
						HorizontalVelocity = Vector3.zero;
				
					//HorizontalVelocity = Vector3.Lerp (new Vector3(rb.velocity.x, 0), HorizontalVelocity, Time.deltaTime * 30);
				
					VerticalVelocity = new Vector3 (0.0f, up ? ballSpeed : -ballSpeed);		

					if (!rotated)
						OverallVelocity = new Vector3 (HorizontalVelocity.x, VerticalVelocity.y);
					else
						OverallVelocity = new Vector3 (VerticalVelocity.y, HorizontalVelocity.x);
				}
			} else {
				if (timer1 > 0) 
					timer1 -= Time.deltaTime;
				if (timer1 > 0.3f) 
					timer1 = 0.3f;
			}
		} else
			OverallVelocity = Vector3.zero;
		rb.velocity = OverallVelocity;
	}
	
	void OnCollisionEnter(Collision col){
		if (!col.collider.CompareTag ("Cube"))
			return;
		
		Vector3 normal = col.GetContact (0).normal;
		Vector3 cubePosition = col.gameObject.transform.position;

		int currentHitX = Mathf.RoundToInt (cubePosition.x) / 3;
		int currentHitY = Mathf.RoundToInt (cubePosition.y) / 3;

		if (!rotated) {
			if ((Mathf.Abs (currentHitX - lastHitX) == 1 && currentHitY == lastHitY) ||
			    (Mathf.Abs (currentHitY - lastHitY) == 1 && currentHitX == lastHitX && timer1 > 0f))
				return;
		} else {
			if ((Mathf.Abs (currentHitX - lastHitX) == 1 && currentHitY == lastHitY && timer1 > 0f) ||
			    (Mathf.Abs (currentHitY - lastHitY) == 1 && currentHitX == lastHitX))
				return;
		}

		if ((currentHitX == 0 || isNotEmpty [currentHitX - 1, currentHitY]) && (currentHitX == 79 || isNotEmpty [currentHitX + 1, currentHitY]) &&
			(currentHitY == 0 || isNotEmpty [currentHitX, currentHitY - 1]) && (currentHitY == 79 || isNotEmpty [currentHitX, currentHitY + 1]))
			return;
		
		lastHitX = currentHitX;
		lastHitY = currentHitY;

		isUp = col.transform.position.y <= transform.position.y;
		isRight = col.transform.position.x <= transform.position.x;

		iced = col.gameObject.GetComponent<CubeCheck> ().CubeType == -2;

		if (Mathf.Abs (normal.x) > Mathf.Abs (normal.y)) {
			if (rotated && ((normal.x < -0.5f && !up) || (normal.x > 0.5f && up)))
				TreatAsVerticalCollision (col);
			if (isRight && isNotEmpty [currentHitX + 1, currentHitY])
				TreatAsVerticalCollision (col);
			else if (!isRight && isNotEmpty[currentHitX - 1, currentHitY])
				TreatAsVerticalCollision (col);
			else
				TreatAsHorizontalCollision (col);
		} else {
			if (!rotated && ((normal.y < -0.5f && !up) || (normal.y > 0.5f && up)))
				TreatAsHorizontalCollision (col);
			else if (isUp && isNotEmpty [currentHitX, currentHitY + 1])
				TreatAsHorizontalCollision (col);
			else if (!isUp && isNotEmpty [currentHitX, currentHitY - 1])
				TreatAsHorizontalCollision (col);
			else
				TreatAsVerticalCollision (col);
		}

		col.gameObject.GetComponent<CubeCheck> ().ProcessCollision (this.gameObject);
	}

	void TreatAsVerticalCollision (Collision col) {
		if (!rotated) {
			up = isUp;
			if (timer1 > 0 || iced) 
			OverallVelocity = new Vector3 (OverallVelocity.x, -OverallVelocity.y);
		} else {
			OverallVelocity = new Vector3 ((ballSpeed > 6) ? (up ? 12 : -12) : (up ? 6 : -6), isUp ? 6 : -6);
			timer1 = 0.3f;
		}
	}

	void TreatAsHorizontalCollision (Collision col) {
		if (!rotated) {
			OverallVelocity = new Vector3 (isRight ? 6 : -6, (ballSpeed > 6) ? (up ? 12 : -12) : (up ? 6 : -6)); 
			timer1 = 0.3f;
		} else {
			up = isRight;
			if (timer1 > 0 || iced) 
				OverallVelocity = new Vector3 (-OverallVelocity.x, OverallVelocity.y);
		}
	}

	public void DestroyObject () {
		isDead = true;
	}

	public void DefaultObject () {
		lastHitX = -100;
		lastHitY = -100;
		isDead = false;
		up = false;
		iced = false;
		rotated = false;
		timer1 = 0f;
		transform.position = defPosition;
		isNotEmpty = LevelScript.isNotEmpty;
	}
}