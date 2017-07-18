using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Beat;

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
	public GameObject[] bassRibbons;

	public bool[] bassSequencePlaying;

	//private GameObject _bassSequence;
	//public List<GameObject> bassSequence;
	double[] bassSequenceStartTimes;

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

		//bassSequence = new List<double> ();
		//bassSequence [0] = 0;

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
		//WHAT THE FUCK AM I FUCKING DOING I SHOULD JUST FUCKING KILL MYSELF
	
		bassRibbons = GameObject.FindGameObjectsWithTag("BassStem");

		bassSequencePlaying = new bool[bassRibbons.Length];
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

	public void PlayClipSequence () {



			if (bassRibbons.Length > 0) {
				for (int i = 0; i < bassRibbons.Length; i++) {
					AudioSource bassSource = bassRibbons [i].GetComponentInChildren<AudioSource> ();
					bassSequencePlaying [i] = bassSource.isPlaying;

					if (!bassSequencePlaying [i] && bassSequencePlaying [(i - 1) % bassSequencePlaying.Length]) {
						AudioSource lastClip = bassRibbons [i].GetComponentInChildren<AudioSource> ();
					bassSource.PlayScheduled (lastClip.time);
					}


				}
					
			}


	

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
