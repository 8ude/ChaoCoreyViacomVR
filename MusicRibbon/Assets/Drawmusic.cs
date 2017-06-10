using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawmusic : MonoBehaviour {

	public bool triggerButtonDown = false;
	private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
	private SteamVR_Controller.Device controller{
		get{ return SteamVR_Controller.Input ((int)SteamVR_TrackedObject.index); }
	}

	private SteamVR_TrackedObject TrackedObj;

	// Use this for initialization
	void Start () {
		TrackedObj = GetComponent ();
	}
	
	// Update is called once per frame
	void Update () {

		if (triggerButtonDown) {
			
			this.gameObject.GetComponent<TrailRenderer> ().enabled = true;
		}
		
	}
}
