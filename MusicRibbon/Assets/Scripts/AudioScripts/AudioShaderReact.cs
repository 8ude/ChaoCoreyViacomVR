using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShaderReact : MonoBehaviour {

    SpectrumAnalysis analyzer;
	public float smoothing = 0.1f;

    float normalizedEnergy = 0f;
	float prevEnergy = 0f;
	float smoothedEnergy = 0f;
	[SerializeField] Material myMaterial;

	// Use this for initialization
	void Start () {

        analyzer = GetComponent<SpectrumAnalysis>();
		//myMaterial = GetComponent<Renderer> ().material;


	}
	
	// Update is called once per frame
	void Update () {
		//smoothing out to prevent jitter;
		normalizedEnergy = Mathf.Clamp01(analyzer.GetWholeEnergy())*2f;
		smoothedEnergy = Mathf.Lerp (prevEnergy, normalizedEnergy, smoothing);
		prevEnergy = smoothedEnergy;

        //Debug.Log(normalizedEnergy);

		myMaterial.SetFloat ("_AudioInput", smoothedEnergy);

		Debug.Log (myMaterial.GetFloat("_AudioInput"));
		myMaterial.SetVector ("_AudioPosition", 
			new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f));
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}
}
