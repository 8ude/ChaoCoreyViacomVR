using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchStems : MonoBehaviour {

	public int Stemnum = 0;

	public static Color drumColor = new Color((float)177/255, (float)235/255, 0f);
	public static Color bassColor = new Color((float)83/255, (float)187/255, (float)244/255);
	public static Color melodyColor = new Color((float)1, (float)103/255, (float)233/255) ;
	public static Color harmonyColor = new Color((float)1, (float)67/255, (float)46/255) ;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DrawBassRibbon(){
		Stemnum = 0;
		//Debug.Log ("Bass: "+ Stemnum);
	}

	public void DrawDrumRibbon(){
		Stemnum = 1;
		//Debug.Log ("Drum: "+ Stemnum);
	}

	public void DrawHarmonyRibbon(){
		Stemnum = 2;
		//Debug.Log ("Harmony: "+ Stemnum);
	}

	public void DrawMelodyRibbon(){
		Stemnum = 3;
		//Debug.Log ("Melody: "+ Stemnum);
	}

}
