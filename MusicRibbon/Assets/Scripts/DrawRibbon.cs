using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;

public class DrawRibbon: MonoBehaviour {

	private SteamVR_TrackedController device;
	public GameObject Switchstems;

	public EraseRibbon eraseRibbon;
	public float timeInterval = 0f;
	public float pointGenTime = 0.1f;

	[SerializeField] float wandTipOffset = 0.4f;

	float minGenTime;

	//PREFABS
	public GameObject markerPrefab;
	List<GameObject> markerChain;
	public GameObject markerParentPrefab;
	public GameObject curvySplinePrefab;
	public GameObject ribbonSoundPrefab;

	//reference to switch stems - use to change color of ribbons to match instruments
	[SerializeField] SwitchStems switchStems;

    
	GameObject currentRibbonSound;

	public MeshFilter currentMesh;


	//public int numRibbons = 0;

	//public CurvySpline ribbonSpline;

	void Awake() {
		minGenTime = pointGenTime * 4f;
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
		Debug.Log ("trigger pressed");	

		if (currentRibbonSound) {
			currentRibbonSound = null;
		}

		currentRibbonSound = Instantiate (ribbonSoundPrefab, transform.position, Quaternion.identity);
		currentRibbonSound.transform.SetParent (gameObject.transform);
		//currentRibbonSound.GetComponent<DrawRibbonSound> ().clipIndex = numRibbons % currentRibbonSound.GetComponent<DrawRibbonSound> ().origClips.Length;
		currentRibbonSound.GetComponent<DrawRibbonSound> ().clipIndex = Switchstems.GetComponent<SwitchStems>().Stemnum;
		currentRibbonSound.GetComponent<DrawRibbonSound> ().StartDrawingRibbon ();


		//numRibbons++;

	}

	void TriggerReleased(object sender, ClickedEventArgs e) {

		currentRibbonSound.GetComponent<DrawRibbonSound> ().StopDrawingRibbon ();

		GameObject parentObject = Instantiate(markerParentPrefab, Vector3.zero, Quaternion.identity);
		RibbonGenerator ribbonGenerator = parentObject.GetComponent<RibbonGenerator> ();
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
        foreach (GameObject go in markerChain) {

            newPoints[i] = go.transform.position;
            i++;
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
		Debug.Log ("trigger released?");
		timeInterval = 0f;

		RibbonGameManager.instance.RibbonObjects.Add (parentObject);


	}

	// Update is called once per frame
	void Update () {
	
		timeInterval += Time.deltaTime;
		if (device.triggerPressed && timeInterval > pointGenTime) {
			Vector3 newPosition = transform.position + (wandTipOffset * transform.forward);
			GameObject newMarker = Instantiate (markerPrefab, newPosition, Quaternion.identity);
			markerChain.Add (newMarker);
			timeInterval = 0f;
		}

	}






}
