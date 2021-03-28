[System.Serializable]
public class Settings {
	public int Resolution;
	public bool fullScreen;
	public int antialiasingQuality;
	public bool softParticles;
	public bool VSync;
	public int textureQuality;
	public bool lightingEnabled;
	public bool blurEnabled;
	public bool transparencyEnabled;
	public bool Use2DRenderMode;
	public float CameraDistance;
	public int CameraType;
	public float MasterVolume;
	public float MusicVolume;
	public float SoundVolume;
	public bool FPSCounterOn;
	public int FPSRefreshRate;
	public bool TimerCounterOn;

	public Settings (Menu menu) {
		Resolution = menu.Resolution;
		fullScreen = menu.fullScreen;
		antialiasingQuality = menu.antialiasingQuality;
		softParticles = menu.softParticles;
		VSync = menu.VSync;
		textureQuality = menu.textureQuality;
		blurEnabled = menu.blurEnabled;
		lightingEnabled = menu.lightingEnabled;
		transparencyEnabled = menu.transparencyEnabled;
		Use2DRenderMode = menu.Use2DRenderMode;
		CameraDistance = menu.CameraDistance;
		CameraType = menu.CameraType;
		MasterVolume = menu.MasterVolume;
		MusicVolume = menu.MusicVolume;
		SoundVolume = menu.SoundVolume;
		FPSCounterOn = menu.FPSCounterOn;
		FPSRefreshRate = menu.FPSRefreshRate;
		TimerCounterOn = menu.TimerCounterOn;
	}
}