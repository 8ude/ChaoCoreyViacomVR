using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy.Generator.Modules;

public class MeshDistortion : MonoBehaviour {

	public GameObject[] wandObjects;

	public Mesh meshToDraw;
	public Material distortMaterial;

	public Transform distortTarget;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		wandObjects = GameObject.FindGameObjectsWithTag ("Hand");
		if (transform.GetComponentInChildren<CreateMesh> ().gameObject.GetComponentInChildren<MeshFilter> ()) {
			meshToDraw = transform.GetComponentInChildren<CreateMesh> ().gameObject.GetComponentInChildren<MeshFilter> ().mesh;
			distortMaterial = transform.GetComponentInChildren<CreateMesh> ().gameObject.GetComponentInChildren<MeshRenderer> ().material;

			distortTarget = wandObjects [0].transform;

			Shader.SetGlobalVector ("_DisplaceTarget", transform.InverseTransformPoint (distortTarget.position));

			Graphics.DrawMesh (meshToDraw, transform.position, transform.rotation, distortMaterial, 0);
		}
	}
}
