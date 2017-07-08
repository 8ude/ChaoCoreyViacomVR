using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStems : MonoBehaviour {

	public int Stemnum;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DrawBassRibbon(){
		Stemnum = 0;
		Debug.Log ("Bass: "+ Stemnum);
	}

	public void DrawDrumRibbon(){
		Stemnum = 1;
		Debug.Log ("Drum: "+ Stemnum);
	}

	public void DrawHarmonyRibbon(){
		Stemnum = 2;
		Debug.Log ("Harmony: "+ Stemnum);
	}

	public void DrawMelodyRibbon(){
		Stemnum = 3;
		Debug.Log ("Melody: "+ Stemnum);
	}

}
