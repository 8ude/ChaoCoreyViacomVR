using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SkyGameManager : MonoBehaviour {

	public GameObject SkyLine;
    public float Distance;
    private LineRenderer skylinerender;
    

    public int i;

	// Use this for initialization
	void Start () {

       skylinerender = SkyLine.GetComponent<LineRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		
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

}
