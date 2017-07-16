using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerObjectBehavior : MonoBehaviour {

	public Material DrumRibbonMaterial;
	public Material BassRibbonMaterial;
	public Material MelodyRibbonMaterial;
	public Material HarmonyRibbonMaterial;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void DrumMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = DrumRibbonMaterial;

	}

	public void BassMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = BassRibbonMaterial;
	}

	public void MelodyRibbonObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = MelodyRibbonMaterial;
	
	}

	public void HarmonyRibbonObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = HarmonyRibbonMaterial;
	
	}


}
