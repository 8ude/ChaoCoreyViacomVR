using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
public class RibbonFilters : MonoBehaviour {

	public float origLPCutoff = 20000; 
	public float origHPCutoff = 10;
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
		origHPCutoff = hiPassFilter.cutoffFrequency;

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
			lowPassFilter.cutoffFrequency = Mathf.Lerp (oldLPFreq, newLPFreq, (float)frameLPBuffer / frameBufferSize);
			oldLPFreq = lowPassFilter.cutoffFrequency;
		}

		if (oldLPRes != newLPRes) {
			lowPassFilter.lowpassResonanceQ = Mathf.Lerp (oldLPRes, newLPRes, (float)frameLPBuffer / frameBufferSize);
			oldLPRes = lowPassFilter.lowpassResonanceQ;
		}

		if (oldHPFreq != newHPFreq) {
			hiPassFilter.cutoffFrequency = Mathf.Lerp (oldHPFreq, newHPFreq, (float)frameHPBuffer / frameBufferSize);
			oldHPFreq = hiPassFilter.cutoffFrequency;
		}

		if (oldHPRes != newHPRes) {
			hiPassFilter.highpassResonanceQ = Mathf.Lerp (oldHPRes, newHPRes, (float)frameHPBuffer / frameBufferSize);
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
}
