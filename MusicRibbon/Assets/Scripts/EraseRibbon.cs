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

	public DrawRibbon drawRibbonScript;

	// Use this for initialization
	void Start () {

		isErasing = false;

		StartCoroutine (WaitForController());

		//drawRibbonScript = GetComponent<DrawRibbon> ();


    }
	
	// Update is called once per frame
	void Update () {

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

	IEnumerator WaitForController(){

		yield return new WaitForSeconds(120);

		LeftSword = LeftHand.GetComponentInChildren<Sword>().gameObject;
		RightSword = RightHand.GetComponentInChildren<Sword>().gameObject;

		LeftWand = LeftSword.transform.Find ("Wand").gameObject;
		LeftRubber = LeftSword.transform.Find("Capsule").gameObject;

		RightWand = RightSword.transform.Find("Wand").gameObject;
		RightRubber = RightSword.transform.Find("Capsule").gameObject;
	
	}




	void OnTriggerEnter(Collider other){

		if (isErasing == true) {

			//drawRibbonScript.enabled = false;

			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {
			
				Destroy (other.transform.parent.parent.gameObject);
			}
		}
	}
}
