using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractorMotion : MonoBehaviour {

	public float radius = 50;

	public ListenerSpectrumAnalysis listenerSpectrum;




	// Use this for initialization
	void Start () {
	
		transform.position = new Vector3 (radius, 1.5f, 0f); 

	}
	
	// Update is called once per frame
	void Update () {

		transform.position = new Vector3 (radius * Mathf.Cos (Time.time/4), 1.5f + 10 * listenerSpectrum.GetWholeEnergy() * Mathf.Cos (Time.time/16), radius * Mathf.Sin (Time.time/4));

	}
}
