using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

public class DrawRibbonSound : MonoBehaviour {

	AudioSource mySource;
	public AudioClip[] origClips;

	public int clipIndex = 0;

	double startTime, stopTime;
	double timeDifference;

	int sampleOffset = 0;

	//time to fadeout after 
	public float fadeOutTime = 0.2f;

	//float[] audioData;
	float[] origAudioData;



	void Start() {
		


	}

	public void StartDrawingRibbon() {

		mySource = GetComponent<AudioSource> ();
		mySource.clip = origClips[clipIndex];

		startTime = Clock.instance.AtNextMeasure();

		mySource.PlayScheduled (startTime);


		origAudioData = new float[mySource.clip.samples*mySource.clip.channels];

		mySource.clip.GetData (origAudioData, 0);



	}

	public void StopDrawingRibbon() {
		stopTime = Clock.instance.AtNextMeasure();

		float newClipLength = (float)stopTime - (float)startTime;
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

		mySource.PlayScheduled (Clock.instance.AtNextMeasure());

	}


	public void DrawBassRibbon(){
		clipIndex = 0;
		Debug.Log ("Bass");
	}

	public void DrawDrumRibbon(){
		clipIndex = 1;
		Debug.Log ("Drum");
	}

	public void DrawHarmonyRibbon(){
		clipIndex = 2;
		Debug.Log ("Harmony");
	}

	public void DrawMelodyRibbon(){
		clipIndex = 3;
		Debug.Log ("Melody");
	}


}
