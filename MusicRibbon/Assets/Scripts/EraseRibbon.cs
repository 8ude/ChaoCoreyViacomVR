using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseRibbon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (other.transform.parent.parent.name);
		if (other.transform.parent.parent.name == "MarkerParent(clone)") {
			Destroy (other.transform.parent.parent);
		}
	}
}
