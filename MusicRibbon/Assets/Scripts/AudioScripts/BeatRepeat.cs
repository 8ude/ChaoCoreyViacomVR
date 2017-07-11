using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;


public class BeatRepeat : MonoBehaviour {

	public AudioSource mySource;

	public int beatRepeatLength = 1;
	public float fadeLength;
	public float beatJitter;

	AudioClip originalClip;
	float [] origClipSamples;
	int fadeSamples;
	float volume;
	int tempo;
	int clipBeats;

	AudioClip nextClip;

	void Start() {

		tempo = Mathf.RoundToInt((float)Clock.Instance.BPM);

	}


	void Update() {

	}

	public void PopulateSamples(AudioSource source) {



	}

	public void SetClip () {

		originalClip = mySource.clip;
		origClipSamples = new float[originalClip.samples * originalClip.channels];

		clipBeats = Mathf.RoundToInt (originalClip.length / Clock.Instance.QuarterLength ());



		Debug.Log (clipBeats);

	}

}
