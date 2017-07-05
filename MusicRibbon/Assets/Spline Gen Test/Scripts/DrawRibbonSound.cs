using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRibbonSound : MonoBehaviour {

	AudioSource mySource;
	public AudioClip[] origClips;

	public int clipIndex = 0;

	float startTime, stopTime;
	float timeDifference;

	int sampleOffset = 0;

	//float[] audioData;
	float[] origAudioData;

	void Start() {
		


	}

	public void StartDrawingRibbon() {

		mySource = GetComponent<AudioSource> ();
		mySource.clip = origClips[clipIndex];
		if (clipIndex == 2) {
			mySource.time = 30f;
		} else if (clipIndex == 3) {
			mySource.time = 60f;
		}
		mySource.Play ();


		origAudioData = new float[mySource.clip.samples*mySource.clip.channels];
		if (clipIndex == 2) {
			//offset for drum loop
			sampleOffset = Mathf.RoundToInt (30 * origClips [clipIndex].frequency * origClips [clipIndex].channels);

		} else if (clipIndex == 3) {
			sampleOffset = Mathf.RoundToInt (60 * origClips [clipIndex].frequency * origClips [clipIndex].channels);
		}
		mySource.clip.GetData (origAudioData, sampleOffset);

		startTime = Time.time;

	}

	public void StopDrawingRibbon() {
		stopTime = Time.time;

		float newClipLength = stopTime - startTime;
		int newClipSamples = Mathf.RoundToInt (newClipLength * origClips[clipIndex].frequency);

		//Debug.Log ("Clip Samples:" + newClipSamples);

		float[] audioData = new float[newClipSamples*mySource.clip.channels];

		AudioClip newClip = AudioClip.Create ("RibbonClip", newClipSamples, origClips[clipIndex].channels, origClips[clipIndex].frequency, false);



		for (int i = 0; i < (newClipSamples*mySource.clip.channels); i++) {

			audioData [i] = origAudioData [i];



		}

		//Debug.Log ("10000th sample:" + audioData[10000]);

		newClip.SetData (audioData, 0);



		mySource.clip = newClip;

		mySource.Play ();

	}


}
