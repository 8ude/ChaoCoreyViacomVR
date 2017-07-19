using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class EraseRibbon : MonoBehaviour {

	public bool isErasing;
	public GameObject LeftHand;
	public GameObject RightHand;
	public GameObject LeftSword;
	public GameObject RightSword;
	public GameObject LeftWand;
	public GameObject LeftRubber;
	public GameObject RightWand;
	public GameObject RightRubber;

	public GameObject EraserCubePrefab;
	public DrawRibbon drawRibbonScript;


	public float initTimeDelay = 4f;

	bool bothControllersFound = false;

	public enum controllerFoundStatus {NeitherFound = 0, LeftFound = 1, RightFound = 2, BothFound = 3};

	controllerFoundStatus currentStatus;

	// Use this for initialization
	void Start () {

		isErasing = false;
		currentStatus = controllerFoundStatus.NeitherFound;
		bothControllersFound = false;


		//StartCoroutine ("WaitForController");
		//Debug.Log ("Started");
		//drawRibbonScript = GetComponent<DrawRibbon> ();


    }
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Time.time);
		if (Time.time > initTimeDelay && currentStatus != controllerFoundStatus.BothFound) {
			FindController ();
		}

		drawRibbonScript.enabled = !isErasing;

    }

	public void EnableErasing(){
		isErasing = true;
		drawRibbonScript.enabled = false;
	}

	public void DisableErasing(){
		isErasing = false;
		drawRibbonScript.enabled = true;
	}

	public void EnableLeftRubber(){
		
		LeftWand.SetActive(false);
		LeftRubber.SetActive(true);
	}

	public void DisableLeftRubber(){
        LeftWand.SetActive(true) ;
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

	/*
	IEnumerator WaitForController(){

		//Debug.Log ("Why aren't you working");

		yield return new WaitForSeconds(3f);

		Debug.Log ("wait over");

		LeftSword = LeftHand.GetComponentInChildren<Sword>().gameObject;
		RightSword = RightHand.GetComponentInChildren<Sword>().gameObject;

		LeftWand = LeftSword.transform.Find ("Wand").gameObject;
		LeftRubber = LeftSword.transform.Find("Capsule").gameObject;
		Debug.Log ("found object: " + LeftRubber.name);

		RightWand = RightSword.transform.Find("Wand").gameObject;
		RightRubber = RightSword.transform.Find("Capsule").gameObject;
	
	}
	*/

	void FindController(){

		//Debug.Log ("Why aren't you working");

		Debug.Log ("searching for controllers");

		//Seperating out finding L controller and finding R controller

		if (LeftHand.GetComponentInChildren<Sword> ()) {
			LeftSword = LeftHand.GetComponentInChildren<Sword>().gameObject;
			LeftWand = LeftSword.transform.Find ("Wand").gameObject;
			LeftRubber = LeftSword.transform.Find("Capsule").gameObject;
			Debug.Log ("found object: " + LeftRubber.name);
			//change our status if we've found this particular component
			currentStatus = controllerFoundStatus.LeftFound;
		}

		if (RightHand.GetComponentInChildren<Sword> ()) {
			RightSword = RightHand.GetComponentInChildren<Sword>().gameObject;
			RightWand = RightSword.transform.Find("Wand").gameObject;
			RightRubber = RightSword.transform.Find("Capsule").gameObject;
			if (currentStatus == controllerFoundStatus.LeftFound) {
				//if we've already found the other controller, set current status accordingly and stop searching
				currentStatus = controllerFoundStatus.BothFound;
			} else
				currentStatus = controllerFoundStatus.RightFound;
		}





		if (currentStatus == controllerFoundStatus.BothFound) {
			bothControllersFound = true;
			Debug.Log ("both controllers found");
		}

	}




	void OnTriggerEnter(Collider other){

		if (isErasing == true) {

			if (other.transform.name == "MarkerObject(Clone)") {

				Destroy (other.transform.gameObject);
				drawRibbonScript.markerChain.Clear();
			}

			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {


				//Want to add juice by having an "eraser" particle follow the ribbon...
				//the sound follower object already has a reference to the points
				Vector3[] splinePoints = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound> ().splinePoints;

				GameObject eraserCube = Instantiate (EraserCubePrefab, splinePoints [0], Quaternion.identity);
				eraserCube.GetComponent<EraseRibbonAnim> ().EraseRibbon (splinePoints, 1.0f);
			
				Destroy (other.transform.parent.parent.gameObject, 1.0f);


			}



		}
	}
}
