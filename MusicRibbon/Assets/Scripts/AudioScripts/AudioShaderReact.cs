using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShaderReact : MonoBehaviour {

    SpectrumAnalysis analyzer;
	public float smoothing = 0.1f;

    float normalizedEnergy = 0f;
	float prevEnergy = 0f;
	float smoothedEnergy = 0f;
	Material myMaterial;

	// Use this for initialization
	void Start () {

        analyzer = GetComponent<SpectrumAnalysis>();
		myMaterial = GetComponent<Renderer> ().material;


	}
	
	// Update is called once per frame
	void Update () {
		//smoothing out to prevent jitter;
		normalizedEnergy = Mathf.Clamp01(analyzer.GetWholeEnergy());
		smoothedEnergy = Mathf.Lerp (prevEnergy, normalizedEnergy, smoothing);
		prevEnergy = smoothedEnergy;

        //Debug.Log(normalizedEnergy);

		myMaterial.SetFloat ("_AudioInput", smoothedEnergy);
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}
}
