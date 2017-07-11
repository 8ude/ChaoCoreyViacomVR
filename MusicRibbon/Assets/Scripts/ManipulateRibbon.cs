using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateRibbon : MonoBehaviour {

	public float distanceThreshold = 0.5f;

	public BeatRepeat beatRepeatScript;



	// Use this for initialization
	void Start () {

		beatRepeatScript = GetComponent<BeatRepeat> ();

	}
	
	// Update is called once per frame
	void Update () {

		GameObject[] Markers = GameObject.FindGameObjectsWithTag ("MarkerObject");

		RaycastHit hit;


		if (Physics.Raycast (transform.position, transform.forward, out hit, distanceThreshold)) {

			GameObject hitParent = hit.collider.transform.parent.gameObject;

			Debug.Log ("raycasted");

			if (hitParent.transform.parent.GetComponentInChildren<AudioSource>()) {
				
				beatRepeatScript.mySource = hitParent.transform.parent.GetComponentInChildren<AudioSource> ();
				beatRepeatScript.SetClip ();

			}

		}


	}
}
