using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

public class DrawRibbonSound : MonoBehaviour {

    /// <summary>
    /// Class for Audio that is attached to Ribbons and plays the associated clip
    /// Controls behavior for audiosource moving along ribbon, HP+LP filter
    /// </summary>

	public AudioSource mySource;

	public string instrumentType;

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

	//need this for debugging Audio Sync issues
	[SerializeField] double sourceStartTime;

	void Start() {

		lpFilter = GetComponent<AudioLowPassFilter> ();
		hpFilter = GetComponent<AudioHighPassFilter> ();

        float hpCutoff;
        float lpCutoff;


        if (transform.position.y > 1.5f)
        {
            hpCutoff = RibbonGameManager.instance.RemapRange(transform.position.y, 1.5f, 2.5f, 10, 4000);
        }
        else
            hpCutoff = 10f;

        if (transform.position.y < 1.5f)
        {
            lpCutoff = RibbonGameManager.instance.RemapRange(transform.position.y, 0.3f, 1.5f, 100, 22000);
        }
        else
            lpCutoff = 22000;

        hpFilter.cutoffFrequency = hpCutoff;
        lpFilter.cutoffFrequency = lpCutoff;


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
	    hpFilter.cutoffFrequency = 
			Mathf.Lerp (hiPassCutoff, newHPCutoff, Time.deltaTime);
			
	}

	public void StartDrawingRibbon(AudioClip origClip) {


		mySource = gameObject.GetComponent<AudioSource> ();
		mySource.clip = origClip;

		startTime = Clock.Instance.AtNextHalf();

		origAudioData = new float[mySource.clip.samples*mySource.clip.channels];

		mySource.clip.GetData (origAudioData, 0);

	}

    //
	public void StopDrawingRibbon(AudioClip origClip) {
		stopTime = Clock.Instance.AtNextHalf();

		float newClipLength = (float)stopTime - (float)startTime;
        if (newClipLength <= Clock.Instance.MeasureLength()) {
			newClipLength = Clock.Instance.MeasureLength ();
      
		} else if (newClipLength > origClip.length) {
			newClipLength = origClip.length;
		}

		int newClipSamples = Mathf.RoundToInt (newClipLength * origClip.frequency);
        

		float[] audioData = new float[newClipSamples*mySource.clip.channels];
        
        //newClip to be attached to this game object
		AudioClip newClip = AudioClip.Create ("RibbonClip", newClipSamples, origClip.channels, origClip.frequency, false);


        //write the Audio data for the clip
		for (int i = 0; i < (newClipSamples*mySource.clip.channels); i++) {

			audioData [i] = origAudioData [i];

            //fade out over the last 10,000 samples (0.25 s) to prevent popping
			if (i > (newClipSamples * mySource.clip.channels) - 10000) {

				int j = i - ((newClipSamples * mySource.clip.channels) - 10000);

				audioData[i] *= Mathf.Lerp(1f, 0f, (float) j / 10000); 
			}



		}


		newClip.SetData (audioData, 0);

		mySource.clip = newClip;


		sourceStartTime = Clock.Instance.AtNextMeasure ();
		mySource.PlayScheduled (sourceStartTime);

	}

	public void FollowRibbon() {

		if (splinePoints != null) {
			//This coroutine is causing issues with small splines/drawing in small areas
			StartCoroutine(MoveToNextPoint (splinePoints));
			//index++;

		}
	}

	IEnumerator MoveToNextPoint(Vector3[] sPoints) {

		while (true) {
			for (int i = 0; i < sPoints.Length; i++) {
				while (Vector3.Distance (transform.position, sPoints [i]) > 0.001f) {
					transform.position += (sPoints[i] - transform.position) * Time.deltaTime * 2;
					yield return null;
				}
			}
		}
	}
		
}
