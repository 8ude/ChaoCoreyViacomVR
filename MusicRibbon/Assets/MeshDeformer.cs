using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshDeformer : MonoBehaviour {
	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

	void Start() {
		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices [i] = originalVertices [i];
		}

		vertexVelocities = new Vector3[originalVertices.Length];
	}

	void Update() {
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}

	public void AddDeformingForce (Vector3 point, float force) {
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex (i, point, force);
		}
	}

	void AddForceToVertex(int i, Vector3 point, float force) {
		//need both dir and dist of deforming force per vertex
		Vector3 pointToVertex = displacedVertices [i] - point;
		//use Fv = F/(1+d^2) so force is at full strength at d = 0
		float attenuatedForce = force/(1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities [i] += pointToVertex.normalized * velocity;
			
	}

	void UpdateVertex(int i) {
		Vector3 velocity = vertexVelocities [i];
		displacedVertices [i] += velocity * Time.deltaTime;
	}


}
