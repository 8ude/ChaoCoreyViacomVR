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

public class RibbonGenerator : MonoBehaviour {
	
	public CurvySpline ribbonSpline;
	public Mesh ribbonMesh;
	public CurvyController ribbonController;

	public CurvyGenerator curvyGenerator;


	// Use this for initialization
	IEnumerator Start () {

		curvyGenerator = GetComponent<CurvyGenerator> ();

		ribbonMesh = GetComponentInChildren<Mesh> ();

		if (!ribbonSpline.IsInitialized) {
			yield return null;
		}

		if (!ribbonMesh) {
			yield return null;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



}
