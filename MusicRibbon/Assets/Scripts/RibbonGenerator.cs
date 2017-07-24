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

using DG.Tweening;
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

    public GameObject switchStems;

	public float lifeTime;
	float fadeoutTime;

	public float ribbonLength;
	

	//float endWidth = 0.1f;
	//float endHeight = 0.5f;

	public Color myColor = new Color(1,1,1);

	public float transparency = 0f;


	// Use this for initialization
	IEnumerator Start () {
		lifeTime = 0f;

		fadeoutTime = RibbonGameManager.instance.autoKillFadeOutTime;

        //switchStems = GameObject.Find("SwitchStems");

		myStem = (musicStem)stemIntValue;

		curvyGenerator = GetComponent<CurvyGenerator> ();
		ribbonRenderer = null;




		while (!ribbonSpline.IsInitialized) {
			yield return null;
		}

		drawRibbonSound = GetComponentInChildren<DrawRibbonSound> ();

		//Set Spline Controller Values Here
		//...except they don't work
		//ribbonController = drawRibbonSound.gameObject.GetComponent<SplineController> ();
		
		//ribbonController.Spline = ribbonSpline;
		
		StartCoroutine (WaitForMeshRenderer());

		
	}
	
	// Update is called once per frame
	void Update () {

		lifeTime += Time.deltaTime;

		if (lifeTime > RibbonGameManager.instance.autoKillLifetime  && RibbonGameManager.instance.autoKillRibbons) {

			FadeOutRibbon (fadeoutTime);
			Destroy (gameObject, fadeoutTime);

		}

		if (ribbonRenderer) {
			
			ribbonRenderer.material.SetFloat ("_InputAlpha", transparency);
		}


	}

	IEnumerator WaitForMeshRenderer() {

        //Debug.Log("waiting");

		//yield return new WaitForSeconds (0.5f);

        //Debug.Log("done waiting");

        while (GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>() == null)   {
            yield return null;
        }

        if (GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>()) {
			
            ribbonRenderer = GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>();
			ribbonMesh = GetComponentInChildren<CreateMesh> ().transform.GetComponentInChildren<MeshFilter> ();
			Mesh mesh = ribbonMesh.mesh;
			MeshHelper.Subdivide (mesh, 8);
			ribbonMesh.mesh = mesh;
        } 

		while (ribbonRenderer == null) {
			Debug.Log ("I should be fucking yielding");
			yield return null;
		}

		//Debug.Log (ribbonRenderer.name);

		//Set Mesh Material Values Here

		switch (myStem) {
		case musicStem.Bass:
            myColor = switchStems.GetComponent<SwitchStems>().bassColor;
			break;
		case musicStem.Drum:
			myColor = switchStems.GetComponent<SwitchStems>().drumColor;
			break;
		case musicStem.Melody:
			myColor = switchStems.GetComponent<SwitchStems>().melodyColor;
			break;
		case musicStem.Harmony:
			myColor = switchStems.GetComponent<SwitchStems>().harmonyColor;
			break;
		}

		ribbonRenderer.material.color = myColor;
		DOTween.To (() => transparency, x => transparency = x, 1, 1);

        MarkerObjectBehavior[] markerObjects = GetComponentsInChildren<MarkerObjectBehavior>();
        foreach(MarkerObjectBehavior markerBehavior in markerObjects) {

            markerBehavior.gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

	}

	public void FadeOutRibbon(float time) {
		DOTween.To (() => transparency, x => transparency = x, 0, time);
		drawRibbonSound.mySource.DOFade (0f, fadeoutTime);
	}




}
