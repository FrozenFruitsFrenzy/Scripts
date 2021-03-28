using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using TMPro;

public class SoundManager : MonoBehaviour {
	public AudioMixer Mixer;
	public AudioSource Source;


	public AudioClip[] MusicClips;

	public int currentTrack;

	public GameObject TrackName;

	public bool dead;
	public float currentFrequency = 22000.0f;

	public bool paused;
	public float currentPitch = 1.0f;
	public float currentPitchShift = 1.0f;

	void Start () {
		Shuffle ();
	}

	void OnEnable() {
		Shuffle ();
	}

	void Update () {

		if (Source.time >= Source.clip.length)
			GetNextTrack ();

		if (dead)
			currentFrequency = Mathf.Lerp(currentFrequency, 500.0f, 9.0f * Time.deltaTime);
		else
			currentFrequency = Mathf.Lerp(currentFrequency, 22000.0f, 9.0f * Time.deltaTime);
		Mixer.SetFloat ("Frequency", currentFrequency);

		if (paused) 
			currentPitch = Mathf.Lerp(currentPitch, 0.0f, 2.1f * Time.unscaledDeltaTime);
		else
			currentPitch = Mathf.Lerp(currentPitch, 1.0f, 2.1f * Time.unscaledDeltaTime);
		currentPitchShift = 1.0f / currentPitch;

		Mixer.SetFloat ("Pitch", currentPitch);
		Mixer.SetFloat ("PitchShift", currentPitchShift);
	}

	public void SetDead (bool isDead) {
		dead = isDead;
    }

	public void SetPaused (bool isPaused) {
		paused = isPaused;
	}

	public void GetNextTrack () {
		Source.Stop ();
		currentTrack++;
		if (currentTrack >= MusicClips.Length)
			Shuffle ();
		Source.clip = MusicClips [currentTrack];
		Source.Play ();
		DisplayName ();
	}

	public void GetPreviousTrack () {
		if (currentTrack == 0)
			return;
		Source.Stop ();
		currentTrack--;
		Source.clip = MusicClips [currentTrack];
		Source.Play ();
		DisplayName ();
	}

	void Shuffle () {

		for (int j = 1; j < MusicClips.Length; j++) {
			AudioClip tempClip = MusicClips[j];
			int Rand = Random.Range(j, MusicClips.Length);
			MusicClips[j] = MusicClips[Rand];
			MusicClips[Rand] = tempClip;
		}

		currentTrack = -1;
		#if UNITY_EDITOR
			currentTrack = 0;
		#endif
		GetNextTrack ();
	}

	void DisplayName() {
		//TrackName.GetComponent<TMP_Text> ().text = "NOW PLAYING: APHENE - " + MusicClips[currentTrack].name.ToUpper();
	}
}
