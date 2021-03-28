using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using TMPro;

public class Menu : MonoBehaviour {

	public static bool firstTime;
	public Resolution[] AvailableResolutions;
	public AudioMixer Mixer;

	[Header ("Fast Access")]
	public Material[] TransparentMaterialList;
	public GameObject myCamera;
	public CameraControl CameraControlScript;
	public FPSCounter FPSCounterScript;
	public BlurControl BlurControlScript;
	public TimerCounter TimerCounterScript;
	public LevelChanger LevelChangerScript;
	public LightingManager LightingManagerScript;
	[Header ("Settings save information")]
	public int Resolution;
	public bool fullScreen;
	[Space(10)]
	public int antialiasingQuality;
	public bool softParticles;
	public bool VSync;
	public int textureQuality;
	public bool lightingEnabled;
	public bool blurEnabled;
	public bool transparencyEnabled;
	[Space(10)]
	public bool Use2DRenderMode;
	public float CameraDistance;
	public int CameraType;
	[Space(10)]
	public float MasterVolume;
	public float MusicVolume;
	public float SoundVolume;
	[Space(10)]
	public bool FPSCounterOn;
	public int FPSRefreshRate;
	public bool TimerCounterOn;
	[Header ("Menu Elements")]
	public TMP_Dropdown ResolutionsDropdown;
	public Toggle FullscreenToggle;
	[Space(10)]
	public TMP_Dropdown AntialiasingDropdown;
	public TMP_Dropdown SoftParticlesDropdown;
	public Toggle VSyncToggle;
	public TMP_Dropdown TextureQualityDropdown;
	public Toggle LightingEnabledToggle;
	public Toggle BlurEnabledToggle;
	public Toggle TransparencyEnabledToggle;
	[Space(10)]
	public TMP_Dropdown RenderModeDropdown;
	public Slider CameraDistanceSlider;
	public TMP_Dropdown CameraTypeDropdown;
	[Space(10)]
	public Slider MasterVolumeSlider;
	public Slider MusicVolumeSlider;
	public Slider SoundVolumeSlider;
	[Space(10)]
	public Toggle FPSCounterToggle;
	public Slider FPSRefreshRateSlider;
	public Toggle TimerCounterToggle;

