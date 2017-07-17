using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
public class RibbonFilters : MonoBehaviour {

	public float origLPCutoff = 20000; 
	public float origLPRes = 1.0f;
	public float origHPCutoff = 10;
	public float origHPRes = 1.0f;
	public float lowPassResonance = 2.0f;
	public float hiPassResonance = 2.0f;

	[SerializeField] float oldLPFreq;
	[SerializeField] float newLPFreq;

	[SerializeField] float oldHPFreq;
	[SerializeField] float newHPFreq;

	[SerializeField] float oldLPRes;
	[SerializeField] float newLPRes;

	[SerializeField] float oldHPRes;
	[SerializeField] float newHPRes;


	public int frameBufferSize = 10;
	int frameLPBuffer = 0;
	int frameHPBuffer = 0;

	AudioLowPassFilter lowPassFilter;
	AudioHighPassFilter hiPassFilter;


	// Use this for initialization
	void Start () {
		lowPassFilter = GetComponent<AudioLowPassFilter> ();
		hiPassFilter = GetComponent<AudioHighPassFilter> ();

		origLPCutoff = lowPassFilter.cutoffFrequency;
		origLPRes = lowPassFilter.lowpassResonanceQ;
		origHPCutoff = hiPassFilter.cutoffFrequency;
		origHPRes = hiPassFilter.highpassResonanceQ;

		oldLPFreq = origLPCutoff;
		oldLPRes = lowPassFilter.lowpassResonanceQ;

		oldHPFreq = origHPCutoff;
		oldHPRes = hiPassFilter.highpassResonanceQ;
		
	}
	
	// Update is called once per frame
	void Update () {

		frameLPBuffer++;
		frameHPBuffer++;

		if (oldLPFreq != newLPFreq) {
			lowPassFilter.cutoffFrequency = Mathf.Clamp(Mathf.Lerp (oldLPFreq, newLPFreq, (float)frameLPBuffer / frameBufferSize), 300f, 20000f);
			oldLPFreq = lowPassFilter.cutoffFrequency;
		}

		if (oldLPRes != newLPRes) {
			lowPassFilter.lowpassResonanceQ = Mathf.Clamp(Mathf.Lerp (oldLPRes, newLPRes, (float)frameLPBuffer / frameBufferSize), 1f, 2f);
			oldLPRes = lowPassFilter.lowpassResonanceQ;
		}

		if (oldHPFreq != newHPFreq) {
			hiPassFilter.cutoffFrequency = Mathf.Clamp(Mathf.Lerp (oldHPFreq, newHPFreq, (float)frameHPBuffer / frameBufferSize), 10f, 10000f);
			oldHPFreq = hiPassFilter.cutoffFrequency;
		}

		if (oldHPRes != newHPRes) {
			hiPassFilter.highpassResonanceQ = Mathf.Clamp(Mathf.Lerp (oldHPRes, newHPRes, (float)frameHPBuffer / frameBufferSize), 1f, 2f);
			oldHPRes = hiPassFilter.highpassResonanceQ;
		} 
		
	}

	public void ChangeLowPassFrequency(float inputFreq, float inputRes){

		if (inputFreq <= 100f) {
			Debug.Log ("Improper LP Filter Input");
			return;
		}

		if (frameLPBuffer < frameBufferSize) {
			return;
		} else {
			newLPFreq = inputFreq;
			newLPRes = inputRes;

			frameLPBuffer = 0;
		}

	}

	public void ChangeHighPassFrequency(float inputFreq, float inputRes) {

		if (frameHPBuffer < frameBufferSize) {
			return;
		} else { 
			newHPFreq = inputFreq;
			newHPRes = inputRes;

			frameHPBuffer = 0;

		}

	}

	public void ResetLPFrequency() {

		if (lowPassFilter.cutoffFrequency <= origLPCutoff) {

			//Debug.Log ("current cutoff: " + lowPassFilter.cutoffFrequency);
			//Debug.Log ("target cutoff: " + origLPCutoff);
			//Debug.Log ("the fucking difference is " + ((origLPCutoff - lowPassFilter.cutoffFrequency) * Time.deltaTime));

			ChangeLowPassFrequency(lowPassFilter.cutoffFrequency += (origLPCutoff - lowPassFilter.cutoffFrequency) * 0.1f, origLPRes);

		
			//DOTween.To (() => lowPassFilter.cutoffFrequency, x => lowPassFilter.cutoffFrequency = x, origLPCutoff, 1f);
		}


	}

	public void ResetHPFrequency() {

		if (hiPassFilter.cutoffFrequency >= origHPCutoff) {

			ChangeHighPassFrequency(hiPassFilter.cutoffFrequency -= (hiPassFilter.cutoffFrequency - origHPCutoff) * 0.1f, origHPRes); 

			//DOTween.To (() => hiPassFilter.cutoffFrequency, x => hiPassFilter.cutoffFrequency = x, origHPCutoff, 1f);
		}

	}

	
}
