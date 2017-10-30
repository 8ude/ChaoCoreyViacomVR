using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SkyGameManager : MonoBehaviour {

	public GameObject SkyLineRender;
	public int i;

	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPoints(){

		SkyLineRender.GetComponent<LineRenderer> ().SetPosition (i, this.gameObject.transform.position);
		i = i + 1;
		Debug.Log ("i:" + i + " position:" + this.gameObject.transform.position);
	}

}