	void Awake () {
		Cursor.visible = true;
		myCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void Start () {
		Vector3 tempTransform = myCamera.transform.position;
		tempTransform.z = 0;
		myCamera.transform.position = tempTransform;
		FPSCounterScript = myCamera.GetComponent<FPSCounter> ();
		TimerCounterScript = myCamera.GetComponent<TimerCounter> ();
		CameraControlScript = myCamera.GetComponent<CameraControl> ();
		LevelChangerScript = myCamera.GetComponent<LevelChanger> ();
		LightingManagerScript = myCamera.GetComponent<LightingManager> ();
		BlurControlScript = myCamera.GetComponent<BlurControl> ();

		AvailableResolutions = Screen.resolutions;
		ResolutionsDropdown.ClearOptions();
		int currentResolutionIndex = 0;
		List<string> options = new List<string>();
		for (int i = 0; i < AvailableResolutions.Length; i++) {
			string option = AvailableResolutions [i].width + "x" + AvailableResolutions [i].height;
			options.Add (option);
			if (AvailableResolutions [i].width == Screen.currentResolution.width && AvailableResolutions [i].height == Screen.currentResolution.height)
				currentResolutionIndex = i;
		}
		ResolutionsDropdown.AddOptions(options);
		ResolutionsDropdown.value = currentResolutionIndex;
		Resolution = currentResolutionIndex;
		ResolutionsDropdown.RefreshShownValue();

		LoadSettings ();
	}

	/*void Update () {
		if (firstTime && Input.anyKeyDown) {
			//код определения контроллера
		}
	}*/

	public void FirstSelectedChange (GameObject obj) {
		EventSystem.current.SetSelectedGameObject(obj);
	}

	public void SetResolution (int ResolutionIndex) {
		Resolution = ResolutionIndex;
	}

	public void ChangeFullscreen (bool isFullScreen) {
		fullScreen = isFullScreen;
	}

	public void SetAntialiasingQuality (int amount) {
		antialiasingQuality = amount;
	} 

	public void SetTextureQuality (int amount) {
		textureQuality = amount;
	} 

	public void SetParticlesSoftness (int amount) {
		softParticles = amount != 0;
	}

	public void SetLightingEnabled (bool value) {
		lightingEnabled = value;
	}

	public void SetBlurEnabled (bool value) {
		blurEnabled = value;
	}

	public void SetTransparencyEnabled (bool value) {
		transparencyEnabled = value;
	}

	public void TurnVSync (bool value) {
		VSync = value;
	}

	public void ChangeMasterVolume(float volume) {
		MasterVolume = volume;
		Mixer.SetFloat ("Master",  Mathf.Log10(volume) * 20);
	}

	public void ChangeMusicVolume(float volume) {
		MusicVolume = volume;
		Mixer.SetFloat ("Music",  Mathf.Log10(volume) * 20);
	}

	public void ChangeSoundVolume (float volume) {
		SoundVolume = volume;
		Mixer.SetFloat ("Sound", Mathf.Log10 (volume) * 20);
	}

	public void ChangeRenderMode (int value) {
		Use2DRenderMode = value != 0;
	}

	public void ChangeCameraDistance (float Distance) {
		CameraDistance = Distance;
	}

	public void ChangeCameraType (int CurrentType) {
		CameraType = CurrentType;
	}

	public void SwitchFPSCounter (bool value) {
		FPSCounterOn = value;
	}

	public void ChangeRefreshRate (float value) {
		FPSRefreshRate = (int)value;
	}

	public void SwitchTimeCounter (bool value) {
		TimerCounterOn = value;
	}
		
	public void LoadSettings () {
		Settings settings = SaveSystem.LoadSettings ();
		if (settings != null) {
			ResolutionsDropdown.value = settings.Resolution;
			FullscreenToggle.isOn = settings.fullScreen;

			AntialiasingDropdown.value = settings.antialiasingQuality;
			SoftParticlesDropdown.value = settings.softParticles ? 1 : 0;
			VSyncToggle.isOn = settings.VSync;
			TextureQualityDropdown.value = settings.textureQuality;
			LightingEnabledToggle.isOn = settings.lightingEnabled;
			BlurEnabledToggle.isOn = settings.blurEnabled;
			TransparencyEnabledToggle.isOn = settings.transparencyEnabled;

			MasterVolumeSlider.value = settings.MasterVolume;
			MusicVolumeSlider.value = settings.MusicVolume;
			SoundVolumeSlider.value = settings.SoundVolume;

			RenderModeDropdown.value = settings.Use2DRenderMode ? 1 : 0;
			CameraDistanceSlider.value = settings.CameraDistance;
			CameraTypeDropdown.value = settings.CameraType;

			FPSCounterToggle.isOn = settings.FPSCounterOn;
			FPSRefreshRateSlider.value = settings.FPSRefreshRate;
			TimerCounterToggle.isOn = settings.TimerCounterOn;
		} else
			Default ();
		Apply ();
	}

	public void Apply () {
		Resolution = ResolutionsDropdown.value;
		Screen.SetResolution (AvailableResolutions[Resolution].width, AvailableResolutions[Resolution].height, fullScreen);
		if (antialiasingQuality > 0)
			QualitySettings.antiAliasing = Mathf.RoundToInt (Mathf.Pow (2, antialiasingQuality));
		else
			QualitySettings.antiAliasing = 0;
		QualitySettings.softParticles = softParticles;
		QualitySettings.vSyncCount = VSync ? 1 : 0;
		QualitySettings.masterTextureLimit = textureQuality;
		LightingManagerScript.lightingEnabled = lightingEnabled;
		if (blurEnabled)
			BlurControlScript.EnableBlur ();
		else
			BlurControlScript.DisableBlur ();

		for (int i = 0; i < 7; i++) {
			TransparentMaterialList [i].SetFloat ("_Mode", transparencyEnabled ? 3 : 0);
			TransparentMaterialList [i].SetFloat ("_DstBlend", transparencyEnabled ? (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha : (int)UnityEngine.Rendering.BlendMode.Zero);
			TransparentMaterialList [i].SetInt("_ZWrite", transparencyEnabled ? 0 : 1);
			if(transparencyEnabled)
				TransparentMaterialList [i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
			else
				TransparentMaterialList [i].DisableKeyword("_ALPHAPREMULTIPLY_ON");
			TransparentMaterialList [i].renderQueue = transparencyEnabled ? 3000 : -1;
			CameraControlScript.RenderDetails (transparencyEnabled);
		}

		if (CameraType == 0)
			CameraControlScript.CameraType = CameraControl.CameraTypes.Smooth;
		if (CameraType == 1)
			CameraControlScript.CameraType = CameraControl.CameraTypes.Centered;
		if (CameraType == 2)
			CameraControlScript.CameraType = CameraControl.CameraTypes.Vanilla;
		CameraControlScript.CameraDistance = Mathf.RoundToInt(CameraDistance);
		if (Use2DRenderMode) {
			CameraControlScript.Orthographic = true;
			CameraControlScript.myCamera.orthographic = true;
			CameraControlScript.myCamera.orthographicSize = CameraDistance / 2f;
		} else {
			CameraControlScript.Orthographic = false;
			CameraControlScript.myCamera.orthographic = false;
			CameraControlScript.myCamera.fieldOfView = 55;
		}

		FPSCounterScript.Delay = FPSRefreshRate;
		if (FPSCounterOn)
			FPSCounterScript.EnableCounter ();
		else
			FPSCounterScript.DisableCounter ();
		TimerCounterScript.isOn = TimerCounterOn;
		SaveSystem.SaveSettings (this);
	}

	public void Cancel () {
		LoadSettings ();
	}

	public void Delete () {
		SaveSystem.Delete ("settings");
		Default ();
		Apply ();
	}

	public void EraseSave () {
		SaveGame.DeleteProgress ();
	}

	public void Default () {
		//ResolutionsDropdown.value = 0;
		FullscreenToggle.isOn = true;
		AntialiasingDropdown.value = 0;
		SoftParticlesDropdown.value = 0;
		VSyncToggle.isOn = false;
		TextureQualityDropdown.value = 2;
		LightingEnabledToggle.isOn = true;
		BlurEnabledToggle.isOn = true;
		TransparencyEnabledToggle.isOn = true;
		RenderModeDropdown.value = 0;
		CameraDistanceSlider.value = 30.0f;
		CameraTypeDropdown.value = 0;
		MasterVolumeSlider.value = 1.0f;
		MusicVolumeSlider.value = 1.0f;
		SoundVolumeSlider.value = 1.0f;
		FPSCounterToggle.isOn = false;
		FPSRefreshRateSlider.value = 10;
		TimerCounterToggle.isOn = false;
	}

	public void PlayLevel (string levelName) {
		LevelChangerScript.ActivateGame(levelName);
	}

	public void LoadEditor () {
		LevelChangerScript.ActivateEditor (false);
	}

	public void Exit () {
		Application.Quit ();
	}
}