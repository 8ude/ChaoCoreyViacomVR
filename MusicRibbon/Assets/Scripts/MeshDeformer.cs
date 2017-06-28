using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshDeformer : MonoBehaviour {
	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

	public float springForce = 20f;
	public float damping = 5f;
	public bool wavesOn = true;
	public float waveForce = 1f;

	float uniformScale = 1f;

	SpectrumAnalysis soundAnalyzer;
	float soundForceAttenuation = 0.1f;

	void Start() {
		soundAnalyzer = GetComponent<SpectrumAnalysis> ();
		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices [i] = originalVertices [i];
		}

		vertexVelocities = new Vector3[originalVertices.Length];
	}

	void Update() {
		uniformScale = transform.localScale.x;
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}

	public void AddDeformingForce (Vector3 point, float force) {
		point = transform.InverseTransformPoint (point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex (i, point, force);
		}
	}

	void AddForceToVertex(int i, Vector3 point, float force) {
		//need both dir and dist of deforming force per vertex
		Vector3 pointToVertex = displacedVertices [i] - point;
		pointToVertex *= uniformScale;
		//use Fv = F/(1+d^2) so force is at full strength at d = 0
		float attenuatedForce = force/(1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities [i] += pointToVertex.normalized * velocity;
			
	}
	Vector3 AddWaveForceToVertex(int i, float force) {
		
		return Vector3.up * Mathf.Sin (Time.time * i/500f) * force;

	}

	void UpdateVertex(int i) {
		//AddWaveForceToVertex(i, Mathf.Sin(Time.time*i)*5f);
		Vector3 velocity = vertexVelocities [i];

		velocity += AddWaveForceToVertex (i, waveForce * Time.deltaTime);


		Vector3 displacement = displacedVertices [i] - originalVertices [i];
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - (damping * Time.deltaTime);
		vertexVelocities [i] = velocity;
		displacedVertices [i] += velocity * (Time.deltaTime / uniformScale);
		
	}

	void AddSoundForceToVertex(int i, float soundForce) {
		//start by adding force to a vertex picked at random


	}
		


}
