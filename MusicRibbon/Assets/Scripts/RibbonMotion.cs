using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMotion : MonoBehaviour {

	SpectrumAnalysis analyzer;


	// Use this for initialization
	void Start () {
		analyzer = GetComponentInParent<SpectrumAnalysis> ();	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (transform.position.x, 1f+(analyzer.GetWholeEnergy () * 0.1f), transform.position.z);
		
	}
}
