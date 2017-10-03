using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarkerObjectBehavior : MonoBehaviour {

	public new List<Color> OriginalColors;
	public new List<Color> MountainColors;
	public new List<Color> DesertColors;
	public new List<Color> SnowColors;
	public new List<Color> SeaColors;

	public Color drumColor;
	public Color bassColor;
	public Color melodyColor;
	public Color harmonyColor;


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

		if(SceneManager.GetActiveScene().name == "Echo"){

		drumColor = OriginalColors[0];
		bassColor = OriginalColors[1];
		melodyColor = OriginalColors[2];
		harmonyColor = OriginalColors[3];

		}

		if(SceneManager.GetActiveScene().name == "Mountain"){

			drumColor = MountainColors[0];
			bassColor = MountainColors[1];
			melodyColor = MountainColors[2];
			harmonyColor = MountainColors[3];

		}

		if(SceneManager.GetActiveScene().name == "Desert"){

			drumColor = DesertColors[0];
			bassColor = DesertColors[1];
			melodyColor = DesertColors[2];
			harmonyColor = DesertColors[3];

		}

		if(SceneManager.GetActiveScene().name == "Snow"){

			drumColor = SnowColors[0];
			bassColor = SnowColors[1];
			melodyColor = SnowColors[2];
			harmonyColor = SnowColors[3];

		}

		if(SceneManager.GetActiveScene().name == "Sea"){

			drumColor = SeaColors[0];
			bassColor = SeaColors[1];
			melodyColor = SeaColors[2];
			harmonyColor = SeaColors[3];

		}


		
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void DrumMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = DrumRibbonMaterial;
		this.gameObject.GetComponent<MeshRenderer> ().material.color = drumColor;
		this.gameObject.GetComponent<MeshFilter> ().mesh = DrumMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;


	}

	public void BassMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = BassRibbonMaterial;
		this.gameObject.GetComponent<MeshRenderer> ().material.color = bassColor;
		this.gameObject.GetComponent<MeshFilter> ().mesh = BassMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	}

	public void MelodyMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = MelodyRibbonMaterial;
		this.gameObject.GetComponent<MeshRenderer> ().material.color = melodyColor;
		this.gameObject.GetComponent<MeshFilter> ().mesh = MelodyMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	
	}

	public void HarmonyMarkerObject(){
		
		this.gameObject.GetComponent<MeshRenderer> ().material = HarmonyRibbonMaterial;
		this.gameObject.GetComponent<MeshRenderer> ().material.color = harmonyColor;
		this.gameObject.GetComponent<MeshFilter> ().mesh = HarmonyMesh.gameObject.GetComponent<MeshFilter> ().sharedMesh;
		this.gameObject.transform.Rotate (-90f, 0f, 0f);
	
	}


}
