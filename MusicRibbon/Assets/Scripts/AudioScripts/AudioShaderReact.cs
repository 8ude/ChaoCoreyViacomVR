using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShaderReact : MonoBehaviour {

    SpectrumAnalysis analyzer;

    float normalizedEnergy = 0f;

	// Use this for initialization
	void Start () {

        analyzer = GetComponent<SpectrumAnalysis>();



	}
	
	// Update is called once per frame
	void Update () {
        normalizedEnergy = Mathf.Clamp01(analyzer.GetWholeEnergy());
        //Debug.Log(normalizedEnergy);

        Shader.SetGlobalFloat("_AudioInput", normalizedEnergy * 5f);
        Debug.Log(Shader.GetGlobalFloat("_AudioInput"));

	}
}
