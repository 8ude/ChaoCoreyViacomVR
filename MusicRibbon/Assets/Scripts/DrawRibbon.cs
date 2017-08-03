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

    
	GameObject currentRibbonSound;

	public MeshFilter currentMesh;

	AudioClip nextClip;


	//public int numRibbons = 0;

	//public CurvySpline ribbonSpline;

	void Awake() {
		minGenTime = pointGenTime * 4f;
		switchStems = gameObject.GetComponentInChildren<SwitchStems> ();
	}

	// Use this for initialization
	void Start () {

		markerChain = new List<GameObject> ();

		device = GetComponent<SteamVR_TrackedController> ();
		device.TriggerClicked += Trigger;
		device.TriggerUnclicked += TriggerReleased;


	}

	void Trigger (object sender, ClickedEventArgs e)
	{
		//Debug.Log (eraseRibbon.isErasing);

		if (!eraseRibbon.isErasing) {

			if (currentRibbonSound) {
				currentRibbonSound = null;

			}

			nextClip = null;


			currentRibbonSound = Instantiate (ribbonSoundPrefab, transform.position, Quaternion.identity);
			currentRibbonSound.transform.SetParent (gameObject.transform);

			//use switch stems to find the current instrument, then cycle through corresponding clips using
			//Ribbon Game Manager

			//Debug.Log (switchStems.currentInstrument);
			switch (switchStems.currentInstrument) {
			    case "Bass":
				    nextClip = RibbonGameManager.instance.bassClips [
					    RibbonGameManager.instance.bassRibbonsDrawn % RibbonGameManager.instance.bassClips.Length
				    ];
				    RibbonGameManager.instance.bassRibbonsDrawn++;
                    currentRibbonSound.gameObject.tag = "BassStem";
				break;
			    case "Drums":
				    nextClip = RibbonGameManager.instance.drumClips [
					    RibbonGameManager.instance.drumRibbonsDrawn % RibbonGameManager.instance.drumClips.Length
				    ];
                    RibbonGameManager.instance.drumRibbonsDrawn++;
                    currentRibbonSound.gameObject.tag = "DrumStem";
                    break;
			    case "Harmony":
				    nextClip = RibbonGameManager.instance.harmonyClips [
				    	RibbonGameManager.instance.harmonyRibbonsDrawn % RibbonGameManager.instance.harmonyClips.Length
			    	];
                    RibbonGameManager.instance.harmonyRibbonsDrawn++;
                    currentRibbonSound.gameObject.tag = "HarmonyStem";
                    break;
			    case "Melody":
			    	nextClip = RibbonGameManager.instance.melodyClips [
				    	RibbonGameManager.instance.melodyRibbonsDrawn % RibbonGameManager.instance.melodyClips.Length
				    ];
                    RibbonGameManager.instance.melodyRibbonsDrawn++;
                    currentRibbonSound.gameObject.tag = "MelodyStem";
                    break;
			}

			currentRibbonSound.GetComponent<DrawRibbonSound> ().clipIndex = switchStems.Stemnum;

			//Debug.Log (nextClip.name);
            
            currentRibbonSound.GetComponent<DrawRibbonSound> ().StartDrawingRibbon (nextClip);
		}


		//numRibbons++;

	}

	void TriggerReleased(object sender, ClickedEventArgs e) {

		if (!eraseRibbon.isErasing) {

			currentRibbonSound.GetComponent<DrawRibbonSound> ().StopDrawingRibbon (nextClip);

			if (markerChain.Count > 1) {

				GameObject parentObject = Instantiate (markerParentPrefab, Vector3.zero, Quaternion.identity);
				RibbonGenerator ribbonGenerator = parentObject.GetComponent<RibbonGenerator> ();
				ribbonGenerator.switchStems = switchStems.gameObject;
				ribbonGenerator.stemIntValue = switchStems.Stemnum;
				//ribbonGenerator.curvyGenerator = parentObject.GetComponent<CurvyGenerator> ();
				ribbonGenerator.drawRibbonSound = currentRibbonSound.GetComponent<DrawRibbonSound> ();

				currentRibbonSound.transform.SetParent (parentObject.transform);

				GameObject splineObject = Instantiate (curvySplinePrefab, transform.position, Quaternion.identity);
				CurvySpline spline = splineObject.GetComponent<CurvySpline> ();

				//reference to spline path that will be used to create the ribbon
				InputSplinePath isp = ribbonGenerator.gameObject.GetComponentInChildren<InputSplinePath> ();

				//reference to volume mesh material - set appropriate color


				//turn the List into an array, to be fed to the spline generator
				Vector3[] newPoints = new Vector3[markerChain.Count];
				int i = 0;
				ribbonGenerator.ribbonLength = 0;
				foreach (GameObject go in markerChain) {

					newPoints [i] = go.transform.position;
					i++;
					ribbonGenerator.ribbonLength += 1f;
					go.transform.SetParent (spline.transform);


				}

				//For now, feed the point array to the sound object, which runs a coroutine to (mostly) follow the path
				currentRibbonSound.GetComponent<DrawRibbonSound> ().splinePoints = newPoints;
				currentRibbonSound.GetComponent<DrawRibbonSound> ().FollowRibbon ();

				//generate our spline, set it's parent (just for organization for now)
				spline.Add (newPoints);
				isp.Spline = spline;

				spline.transform.SetParent (parentObject.transform);

				//feed this spline to the 
				ribbonGenerator.ribbonSpline = spline;

				//ribbonSpline.Add (newPoints);

				//InputSplinePath inputSpline = parentObject.GetComponentInChildren<InputSplinePath> ();
				//inputSpline.Spline = ribbonSpline;

				markerChain.Clear ();
				//Debug.Log("trigger released?");
				timeInterval = 0f;

				//RibbonGameManager.instance.RibbonObjects.Add (parentObject);
			} else {
				//ribbon length is less than one, so we'll just shoot out some particles?
				Destroy (currentRibbonSound);
				ShortStem (markerChain [0]);
				markerChain.Clear ();
			

			}
		}


	}

	// Update is called once per frame
	void Update () {
	
		timeInterval += Time.deltaTime;
		if (device.triggerPressed && timeInterval > pointGenTime && !eraseRibbon.isErasing) {
			Vector3 newPosition = transform.position + (wandTipOffset * transform.forward);
			GameObject newMarker = Instantiate (markerPrefab, newPosition, Quaternion.identity);
			newMarker.GetComponent<PreRibbon> ().stemIndex = switchStems.Stemnum;

			switch (switchStems.Stemnum) {
			case 0:
				newMarker.GetComponent<MarkerObjectBehavior> ().BassMarkerObject ();
				newMarker.GetComponent<PreRibbon> ().myClip = RibbonGameManager.instance.preBassClip;
				break;
			case 1:
				newMarker.GetComponent<MarkerObjectBehavior> ().DrumMarkerObject ();
				newMarker.GetComponent<PreRibbon> ().myClip = RibbonGameManager.instance.preDrumClip;
				break;
			case 2:
				newMarker.GetComponent<MarkerObjectBehavior> ().HarmonyMarkerObject ();
				newMarker.GetComponent<PreRibbon> ().myClip = RibbonGameManager.instance.preHarmonyClip;
				break;
			case 3:
				newMarker.GetComponent<MarkerObjectBehavior> ().MelodyMarkerObject ();
				newMarker.GetComponent<PreRibbon> ().myClip = RibbonGameManager.instance.preMelodyClip;
				break;
			}
			newMarker.GetComponent<PreRibbon> ().PlayPreStem ();

			markerChain.Add (newMarker);
			timeInterval = 0f;
		}

	}

	void ShortStem (GameObject go) {

		GameObject newParticles = Instantiate (
			RibbonGameManager.instance.particlePrefab, 
			markerChain [0].transform.position, 
			Quaternion.identity);

		//set the audio clip in accordance with the collision audio in the game manager
		AudioSource aSource = newParticles.GetComponent<AudioSource>();

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
		aSource.Play();

		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;

		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient( go.GetComponent<Renderer>().material.color, Color.white);

		Destroy (go);

		Destroy(newParticles, 2.0f);

	}






}
