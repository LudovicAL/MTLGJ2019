using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[Range(0.0f, 1.0f)] public float themeSongVolume = 0.5f;
	public List<AudioClip> listOfJumpClips;
	public List<AudioClip> listOfSmallExplosionClips;
	public List<AudioClip> listOfBigExplosionClips;
	public List<AudioClip> listOfMaleDeathScreamClips;
	public List<AudioClip> listOfFemaleDeathScreamClips;
	public List<AudioClip> listOfPlayingThemeClips;
	public AudioClip menuThemeClip;
	public AudioClip menuClickClip;
	public AudioClip ticTacClip;
	public AudioClip victoryClip;
	public AudioClip warningClip;
	public AudioClip playerJoiningClip;
	public AudioClip playerLeavingClip;
	public AudioClip itemGrabClip;
	public AudioClip crouchClip;
	public AudioClip weaponSwitch;
	public AudioClip explosion1;
	public AudioClip explosion2;
	public AudioClip explosion3;
	public AudioClip explosion4;
	public AudioClip explosion5;

	public static AudioManager Instance { get; private set; }

	private List<AudioSource> audioSourceList = new List<AudioSource>();
    private AudioSource pausedThemeSongAudioSource;

    #region singletonpattern
    private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
    #endregion

    private void Start() {
		audioSourceList.AddRange(GetComponents<AudioSource>());

		GameStatesManager.Instance.GameStateChanged.AddListener(OnGameStateChange);
		OnGameStateChange();
	}

	private void OnGameStateChange() {
		switch (GameStatesManager.gameState) {
			case (StaticData.AvailableGameStates.Menu):
				StopAllClips();
				PlayClip(menuThemeClip, true, 0.0f, 0.2f);
				StopAllClipsInList(listOfPlayingThemeClips);
				break;
			case (StaticData.AvailableGameStates.Starting):
				StopClip(menuThemeClip, 10f);
				StopAllClipsInList(listOfPlayingThemeClips);
				break;
			case (StaticData.AvailableGameStates.Playing):
				StopClip(menuThemeClip, 5f);
				if (pausedThemeSongAudioSource) {
					pausedThemeSongAudioSource.Play();
					pausedThemeSongAudioSource = null;
				} else {
					PlayClip(GetRandomClipFromList(listOfPlayingThemeClips), true, 0.0f, 0.2f);
				}
				break;
			case (StaticData.AvailableGameStates.Pausing):
				StopClip(menuThemeClip);
				pausedThemeSongAudioSource = PauseAllClipsInList(listOfPlayingThemeClips);
				break;
			case (StaticData.AvailableGameStates.Ending):
				StopClip(menuThemeClip);
				StopAllClipsInList(listOfPlayingThemeClips, 2f);
				break;
		}
	}

    #region Play
    //Plays an AudioClip
    public void PlayClip(AudioClip clip) {
		PlayClip(clip, false, 0.0f, 1.0f);
	}

	//Plays an AudioClip and loops it
	public void PlayClip(AudioClip clip, bool loop) {
		PlayClip(clip, loop, 0.0f, 1.0f);
	}

	//Plays an AudioClip with a fade in
	public void PlayClip(AudioClip clip, float fadeInLength) {
		PlayClip(clip, false, fadeInLength, 1.0f);
	}

	//Plays an AudioClip with a fade in and loops it
	public void PlayClip(AudioClip clip, bool loop, float fadeInLength, float desiredVolume) {
		if (clip != null) {
			AudioSource audioSource = null;
			foreach (AudioSource tempSource in audioSourceList) {
				if (!tempSource.isPlaying) {
					audioSource = tempSource;
					break;
				}
			}
			if (audioSource == null) {
				audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
				audioSource.playOnAwake = false;
				audioSourceList.Add(audioSource);
			}
			audioSource.loop = loop;
			audioSource.clip = clip;
			audioSource.Play();
			if (fadeInLength > 0.0f) {
				StartCoroutine(AudioSourceFadeIn(audioSource, fadeInLength, desiredVolume));
			} else {
				audioSource.volume = desiredVolume;
			}
		} else {
			Debug.Log("You passed a null AudioClip. What is wrong with you?");
		}
	}
    #endregion

    #region Pause
    //Pauses an AudioClip
    public AudioSource PauseClip(AudioClip clip) {
		foreach (AudioSource audioSource in audioSourceList) {
			if (audioSource.isPlaying) {
				if (audioSource.clip.Equals(clip)) {
					audioSource.Pause();
					return audioSource;
				}
			}
		}
		return null;
	}

	//Pauses an AudioClip with a fadeout
	public AudioSource PauseClip(AudioClip clip, float fadeOutLength) {
		foreach (AudioSource audioSource in audioSourceList) {
			if (audioSource.isPlaying) {
				if (audioSource.clip.Equals(clip)) {
					if (fadeOutLength > 0f) {
						StartCoroutine(AudioSourceFadeOut(audioSource, fadeOutLength, true));
					} else {
						audioSource.Pause();
					}
					return audioSource;
				}
			}
		}
		return null;
	}

	//Pauses all AudioClips
	public void PauseAllClips() {
		PauseAllClips(0.0f);
	}

	//Finds and pauses all the AudioClips in a list and return the last AudioSource found playing it
	public AudioSource PauseAllClipsInList(List<AudioClip> listOfClips) {
		AudioSource audioSource = null;
		foreach (AudioClip clip in listOfClips) {
			AudioSource temp = PauseClip(clip);
			if (temp) {
				audioSource = temp;
			}
		}
		return audioSource;
	}

	//Finds and pauses all the AudioClips in a list
	public AudioSource PauseAllClipsInList(List<AudioClip> listOfClips, float fadeOutLength) {
		AudioSource audioSource = null;
		foreach (AudioClip clip in listOfClips) {
			AudioSource temp = PauseClip(clip, fadeOutLength);
			if (temp) {
				audioSource = temp;
			}
		}
		return audioSource;
	}

	//Pauses all AudioClips with a fade out
	public void PauseAllClips(float fadeOutLength) {
		foreach (AudioSource audioSource in audioSourceList) {
			if (audioSource.isPlaying) {
				if (fadeOutLength > 0.0f) {
					StartCoroutine(AudioSourceFadeOut(audioSource, fadeOutLength, true));
				} else {
					audioSource.Pause();
				}
			}
		}
	}
    #endregion

    #region Stop
    //Stops an AudioClip
    public void StopClip(AudioClip clip) {
		StopClip(clip, 0f);
	}

	//Stops an AudioClip with a fadeout
	public void StopClip(AudioClip clip, float fadeOutLength) {
		foreach (AudioSource audioSource in audioSourceList) {
			if (audioSource.isPlaying) {
				if (audioSource.clip.Equals(clip)) {
					if (fadeOutLength > 0f) {
						StartCoroutine(AudioSourceFadeOut(audioSource, fadeOutLength, false));
					} else {
						audioSource.Stop();
					}
				}
			}
		}
	}

	//Stops all AudioClips
	public void StopAllClips() {
		StopAllClips(0.0f);
	}

	//Finds and stops all the AudioClips in a list
	public void StopAllClipsInList(List<AudioClip> listOfClips) {
		foreach (AudioClip clip in listOfClips) {
			StopClip(clip);
		}
	}

	//Finds and stops all the AudioClips in a list
	public void StopAllClipsInList(List<AudioClip> listOfClips, float fadeOutLength) {
		foreach (AudioClip clip in listOfClips) {
			StopClip(clip, fadeOutLength);
		}
	}

	//Stops all AudioClips with fade out
	public void StopAllClips(float fadeOutLength) {
		foreach (AudioSource audioSource in audioSourceList) {
			if (audioSource.isPlaying) {
				if (fadeOutLength > 0.0f) {
					AudioSourceFadeOut(audioSource, fadeOutLength, false);
				} else {
					audioSource.Stop();
				}
			}
		}
	}
    #endregion

    #region Fades
    //Fades in an AudioSource
    private IEnumerator AudioSourceFadeIn(AudioSource audioSource, float fadeInLength, float desiredVolume) {
		while (audioSource.volume < desiredVolume) {
			audioSource.volume = Mathf.Clamp(audioSource.volume + (Time.deltaTime * (desiredVolume / fadeInLength)), 0.0f, desiredVolume);
			yield return null;
		}
		audioSource.Stop();
	}

	//Fades out an AudioSource
	private IEnumerator AudioSourceFadeOut(AudioSource audioSource, float fadeOutLength, bool pause) {
		while (!Mathf.Approximately(audioSource.volume, 0.0f)) {
			audioSource.volume = Mathf.Clamp(audioSource.volume - (Time.deltaTime * (1.0f / fadeOutLength)), 0.0f, 1.0f);
			yield return null;
		}
		if (pause) {
			audioSource.Pause();
		} else {
			audioSource.Stop();
		}
	}
    #endregion

    //Returns a random AudioClip from a passed list
    public AudioClip GetRandomClipFromList(List<AudioClip> clipList) {
		return clipList[Random.Range(0, clipList.Count)];
	}
}
