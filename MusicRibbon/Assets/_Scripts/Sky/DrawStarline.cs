using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawStarline : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Trigger (object sender, ClickedEventArgs e){
	
		Debug.Log (this.gameObject.transform.position);
	}
}
