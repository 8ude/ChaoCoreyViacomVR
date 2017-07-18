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

	[SerializeField] GameObject[] drumRibbons;
	[SerializeField] GameObject[] bassRibbons;
	[SerializeField] GameObject[] melodyRibbons;
	[SerializeField] GameObject[] harmonyRibbons;

    public int totalRibbons;


	public float endingWidth;
	public float endingHeight;

	//public List<GameObject> RibbonObjects;
	//public int maxRibbons = 4;

	public bool LimitRibbonAmount = false;

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
		
	}
	
	// Update is called once per frame
	void Update () {

        drumRibbons = GameObject.FindGameObjectsWithTag("DrumStem");
        
		//worst volume problems are coming from the bass
		bassRibbons = GameObject.FindGameObjectsWithTag("BassStem");
		foreach (GameObject go in bassRibbons) {

			go.GetComponent<AudioSource> ().volume = (float)1f / bassRibbons.Length;

		}



        harmonyRibbons = GameObject.FindGameObjectsWithTag("HarmonyStem");
        melodyRibbons = GameObject.FindGameObjectsWithTag("MelodyStem");

        totalRibbons = drumRibbons.Length + bassRibbons.Length + harmonyRibbons.Length + melodyRibbons.Length;

        if (LimitRibbonAmount) {
			//CapRibbons ();
		}


		
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

}
