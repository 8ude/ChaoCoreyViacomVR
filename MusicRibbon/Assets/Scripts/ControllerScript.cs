using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class ControllerScript : MonoBehaviour {

	public bool triggerButtonDown = false;
    public Hand handScript;

	private SteamVR_TrackedObject trackedObj;

	// Use this for initialization
	void Start () {
		//trackedObj = GetComponent<SteamVR_TrackedObject>();
        handScript = GetComponent<Hand>();
    }
	
	// Update is called once per frame
	void Update () {

        triggerButtonDown = handScript.GetStandardInteractionButton();

		
			
		this.gameObject.GetComponent<TrailRenderer> ().enabled = triggerButtonDown;

		
		
	}
}
