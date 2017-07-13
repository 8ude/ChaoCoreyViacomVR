using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Beat;

public class PreRibbon : MonoBehaviour {

	public int stemIndex = 0;

	public AudioClip myClip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayPreStem() {

		GetComponent<AudioSource> ().clip = myClip;
		GetComponent<AudioSource> ().PlayScheduled (Clock.Instance.AtNextEighth ());

	}
}
