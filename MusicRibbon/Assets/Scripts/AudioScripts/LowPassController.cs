using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LowPassController : MonoBehaviour {

	float origFreq;
	public float minFreq = 1000;
	public float maxFreq = 5000;
	public float minRes = 0.5f;
	public float maxRes = 2.0f;

	float distanceThreshold = 0.5f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AdjustFrequency(Transform target, float distance) {

		AudioLowPassFilter targetFilter = target.gameObject.GetComponent<AudioLowPassFilter> ();
		RibbonFilters ribbonFilter = target.gameObject.GetComponent<RibbonFilters> ();

		//Vector Math to get distance from line running through the local z-axis of the object
		Vector3 targetForward = target.forward;

		Vector3 aVector = target.position - transform.position;
		float angle = Vector3.Angle (targetForward, aVector);
		float targetMag = Mathf.Abs(aVector.magnitude * Mathf.Cos (angle * Mathf.PI / 180f));


		if (distance < distanceThreshold) {
			if (targetFilter.cutoffFrequency > maxFreq) {
				DOTween.To (() => targetFilter.cutoffFrequency, x => targetFilter.cutoffFrequency = x, maxFreq, 0.2f);
			} else {
				Debug.Log ("LP Frequency magnitude = " + targetMag); 
				ribbonFilter.ChangeLowPassFrequency (MathUtil.Remap (targetMag, 0f, 2f, minFreq, maxFreq), MathUtil.Remap (distance, 0f, 1f, minRes, maxRes));

			}
		} else {
			//OutOfRange -- revert frequency to original frequency
			DOTween.To (() => targetFilter.cutoffFrequency, x => targetFilter.cutoffFrequency = x, ribbonFilter.origLPCutoff, 0.2f);
		}


	}
}
