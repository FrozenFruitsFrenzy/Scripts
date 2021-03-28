using UnityEngine;
using UnityEngine.Audio;

public class TimeManager : MonoBehaviour {
	public float defaultTimeScale;
	public bool paused;

	public AudioClip PauseClip;

	private AudioSource Source;
	private SoundManager sManager;

	void Start() {
		Source = GetComponent<AudioSource> ();
		sManager = GetComponent<SoundManager> ();
	}

    void Update() {
		Time.timeScale = Mathf.Lerp(Time.timeScale, paused ? 0.0f : defaultTimeScale, 2.1f * Time.unscaledDeltaTime);
    }

	public void ChangeState(bool isPaused) {
		paused = isPaused;

		if (paused)
			Source.PlayOneShot (PauseClip, 1.0f);

		sManager.SetPaused (paused);
	}
}
