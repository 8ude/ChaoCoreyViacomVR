using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

public class DrawRibbonSound : MonoBehaviour {
    /// <summary>
    /// Responsible for creating clips that correspond with drawing motion
    /// </summary>

    AudioSource[] myAudioSources;

	public AudioSource myHighSource;
    public AudioSource myLowSource;

    public float currentMaxVolume = 1f;

	public string instrumentType;

	public int clipIndex = 0;

	double startTime, stopTime;
	double timeDifference;

	int sampleOffset = 0;

	//time to fadeout at end of clip
	public float fadeOutTime = 0.2f;

	
    float[] origHighAudioData;
    float[] origLowAudioData;

	public Vector3[] splinePoints;
    public GameObject[] markerObjects;


	int splinePointIndex;
    int _halfNotes;
    public int LengthInHalfNotes {
        get{
            return _halfNotes;
        }
    }


    //flip this to false to turn off the movement of the source along the ribbon
    public bool autoMoveSound = true;

	//need this for debugging Audio Sync issues
	[SerializeField] double sourceStartTime;

	void Awake() {
        
        myAudioSources = gameObject.GetComponents<AudioSource>();
        //Debug.Log(myAudioSources.Length);
        if (myAudioSources.Length > 1) {
            myHighSource = myAudioSources[0];
            myLowSource = myAudioSources[1];
        }
        else Debug.Log("Error - Ribbon doesn't have 2 audio sources");

    }

	void Update() {

        BalanceAudioSources(RibbonGameManager.instance.RemapRange(transform.position.y, 0f, 2f, -1f, 1f), currentMaxVolume);
	
	}

    public void StartDrawingRibbon(AudioClip origHighClip, AudioClip origLowClip) {


		myHighSource.clip = origHighClip;
        Debug.Log("high clip " + origHighClip.name);

        myLowSource.clip = origLowClip;
        Debug.Log("low clip " + origLowClip.name);


        startTime = Clock.Instance.AtNextHalf();

		origHighAudioData = new float[myHighSource.clip.samples * myHighSource.clip.channels];
        origLowAudioData = new float[myLowSource.clip.samples * myLowSource.clip.channels];

		myHighSource.clip.GetData (origHighAudioData, 0);
        myLowSource.clip.GetData(origLowAudioData, 0);



	}

	public void StopDrawingRibbon(AudioClip origClip) {
		stopTime = Clock.Instance.AtNextHalf();

		float newClipLength = (float)stopTime - (float)startTime;
        if (newClipLength <= Clock.Instance.MeasureLength()) {
			newClipLength = Clock.Instance.MeasureLength ();
      
		} else if (newClipLength > origClip.length) {
			newClipLength = origClip.length;
		}

		int newClipSamples = Mathf.RoundToInt (newClipLength * origClip.frequency);
        

        float[] highAudioData = new float[newClipSamples * myHighSource.clip.channels];
        float[] lowAudioData = new float[newClipSamples * myLowSource.clip.channels];
        

		AudioClip newHighClip = AudioClip.Create ("RibbonHighClip", newClipSamples, origClip.channels, origClip.frequency, false);
        AudioClip newLowClip = AudioClip.Create("RibbonLowClip", newClipSamples, origClip.channels, origClip.frequency, false);


		for (int i = 0; i < (newClipSamples*myHighSource.clip.channels); i++) {

			highAudioData [i] = origHighAudioData [i];
            lowAudioData[i] = origLowAudioData[i];

            // fade out last 10000 samples (roughly 1/4 sec);

			if (i > (newClipSamples * myHighSource.clip.channels) - 10000) {

				int j = i - ((newClipSamples * myHighSource.clip.channels) - 10000);

				highAudioData[i] *= Mathf.Lerp(1f, 0f, (float) j / 10000);
                lowAudioData[i] *= Mathf.Lerp(1f, 0f, (float)j / 10000);
			}

		}

		newHighClip.SetData (highAudioData, 0);
        newLowClip.SetData(lowAudioData, 0);

		myHighSource.clip = newHighClip;
        myLowSource.clip = newLowClip;

		sourceStartTime = Clock.Instance.AtNextMeasure ();
        Debug.Log("source start time: " + sourceStartTime);
		myHighSource.PlayScheduled (sourceStartTime);
        myLowSource.PlayScheduled (sourceStartTime);

        _halfNotes = Mathf.RoundToInt(newHighClip.length / Clock.Instance.HalfLength()); 

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
                    if (autoMoveSound) {
                        transform.position += (sPoints[i] - transform.position) * Time.deltaTime * 4f;
                        yield return null;
                    }
                    else yield return null;
				}
			}
		}
	}

    public void BalanceAudioSources (float heightValue, float maxVolume) {
        ///<summary>
        /// Balance between the two different audio loops according
        /// to constant power curve
        ///</summary>

        myHighSource.volume = Mathf.Sqrt(0.5f * (1f + heightValue)) * maxVolume;
        myLowSource.volume = Mathf.Sqrt(0.5f * (1f - heightValue)) * maxVolume;

    }

    public void RestartClips(int halfNoteIndex) {
        //TODO Start the clip at ((time/_halfnotes) * noteIndex)

        myHighSource.PlayScheduled(Clock.Instance.AtNextBeat());
        myLowSource.PlayScheduled(Clock.Instance.AtNextBeat());

    }
		
}
