using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMovement : MonoBehaviour {

	public GameObject player;
	public float speed;
	public float time;

	// Use this for initialization
	void Start () {
		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
		this.transform.position = Vector3.MoveTowards (this.transform.position, player.transform.position + new Vector3 (0, 0, 0), speed * Time.deltaTime);
	
		float distance = Vector3.Distance (this.gameObject.transform.position, player.gameObject.transform.position);

		if (distance < 5f) {

			time = time + Time.deltaTime;
			
			float x = Mathf.Cos (time);
			float z = Mathf.Sin (time);
			float y = 0;

			this.gameObject.transform.position = new Vector3 (x, y, z);


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


}
