using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMovement : MonoBehaviour {

	public GameObject player;
	public float speed;
	public float time;

	public Vector3 StartPosition;
	public Vector3 OffsetPosition = new Vector3 (0f, 0f, 0f);

	public float heightOffset;

	// Use this for initialization
	void Start () {
		//this may change later
		StartPosition = transform.position;

		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position + new Vector3 (0, 0, 0), speed * Time.deltaTime);
	
		float distance = Vector3.Distance (gameObject.transform.position, player.gameObject.transform.position);

		if (distance < 5f) {

			time = time + Time.deltaTime;
			
			float x = Mathf.Cos (time);
			float z = Mathf.Sin (time);
			float y = heightOffset;

			gameObject.transform.position = new Vector3 (x, y, z);
			UpdateLineRenderer ();


//			if (this.gameObject.name == "Sphere2") {
//				this.gameObject.transform.position = new Vector3 (x, y+0.5f, z);
//			}
//			if (this.gameObject.name == "Sphere3") {
//				this.gameObject.transform.position = new Vector3 (x, y+1f, z);
//			}
//			if (this.gameObject.name == "Sphere4") {
//				this.gameObject.transform.position = new Vector3 (x, y+1.5f, z);
//			}
		}

	}

	public void UpdateLineRenderer() {
		LineRenderer myLineRenderer = GetComponent<LineRenderer> ();

		//null check in case this becomes obsolete
		if (myLineRenderer != null) {

			myLineRenderer.SetPosition (0, transform.position);
			myLineRenderer.SetPosition (1, transform.GetChild (0).position);
		}


	}


}
