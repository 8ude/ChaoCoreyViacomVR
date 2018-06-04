﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;
using DG.Tweening;

public class EraseRibbon : MonoBehaviour {

    //Attempting to decouple things by eliminating hard references
	public bool isErasing;
	[SerializeField] GameObject LeftHand;
	[SerializeField] GameObject RightHand;
	GameObject LeftSword;
	GameObject RightSword;
	GameObject LeftWand;
	GameObject LeftRubber;
	GameObject RightWand;
	GameObject RightRubber;
    [SerializeField] GameObject LeftColorBall;
    [SerializeField] GameObject RightColorBall;


    [SerializeField] GameObject EraserCubePrefab;

    //todo -- decouple this hard reference
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
        LeftColorBall.SetActive(false);
		LeftRubber.SetActive(true);
	}

	public void DisableLeftRubber(){
        LeftWand.SetActive(true) ;
        LeftColorBall.SetActive(true);
        LeftRubber.SetActive(false);
		
	}

	public void EnableRightRubber(){
		RightWand.SetActive (false);
        RightColorBall.SetActive(false);
        RightRubber.SetActive(true);
	
	}

	public void DiableRightRubber(){
		RightWand.SetActive (true);
        RightColorBall.SetActive(true);
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

        //Seperating out finding L controller and finding R controller
        //better solution will be to not have any references to other hand - need to encapsulate
        LeftHand = GameObject.Find("[CameraRig]/Controller (left)");
        RightHand = GameObject.Find("[CameraRig]/Controller (right)");



        if (LeftHand.GetComponentInChildren<Sword> () != null) {
			LeftSword = LeftHand.GetComponentInChildren<Sword>().gameObject;
			LeftWand = LeftSword.transform.Find ("Wand").gameObject;
			LeftRubber = LeftSword.transform.Find("Capsule").gameObject;
            LeftColorBall = LeftSword.transform.Find("StemIndicator").gameObject;
            
            currentStatus = controllerFoundStatus.LeftFound;
		}

		if (RightHand.GetComponentInChildren<Sword> () != null) {
			RightSword = RightHand.GetComponentInChildren<Sword>().gameObject;
			RightWand = RightSword.transform.Find("Wand").gameObject;
			RightRubber = RightSword.transform.Find("Capsule").gameObject;
            RightColorBall = RightSword.transform.Find("StemIndicator").gameObject;
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

            //edge case and bug where the marker object is the only thing remaining
			if (other.transform.name == "MarkerObject(Clone)") {

				Destroy (other.transform.gameObject);
				drawRibbonScript.markerChain.Clear();
			}


			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {

				GameObject otherParent = other.transform.parent.parent.gameObject;

                if (otherParent.GetComponentInChildren<DrawRibbonSound>() != null) {
                    
                    //Eraser particle follows ribbon to add visual juice

                    Vector3[] splinePoints = otherParent.GetComponentInChildren<DrawRibbonSound>().splinePoints;

                    GameObject eraserCube = Instantiate(EraserCubePrefab, splinePoints[0], Quaternion.identity);
                    eraserCube.GetComponent<EraseRibbonAnim>().EraseRibbon(splinePoints, 1.0f);

                    //fade out ribbon, tween out sound
                    otherParent.GetComponent<RibbonGenerator>().FadeOutRibbon(0.5f);
                    DOTween.To(() => otherParent.GetComponentInChildren<DrawRibbonSound>().currentMaxVolume,
                              x => otherParent.GetComponentInChildren<DrawRibbonSound>().currentMaxVolume = x,
                               0f, 0.6f);

                    RibbonGameManager.instance.ribbonObjects.Remove(otherParent);
                }

				Destroy (other.transform.parent.parent.gameObject, 1.0f);


			}
		}
	}
}
