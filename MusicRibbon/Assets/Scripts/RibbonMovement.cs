using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMovement : MonoBehaviour {

	public GameObject player;
	public float speed;
	public float orbitTime;
	public bool pickedUp;
    public float orbitSpeed;
    

	public Vector3 StartPosition;
	public Vector3 OffsetPosition = new Vector3 (0f, 0f, 0f);

	public float heightOffset;
    public Vector3 OrbitStartOffset;

    public float orbitRadius = 0.5f;

    // Use this for initialization
    void Start () {
		//this may change later
		StartPosition = transform.position;
        OrbitStartOffset = new Vector3(1f, heightOffset, 0f);

        orbitTime = 0f;
		pickedUp = false;
        StartCoroutine(OrbitPlayer());
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = Vector3.MoveTowards (transform.position, player.transform.position, speed * Time.deltaTime);
	
		float distance = Vector3.Distance (gameObject.transform.position, player.gameObject.transform.position);


        /*
		if (distance < 5f) {

			if (pickedUp == false) {

				time = time + Time.deltaTime;
			
				float x = Mathf.Cos (time);
				float z = Mathf.Sin (time);
				float y = heightOffset;

				gameObject.transform.position = new Vector3 (x, y, z);
				
			}

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
        */

        UpdateLineRenderer();
        

    }

	public void UpdateLineRenderer() {
		LineRenderer myLineRenderer = GetComponent<LineRenderer> ();

		//null check in case this becomes obsolete
		if (myLineRenderer != null) {

			myLineRenderer.SetPosition (0, transform.position);
			myLineRenderer.SetPosition (1, transform.GetChild (0).position);
		}


	}

    IEnumerator TravelToOrbitPos() {

        Vector3 targetPosition = player.transform.position + OrbitStartOffset;

        orbitTime = 0f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) {

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;

        } 


    }

    IEnumerator OrbitPlayer() {

        yield return StartCoroutine(TravelToOrbitPos());

        while (!pickedUp) {

            Vector3 nextPosition = new Vector3(
                Mathf.Cos(orbitTime)*orbitRadius,
                heightOffset,
                Mathf.Sin(orbitTime)*orbitRadius
            );

            transform.position = nextPosition;

            orbitTime += Time.deltaTime;

            yield return null;


        }

    }

    public void ReleasedByPlayer() {

        pickedUp = false;

        StartCoroutine(OrbitPlayer());

    }

    public void PickedUpByPlayer() {

        pickedUp = true;
    }


}
