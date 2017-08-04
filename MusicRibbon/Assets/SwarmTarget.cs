using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmTarget : MonoBehaviour {

	public float radius = 80;
	GameObject player;
	public float rotateDegreeSpeed = 10;

	// Use this for initialization

	void Awake () {
		player = GameObject.FindGameObjectWithTag ("MainCamera");
		transform.position = new Vector3 (radius, 0f, 0f);
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		transform.RotateAround (player.transform.position, Vector3.up, rotateDegreeSpeed * Time.deltaTime); 



		transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time / 20f) * 20 + 10, transform.position.z); 

	}
}
