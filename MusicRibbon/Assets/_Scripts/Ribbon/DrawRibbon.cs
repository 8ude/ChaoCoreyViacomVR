using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;

public class DrawRibbon: MonoBehaviour {

	private SteamVR_TrackedController device;
	//public GameObject Switchstems;

	public EraseRibbon eraseRibbon;
	public float timeInterval = 0f;
	public float pointGenTime = 0.1f;

	[SerializeField] float wandTipOffset = 0.4f;

	float minGenTime;

	//PREFABS
	public GameObject markerPrefab;
	public List<GameObject> markerChain;
	public GameObject markerParentPrefab;
	public GameObject curvySplinePrefab;
	public GameObject ribbonSoundPrefab;

	//reference to switch stems - use to change color of ribbons to match instruments
	public SwitchStems switchStems;

    
    [SerializeField] GameObject currentRibbonSound;

	public MeshFilter currentMesh;

	AudioClip nextHighClip;
    AudioClip nextLowClip;

    bool isDrawing;


	//public int numRibbons = 0;

	//public CurvySpline ribbonSpline;

	void Awake() {
		minGenTime = pointGenTime * 4f;
        if (gameObject.GetComponentInChildren<SwitchStems>() != null) {
            switchStems = gameObject.GetComponentInChildren<SwitchStems>();
        }
	}

	// Use this for initialization
	void Start () {

		markerChain = new List<GameObject> ();

        if (GetComponent<SteamVR_TrackedController>()) {
            device = GetComponent<SteamVR_TrackedController>();
            device.TriggerClicked += Trigger;
            device.TriggerUnclicked += TriggerReleased;
        }
        else device = null;


	}

	void Trigger (object sender, ClickedEventArgs e)
	{
        //Debug.Log (eraseRibbon.isErasing);

		if (!eraseRibbon.isErasing) {
            StartDrawing();
            isDrawing = true;
		}

		//numRibbons++;

	}

	void TriggerReleased(object sender, ClickedEventArgs e) {

        StopDrawing();

        isDrawing = false;


	}

