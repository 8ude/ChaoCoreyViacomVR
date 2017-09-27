using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject LeftColorBall;
    public GameObject RightColorBall;

    // Use this for initialization
    void Start () {

    	drumColor = Colors[0];
    	bassColor = Colors[1];
    	melodyColor = Colors[2];
        harmonyColor = Colors[3];


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FindColorBall() {


        LeftSword = LeftHand.GetComponent<EraseRibbon>().LeftSword;
        RightSword = RightHand.GetComponent<EraseRibbon>().RightSword;
        //need to have null checks, otherwise game will crash every time we lose tracking
        if (LeftSword != null) {
            LeftColorBall = LeftSword.transform.Find("StemIndicator").gameObject;
        } 
        if (RightSword != null) { 
            RightColorBall = RightSword.transform.Find("StemIndicator").gameObject;
        }
    }

    public void ChangeLeftBallColor() {

        FindColorBall();

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

        FindColorBall();

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
