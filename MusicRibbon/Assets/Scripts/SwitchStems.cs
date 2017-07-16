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

	public string currentInstrument;


    //public static Color drumColor = new Color((float)177/255, (float)235/255, 0f);
    //public static Color bassColor = new Color((float)83/255, (float)187/255, (float)244/255);
    //public static Color melodyColor = new Color((float)1, (float)103/255, (float)233/255) ;
    //public static Color harmonyColor = new Color((float)1, (float)67/255, (float)46/255) ;


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
