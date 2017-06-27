using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class RibbonFilters : MonoBehaviour {

	public float origLPCutoff = 20000; 
	public float lowPassResonance = 2.0f;

	[SerializeField] float oldLPFreq;
	[SerializeField] float newLPFreq;

	[SerializeField] float oldLPRes;
	[SerializeField] float newLPRes;



	public int frameBufferSize = 10;
	int frameBuffer = 0;

	AudioLowPassFilter lowPassFilter;


	// Use this for initialization
	void Start () {
		lowPassFilter = GetComponent<AudioLowPassFilter> ();
		origLPCutoff = lowPassFilter.cutoffFrequency;

		oldLPFreq = origLPCutoff;
		
	}
	
	// Update is called once per frame
	void Update () {

		frameBuffer++;

		if (oldLPFreq != newLPFreq) {
			lowPassFilter.cutoffFrequency = Mathf.Lerp (oldLPFreq, newLPFreq, (float)frameBuffer / frameBufferSize);
			oldLPFreq = lowPassFilter.cutoffFrequency;
		}

		if (oldLPRes != newLPRes) {
			lowPassFilter.lowpassResonanceQ = Mathf.Lerp (oldLPRes, newLPRes, (float)frameBuffer / frameBufferSize);
			oldLPRes = lowPassFilter.lowpassResonanceQ;
		}
		
	}

	public void ChangeLowPassFrequency(float inputFreq, float inputRes){

		if (frameBuffer < frameBufferSize) {
			return;
		} else {
			newLPFreq = inputFreq;
			newLPRes = inputRes;

			frameBuffer = 0;
		}

	}
}
