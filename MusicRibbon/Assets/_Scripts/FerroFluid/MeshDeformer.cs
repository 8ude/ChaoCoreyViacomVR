using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]

public class MeshDeformer : MonoBehaviour {
	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;

    public int numSoundSpikes;
    GameObject[] soundSpikes;
    public GameObject soundSpikePrefab;

	public float springForce = 20f;
	public float damping = 5f;
	public bool wavesOn = true;
	public float waveForce = 1f;

	float uniformScale = 1f;

	SpectrumAnalysis soundAnalyzer;
	float soundForceAttenuation = 0.1f;

    float averageXVelocity, averageZVelocity, averageYVelocity;


    Hv_testGrain_AudioLib pdPlugin;

	void Start() {
		soundAnalyzer = GetComponent<SpectrumAnalysis> ();
		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;

        soundSpikes = new GameObject[numSoundSpikes];
        //sound spikes are points on the sphere that add percussive texture 
        //when displacement is large enough
        for (int i = 0; i < numSoundSpikes; i++){
            int associatedVertex = Mathf.RoundToInt(deformingMesh.vertices.Length / (i + 1)) - 1;
            Vector3 worldPos = transform.TransformPoint(originalVertices[associatedVertex]);
            soundSpikes[i] = Instantiate(soundSpikePrefab, worldPos, Quaternion.identity);
        }


		//duplicate original vertices into displaced vertices
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices [i] = originalVertices [i];
		}


        pdPlugin = GetComponent<Hv_testGrain_AudioLib>();
		

        vertexVelocities = new Vector3[originalVertices.Length];
	}

	void Update() {
        averageXVelocity = 0;
        averageYVelocity = 0;
        averageZVelocity = 0;

		uniformScale = transform.localScale.x;
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();

        for(int i = 0; i <vertexVelocities.Length; i ++) {
            averageXVelocity += vertexVelocities[i].magnitude;
            averageYVelocity += vertexVelocities[i].magnitude;
            averageZVelocity += vertexVelocities[i].magnitude;
        }
        averageXVelocity /= vertexVelocities.Length;
        averageYVelocity /= vertexVelocities.Length;
        averageZVelocity /= vertexVelocities.Length;

		for (int i = 0; i < numSoundSpikes; i++) {
			int associatedVertex = Mathf.RoundToInt(deformingMesh.vertices.Length / (i + 1)) - 1;
            Vector3 worldPos = transform.TransformPoint(displacedVertices[associatedVertex]);
			soundSpikes[i].transform.position = worldPos;
		}


        pdPlugin.SetFloatParameter(Hv_testGrain_AudioLib.Parameter.Grainadjust, Mathf.Abs(averageXVelocity)*100f);
        pdPlugin.SetFloatParameter(Hv_testGrain_AudioLib.Parameter.Playback, Mathf.Abs(averageZVelocity)*10f);
        pdPlugin.SetFloatParameter(Hv_testGrain_AudioLib.Parameter.Volume, Mathf.Abs(averageYVelocity/2f));



    }

	public void AddDeformingForce (Vector3 point, float force) {
		//world --> local space
		point = transform.InverseTransformPoint (point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			//each vertex will receive the same force, but this will be attenuated by the
			//inverse square of the distance from the vertex to the point 
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
