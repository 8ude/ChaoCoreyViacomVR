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

	[SerializeField] RibbonGenerator parentRibbonGenerator;

	// Use this for initialization
	void Start () {

        analyzer = GetComponent<SpectrumAnalysis>();
		//myMaterial = GetComponent<Renderer> ().material;


	}
	
	// Update is called once per frame
	void Update () {
		//smoothing out to prevent jitter;
		normalizedEnergy = analyzer.GetWholeEnergy()*0.1f;
		smoothedEnergy = Mathf.Lerp (prevEnergy, normalizedEnergy, smoothing);
		prevEnergy = smoothedEnergy;

        //Debug.Log(normalizedEnergy);

		parentRibbonGenerator = GetComponentInParent<RibbonGenerator>();

		//check if we have a parent ribbon, else keep querying
		if (parentRibbonGenerator) {
			myMaterial = parentRibbonGenerator.ribbonRenderer.material;

		}


		//check to see if we have a material, then start adjusting shader values
		if (myMaterial) {
			myMaterial.SetFloat ("_AudioInput", smoothedEnergy);

			//Debug.Log (myMaterial.GetFloat ("_AudioInput"));
			myMaterial.SetVector ("_AudioPosition", 
				new Vector4 (transform.position.x, transform.position.y, transform.position.z, 1.0f));
		}
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}
}
