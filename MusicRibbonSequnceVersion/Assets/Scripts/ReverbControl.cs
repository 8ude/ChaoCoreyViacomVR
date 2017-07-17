using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverbControl : MonoBehaviour {

	AudioReverbFilter reverbFilter;
	public float minReverb = -4000f;
	public float maxReverb = 300f;

	public float minScale = 0.01f;
	public float maxScale = 0.3f;

	//used for uniform scale
	//TODO - make this axis-dependent
	public float originalScale;
	[SerializeField] float currentScale;

	public float destReverb;
	public float prevReverb;




	public int frameBuffer;
	int frameCount;


	// Use this for initialization
	void Start () {
		frameCount = 0;
		
		originalScale = transform.localScale.magnitude;


		reverbFilter = gameObject.GetComponent<AudioReverbFilter> ();
		prevReverb = reverbFilter.reverbLevel;

	}
	
	// Update is called once per frame
	void Update () {
		
		//check to see if we're out of the frame buffer
		if (frameCount > frameBuffer) {
			//if yes, reset the frame buffer and our reverb level

			prevReverb = reverbFilter.reverbLevel;

			//check to see if our dest Reverb has changed

			destReverb = MathUtil.Remap(Mathf.Clamp(transform.localScale.magnitude-originalScale, 0f, maxScale), minScale, maxScale, minReverb, maxReverb);
			if (destReverb != prevReverb) {
				//if so, our scale has changed, so we set our frame count to 0
				frameCount = 0;

			}
	
		} else {
			frameCount++;
			UpdateReverb ();
		}

		currentScale = transform.localScale.magnitude;




	}

	void UpdateReverb() {

		float currentReverb = Mathf.Lerp (prevReverb, destReverb, (float)frameCount / frameBuffer);

		reverbFilter.reverbLevel = currentReverb;




	}
}
