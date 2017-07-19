using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

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

	public int RibbonMoveTimes;

	[SerializeField] GameObject[] drumRibbons;
	[SerializeField] GameObject[] bassRibbons;
	[SerializeField] GameObject[] melodyRibbons;
	[SerializeField] GameObject[] harmonyRibbons;
	[SerializeField] GameObject[] Ribbons;
	 

    public int totalRibbons;

	public GameObject[] ribbonObjects;

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
		Ribbons = GameObject.FindGameObjectsWithTag("SplinePrefab");

		ribbonObjects = GameObject.FindGameObjectsWithTag ("MarkerParent");

		/*
		if (ribbonObjects.Length > 0) {
			MoveRibbons ();
		}
		*/

        totalRibbons = drumRibbons.Length + bassRibbons.Length + harmonyRibbons.Length + melodyRibbons.Length;
	
		if (Ribbons.Length > LimitRibbonAmount * RibbonMoveTimes){
			Debug.Log (LimitRibbonAmount * RibbonMoveTimes);
			MoveRibbons ();
			RibbonMoveTimes++;
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
	public void MoveRibbons(){
		
		//This first method of moving ribbons works
		foreach (GameObject ribbonParent in ribbonObjects) {


			//using the sound prefab, because the other objects in the ribbon seem to have weird positions
			Vector3 direction = ribbonParent.GetComponentInChildren<DrawRibbonSound>().transform.position - Camera.main.transform.position;
			//adding an offset to compensate for the position of the transform (not sure why it is offset to begin with)
			//direction.y += 1f;

			direction.Normalize ();

			Vector3 endingPosition = ribbonParent.transform.position + (direction * 1.5f);

			ribbonParent.transform.DOMove(endingPosition, 15f);

		}

		foreach (GameObject ribbon in Ribbons) {

			int childnum = ribbon.transform.childCount;
//
//			GameObject[] ribbonchildren= new GameObject[100];
//
//			if (childnum > 0) {
//
//				for (int i = 0; i <= childnum; i++) {
//
//					ribbonchildren[i] = ribbon.transform.GetChild(0).gameObject;
//					Debug.Log (ribbonchildren[i].gameObject.name);
//				}
//			}
//

			
			}
				

		
//	if (drumribbon.transform.position.z > 0f) {
//		currentPosition = new Vector3 (drumribbon.transform.parent.position.x, drumribbon.transform.parent.position.y, drumribbon.transform.parent.position.z + 50f);

//	} else if (drumribbon.transform.position.z <= 0f) {
//		currentPosition = new Vector3 (drumribbon.transform.parent.position.x, drumribbon.transform.parent.position.y, drumribbon.transform.parent.position.z - 50f);
//	}

	}







}
