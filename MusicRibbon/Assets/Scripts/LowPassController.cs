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

	float distanceThreshold = 0.2f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AdjustFrequency(Transform target, float distance) {

		AudioLowPassFilter targetFilter = target.gameObject.GetComponent<AudioLowPassFilter> ();
		origFreq = targetFilter.cutoffFrequency;

		Vector3 targetForward = target.forward;

		Vector3 aVector = target.position - transform.position;
		float angle = Vector3.Angle (targetForward, aVector);
		float targetMag = aVector.magnitude * Mathf.Cos (angle * Mathf.PI / 180f);

		if (distance < distanceThreshold) {
			if (targetFilter.cutoffFrequency > maxFreq) {
				DOTween.To (() => targetFilter.cutoffFrequency, x => targetFilter.cutoffFrequency = x, maxFreq, 0.2f);
			} else {
				targetFilter.cutoffFrequency = MathUtil.Remap (targetMag, 0f, 2f, minFreq, maxFreq);
				targetFilter.lowpassResonanceQ = MathUtil.Remap (distance, 0f, 1f, minRes, maxRes);
			}
		} else {
			DOTween.To (() => targetFilter.cutoffFrequency, x => targetFilter.cutoffFrequency = x, origFreq, 0.2f);
		}


	}
}
