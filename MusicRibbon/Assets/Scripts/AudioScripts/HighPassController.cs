using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HighPassController : MonoBehaviour {

	float origFreq;
	public float minFreq = 200f;
	public float maxFreq = 5000f;
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

		AudioHighPassFilter targetFilter = target.gameObject.GetComponent<AudioHighPassFilter> ();
		RibbonFilters ribbonFilter = target.gameObject.GetComponent<RibbonFilters> ();

		Vector3 targetForward = target.forward;

		Vector3 aVector = target.position - transform.position;
		float angle = Vector3.Angle (targetForward, aVector);
		float targetMag = Mathf.Abs(aVector.magnitude * Mathf.Cos (angle * Mathf.PI / 180f));

		//Debug.Log("HPDistance " + distance);

		if (distance < distanceThreshold) {
			if (targetFilter.cutoffFrequency < minFreq) {

				//TODO - MAKE SURE THIS IS ALL ON THE RIBBON FILTERS SCRIPT
				DOTween.To (() => targetFilter.cutoffFrequency, x => targetFilter.cutoffFrequency = x, minFreq, 2f);
			} else {

				ribbonFilter.ChangeHighPassFrequency (Mathf.Clamp(MathUtil.Remap (targetMag, 0f, 0.5f, minFreq, maxFreq), minFreq, maxFreq), 
					Mathf.Clamp(MathUtil.Remap (distance, 0f, 1f, minRes, maxRes), minRes, maxRes));

			}
		}


	}


}
