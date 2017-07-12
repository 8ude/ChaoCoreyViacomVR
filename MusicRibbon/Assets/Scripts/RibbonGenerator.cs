using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using FluffyUnderware.Curvy.Controllers;
using FluffyUnderware.Curvy.Shapes;
using FluffyUnderware.DevTools;
using UnityEngine.UI;
/// <summary>
/// Ideally this will be used to store references to the the
/// spline and meshes of a particular ribbon, after it is drawn and initialized
/// </summary>
public class RibbonGenerator : MonoBehaviour {
	
	public CurvySpline ribbonSpline;
	public MeshFilter ribbonMesh;
	public MeshRenderer ribbonRenderer;
	//public SplineController ribbonController;

	public CurvyGenerator curvyGenerator;
	public DrawRibbonSound drawRibbonSound;

	public enum musicStem {Bass = 0, Drum = 1, Harmony = 2, Melody = 3};
	public musicStem myStem;
	public int stemIntValue;

    public GameObject SwitchStems;

	Color myColor = new Color(1,1,1);


	// Use this for initialization
	IEnumerator Start () {

        SwitchStems = GameObject.Find("SwitchStems");

		myStem = (musicStem)stemIntValue;

		curvyGenerator = GetComponent<CurvyGenerator> ();

		ribbonMesh = GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshFilter> ();


		if (!ribbonSpline.IsInitialized) {
			yield return null;
		}

		//Set Spline Controller Values Here
		//...except they don't work
		//ribbonController = drawRibbonSound.gameObject.GetComponent<SplineController> ();
		
		//ribbonController.Spline = ribbonSpline;
		
		StartCoroutine (WaitForMeshRenderer());

		
	}
	
	// Update is called once per frame
	void Update () {



	}

	IEnumerator WaitForMeshRenderer() {

        Debug.Log("waiting");

		yield return new WaitForSeconds (0.5f);

        Debug.Log("done waiting");


        if (GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>()) {
            ribbonRenderer = GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>();
        } else {
            yield return null;
        }

		if (ribbonRenderer == null) {
			yield return null;
		}

		//Set Mesh Material Values Here

		switch (myStem) {
		case musicStem.Bass:
                myColor = SwitchStems.GetComponent<SwitchStems>().bassColor;
			break;
		case musicStem.Drum:
			myColor = SwitchStems.GetComponent<SwitchStems>().drumColor;
			break;
		case musicStem.Melody:
			myColor = SwitchStems.GetComponent<SwitchStems>().melodyColor;
			break;
		case musicStem.Harmony:
			myColor = SwitchStems.GetComponent<SwitchStems>().harmonyColor;
			break;
		}

		ribbonRenderer.material.color = myColor;
	}



}
