using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class RibbonGameManager : MonoBehaviour {

  

	public AudioClip preDrumClip;
	public AudioClip preBassClip;
	public AudioClip preHarmonyClip;
	public AudioClip preMelodyClip;

	public AudioClip[] drumCollisionClips;
	public AudioClip[] bassCollisionClips;
	public AudioClip[] harmonyCollisionClips;
	public AudioClip[] melodyCollisionClips;

	public AudioClip[] drumClips;
	public AudioClip[] bassClips;
	public AudioClip[] harmonyClips;
	public AudioClip[] melodyClips;

    [Space(20)]
    public AudioMixerSnapshot audioOutSnapshot, audioInSnapshot;
    public float gameFadeTime;

    [Space(20)]
	public GameObject particlePrefab;
	[Space(20)]



	[HideInInspector]
	public int drumRibbonsDrawn, bassRibbonsDrawn, harmonyRibbonsDrawn, melodyRibbonsDrawn;

	public int ribbonMoveTimes;

	[Space(20)]

	public GameObject[] drumRibbons;
	public GameObject[] bassRibbons;
    public GameObject[] melodyRibbons;
    public GameObject[] harmonyRibbons;
<<<<<<< HEAD
    //public GameObject[] Ribbons;


=======
    public GameObject[] Ribbons;
>>>>>>> origin/NewInteractions
	 

    public int totalRibbons;

<<<<<<< HEAD
    //ribbonObjects refers to the Marker Parent, which is the root (top parent)
    //of each ribbon's hierarchy 
	//public GameObject[] ribbonObjects;
=======
	public GameObject[] ribbonObjects;
>>>>>>> origin/NewInteractions

	[Space(20)]

	public float endingWidth;
	public float endingHeight;

    public List<GameObject> ribbonObjects;
    //public int maxRibbons = 4;
    public float ribbonMoveDistance;
	public float limitRibbonAmount;

    //public Material ribbonOffMaterial;

    public static RibbonGameManager instance = null;

	public bool autoKillRibbons;
	public float autoKillLifetime;
	public float autoKillFadeOutTime;

	public float maxDistanceBeforeFade;

    private void Awake() {

		drumClips = Resources.LoadAll <AudioClip>("Audio/Drums");
		bassClips = Resources.LoadAll <AudioClip>("Audio/Bass");
		harmonyClips = Resources.LoadAll <AudioClip>("Audio/Harmony");
		melodyClips = Resources.LoadAll <AudioClip>("Audio/Melody");

		drumCollisionClips = Resources.LoadAll<AudioClip>("Audio/DrumCollision");
		bassCollisionClips = Resources.LoadAll<AudioClip>("Audio/BassCollision");
		harmonyCollisionClips = Resources.LoadAll<AudioClip>("Audio/HarmonyCollision");
		melodyCollisionClips = Resources.LoadAll<AudioClip>("Audio/MelodyCollision");


		drumRibbonsDrawn = 0;
		bassRibbonsDrawn = 0;
		harmonyRibbonsDrawn = 0;
		melodyRibbonsDrawn = 0;

        ribbonObjects = new List<GameObject>();

        if (instance == null) {
            instance = this;

        } else {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {
        FadeFromWhite();
	}
	
	// Update is called once per frame
	void Update () {

        /////////////////////////////////////////////////////////////
        //populate arrays with each stem type, then adjust the volumes of each stem according
        //to how many stems of that type currently exist
		drumRibbons = GameObject.FindGameObjectsWithTag ("DrumStem");

		foreach (GameObject go in drumRibbons) {

            if (go.transform.root.GetComponent<RibbonGenerator>() && !go.transform.root.GetComponent<RibbonGenerator>().fadingOut) {

                go.GetComponent<AudioSource>().volume = Mathf.Sqrt((float)1f / drumRibbons.Length);

                if (Vector3.Distance(Camera.main.transform.position, go.transform.position) > maxDistanceBeforeFade)
                {
                    go.transform.root.GetComponent<RibbonGenerator>().FadeOutRibbon(10f);
                }
            }

		}
        
		//worst volume problems are coming from the bass
		bassRibbons = GameObject.FindGameObjectsWithTag("BassStem");

		foreach (GameObject go in bassRibbons) {
            if (go.transform.root.GetComponent<RibbonGenerator>() && !go.transform.root.GetComponent<RibbonGenerator>().fadingOut) {
                go.GetComponent<AudioSource>().volume = Mathf.Sqrt((float)0.7f / bassRibbons.Length);
                if (Vector3.Distance(Camera.main.transform.position, go.transform.position) > maxDistanceBeforeFade)
                {
                    go.transform.root.GetComponent<RibbonGenerator>().FadeOutRibbon(10f);
                }
            }

		}
			
        harmonyRibbons = GameObject.FindGameObjectsWithTag("HarmonyStem");
		foreach (GameObject go in harmonyRibbons) {
                if (go.transform.root.GetComponent<RibbonGenerator>() && !go.transform.root.GetComponent<RibbonGenerator>().fadingOut) {

                    go.GetComponent<AudioSource>().volume = Mathf.Sqrt((float)1f / harmonyRibbons.Length);
                    if (Vector3.Distance(Camera.main.transform.position, go.transform.position) > maxDistanceBeforeFade)
                    {
                        go.transform.root.GetComponent<RibbonGenerator>().FadeOutRibbon(10f);
                    }
                }
		
		}

        melodyRibbons = GameObject.FindGameObjectsWithTag("MelodyStem");
        foreach (GameObject go in melodyRibbons) {
            if (go.transform.root.GetComponent<RibbonGenerator>() && !go.transform.root.GetComponent<RibbonGenerator>().fadingOut) { 

                go.GetComponent<AudioSource>().volume = Mathf.Sqrt((float)1f / melodyRibbons.Length);
                if (Vector3.Distance(Camera.main.transform.position, go.transform.position) > maxDistanceBeforeFade) {
                    go.transform.root.GetComponent<RibbonGenerator>().FadeOutRibbon(10f);
                }
            }

		}

        //Ribbons = GameObject.FindGameObjectsWithTag("SplinePrefab");



<<<<<<< HEAD
=======
		/*
		if (ribbonObjects.Length > 0) {
			MoveRibbons ();
		}
		*/
>>>>>>> origin/NewInteractions


        totalRibbons = ribbonObjects.Count;


<<<<<<< HEAD
        if (ribbonObjects.Count > limitRibbonAmount * ribbonMoveTimes && autoMoveRibbons){
=======
		if (Ribbons.Length > limitRibbonAmount * ribbonMoveTimes){
			//Debug.Log (LimitRibbonAmount * RibbonMoveTimes);
>>>>>>> origin/NewInteractions
			MoveRibbons ();
			ribbonMoveTimes++;
		}

		MoveSmallRibbons ();

        if (Input.GetKeyDown(KeyCode.R)) {
            FadeToWhite();
            Invoke("ResetGame", gameFadeTime);
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
			//direction.y *= Mathf.Sin (Time.time);
			float clampedRibbonLength = Mathf.Clamp (ribbonParent.GetComponent<RibbonGenerator> ().ribbonLength, 0f, 20f);

			float ribbonMoveAmount = ribbonMoveDistance * RemapRange (clampedRibbonLength, 0f, 20f, 2f, 0.1f);
			Vector3 endingPosition = ribbonParent.transform.position + (direction * ribbonMoveAmount);

			StartCoroutine(MoveAlongSineCurve(ribbonParent, direction, 8f));

			//ribbonParent.transform.Translate(direction * Time.deltaTime);

			//ribbonParent.transform.DOMove(endingPosition, 15f);

		}
			

	}

	void MoveSmallRibbons() {

		foreach (GameObject ribbonParent in ribbonObjects) {

			if (ribbonParent.GetComponent<RibbonGenerator> ().ribbonLength < 3.0f) {

				//using the sound prefab, because the other objects in the ribbon seem to have weird positions
				Vector3 direction = ribbonParent.GetComponentInChildren<DrawRibbonSound>().transform.position - Camera.main.transform.position;
				//adding an offset to compensate for the position of the transform (not sure why it is offset to begin with)
				//direction.y += 1f;

				direction.Normalize ();
				direction.y *= Mathf.Sin (Time.time);
				float clampedRibbonLength = Mathf.Clamp (ribbonParent.GetComponent<RibbonGenerator> ().ribbonLength, 0f, 20f);

				float ribbonMoveAmount = ribbonMoveDistance * RemapRange (clampedRibbonLength, 0f, 20f, 2f, 0.1f);
	

				//ribbonParent.GetComponent<RibbonGenerator> ().FadeOutRibbon (30f);
				ribbonParent.transform.Translate(direction * Time.deltaTime * 0.1f);
				if (ribbonParent.GetComponent<RibbonGenerator> ().lifeTime >= 1.0) {
					ribbonParent.GetComponent<RibbonGenerator> ().FadeOutRibbon (15f);
				}

			}

		}

	}
		
	IEnumerator MoveAlongSineCurve(GameObject go, Vector3 direction, float timeToComplete) {
		float timeElapsed = 0;

		while (timeElapsed < timeToComplete && go != null) {

			Vector3 newDirection = direction;
			newDirection.y *= Mathf.Cos (Time.time) * 0.4f;
			 
			go.transform.Translate (newDirection * (Mathf.Abs(0.5f * Mathf.Cos(Time.time)) + 0.5f) * (Time.deltaTime / 5f) 
				* ((timeToComplete - timeElapsed)/timeToComplete));
			timeElapsed += Time.deltaTime;
			yield return null;

		}

	}

    void ResetGame() {

        SceneManager.LoadScene(0);
    }

    void FadeFromWhite() {
        SteamVR_Fade.Start(Color.white, 0f);

        SteamVR_Fade.Start(Color.clear, gameFadeTime);
        audioInSnapshot.TransitionTo(gameFadeTime);
    }

    void FadeToWhite() {
        SteamVR_Fade.Start(Color.white, gameFadeTime);
        audioOutSnapshot.TransitionTo(gameFadeTime);


    }





}
