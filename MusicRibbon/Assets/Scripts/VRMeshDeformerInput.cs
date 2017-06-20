using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			if (distance < distanceThreshold) {
				HandleInput (obj.transform);
			}
		}

	}

	void HandleInput(Transform target) {
		Ray inputRay;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, target.position, out hit)) {
			MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer> ();
			if (deformer) {
				Vector3 point = hit.point;
				point += hit.normal * forceOffset;
				deformer.AddDeformingForce (point, force);
			}
		}

	}
}
