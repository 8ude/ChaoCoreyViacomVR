using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonGameManager : MonoBehaviour {

    //We don't need this quite yet, but probably will in the near future

	public AudioClip preDrumClip;
	public AudioClip preBassClip;
	public AudioClip preHarmonyClip;
	public AudioClip preMelodyClip;

	public AudioClip[] drumClips;
	public AudioClip[] bassClips;
	public AudioClip[] harmonyClips;
	public AudioClip[] melodyClips;

	public int drumRibbonCount;
	public int bassRibbonCount;
	public int harmonyRibbonCount;
	public int melodyRibbonCount;

	[SerializeField] GameObject[] drumRibbons;
	[SerializeField] GameObject[] bassRibbons;
	[SerializeField] GameObject[] melodyRibbons;
	[SerializeField] GameObject[] harmonyRibbons;


	public float endingWidth;
	public float endingHeight;

	public List<GameObject> RibbonObjects;
	public int maxRibbons = 4;

	public bool LimitRibbonAmount = false;

    //public Material ribbonOffMaterial;

    public static RibbonGameManager instance = null;

    private void Awake() {

		drumClips = Resources.LoadAll <AudioClip>("Audio/Drums");
		bassClips = Resources.LoadAll <AudioClip>("Audio/Bass");
		harmonyClips = Resources.LoadAll <AudioClip>("Audio/Harmony");
		melodyClips = Resources.LoadAll <AudioClip>("Audio/Melody");

		drumRibbonCount = 0;
		bassRibbonCount = 0;
		harmonyRibbonCount = 0;
		melodyRibbonCount = 0;

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

		if (LimitRibbonAmount) {
			CapRibbons ();
		}


		
	}

	public float RemapRange(float value, float oldMin, float oldMax, float newMin, float newMax) {
		return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
	}

	public void CapRibbons() {

		if (RibbonObjects.Count > maxRibbons) {
			GameObject ribbonToDestroy = RibbonObjects [0];
			RibbonObjects.RemoveAt (0);
			Destroy (ribbonToDestroy);
		}

	}

}
