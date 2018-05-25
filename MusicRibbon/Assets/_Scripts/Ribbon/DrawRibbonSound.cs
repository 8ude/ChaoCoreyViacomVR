using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Controllers;
using DG.Tweening;
using Beat;

/// <summary>
/// Responsible for creating clips that correspond with drawing motion
/// </summary>
public class DrawRibbonSound : MonoBehaviour {
    

    AudioSource[] myAudioSources;

	public AudioSource myHighSource;
    public AudioSource myLowSource;

    float prevHighVolume;
    float prevLowVolume;

    public float currentMaxVolume = 1f;

    bool pauseBalancing = false;

	public int clipIndex;

	double startTime, stopTime;
	double timeDifference;
	
    float[] origHighAudioData;
    float[] origLowAudioData;

	public Vector3[] splinePoints;
    //public GameObject[] markerObjects;


	int splinePointIndex;
    int _halfNotes;
    public int LengthInHalfNotes {
        get{
            return _halfNotes;
        }
    }


    //flip this to false to turn off the movement of the music source along the ribbon
    public bool autoMoveSound = true;

	//need this for debugging Audio Sync issues
	[SerializeField] double sourceStartTime;

	void Awake() {

        prevHighVolume = 0f;
        prevLowVolume = 0f;

        pauseBalancing = false;
        
        myAudioSources = gameObject.GetComponents<AudioSource>();
       
        if (myAudioSources.Length > 1) {
            myHighSource = myAudioSources[0];
            myLowSource = myAudioSources[1];
        }
        else Debug.Log("Error - Ribbon doesn't have 2 audio sources");

    }

	void Update() {

        if (!pauseBalancing) {
            //y-position of game object acts as a crossfader between high and low audio sources
            float balanceValue = RibbonGameManager.instance.RemapRange(transform.position.y, -0.5f, 2f, -1f, 1f);
            balanceValue = Mathf.Clamp(balanceValue, -1f, 1f);
            
            BalanceAudioSources(balanceValue, currentMaxVolume);
        } 
	
	}


    public void StartDrawingRibbon(AudioClip origHighClip, AudioClip origLowClip) {


		myHighSource.clip = origHighClip;
        myLowSource.clip = origLowClip;

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
        

		AudioClip newHighClip = AudioClip.Create (origClip.name + "_HighClip", newClipSamples, origClip.channels, origClip.frequency, false);
        AudioClip newLowClip = AudioClip.Create(origClip.name + "_LowClip", newClipSamples, origClip.channels, origClip.frequency, false);

        //write data to audio clips
		for (int i = 0; i < (newClipSamples*myHighSource.clip.channels); i++) {

			highAudioData [i] = origHighAudioData [i];
            lowAudioData[i] = origLowAudioData[i];

            // linear fade out last 10000 samples (about 1/4 sec) to minimize pops

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
        //Debug.Log("source start time: " + sourceStartTime);
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
				while (Vector3.Distance (transform.position, sPoints [i]) > 0.01f) {
                    if (autoMoveSound) {
                        transform.Translate ((sPoints[i]-transform.position).normalized * Time.deltaTime * RibbonGameManager.instance.followRibbonSpeed, Space.World);
                        yield return null;
                    }
                    else yield return null;
				}
			}
		}
	}

    ///<summary>
    /// Crossfade between the two different audio loops according
    /// to constant power curve
    ///</summary>
    public void BalanceAudioSources (float heightValue, float maxVolume) {
        
        //Debug.Log ("height value " + heightValue);
        //Debug.Log("power scale " + Mathf.Sqrt(0.5f * (1f + heightValue)));

        myHighSource.volume = Mathf.Lerp(prevHighVolume, Mathf.Sqrt(0.5f * (1f + heightValue)) * maxVolume, Time.deltaTime);
        prevHighVolume = myHighSource.volume;
        myLowSource.volume = Mathf.Lerp(prevLowVolume, Mathf.Sqrt(0.5f * (1f - heightValue)) * maxVolume, Time.deltaTime);
        prevLowVolume = myLowSource.volume;

    }

    public void RestartClips(int halfNoteIndex) {
        //TODO Start the clip at ((time/_halfnotes) * noteIndex)

        myHighSource.PlayScheduled(Clock.Instance.AtNextBeat());
        myLowSource.PlayScheduled(Clock.Instance.AtNextBeat());

    }

    public void UnPauseBalancing() {
        pauseBalancing = false;
        prevLowVolume = 0f;
        prevHighVolume = 0f;
    }



    /*
     * Built-in Curvy Spline following methods not functioning properly
     * 
    public void FollowRibbonSpline() {
        SplineController sController = gameObject.AddComponent<SplineController>();
        sController.Spline = transform.root.gameObject.GetComponentInChildren<CurvySpline>();
        sController.Speed = 20f;
        sController.Clamping = CurvyClamping.Loop;
        sController.Play();
    }
    */

}
