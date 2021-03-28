using UnityEngine;
using System.Collections;

public class CubeCheck : MonoBehaviour {

	[Header ("Fast Access")]
	public MeshRenderer cubeRenderer; //Used by ColorSwap
	public Level LevelScript;
	public BallColor BallColorScript;
	public GameObject[] Enemies; //Used by Mine
	public CubeCheck[,] CubeCheckScripts; //Used by Mine
	public bool[,] isNotEmpty; //Used by Mine

	[Header ("Current Variables")]
	private bool blownUp; //Used by Mine
	public int CubeType;
	public bool toBeDefaulted;

	[Header ("Default Variables")]
	public int defaultCubeType;

	void Start () {
		LevelScript = transform.parent.GetComponent<Level>();
		Enemies = LevelScript.Enemies;
		CubeCheckScripts = LevelScript.CubeCheckScripts;

		defaultCubeType = CubeType;
	}

	public void ProcessCollision (GameObject colSource) {
		if (!BallColorScript) {
			BallColorScript = colSource.GetComponent<BallColor> ();
		}

		if (BallColorScript.sphereType == 6 && CubeType >= 0) {
			if(CubeType != 32)
				BallColorScript.sphereType = BallColorScript.lastType;
			DeleteCube ();
			if (CubeType >= 7 && CubeType <= 13)
				Death ();
			return;
		}

		if (CubeType != 2)
			BallColorScript.Unsnow ();

		if (CubeType == -3) // Mine
			return;

		else if (CubeType == -2) // Snow
			BallColorScript.Snow ();
		
		else if (CubeType == -1) // Stone
			return;
		
		else if (CubeType == 0) { // Pear
			if (BallColorScript.sphereType == 0)
				DeleteCube ();
					
		} else if (CubeType == 1) { // Apple
			if (BallColorScript.sphereType == 1)
				DeleteCube ();
					
		} else if (CubeType == 2) { // Banana
			if (BallColorScript.sphereType == 2)
				DeleteCube ();
					
		} else if (CubeType == 3) { // Orange
			if (BallColorScript.sphereType == 3)
				DeleteCube ();
					
		} else if (CubeType == 4) { // Fig
			if (BallColorScript.sphereType == 4)
				DeleteCube ();
					
		} else if (CubeType == 5) { // Plum
			if (BallColorScript.sphereType == 5)
				DeleteCube ();
					
		} else if (CubeType == 6) // Ice
			DeleteCube ();
		
		else if (CubeType == 7) { // PearSkull
			if (BallColorScript.sphereType == 0) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 8) { // AppleSkull
			if (BallColorScript.sphereType == 1) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 9) {// BananaSkull
			if (BallColorScript.sphereType == 2) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 10) {// OrangeSkull
			if (BallColorScript.sphereType == 3) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 11) {// FigSkull
			if (BallColorScript.sphereType == 4) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 12) { // PlumSkull
			if (BallColorScript.sphereType == 5) {
				DeleteCube ();
				Death ();
			}

		} else if (CubeType == 13) { // IceSkull
			DeleteCube ();
			Death ();

		} else if (CubeType == 14) { // PearChange
			BallColorScript.sphereType = 0;

		} else if (CubeType == 15) { // AppleChange
			BallColorScript.sphereType = 1;

		} else if (CubeType == 16) { // BananaChange
			BallColorScript.sphereType = 2;

		} else if (CubeType == 17) { // OrangeChange
			BallColorScript.sphereType = 3;

		} else if (CubeType == 18) { // FigChange
			BallColorScript.sphereType = 4;

		} else if (CubeType == 19) { // PlumChange
			BallColorScript.sphereType = 5;

		} else if (CubeType >= 20 && CubeType <= 25) { // PearSwap, AppleSwap, BananaSwap, OrangeSwap, FigSwap, PlumSwap
			SwitchColors ();

		} else if (CubeType == 26) { // PearChangeBroken
			BallColorScript.sphereType = 0;
			DeleteCube ();

		} else if (CubeType == 27) { // AppleChangeBroken
			BallColorScript.sphereType = 1;
			DeleteCube ();

		} else if (CubeType == 28) { // BananaChangeBroken
			BallColorScript.sphereType = 2;
			DeleteCube ();

		} else if (CubeType == 29) { // OrangeChangeBroken
			BallColorScript.sphereType = 3;
			DeleteCube ();

		} else if (CubeType == 30) { // FigChangeBroken
			BallColorScript.sphereType = 4;
			DeleteCube ();

		} else if (CubeType == 31) { // PlumChangeBroken
			BallColorScript.sphereType = 5;
			DeleteCube ();

		} else if (CubeType == 32) { // Bomb
			BallColorScript.lastType = BallColorScript.sphereType;
			BallColorScript.sphereType = 6;
			DeleteCube ();

		} else if (CubeType == 33) // RandomChange
			BallColorScript.sphereType = Random.Range(0, 6);

		else if (CubeType == 34) // Wood
			return;
	}

