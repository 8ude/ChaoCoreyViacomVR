using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class DrawRibbon: MonoBehaviour {

	private SteamVR_TrackedController device;
	public float timeInterval = 0f;
	public float pointGenTime = 0.1f;

	public GameObject markerPrefab;
	List<GameObject> markerChain;
	public GameObject markerParentPrefab;


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



	}

	void TriggerReleased(object sender, ClickedEventArgs e) {

		GameObject parentObject = Instantiate(markerParentPrefab, transform.position, Quaternion.identity);
		foreach (GameObject go in markerChain) {
			go.transform.SetParent (parentObject.transform);
		}
		markerChain.Clear ();
		Debug.Log ("trigger released?");
		timeInterval = 0f;


	}

	// Update is called once per frame
	void Update () {
	
		timeInterval += Time.deltaTime;
		if (device.triggerPressed && timeInterval > pointGenTime) {
			GameObject newMarker = Instantiate (markerPrefab, transform.position, Quaternion.identity);
			markerChain.Add (newMarker);
			timeInterval = 0f;
		}

	}



}
