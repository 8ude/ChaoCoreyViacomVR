using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRMeshDeformerInput : MonoBehaviour {

	public float force = 10f;
	public float forceOffset = 0.1f;
	public float distanceThreshold = 0.2f;


	public GameObject[] interactableObjects;

	// Use this for initialization
	void Start () {
		interactableObjects = GameObject.FindGameObjectsWithTag("InteractableMesh");
	}

	// Update is called once per frame
	void Update () {
		
		foreach (GameObject obj in interactableObjects) {
			float distance = Vector3.Distance (transform.position, obj.transform.position);
			// this assumes that obj is uniform scale
			float surfaceOffset = obj.transform.lossyScale.x;
            


			HandleInput (obj.transform);
		
		}

	}

	void HandleInput(Transform target) {
       
		Ray inputRay = new Ray(transform.position, target.position);
		RaycastHit hit;


		Vector3 targetForward = target.forward;

		Vector3 aVector = target.position - transform.position;
		float angle = Vector3.Angle (targetForward, aVector);
		Vector3 rayDirection = aVector - ((aVector.magnitude * Mathf.Cos (angle * Mathf.PI / 180f)) * targetForward);
		//with prismatic elements, we want the raycast direction to be towards the centerline
		//
	

		if (Physics.Raycast (transform.position, rayDirection, out hit, distanceThreshold)) {
			//Check for audio filter controllers on self

			float distanceToPoint = Vector3.Distance (transform.position, hit.point);

			if (GetComponent<LowPassController> ()) {
				GetComponent<LowPassController>().AdjustFrequency (target, distanceToPoint);
			} else if (GetComponent<HighPassController>()) {
				GetComponent<HighPassController>().AdjustFrequency (target, distanceToPoint);
			}



            //Debug.DrawLine(transform.position, target.position);
            //Debug.Log("raycasting");
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer> ();
			if (deformer) {



				Vector3 point = hit.point;
				Debug.DrawLine (transform.position, point);
				// multiply by normal and force offset to push towards center
				point += (-1f * hit.normal) * forceOffset;
				float forceMag = Vector3.Distance (transform.position, point);
				float handForce = MathUtil.Remap(forceMag, 0f, 1f, force, 0f);
                //Debug.Log(handForce);
				deformer.AddDeformingForce (point, handForce);
			}
		}
	}




}