	void Death() {
		LevelScript.Death ();
	}

	public void DeleteCube() {
		if (CubeType == -1 || CubeType == -2)
			return;
		
		LevelScript.isNotEmpty [(int)(transform.position.x / 3.0f), (int)(transform.position.y / 3.0f)] = false;

		if (CubeType == -3)
			StartCoroutine (BlowUp ());
		else {
			LevelScript.AddToDestroyed (this.gameObject);
			if (CubeType <= 6 && CubeType != -3)
				LevelScript.currentFruitAmount--;
			if (LevelScript.currentFruitAmount == 0)
				LevelScript.Victory ();
		
			this.gameObject.SetActive (false);
		}
	}

	void SwitchColors() {
		if (!toBeDefaulted) {
			LevelScript.AddToDefaulted (this.gameObject);
			toBeDefaulted = true;
		}
		int sphereType = BallColorScript.sphereType;
		if (CubeType == 20) {
			if (sphereType == 1) {
				CubeType = 21;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			}
			if (sphereType == 2) {
				CubeType = 22;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			}
			if (sphereType == 3) {
				CubeType = 23;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			}
			if (sphereType == 4) {
				CubeType = 24;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			}
			if (sphereType == 5) {
				CubeType = 25;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
			}
			BallColorScript.sphereType = 0;
		} else if (CubeType == 21) {
			if (sphereType == 0) {
				CubeType = 20;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			}
			if (sphereType == 2) {
				CubeType = 22;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			}
			if (sphereType == 3) {
				CubeType = 23;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			}
			if (sphereType == 4) {
				CubeType = 24;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			}
			if (sphereType == 5) {
				CubeType = 25;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
			}
			BallColorScript.sphereType = 1;
		} else if (CubeType == 22) {
			if (sphereType == 0) {
				CubeType = 20;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			}
			if (sphereType == 1) {
				CubeType = 21;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			}
			if (sphereType == 3) {
				CubeType = 23;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			}
			if (sphereType == 4) {
				CubeType = 24;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			}
			if (sphereType == 5) {
				CubeType = 25;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
			}
			BallColorScript.sphereType = 2;
		} else if (CubeType == 23) {
			if (sphereType == 0) {
				CubeType = 20;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			}
			if (sphereType == 1) {
				CubeType = 21;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			}
			if (sphereType == 2) {
				CubeType = 22;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			}
			if (sphereType == 4) {
				CubeType = 24;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			}
			if (sphereType == 5) {
				CubeType = 25;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
			}
			BallColorScript.sphereType = 3;
		} else if (CubeType == 24) {
			if (sphereType == 0) {
				CubeType = 20;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			}
			if (sphereType == 1) {
				CubeType = 21;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			}
			if (sphereType == 2) {
				CubeType = 22;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			}
			if (sphereType == 3) {
				CubeType = 23;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			}
			if (sphereType == 5) {
				CubeType = 25;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));
			}
			BallColorScript.sphereType = 4;
		} else if (CubeType == 25) {
			if (sphereType == 0) {
				CubeType = 20;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
			}
			if (sphereType == 1) {
				CubeType = 21;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
			}
			if (sphereType == 2) {
				CubeType = 22;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
			}
			if (sphereType == 3) {
				CubeType = 23;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
			}
			if (sphereType == 4) {
				CubeType = 24;
				cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
			}
			BallColorScript.sphereType = 5;
		}
	}

	public void DefaultObject() {
		toBeDefaulted = false; 

		if (defaultCubeType == -3)
			blownUp = false;
		else if (defaultCubeType == 20)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 255, 25, 255));
		else if (defaultCubeType == 21)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 0, 0, 255));
		else if (defaultCubeType == 22)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 174, 0, 255));
		else if (defaultCubeType == 23)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (255, 79, 0, 255));
		else if (defaultCubeType == 24)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (128, 0, 255, 255));
		else if (defaultCubeType == 25)
			cubeRenderer.material.SetColor ("_EmissionColor", new Color32 (0, 86, 255, 255));

		CubeType = defaultCubeType;
	}

	IEnumerator BlowUp() {
		blownUp = true;
		LevelScript.AddToDefaulted (this.gameObject);
		toBeDefaulted = true;

		yield return new WaitForSeconds (0.075f);
		yield return new WaitForFixedUpdate ();

		if (!blownUp)
			yield break;

		isNotEmpty = LevelScript.isNotEmpty;

		Vector3 currentTransformPosition = transform.position;

		int x = (int)(currentTransformPosition.x / 3.0f);
		int y = (int)(currentTransformPosition.y / 3.0f);

		if(x > 0 && y > 0)
			if(isNotEmpty [x - 1, y - 1])
				CubeCheckScripts [x - 1, y - 1].DeleteCube ();
		if(x > 0)
			if(isNotEmpty [x - 1, y])
				CubeCheckScripts [x - 1, y].DeleteCube ();
		if(x > 0 && y < 79)
			if(isNotEmpty [x - 1, y + 1])
				CubeCheckScripts [x - 1, y + 1].DeleteCube ();
		if(y > 0)
			if(isNotEmpty [x, y - 1])
				CubeCheckScripts [x, y - 1].DeleteCube ();
		if(y < 79)
			if(isNotEmpty [x, y + 1])
				CubeCheckScripts [x, y + 1].DeleteCube ();
		if(x < 79 && y > 0)
			if(isNotEmpty [x + 1, y - 1])
				CubeCheckScripts [x + 1, y - 1].DeleteCube ();
		if(x < 79)
			if(isNotEmpty [x + 1, y])
				CubeCheckScripts [x + 1, y].DeleteCube ();
		if(x < 79 && y < 79)
			if(isNotEmpty [x + 1, y + 1])
				CubeCheckScripts [x + 1, y + 1].DeleteCube ();

		for (int i = 0; i < 6400; i++) {
			if (Enemies [i] != null) {
				Vector3 VectorDifferenceEnemy = currentTransformPosition - Enemies [i].transform.position;
				if (Mathf.Abs (VectorDifferenceEnemy.x) < 4.5f && Mathf.Abs (VectorDifferenceEnemy.y) < 4.5f)
					Enemies [i].GetComponent<Enemy> ().DestroyObject ();
			} else
				i = 6400;
		}

		Vector3 VectorDifferencePlayer = currentTransformPosition - LevelScript.Player.transform.position;
		if (Mathf.Abs (VectorDifferencePlayer.x) < 4.5f && Mathf.Abs (VectorDifferencePlayer.y) < 4.5f)
			LevelScript.Death ();

		LevelScript.AddToDestroyed (this.gameObject);
		this.gameObject.SetActive (false);
	}
}