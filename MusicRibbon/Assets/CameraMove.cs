using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {
	public bool moveForward, moveBack;

	public float moveSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (moveForward && !moveBack) {
			transform.Translate (moveSpeed * transform.forward);
		} else if (moveBack && !moveForward) {
			transform.Translate (-moveSpeed * transform.forward);
		}
	}
}
