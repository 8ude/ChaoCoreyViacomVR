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

	public Vector3[] splinePoints;

	int splinePointIndex;

	AudioLowPassFilter lpFilter;
	AudioHighPassFilter hpFilter;

	void Start() {

		lpFilter = GetComponent<AudioLowPassFilter> ();
		hpFilter = GetComponent<AudioHighPassFilter> ();


	}

	void Update() {



		float lowPassCutoff = lpFilter.cutoffFrequency;
		float hiPassCutoff = hpFilter.cutoffFrequency;

		float newHPCutoff;
		float newLPCutoff;

		if (transform.position.y > 1.5f) {
			newHPCutoff = RibbonGameManager.instance.RemapRange (transform.position.y, 1.5f, 2.5f, 10, 4000);
		} else
			newHPCutoff = 10f;

		if (transform.position.y < 1.5f) {
			newLPCutoff = RibbonGameManager.instance.RemapRange (transform.position.y, 0.3f, 1.5f, 100, 22000);
		} else
			newLPCutoff = 22000;


		lpFilter.cutoffFrequency = 
			Mathf.Lerp (lowPassCutoff, newLPCutoff, Time.deltaTime);

		//HIGH PASS FILTER IS GIVING WEIRD POPPING NOISES
		//hpFilter.cutoffFrequency = 
			//Mathf.Lerp (hiPassCutoff, newHPCutoff, Time.deltaTime);
			
	}

	public void StartDrawingRibbon() {

		mySource = GetComponent<AudioSource> ();
		mySource.clip = origClips[clipIndex];


		startTime = Clock.Instance.AtNextMeasure();

		//mySource.PlayScheduled (startTime);


		origAudioData = new float[mySource.clip.samples*mySource.clip.channels];

		mySource.clip.GetData (origAudioData, 0);



	}

	public void StopDrawingRibbon() {
		stopTime = Clock.Instance.AtNextMeasure();

		float newClipLength = (float)stopTime - (float)startTime;
        if (newClipLength == 0) {
            newClipLength = Clock.Instance.MeasureLength();
      
        }

		int newClipSamples = Mathf.RoundToInt (newClipLength * origClips[clipIndex].frequency);
        

		float[] audioData = new float[newClipSamples*mySource.clip.channels];
        

		AudioClip newClip = AudioClip.Create ("RibbonClip", newClipSamples, origClips[clipIndex].channels, origClips[clipIndex].frequency, false);



		for (int i = 0; i < (newClipSamples*mySource.clip.channels); i++) {

			audioData [i] = origAudioData [i];



		}

		//Debug.Log ("10000th sample:" + audioData[10000]);

		newClip.SetData (audioData, 0);



		mySource.clip = newClip;

		mySource.PlayScheduled (Clock.Instance.AtNextMeasure());

	}

	public void FollowRibbon() {

		if (splinePoints != null) {
			StartCoroutine(MoveToNextPoint (splinePoints));
			//index++;

		}
	}

	IEnumerator MoveToNextPoint(Vector3[] sPoints) {

		while (true) {
			for (int i = 0; i < sPoints.Length; i++) {
				while (Vector3.Distance (transform.position, sPoints [i]) > 0.1f) {
					transform.position += (sPoints[i] - transform.position) * Time.deltaTime;
					yield return null;
				}
			}
		}
	}
		
}
