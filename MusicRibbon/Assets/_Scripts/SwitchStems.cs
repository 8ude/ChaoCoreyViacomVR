using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class SwitchStems : MonoBehaviour {

    public new List<Color> Colors;
    [Tooltip("A list of customized colors")]

    public Color drumColor;
    public Color bassColor;
    public Color melodyColor;
    public Color harmonyColor;

    public int Stemnum = 0;

	public string currentInstrument = "Bass";

	public GameObject LeftHand;
	public GameObject RightHand;
	public GameObject LeftSword;
	public GameObject RightSword;
	public GameObject LeftWand;
	public GameObject LeftRubber;
	public GameObject RightWand;
	public GameObject RightRubber;
	public GameObject LeftColorBall;
	public GameObject RightColorBall;

	public float initTimeDelay = 4f;

	bool bothControllersFound = false;

	public enum controllerFoundStatus {NeitherFound = 0, LeftFound = 1, RightFound = 2, BothFound = 3};

	controllerFoundStatus currentStatus;

    // Use this for initialization
    void Start () {

    	drumColor = Colors[0];
    	bassColor = Colors[1];
    	melodyColor = Colors[2];
        harmonyColor = Colors[3];

		currentStatus = controllerFoundStatus.NeitherFound;
		bothControllersFound = false;


    }
	
	// Update is called once per frame
	void Update () {

		if (Time.time > initTimeDelay && currentStatus != controllerFoundStatus.BothFound) {
			FindController ();
		}
		
	}


	void FindController(){

		//Seperating out finding L controller and finding R controller
		if (LeftHand.GetComponentInChildren<Sword> ()) {
			LeftSword = LeftHand.GetComponentInChildren<Sword>().gameObject;
			LeftWand = LeftSword.transform.Find ("Wand").gameObject;
			LeftRubber = LeftSword.transform.Find("Capsule").gameObject;
			LeftColorBall = LeftSword.transform.Find("StemIndicator").gameObject;
			currentStatus = controllerFoundStatus.LeftFound;
		}

		if (RightHand.GetComponentInChildren<Sword> ()) {
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

    public void ChangeLeftBallColor() {

        Debug.Log("called");

        if(currentInstrument == "Bass") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = bassColor;
        }
        if (currentInstrument == "Drums") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = drumColor;
        }
        if (currentInstrument == "Harmony") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = harmonyColor;
        }
        if (currentInstrument == "Melody") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = melodyColor;
        }
    }

    public void ChangeRightBallColor() {

        Debug.Log("called");

        if (currentInstrument == "Bass") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = bassColor;
        }
        if (currentInstrument == "Drums") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = drumColor;
        }
        if (currentInstrument == "Harmony") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = harmonyColor;
        }
        if (currentInstrument == "Melody") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = melodyColor;
        }


    }


    public void DrawBassRibbon(){
		Stemnum = 0;
		currentInstrument = "Bass";
        //Debug.Log ("Bass: "+ Stemnum);

    }

	public void DrawDrumRibbon(){
		Stemnum = 1;
		currentInstrument = "Drums";
		//Debug.Log ("Drum: "+ Stemnum);
	}

	public void DrawHarmonyRibbon(){
		Stemnum = 2;
		currentInstrument = "Harmony";
		//Debug.Log ("Harmony: "+ Stemnum);
	}

	public void DrawMelodyRibbon(){
		Stemnum = 3;
		currentInstrument = "Melody";
		//Debug.Log ("Melody: "+ Stemnum);
	}

}
