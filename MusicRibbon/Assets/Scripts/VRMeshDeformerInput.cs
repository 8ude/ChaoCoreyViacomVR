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
            
			if (distance < distanceThreshold + surfaceOffset) {
				
				HandleInput (obj.transform);
			}
		}

	}

	void HandleInput(Transform target) {
       
		Ray inputRay;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, target.position, out hit)) {
            Debug.DrawLine(transform.position, hit.point);
            Debug.Log("raycasting");
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer> ();
			if (deformer) {
				
				Vector3 point = hit.point;

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
