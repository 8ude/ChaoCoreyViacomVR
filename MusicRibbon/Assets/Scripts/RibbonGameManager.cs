using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RibbonGameManager : MonoBehaviour {

  

	public AudioClip preDrumClip;
	public AudioClip preBassClip;
	public AudioClip preHarmonyClip;
	public AudioClip preMelodyClip;

	public AudioClip[] drumClips;
	public AudioClip[] bassClips;
	public AudioClip[] harmonyClips;
	public AudioClip[] melodyClips;

	public int drumRibbonsDrawn;
	public int bassRibbonsDrawn;
	public int harmonyRibbonsDrawn;
	public int melodyRibbonsDrawn;

	public int drumRibbonMoveTimes;
	public int bassRibbonMoveTimes;
	public int harmonyRibbonMoveTimes;
	public int melodyRibbonMoveTimes;

	[SerializeField] GameObject[] drumRibbons;
	[SerializeField] GameObject[] bassRibbons;
	[SerializeField] GameObject[] melodyRibbons;
	[SerializeField] GameObject[] harmonyRibbons;
	 

    public int totalRibbons;


	public float endingWidth;
	public float endingHeight;

	//public List<GameObject> RibbonObjects;
	//public int maxRibbons = 4;

	public float LimitRibbonAmount;

    //public Material ribbonOffMaterial;

    public static RibbonGameManager instance = null;

    private void Awake() {

		drumClips = Resources.LoadAll <AudioClip>("Audio/Drums");
		bassClips = Resources.LoadAll <AudioClip>("Audio/Bass");
		harmonyClips = Resources.LoadAll <AudioClip>("Audio/Harmony");
		melodyClips = Resources.LoadAll <AudioClip>("Audio/Melody");

		drumRibbonsDrawn = 0;
		bassRibbonsDrawn = 0;
		harmonyRibbonsDrawn = 0;
		melodyRibbonsDrawn = 0;

        if (instance == null) {
            instance = this;

        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {
		drumRibbonMoveTimes = 0;
		bassRibbonMoveTimes = 0;
		melodyRibbonMoveTimes = 0;
		harmonyRibbonMoveTimes = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		drumRibbons = GameObject.FindGameObjectsWithTag ("DrumStem");
        
		//worst volume problems are coming from the bass
		bassRibbons = GameObject.FindGameObjectsWithTag("BassStem");

		foreach (GameObject go in bassRibbons) {

			go.GetComponent<AudioSource> ().volume = (float)1f / bassRibbons.Length;

		}
			
        harmonyRibbons = GameObject.FindGameObjectsWithTag("HarmonyStem");
        melodyRibbons = GameObject.FindGameObjectsWithTag("MelodyStem");

		drumRibbonsDrawn = drumRibbons.Length;
		bassRibbonsDrawn = bassRibbons.Length;
		harmonyRibbonsDrawn = harmonyRibbons.Length;
		melodyRibbonsDrawn = melodyRibbons.Length;

        totalRibbons = drumRibbons.Length + bassRibbons.Length + harmonyRibbons.Length + melodyRibbons.Length;
	
//if (drumRibbonsDrawn > 1 *(drumRibbonMoveTimes+1)){
//	MoveDrumRibbon ();
//	drumRibbonMoveTimes++;
//}

		
	}

	public float RemapRange(float value, float oldMin, float oldMax, float newMin, float newMax) {
		return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
	}
    
    /*
	public void CapRibbons() {

		if (RibbonObjects.Count > maxRibbons) {
			GameObject ribbonToDestroy = RibbonObjects [0];
			RibbonObjects.RemoveAt (0);
			Destroy (ribbonToDestroy);
		}

	}
    */
	public void MoveDrumRibbon(){

		foreach(GameObject drumribbon in drumRibbons) {

			Debug.Log(drumribbon.transform.parent.position);
			
			Vector3 currentPosition = new Vector3 (0, 0, 0);

//	if (drumribbon.transform.position.z > 0f) {
//		currentPosition = new Vector3 (drumribbon.transform.parent.position.x, drumribbon.transform.parent.position.y, drumribbon.transform.parent.position.z + 50f);

//	} else if (drumribbon.transform.position.z <= 0f) {
//		currentPosition = new Vector3 (drumribbon.transform.parent.position.x, drumribbon.transform.parent.position.y, drumribbon.transform.parent.position.z - 50f);
//	}

			drumribbon.transform.parent.position = currentPosition;

		}
	}

	public void MoveBassRibbon(){
		foreach (GameObject bassribbon in bassRibbons) {
			if ( bassribbon.transform.position.z > 0f) {
				bassribbon.transform.position =  bassribbon.transform.position + new Vector3 (0, 0, 1f);
			} else if (bassribbon.transform.position.z <= 0f) {
				bassribbon.transform.position = bassribbon.transform.position + new Vector3 (0, 0, -1f);
			}
		}
		
	}

	public void MoveMelodyRibbon(){
		foreach (GameObject melodyribbon in melodyRibbons) {
			if ( melodyribbon.transform.position.z > 0f) {
				melodyribbon.transform.position = melodyribbon.transform.position + new Vector3 (0, 0, 1f);
			} else if (melodyribbon.transform.position.z <= 0f) {
				melodyribbon.transform.position = melodyribbon.transform.position + new Vector3 (0, 0, -1f);
			}
		}

	}

	public void MoveHarmonyRibbon(){
		foreach (GameObject harmonyribbon in harmonyRibbons) {
			if ( harmonyribbon.transform.position.z > 0f) {
				harmonyribbon.transform.position = harmonyribbon.transform.position + new Vector3 (0, 0, 1f);
			} else if (harmonyribbon.transform.position.z <= 0f) {
				harmonyribbon.transform.position = harmonyribbon.transform.position + new Vector3 (0, 0, -1f);
			}
		}

	}





}