	// Update is called once per frame
	void Update () {
	
		timeInterval += Time.deltaTime;
        if (device != null) {
            if (device.triggerPressed && timeInterval > pointGenTime && !eraseRibbon.isErasing) {
                ContinueDrawing();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isDrawing) {
            StartDrawing();
            isDrawing = true;
        } else if (Input.GetMouseButton(0) && isDrawing && timeInterval > pointGenTime) {
            ContinueDrawing();
        } else if (Input.GetMouseButtonUp(0) && isDrawing) {
            StopDrawing();
            isDrawing = false;
        }

	}

	void ShortStem (GameObject go) {

		GameObject newParticles = Instantiate (
			RibbonGameManager.instance.particlePrefab, 
			markerChain [0].transform.position, 
			Quaternion.identity);

		//set the audio clip in accordance with the collision audio in the game manager
		AudioSource aSource = newParticles.GetComponent<AudioSource>();
        if (switchStems != null) {
            string instrumentType = switchStems.currentInstrument;
            //Debug.Log(drawRibbonScript.switchStems.currentInstrument);
            switch (instrumentType) {
                case "Bass":
                    aSource.clip = RibbonGameManager.instance.bassCollisionClips[0];
                    break;
                case "Drums":
                    aSource.clip = RibbonGameManager.instance.drumCollisionClips[0];
                    break;
                case "Harmony":
                    aSource.clip = RibbonGameManager.instance.harmonyCollisionClips[0];
                    break;
                case "Melody":
                    aSource.clip = RibbonGameManager.instance.melodyCollisionClips[0];
                    break;

            }
        } else {
            //fallback mouse mode - bass only
            aSource.clip = RibbonGameManager.instance.bassCollisionClips[0];
        }
		aSource.Play();

		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;

		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient( go.GetComponent<Renderer>().material.color, Color.white);

		Destroy (go);

		Destroy(newParticles, 2.0f);

	}

    /// <summary>
    /// Called when drawing begins.  If no VR controller, will default to bass stem
    /// </summary>
    void StartDrawing() {
        
        if (currentRibbonSound) {
            currentRibbonSound = null;

        }

        nextHighClip = null;
        nextLowClip = null;

       
        currentRibbonSound = Instantiate(ribbonSoundPrefab, transform.position, Quaternion.identity);
        currentRibbonSound.transform.SetParent(gameObject.transform);

        //use switch stems to find the current instrument, then cycle through corresponding clips using
        //Ribbon Game Manager

        //Debug.Log (switchStems.currentInstrument);
        if (switchStems != null) {
            switch (switchStems.currentInstrument) {
                case "Bass":
                    nextHighClip = RibbonGameManager.instance.bassClips[
                        RibbonGameManager.instance.bassRibbonsDrawn % RibbonGameManager.instance.bassClips.Length
                    ];
                    nextLowClip = RibbonGameManager.instance.bassClips[
                        (RibbonGameManager.instance.bassRibbonsDrawn + 1) % RibbonGameManager.instance.bassClips.Length
                    ];
                    RibbonGameManager.instance.bassRibbonsDrawn += 2;
                    currentRibbonSound.gameObject.tag = "BassStem";
                    break;
                case "Drums":
                    nextHighClip = RibbonGameManager.instance.drumClips[
                        RibbonGameManager.instance.drumRibbonsDrawn % RibbonGameManager.instance.drumClips.Length
                    ];
                    nextLowClip = RibbonGameManager.instance.drumClips[
                        (RibbonGameManager.instance.drumRibbonsDrawn + 1) % RibbonGameManager.instance.drumClips.Length
                    ];
                    RibbonGameManager.instance.drumRibbonsDrawn += 2;
                    currentRibbonSound.gameObject.tag = "DrumStem";
                    break;
                case "Harmony":
                    nextHighClip = RibbonGameManager.instance.harmonyClips[
                        RibbonGameManager.instance.harmonyRibbonsDrawn % RibbonGameManager.instance.harmonyClips.Length
                    ];
                    nextLowClip = RibbonGameManager.instance.harmonyClips[
                        (RibbonGameManager.instance.harmonyRibbonsDrawn + 1) % RibbonGameManager.instance.harmonyClips.Length
                    ];
                    RibbonGameManager.instance.harmonyRibbonsDrawn += 2;
                    currentRibbonSound.gameObject.tag = "HarmonyStem";
                    break;
                case "Melody":
                    nextHighClip = RibbonGameManager.instance.melodyClips[
                        RibbonGameManager.instance.melodyRibbonsDrawn % RibbonGameManager.instance.melodyClips.Length
                    ];
                    nextLowClip = RibbonGameManager.instance.melodyClips[
                        (RibbonGameManager.instance.melodyRibbonsDrawn + 1) % RibbonGameManager.instance.melodyClips.Length
                    ];
                    RibbonGameManager.instance.melodyRibbonsDrawn += 2;
                    currentRibbonSound.gameObject.tag = "MelodyStem";
                    break;
            }

            currentRibbonSound.GetComponent<DrawRibbonSound>().clipIndex = switchStems.Stemnum;
        } else {
            //use bass ribbon for fallback testing
            nextHighClip = RibbonGameManager.instance.bassClips[
                        RibbonGameManager.instance.bassRibbonsDrawn % RibbonGameManager.instance.bassClips.Length
                    ];
            nextLowClip = RibbonGameManager.instance.bassClips[
                (RibbonGameManager.instance.bassRibbonsDrawn + 1) % RibbonGameManager.instance.bassClips.Length
            ];
            RibbonGameManager.instance.bassRibbonsDrawn += 2;
            currentRibbonSound.gameObject.tag = "BassStem";
            currentRibbonSound.GetComponent<DrawRibbonSound>().clipIndex = 0;
        }



        //Debug.Log (nextClip.name);

        currentRibbonSound.GetComponent<DrawRibbonSound>().StartDrawingRibbon(nextHighClip, nextLowClip);
        
    }

    void ContinueDrawing() {

        Vector3 newPosition = transform.position + (wandTipOffset * transform.forward);
        GameObject newMarker = Instantiate(markerPrefab, newPosition, Quaternion.identity);

        if (switchStems != null) {
            newMarker.GetComponent<PreRibbon>().stemIndex = switchStems.Stemnum;

            switch (switchStems.Stemnum) {
                case 0:
                    newMarker.GetComponent<MarkerObjectBehavior>().BassMarkerObject();
                    newMarker.GetComponent<PreRibbon>().myClip = RibbonGameManager.instance.preBassClip;
                    break;
                case 1:
                    newMarker.GetComponent<MarkerObjectBehavior>().DrumMarkerObject();
                    newMarker.GetComponent<PreRibbon>().myClip = RibbonGameManager.instance.preDrumClip;
                    break;
                case 2:
                    newMarker.GetComponent<MarkerObjectBehavior>().HarmonyMarkerObject();
                    newMarker.GetComponent<PreRibbon>().myClip = RibbonGameManager.instance.preHarmonyClip;
                    break;
                case 3:
                    newMarker.GetComponent<MarkerObjectBehavior>().MelodyMarkerObject();
                    newMarker.GetComponent<PreRibbon>().myClip = RibbonGameManager.instance.preMelodyClip;
                    break;
            }
        } else {
            newMarker.GetComponent<PreRibbon>().stemIndex = 0;

            newMarker.GetComponent<MarkerObjectBehavior>().BassMarkerObject();
            newMarker.GetComponent<PreRibbon>().myClip = RibbonGameManager.instance.preBassClip;
            
        }

        newMarker.GetComponent<PreRibbon>().PlayPreStem();

        markerChain.Add(newMarker);
        timeInterval = 0f;
        
    }

    void StopDrawing() {
        

        currentRibbonSound.GetComponent<DrawRibbonSound>().StopDrawingRibbon(nextHighClip);

        if (markerChain.Count > 1) {

            GameObject parentObject = Instantiate(markerParentPrefab, Vector3.zero, Quaternion.identity);
            RibbonGenerator ribbonGenerator = parentObject.GetComponent<RibbonGenerator>();

            if (switchStems != null) {
                ribbonGenerator.stemIntValue = switchStems.Stemnum;
            } else {
                ribbonGenerator.stemIntValue = 0;
            }
            //ribbonGenerator.curvyGenerator = parentObject.GetComponent<CurvyGenerator> ();
            ribbonGenerator.drawRibbonSound = currentRibbonSound.GetComponent<DrawRibbonSound>();

            currentRibbonSound.transform.SetParent(parentObject.transform);

            GameObject splineObject = Instantiate(curvySplinePrefab, transform.position, Quaternion.identity);
            CurvySpline spline = splineObject.GetComponent<CurvySpline>();

            //reference to spline path that will be used to create the ribbon
            InputSplinePath isp = ribbonGenerator.gameObject.GetComponentInChildren<InputSplinePath>();

            //reference to volume mesh material - set appropriate color


            //turn the List into an array, to be fed to the spline generator
            Vector3[] newPoints = new Vector3[markerChain.Count];
            
            int i = 0;
            ribbonGenerator.ribbonLength = 0;
            foreach (GameObject go in markerChain) {

                newPoints[i] = go.transform.position;
                i++;
                ribbonGenerator.ribbonLength += 1f;
                go.transform.SetParent(spline.transform);


            }

            //For now, feed the point array to the sound object, which runs a coroutine to (mostly) follow the path
            currentRibbonSound.GetComponent<DrawRibbonSound>().splinePoints = newPoints;
            currentRibbonSound.GetComponent<DrawRibbonSound>().markerObjects = markerChain.ToArray();
            currentRibbonSound.GetComponent<DrawRibbonSound>().FollowRibbon();

            //generate our spline, set it's parent (just for organization for now)
            spline.Add(newPoints);
            isp.Spline = spline;

            spline.transform.SetParent(parentObject.transform);

            //feed this spline to the 
            ribbonGenerator.ribbonSpline = spline;

            //ribbonSpline.Add (newPoints);

            //InputSplinePath inputSpline = parentObject.GetComponentInChildren<InputSplinePath> ();
            //inputSpline.Spline = ribbonSpline;

            markerChain.Clear();
            //Debug.Log("trigger released?");
            timeInterval = 0f;

            //RibbonGameManager.instance.RibbonObjects.Add (parentObject);
        }
        else {
            //ribbon length is less than one, so we'll just shoot out some particles?
            Destroy(currentRibbonSound);
            if (markerChain.Count > 0) {
                ShortStem(markerChain[0]);
            }
            markerChain.Clear();
            timeInterval = 0f;
        }

    }



    //fallback for testing on laptop


}
