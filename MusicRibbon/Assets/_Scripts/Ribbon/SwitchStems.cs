using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Examples;

public class SwitchStems : MonoBehaviour {

    //Update 4 June - decoupling erasing and instrument switching dependencies
    //Issues arising because VRTK Prefabs and Steam VR prefabs are performing different (but similar)
    //functionality and therefore have far too many hardcoded references


    public new List<Color> Colors;
    [Tooltip("A list of customized colors")]

    public Color drumColor;
    public Color bassColor;
    public Color melodyColor;
    public Color harmonyColor;

	public Material DrumRibbonMaterial;
	public Material BassRibbonMaterial;
	public Material MelodyRibbonMaterial;
	public Material HarmonyRibbonMaterial;


    public int Stemnum;

	public string currentInstrument;

	public GameObject LeftHand;
	public GameObject RightHand;
	GameObject LeftSword;
	GameObject RightSword;
	GameObject LeftWand;
	GameObject LeftRubber;
	GameObject RightWand;
	GameObject RightRubber;
	GameObject LeftColorBall;
	GameObject RightColorBall;

	public float initTimeDelay = 4f;

	bool bothControllersFound = false;


    //temp solution to current race condition with controllers
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
        //todo - "onehanded mode" - eliminate hard references to both hands
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

        if(currentInstrument == "Bass") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.bassColor;
			LeftColorBall.GetComponent<MeshRenderer> ().material = BassRibbonMaterial;
        }
        if (currentInstrument == "Drums") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.drumColor;
			LeftColorBall.GetComponent<MeshRenderer> ().material = DrumRibbonMaterial;
        }
        if (currentInstrument == "Harmony") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.harmonyColor;
			LeftColorBall.GetComponent<MeshRenderer> ().material = HarmonyRibbonMaterial;
        }
        if (currentInstrument == "Melody") {
            LeftColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.melodyColor;
			LeftColorBall.GetComponent<MeshRenderer> ().material = MelodyRibbonMaterial;
        }
    }

    public void ChangeRightBallColor() {


        if (currentInstrument == "Bass") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.bassColor;
			RightColorBall.GetComponent<MeshRenderer> ().material = BassRibbonMaterial;
        }
        if (currentInstrument == "Drums") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.drumColor;
			RightColorBall.GetComponent<MeshRenderer> ().material = DrumRibbonMaterial;
        }
        if (currentInstrument == "Harmony") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.harmonyColor;
			RightColorBall.GetComponent<MeshRenderer> ().material = HarmonyRibbonMaterial;
        }
        if (currentInstrument == "Melody") {
            RightColorBall.GetComponent<MeshRenderer>().material.color = RibbonGameManager.instance.melodyColor;
			RightColorBall.GetComponent<MeshRenderer> ().material = MelodyRibbonMaterial;
        }


    }

    //Beginning to refactor - eventually want to move to a more modular system that allows for additional (or replacement) instruments
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
