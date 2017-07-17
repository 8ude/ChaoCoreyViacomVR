using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMotion : MonoBehaviour {

	//Save y motion for audio reactive thing

	public float turnRadius = 5f;
	public float speed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float deltaX = Mathf.Sin (Time.time * speed) * turnRadius;
		float deltaZ = Mathf.Cos (Time.time * speed) * turnRadius;

		transform.position = new Vector3 (deltaX, 1f, deltaZ);

	}
}
