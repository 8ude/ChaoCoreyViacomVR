using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseRibbon : MonoBehaviour {

	public bool isErasing;
	public GameObject LeftHand;
	public GameObject Righthand;
	public GameObject LeftWand;
	public GameObject LeftRubber;
	public GameObject RightWand;
	public GameObject RightRubber;

	// Use this for initialization
	void Start () {

		isErasing = false;
       // Model / tip / attach / Sword(Clone) /
        LeftWand = LeftHand.transform.Find("Wand").gameObject;
        LeftRubber = LeftHand.transform.Find("Capsule").gameObject;

        RightWand = Righthand.transform.Find("Wand").gameObject;
        RightRubber = Righthand.transform.Find("Capsule").gameObject;


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




	void OnTriggerEnter(Collider other){

		if (isErasing == true) {
		
			if (other.transform.parent.parent.name == "MarkerParent(Clone)") {
			
				Destroy (other.transform.parent.parent.gameObject);
			}
		}
	}
}
