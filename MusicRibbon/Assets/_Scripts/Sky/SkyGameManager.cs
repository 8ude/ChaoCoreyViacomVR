using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SkyGameManager : MonoBehaviour {

	public GameObject SkyLine;
    public float Distance;
    private LineRenderer skylinerender;
	public SteamVR_TrackedController device;
	public SwitchStems SwitchStem;
    

    public int i;

	// Use this for initialization
	void Start () {

       skylinerender = SkyLine.GetComponent<LineRenderer>();


    }
	
	// Update is called once per frame
	void Update () {

		if (device.triggerPressed == true) {
            AddPoints();
		}
		
	}

	public void AddPoints(){

        i = i + 1;
        skylinerender.positionCount = i;

        Vector3 newPosition = this.gameObject.transform.position + (Distance * transform.forward);
        skylinerender.SetPosition (i-1, newPosition);
		
        
		Debug.Log ("i:" + i + " position:" + newPosition);
	}

	public void Reset(){

		skylinerender.positionCount = 0;
        i = 0;

	
	}

	public void ChangeLineColor(){

		if (SwitchStem.currentInstrument == "Bass") {
			skylinerender.material.color = SwitchStem.bassColor;
		}
		if (SwitchStem.currentInstrument == "Drums") {
			skylinerender.material.color = SwitchStem.drumColor;
		}
		if (SwitchStem.currentInstrument == "Harmony") {
			skylinerender.material.color = SwitchStem.harmonyColor;
		}
		if (SwitchStem.currentInstrument == "Melody") {
			skylinerender.material.color = SwitchStem.melodyColor;
		}
	
	}

}
