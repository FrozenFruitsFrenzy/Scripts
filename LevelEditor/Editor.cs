using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Editor : MonoBehaviour {

	[Header ("Scene Objects")]
	public Button SaveButton;
	public Button ActualSaveButton;
	public Button LoadButton;
	public Button[] DoablesButtons;
	public Toggle[] TypeToggles;
	public GameObject[] SkyboxChangeButtons;
	public RectTransform SelectionBox;
	public GameObject[] TypeDummies;
	public GameObject parent;
	public Camera thisCamera;
	public GameObject DefaultCamera;
	public Image SkyboxImage;
	public Sprite[] SkyboxImages;
	public TMP_InputField levelName;

	[Header ("Level Information")]
	public GameObject OneAndOnlyPlayer;
	public int[,] blocks = new int[80,80];
	public int fruitAmount;
	public int skyboxType;
	public bool canLevelBeSaved;
	public string actualLevelName;

	[Header ("Editor Information")]
	public int currentMode;
	public float ortoSize = 10.0f;

	public bool isInTypeMode;

	public GameObject currentGameObject;

	public GameObject selectedGameObject;
	public GameObject[] selectedGameObjects;

	public GameObject[,] GameObjectsInCells = new GameObject[80,80];

	private int CellX;
	private int CellY;

	private int OldCellX;
	private int OldCellY;

	private float TempX;
	private float TempY;

	private Vector2 startPos;
	private Vector2 endPos;

	public GameObject draggedcopy;
	public bool selectionBoxOn;

	Vector3 MouseStart;
	Vector3 tempTransform;

	public GameObject levelButtonPrefab;
	public TMP_Text levelNameText;
	public GameObject[] levelButtons = new GameObject[1000];
	public GameObject ScrollViewContent;

	void Start () {
		thisCamera = this.gameObject.GetComponent<Camera> ();
		DefaultCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		tempTransform = transform.position;
	}

	void OnEnable() {
		Cursor.visible = true;
	} 

	void Update () {
		SaveButton.interactable = canLevelBeSaved;
		if (!isInTypeMode) {
			if (Input.GetMouseButtonDown (2)) {
				MouseStart = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
				MouseStart = thisCamera.ScreenToWorldPoint (MouseStart);
				MouseStart.z = transform.position.z;
			} else if (Input.GetMouseButton (2)) {
				var MouseMove = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
				MouseMove = thisCamera.ScreenToWorldPoint (MouseMove);
				MouseMove.z = transform.position.z;
				tempTransform = transform.position - (MouseMove - MouseStart);
			} else {
				if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
					tempTransform += Vector3.left * 2.0f * Time.deltaTime * ortoSize;
				if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
					tempTransform += Vector3.right * 2.0f * Time.deltaTime * ortoSize;
				if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S))
					tempTransform += Vector3.down * 2.0f * Time.deltaTime * ortoSize;
				if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W))
					tempTransform += Vector3.up * 2.0f * Time.deltaTime * ortoSize;
			}

			tempTransform.x = Mathf.Clamp (tempTransform.x, 0, 240);
			tempTransform.y = Mathf.Clamp (tempTransform.y, 0, 240);
			tempTransform.z = -10.0f;

			transform.position = tempTransform;

			if (!selectedGameObject && !selectedGameObjects [0])
				ortoSize -= Input.GetAxis ("Mouse ScrollWheel") * 20;

			if (ortoSize > 50f)
				ortoSize = 50f;
			if (ortoSize < 10f)
				ortoSize = 10f;

			thisCamera.orthographicSize = ortoSize;
		}

		//режим рисования
		if (currentMode == 1) {
			if (Input.GetKey (KeyCode.Mouse0)) {
				if (EventSystem.current.IsPointerOverGameObject())
					return;
				
				GetPositionFromClick ();
				if (!(CellX >= 0 && CellY >= 0 && CellX < 80 && CellY < 80))
					return;


				if (GameObjectsInCells [CellX, CellY])
					return;
				

				if (currentGameObject == TypeDummies [99] && (CellX == 0 || GameObjectsInCells [CellX - 1, CellY]) && GameObjectsInCells [CellX + 1, CellY])
					return;
					
				canLevelBeSaved = false;
				GameObject newObject;

				if (currentGameObject != TypeDummies [99]) {
					GameObjectsInCells [CellX, CellY] = Instantiate (currentGameObject, new Vector3 (TempX, TempY, 0), currentGameObject.transform.rotation, parent.transform);
					newObject = GameObjectsInCells [CellX, CellY];

					if (currentGameObject == TypeDummies [1]) {
						if (OneAndOnlyPlayer) {
							GetPositionFromObject (OneAndOnlyPlayer);
							DeleteObjectOnGrid (CellX, CellY);
						}
						OneAndOnlyPlayer = newObject;
					} /*else if (currentGameObject == TypeDummies [9])
						newObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 180));*/

				} else {
					if (CellX == 0 || GameObjectsInCells [CellX - 1, CellY]) {
						GameObjectsInCells [CellX + 1, CellY] = Instantiate (currentGameObject, new Vector3 (TempX + 1.5f, TempY, 0), currentGameObject.transform.rotation, parent.transform);
						newObject = GameObjectsInCells [CellX + 1, CellY];
					} else {
						GameObjectsInCells [CellX - 1, CellY] = Instantiate (currentGameObject, new Vector3 (TempX - 1.5f, TempY, 0), currentGameObject.transform.rotation, parent.transform);
						newObject = GameObjectsInCells [CellX - 1, CellY];
					}
					GameObjectsInCells [CellX, CellY] = newObject;
				}

			}

			if (Input.GetKey (KeyCode.Mouse1)) {
				if (EventSystem.current.IsPointerOverGameObject())
					return;

				GetPositionFromClick ();
				if (!(CellX >= 0 && CellY >= 0 && CellX < 80 && CellY < 80))
					return;

				if (GameObjectsInCells [CellX, CellY]) {
					canLevelBeSaved = false;
					DeleteObjectOnGrid (CellX, CellY);
				} 
			}
		}

		//режим передвижения
		if (currentMode == 0) {
			//выбираем объект по клику на его клетку
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				if (EventSystem.current.IsPointerOverGameObject ())
					return;
				
				GetPositionFromClick ();
				if (!(CellX >= 0 && CellY >= 0 && CellX < 80 && CellY < 80))
					return;
				
				if (GameObjectsInCells [CellX, CellY]) {
					if ((GameObjectsInCells [CellX, CellY] == selectedGameObject || Input.GetKey (KeyCode.LeftShift)) && !draggedcopy) {
						UnselectObject ();
						SelectObject (GameObjectsInCells [CellX, CellY]);
						StartDragSingular (selectedGameObject);
					} else {
						if (selectedGameObject && !draggedcopy) {
							if (Input.GetKey (KeyCode.LeftControl)) { 
								selectedGameObjects [0] = selectedGameObject;
								selectedGameObject = null;
								AddOneToMultipleSelection (GameObjectsInCells [CellX, CellY]);
							} else {
								ClearSelection ();
								UnselectObject ();
								SelectObject (GameObjectsInCells [CellX, CellY]);
							}
						} else if (selectedGameObjects [0]) {
							if (Input.GetKey (KeyCode.LeftControl)) { 
								if (CheckIfObjectIsSelected (GameObjectsInCells [CellX, CellY])) {
									RemoveOneFromMultipleSelection (GameObjectsInCells [CellX, CellY]);

									if (selectedGameObjects [1] == null) {
										selectedGameObject = selectedGameObjects [0];
										selectedGameObjects [0] = null;
									}
								} else
									AddOneToMultipleSelection (GameObjectsInCells [CellX, CellY]);
							} else {
								ClearSelection ();
								SelectObject (GameObjectsInCells [CellX, CellY]);
							}
						} else
							SelectObject (GameObjectsInCells [CellX, CellY]);
					}
				} else {
					if (!Input.GetKey (KeyCode.LeftControl)) {
						ClearSelection ();
						UnselectObject ();
					}
					CreateSelectionBox (Input.mousePosition);
				}
			}

			if (Input.GetKey (KeyCode.Mouse0))
				UpdateSelectionBox (Input.mousePosition);

			if (Input.GetKeyUp (KeyCode.Mouse0))
				ReleaseSelectionBox ();

			if (draggedcopy) {
				Vector3 dragPosition;
				dragPosition = thisCamera.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, -30f));
				dragPosition.z = 0;
				draggedcopy.transform.position = dragPosition;
			}

			if (Input.GetKeyUp (KeyCode.Mouse0) && draggedcopy && selectedGameObject) {
				StopDragSingular ();

				OldCellX = CellX;
				OldCellY = CellY;

				GetPositionFromClick ();

				if (!(CellX >= 0 && CellY >= 0 && CellX < 80 && CellY < 80))
					return;

				if (GameObjectsInCells [OldCellX, OldCellY].GetComponent<Dummy>().CubeType == 99 && (CellX == 0 || GameObjectsInCells [CellX - 1, CellY]) && GameObjectsInCells [CellX + 1, CellY])
					return;

				if (GameObjectsInCells [CellX, CellY])
					return;
				
				canLevelBeSaved = false;

				if (selectedGameObject.GetComponent<Dummy>().CubeType != 99) {
					GameObjectsInCells [OldCellX, OldCellY] = null;
					selectedGameObject.transform.position = new Vector3 (TempX, TempY);
					GameObjectsInCells [CellX, CellY] = selectedGameObject;
				} else {
					if (GameObjectsInCells [OldCellX, OldCellY] == GameObjectsInCells [OldCellX - 1, OldCellY])
						GameObjectsInCells [OldCellX - 1, OldCellY] = null;
					else
						GameObjectsInCells [OldCellX + 1, OldCellY] = null;
					GameObjectsInCells [OldCellX, OldCellY] = null;
						

					GameObjectsInCells [CellX, CellY] = selectedGameObject;
					if (CellX == 0 || GameObjectsInCells [CellX - 1, CellY]) {
						GameObjectsInCells [CellX + 1, CellY] = selectedGameObject;
						selectedGameObject.transform.position = new Vector3 (TempX + 1.5f, TempY);
					} else {
						GameObjectsInCells [CellX - 1, CellY] = selectedGameObject;
						selectedGameObject.transform.position = new Vector3 (TempX - 1.5f, TempY);
					}

				}
			}




			//удаляем объект по нажатию Delete
			if (Input.GetKeyDown (KeyCode.Delete) && !isInTypeMode) {
				if (selectedGameObject) { 
					StopDragSingular ();
					canLevelBeSaved = false;
					GetPositionFromObject (selectedGameObject);
					DeleteObjectOnGrid (CellX, CellY);
				} else {
					canLevelBeSaved = false;
					for (int i = 0; i < 2499; i++)
						if (selectedGameObjects [i] != null) {
							GetPositionFromObject (selectedGameObjects[i]);
							DeleteObjectOnGrid (CellX, CellY);
						}
				}
			}

			//меняем параметр объекта
			if (!draggedcopy) {
				if ((Input.GetAxis ("Mouse ScrollWheel") > 0 || Input.GetKeyDown(KeyCode.M)) && !isInTypeMode) { 
					if (selectedGameObject) {
						selectedGameObject.GetComponent<Dummy> ().ChangeCubeData (false, true, 0);
						canLevelBeSaved = false;
					} else if (selectedGameObjects [0]) {
						ChangeStateMultiple (true);
						canLevelBeSaved = false;
					}
				} else if ((Input.GetAxis ("Mouse ScrollWheel") < 0 || Input.GetKeyDown(KeyCode.N)) && !isInTypeMode) {
					if (selectedGameObject) {
						canLevelBeSaved = false;
						selectedGameObject.GetComponent<Dummy> ().ChangeCubeData (false, false, 0);
					} else if (selectedGameObjects [0]) {
						ChangeStateMultiple (false);
						canLevelBeSaved = false;
					}
				}
			}
		}
	}
	///////////////////////////////////////////////////////////////////////////////

	public void SwitchIsInTypeMode(bool newValue) {
		isInTypeMode = newValue;
	}

	///////////////////////////////////////////////////////////////////////////////

	void ChangeStateMultiple(bool up) {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects [i] != null) {
				selectedGameObjects [i].GetComponent<Dummy> ().ChangeCubeData (false, up, 0);
			} else
				return;
	}
	///////////////////////////////////////////////////////////////////////////////

	public void GetPositionFromClick () {
		Vector3 point = thisCamera.ScreenToWorldPoint(Input.mousePosition);

		CellX = Mathf.RoundToInt (point.x / 3.0f);
		CellY = Mathf.RoundToInt (point.y / 3.0f);

		TempX = CellX * 3.0f;
		TempY = CellY * 3.0f;
	}

	public void GetPositionFromObject (GameObject Object) {
		TempX = Object.transform.position.x;
		TempY = Object.transform.position.y;

		CellX = (int)(TempX / 3);
		CellY = (int)(TempY / 3);
	}

	void DeleteObjectOnGrid (int x, int y) {
		Destroy (GameObjectsInCells [x, y]);
		GameObjectsInCells [x, y] = null;
	}

	///////////////////////////////////////////////////////////////////////////////

	//Начало перетаскивания объекта
	void StartDragSingular(GameObject objectToDrag) {
		draggedcopy = Instantiate (objectToDrag, new Vector3 (TempX, TempY), objectToDrag.transform.rotation);
		Color dragColor;
		Material dragMaterial = draggedcopy.GetComponent<MeshRenderer> ().material;
		dragMaterial.SetFloat("_Mode", 3);
		dragMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		dragMaterial.SetInt("_ZWrite", 0);
		dragMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
		dragMaterial.renderQueue = 3000;
		dragColor = dragMaterial.color;
		dragColor.a = 0.5f;
		dragMaterial.color = dragColor;
		draggedcopy.GetComponent<Dummy> ().selectedOutline.SetActive (false);
	}

	//Окончание перетаскивания объекта
	void StopDragSingular() {
		Destroy (draggedcopy);
	}

	///////////////////////////////////////////////////////////////////////////////


	//Выбор конкретного предмета
	public void SelectObject(GameObject theObject) {
		selectedGameObject = theObject;
		selectedGameObject.GetComponent<Dummy> ().selectedOutline.SetActive (true);
	}

	//Выборка убрана
	public void UnselectObject () {
		if (selectedGameObject) {
			selectedGameObject.GetComponent<Dummy> ().selectedOutline.SetActive (false);
			selectedGameObject = null;
		}
	}

	void SelectMultipleObjects() {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects [i] != null) {
				selectedGameObjects [i].GetComponent<Dummy> ().selectedOutline.SetActive (true);
			} else
				return;
	}

	void AddOneToMultipleSelection(GameObject newObject) {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects[i] == null) {
				selectedGameObjects [i] = newObject;
				newObject.GetComponent<Dummy> ().selectedOutline.SetActive (true);
				return;
			}
	}

	void RemoveOneFromMultipleSelection(GameObject newObject) {
		bool found = false;

		for (int i = 0; i < 2499; i++) {
			if (selectedGameObjects [i] == null)
				return;
			if (selectedGameObjects [i] == newObject) {
				selectedGameObjects [i] = null;
				newObject.GetComponent<Dummy> ().selectedOutline.SetActive (false);
				found = true;
			} else if (found == true) {
				selectedGameObjects [i - 1] = selectedGameObjects [i];
				selectedGameObjects [i] = null;
			}
		}
	}

	void ClearSelection() {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects [i] != null) {
				selectedGameObjects [i].GetComponent<Dummy> ().selectedOutline.SetActive (false);
				selectedGameObjects [i] = null;
			} else
				return;
	}

	bool CheckIfObjectIsSelected(GameObject selectedObject) {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects [i] == null)
				return false;
			else if (selectedGameObjects [i] == selectedObject) 
				return true;
		return false;
	}

	int GetFirstNotSelectedObject() {
		for (int i = 0; i < 2499; i++)
			if (selectedGameObjects [i] == null)
				return i;
		return 0;
	}

	///////////////////////////////////////////////////////////////////////////////

	//Создание окна выделения
	void CreateSelectionBox (Vector2 currentMousePosition) {
		startPos = currentMousePosition;
		selectionBoxOn = true;
		SelectionBox.gameObject.SetActive (true);
	}

	//Ежекадровое обновление позиции
	void UpdateSelectionBox (Vector2 currentMousePosition) {
		if (!selectionBoxOn)
			return;

		float width = currentMousePosition.x - startPos.x;
		float height = currentMousePosition.y - startPos.y;

		SelectionBox.sizeDelta = new Vector2 (Mathf.Abs(width), Mathf.Abs(height));
		SelectionBox.anchoredPosition = startPos + new Vector2 (width / 2, height / 2);
	}

	void ReleaseSelectionBox() {
		if (!selectionBoxOn)
			return;
		
		DisableSelectionBox ();

		//TODO: Заменить на RectContains()

		Vector2 min = SelectionBox.anchoredPosition - (SelectionBox.sizeDelta / 2);
		Vector2 max = SelectionBox.anchoredPosition + (SelectionBox.sizeDelta / 2);

		int i = GetFirstNotSelectedObject ();

		//TODO: Заменить на for
		foreach(GameObject block in GameObjectsInCells) {
			if (block != null && CheckIfObjectIsSelected(block) == false) {
				Vector2 objectPosition = thisCamera.WorldToScreenPoint (block.transform.position);

				if (objectPosition.x > min.x && objectPosition.x < max.x && objectPosition.y > min.y && objectPosition.y < max.y) {
					selectedGameObjects [i] = block;
					i++;
				}
			}
		}

		if (i == 1) {
			SelectObject (selectedGameObjects [0]);
			selectedGameObjects [0] = null;
		} else
			SelectMultipleObjects ();
	}

	//Удаление окнаа
	void DisableSelectionBox() {
		selectionBoxOn = false;
		SelectionBox.gameObject.SetActive (false);
	}
	///////////////////////////////////////////////////////////////////////////////

	public void Back() {
		Clear ();
		DefaultCamera.GetComponent<LevelChanger> ().ActivateMenu();
	}

	public void Clear() {
		for (int x = 0; x < 80; x++)
			for (int y = 0; y < 80; y++)
				Destroy (GameObjectsInCells[x,y]);
		canLevelBeSaved = false;
	}

	public void SelectNameField() {
		levelName.text = actualLevelName;
	}

	public void UnselectNameField() {
		string text = levelName.text.ToUpper ();

		text = text.Replace (" ", "").Replace (".", "").Replace("/", "").Replace("<", "").Replace(">", "").Replace(":", "").Replace("\"", "").Replace("\\", "").Replace("|", "").Replace("?", "").Replace("*", "");
		if (text == "CON" || text == "PRN" || text == "AUX" || text == "NUL" || text == "COM1" || text == "COM2" || text == "COM3" || text == "COM4" || text == "COM5" || text == "COM6" || text == "COM7" || text == "COM8" || text == "COM9" || text == "LPT1" || text == "LPT2" || text == "LPT3" || text == "LPT4" || text == "LPT5" || text == "LPT6" || text == "LPT7" || text == "LPT8" || text == "LPT9")
			text = "";
		if (Regex.IsMatch(text, @"^[0-9]+$"))
			text = "";
			
		actualLevelName = text;

		if (actualLevelName != "") {
			levelName.text = actualLevelName + ".lvl";
			ActualSaveButton.interactable = true;
		} else {
			levelName.text = "";
			ActualSaveButton.interactable = false;
		}
	}

	public void SaveLevel() {
		blocks = new int[80,80];
		fruitAmount = 0;

		//TODO: заменить на for
		foreach(GameObject block in GameObjectsInCells) {
			if (block != null) {
				Dummy script = block.GetComponent<Dummy> ();
				if (script.CubeType == 2)
					fruitAmount++;
				GetPositionFromObject (block);
				blocks [CellX, CellY] = script.CubeType * 100 + script.CubeData;
			}
		}

		SaveSystem.SaveLevel (this, actualLevelName);
	}

	public void Play() {
		blocks = new int[80,80];
		fruitAmount = 0;

		//TODO: заменить на for
		foreach(GameObject block in GameObjectsInCells) {
			if (block != null) {
				Dummy script = block.GetComponent<Dummy> ();
				if (script.CubeType == 2)
					fruitAmount++;
				GetPositionFromObject (block);
				blocks [CellX, CellY] = script.CubeType * 100 + script.CubeData;
			}
		}

		if (fruitAmount == 0)
			return;

		if (!OneAndOnlyPlayer)
			return;

		SaveSystem.SaveLevel (this, "EditorTest");
		DefaultCamera.GetComponent<LevelChanger> ().ActivateGame("EditorTest");
	}
	///////////////////////////////////////////////////////////////////////////////

	public void LoadExistingCustomLevels() {
		ClearCurrentLevels ();

		int actualNumber = 0;

		List<string> fileNames = new List<string>(Directory.GetFiles(Application.dataPath + "/Levels/"));
		for (int i = 0; i < fileNames.Count; i++) {
			if (Path.GetExtension (fileNames [i]) == ".lvl") {
				string shortName = Path.GetFileName (fileNames [i]);

				GameObject newObject = Instantiate (levelButtonPrefab);
				newObject.transform.SetParent (ScrollViewContent.transform, false);
				GameObject newObjectText = newObject.transform.Find ("Text").gameObject;
				newObjectText.GetComponent<TMP_Text> ().text = shortName;

				newObject.GetComponent<Button> ().onClick.AddListener(() => {ChangeLoadableLevelName (shortName.Remove (shortName.Length - 4));});

				levelButtons [actualNumber] = newObject;
				actualNumber++;
			}
		}
	}

	public void ClearCurrentLevels() {
		for (int i = 0; i < 1000; i++)
			if (levelButtons [i] == null)
				return;
			else
				Destroy (levelButtons [i]);
	}

	public void ChangeLoadableLevelName(string levelName) {
		actualLevelName = levelName;
		levelNameText.text = actualLevelName + ".lvl";
		LoadButton.interactable = true;
	}

	public void LoadCustomLevel() {
		Clear ();

		LevelInformation levelInfo;

		levelInfo = SaveSystem.LoadLevel (actualLevelName);

		if (levelInfo != null) {
			blocks = levelInfo.blocks;
			fruitAmount = levelInfo.fruitAmount;
			skyboxType = levelInfo.skyboxType;
		}

		UpdateSkyboxType ();

		for (int x = 0; x < 80; x++)
			for (int y = 0; y < 80; y++) {
				int CubeType = blocks [x, y] / 100;
				int CubeData = blocks [x, y] % 100;


				if (CubeType != 0) {
					GameObject newObject = null;

					if (CubeType != 99) {
						GameObjectsInCells [x, y] = Instantiate (TypeDummies [CubeType], new Vector3 (x * 3, y * 3, 0), TypeDummies [CubeType].transform.rotation, parent.transform);
						newObject = GameObjectsInCells [x, y];

						if (CubeType == 1)
							OneAndOnlyPlayer = newObject;
						/*else if (CubeType == 9)
							newObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 180));*/
					} else {
						GameObjectsInCells [x, y] = Instantiate (TypeDummies [CubeType], new Vector3 (x * 3 + 1.5f, y * 3, 0), TypeDummies [CubeType].transform.rotation, parent.transform);
						GameObjectsInCells [x + 1, y] = GameObjectsInCells [x, y];
						newObject = GameObjectsInCells [x, y];
					}
					newObject.GetComponent<Dummy> ().ChangeCubeData (true, false, CubeData);
				}
			}
	}

	///////////////////////////////////////////////////////////////////////////////

	public void PreviousSkyboxType() {
		skyboxType--;
		UpdateSkyboxType ();
	}

	public void NextSkyboxType() {
		skyboxType++;
		UpdateSkyboxType ();
	}

	public void UpdateSkyboxType() {
		if (skyboxType == 0)
			SkyboxChangeButtons [0].SetActive (false);
		else
			SkyboxChangeButtons [0].SetActive (true);


		if (skyboxType == 4)
			SkyboxChangeButtons [1].SetActive (false);
		else
			SkyboxChangeButtons [1].SetActive (true);

		SkyboxImage.sprite = SkyboxImages [skyboxType];
	}

	///////////////////////////////////////////////////////////////////////////////

	public void SwitchMode(int Mode) {
		if (Mode == 1) {
			DoablesButtons [0].interactable = true;
			DoablesButtons [1].interactable = false;
		} else {
			DoablesButtons [1].interactable = true;
			DoablesButtons [0].interactable = false;
		}

		DisableSelectionBox ();

		TypeToggles [1].interactable = false;
		TypeToggles [2].interactable = false;
		TypeToggles [3].interactable = false;
		TypeToggles [4].interactable = false;
		TypeToggles [5].interactable = false;
		TypeToggles [6].interactable = false;
		TypeToggles [7].interactable = false;
		TypeToggles [8].interactable = false;
		TypeToggles [9].interactable = false;
		TypeToggles [10].interactable = false;
		TypeToggles [11].interactable = false;
		TypeToggles [12].interactable = false;
		TypeToggles [98].interactable = false;
		TypeToggles [99].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;

		if (Mode == 1) {
			ClearSelection ();
			if (currentGameObject == TypeDummies [1]) {
				TypeToggles [1].isOn = true;
				MakeAllInteractable ();
				TypeToggles [1].interactable = false;
			} else if (currentGameObject == TypeDummies [2]) {
				TypeToggles [2].isOn = true;
				MakeAllInteractable ();
				TypeToggles [2].interactable = false;
			} else if (currentGameObject == TypeDummies [3]) {
				TypeToggles [3].isOn = true;
				MakeAllInteractable ();
				TypeToggles [3].interactable = false;
			} else if (currentGameObject == TypeDummies [4]) {
				TypeToggles [4].isOn = true;
				MakeAllInteractable ();
				TypeToggles [4].interactable = false;
			} else if (currentGameObject == TypeDummies [5]) {
				TypeToggles [5].isOn = true;
				MakeAllInteractable ();
				TypeToggles [5].interactable = false;
			} else if (currentGameObject == TypeDummies [6]) {
				TypeToggles [6].isOn = true;
				MakeAllInteractable ();
				TypeToggles [6].interactable = false;
			} else if (currentGameObject == TypeDummies [7]) {
				TypeToggles [7].isOn = true;
				MakeAllInteractable ();
				TypeToggles [7].interactable = false;
			} else if (currentGameObject == TypeDummies [8]) {
				TypeToggles [8].isOn = true;
				MakeAllInteractable ();
				TypeToggles [8].interactable = false;
			} else if (currentGameObject == TypeDummies [9]) {
				TypeToggles [9].isOn = true;
				MakeAllInteractable ();
				TypeToggles [9].interactable = false;
			} else if (currentGameObject == TypeDummies [10]) {
				TypeToggles [10].isOn = true;
				MakeAllInteractable ();
				TypeToggles [10].interactable = false;
			} else if (currentGameObject == TypeDummies [11]) {
				TypeToggles [11].isOn = true;
				MakeAllInteractable ();
				TypeToggles [11].interactable = false;
			} else if (currentGameObject == TypeDummies [12]) {
				TypeToggles [12].isOn = true;
				MakeAllInteractable ();
				TypeToggles [12].interactable = false;
			} else if (currentGameObject == TypeDummies [98]) {
				TypeToggles [98].isOn = true;
				MakeAllInteractable ();
				TypeToggles [98].interactable = false;
			} else if (currentGameObject == TypeDummies [99]) {
				TypeToggles [99].isOn = true;
				MakeAllInteractable ();
				TypeToggles [99].interactable = false;
			}
		}

		currentMode = Mode;
		UnselectObject ();
	}

	///////////////////////////////////////////////////////////////////////////////

	public void SelectBallType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [1];

		MakeAllInteractable ();
		TypeToggles [1].interactable = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectFruitType (bool value) {
		if (value == false)
			return;
		
		currentGameObject = TypeDummies [2];

		MakeAllInteractable ();
		TypeToggles [2].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectSkullType (bool value) {
		if (value == false)
			return;
		
		currentGameObject = TypeDummies [3];

		MakeAllInteractable ();
		TypeToggles [3].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectChangeType (bool value) {
		if (value == false)
			return;
		
		currentGameObject = TypeDummies [4];

		MakeAllInteractable ();
		TypeToggles [4].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectSwapType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [5];

		MakeAllInteractable ();
		TypeToggles [5].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectСhangeBrokenType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [6];

		MakeAllInteractable ();
		TypeToggles [6].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectStoneType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [7];

		MakeAllInteractable ();
		TypeToggles [7].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectRandomType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [8];

		MakeAllInteractable ();
		TypeToggles [8].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectWoodType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [9];

		MakeAllInteractable ();
		TypeToggles [9].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectBombType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [10];

		MakeAllInteractable ();
		TypeToggles [10].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectSnowType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [11];

		MakeAllInteractable ();
		TypeToggles [11].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectMineType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [12];

		MakeAllInteractable ();
		TypeToggles [12].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [98].isOn = false;
		TypeToggles [99].isOn = false;
	}

	public void SelectAircraftType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [99];

		MakeAllInteractable ();
		TypeToggles [99].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [98].isOn = false;
	}

	public void SelectDemonheadType (bool value) {
		if (value == false)
			return;

		currentGameObject = TypeDummies [98];

		MakeAllInteractable ();
		TypeToggles [98].interactable = false;
		TypeToggles [1].isOn = false;
		TypeToggles [2].isOn = false;
		TypeToggles [3].isOn = false;
		TypeToggles [4].isOn = false;
		TypeToggles [5].isOn = false;
		TypeToggles [6].isOn = false;
		TypeToggles [7].isOn = false;
		TypeToggles [8].isOn = false;
		TypeToggles [9].isOn = false;
		TypeToggles [10].isOn = false;
		TypeToggles [11].isOn = false;
		TypeToggles [12].isOn = false;
		TypeToggles [99].isOn = false;
	}

	void MakeAllInteractable() {
		TypeToggles [1].interactable = true;
		TypeToggles [2].interactable = true;
		TypeToggles [3].interactable = true;
		TypeToggles [4].interactable = true;
		TypeToggles [5].interactable = true;
		TypeToggles [6].interactable = true;
		TypeToggles [7].interactable = true;
		TypeToggles [8].interactable = true;
		TypeToggles [9].interactable = true;
		TypeToggles [10].interactable = true;
		TypeToggles [11].interactable = true;
		TypeToggles [12].interactable = true;
		TypeToggles [98].interactable = true;
		TypeToggles [99].interactable = true;
	}
}