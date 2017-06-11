using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMotion : MonoBehaviour {

	SpectrumAnalysis analyzer;
	float prevAnalyzerValue, prevYPos;
	public float dampening = 0.1f;
	public float smoothingValue = 0.1f;


	// Use this for initialization
	void Start () {
		prevAnalyzerValue = 0f;
		prevYPos = transform.position.y;
		analyzer = GetComponentInParent<SpectrumAnalysis> ();	
	}
	
	// Update is called once per frame
	void Update () {

		float nextYPos = (analyzer.GetWholeEnergy () * dampening) - prevAnalyzerValue;

		transform.position = new Vector3 (transform.position.x, 1f+(nextYPos * smoothingValue), transform.position.z);

		prevAnalyzerValue = analyzer.GetWholeEnergy () * dampening;

	}
}
