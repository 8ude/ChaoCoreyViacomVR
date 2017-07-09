using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseRibbon : MonoBehaviour {

	public bool isErasing;

	// Use this for initialization
	void Start () {

		isErasing = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EnableErasing(){
		isErasing = true;
	}

	public void DisableErasing(){
		isErasing = false;
	}

	void OnTriggerEnter(Collider other){

		if (isErasing == true) {
		
			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {
			
				Destroy (other.transform.parent.parent.gameObject);
			}
		}
	}
}
