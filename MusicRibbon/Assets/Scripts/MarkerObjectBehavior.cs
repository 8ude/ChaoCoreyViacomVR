using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarkerObjectBehavior : MonoBehaviour {

	public Material DrumRibbonMaterial;
	public Material BassRibbonMaterial;
	public Material MelodyRibbonMaterial;
	public Material HarmonyRibbonMaterial;

	public GameObject DrumMesh;
	public GameObject BassMesh;
	public GameObject MelodyMesh;
	public GameObject HarmonyMesh;


	// Use this for initialization
	void Start () {

		Debug.Log (SceneManager.GetActiveScene().name);

	}

	// Update is called once per frame
	void Update () {
		
	}

	public void DrumMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = DrumRibbonMaterial;
		this.gameObject.GetComponent<MeshFilter> ().mesh = DrumMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;


	}

	public void BassMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = BassRibbonMaterial;
		this.gameObject.GetComponent<MeshFilter> ().mesh = BassMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	}

	public void MelodyMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = MelodyRibbonMaterial;
		this.gameObject.GetComponent<MeshFilter> ().mesh = MelodyMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	
	}

	public void HarmonyMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = HarmonyRibbonMaterial;
		this.gameObject.GetComponent<MeshFilter> ().mesh = HarmonyMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	
	}


}
