using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;

public class DrawRibbon: MonoBehaviour {

	private SteamVR_TrackedController device;
	public float timeInterval = 0f;
	public float pointGenTime = 0.1f;
	public int numRibbons = 0;
	[SerializeField] float wandTipOffset = 0.4f;

	float minGenTime;





	public GameObject markerPrefab;
	List<GameObject> markerChain;
	public GameObject markerParentPrefab;
	public GameObject curvySplinePrefab;
	public GameObject ribbonSoundPrefab;
    
	GameObject currentRibbonSound;

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
		currentRibbonSound.GetComponent<DrawRibbonSound> ().clipIndex = numRibbons % currentRibbonSound.GetComponent<DrawRibbonSound> ().origClips.Length;
		currentRibbonSound.GetComponent<DrawRibbonSound> ().StartDrawingRibbon ();


		numRibbons++;

	}

	void TriggerReleased(object sender, ClickedEventArgs e) {

		currentRibbonSound.GetComponent<DrawRibbonSound> ().StopDrawingRibbon ();

		GameObject parentObject = Instantiate(markerParentPrefab, Vector3.zero, Quaternion.identity);
		CurvyGenerator generator = parentObject.GetComponent<CurvyGenerator> ();

		currentRibbonSound.transform.SetParent (parentObject.transform);

		GameObject splineObject = Instantiate (curvySplinePrefab, transform.position, Quaternion.identity);
		CurvySpline spline = splineObject.GetComponent<CurvySpline> ();

		var isp = generator.GetComponentInChildren<InputSplinePath> ();

		

		

		//CurvySpline ribbonSpline = parentObject.GetComponent<CurvySpline> ();

        
        Vector3[] newPoints = new Vector3[markerChain.Count];
        int i = 0;
        foreach (GameObject go in markerChain) {

            newPoints[i] = go.transform.position;
            i++;
			go.transform.SetParent (spline.transform);
            
            
		}

		spline.Add (newPoints);

		isp.Spline = spline;

		spline.transform.SetParent (parentObject.transform);
			
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
