using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseRibbon : MonoBehaviour {

	public bool isErasing;
	public GameObject LeftController;
	public GameObject RightController;
	public GameObject LeftWand;
	public GameObject LeftRubber;
	public GameObject RightWand;
	public GameObject RightRubber;

	// Use this for initialization
	void Start () {

		isErasing = false;

		LeftController = GameObject.Find ("Controller(left)");
		RightController = GameObject.Find ("Controller(right)");

		LeftWand = LeftController.transform.Find ("Wand").gameObject;
		LeftRubber = LeftController.transform.Find ("Capsule").gameObject;

		RightWand = RightController.transform.Find ("Wand").gameObject;
		RightRubber = RightController.transform.Find ("Capsule").gameObject;
		
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

	public void EnableLeftRubber(){
		
		LeftWand.SetActive(false);
		LeftRubber.SetActive(true);
	}

	public void DisableLeftRubber(){
		LeftWand.SetActive(true);
		LeftRubber.SetActive(false);
		
	}

	public void EnableRightRubber(){
		RightWand.SetActive (false);
		RightRubber.SetActive(true);
	
	}

	public void DiableRightRubber(){
		RightWand.SetActive (true);
		RightRubber.SetActive(false);
	
	}




	void OnTriggerEnter(Collider other){

		if (isErasing == true) {
		
			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {
			
				Destroy (other.transform.parent.parent.gameObject);
			}
		}
	}
}
