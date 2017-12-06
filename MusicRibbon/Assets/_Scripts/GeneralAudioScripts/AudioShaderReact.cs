using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShaderReact : MonoBehaviour {

    SpectrumAnalysis analyzer;
	 
	public float smoothing = 0.75f;

    float normalizedEnergy = 0f;
	float prevEnergy = 0f;
	float smoothedEnergy = 0f;

	float energyGate = 0.5f;
	[SerializeField] Material myMaterial;

	[SerializeField] RibbonGenerator parentRibbonGenerator;

	[SerializeField] RibbonGenerator.musicStem myStem;
    AudioSourceTexture aTexture;

    private void Awake() {
        
        aTexture = GetComponent<AudioSourceTexture>();
    
    }

    // Use this for initialization
    void Start () {

        analyzer = GetComponent<SpectrumAnalysis>();
		//myMaterial = GetComponent<Renderer> ().material;


	}
	
	// Update is called once per frame
	void Update () {
		


        //Debug.Log(normalizedEnergy);

		parentRibbonGenerator = GetComponentInParent<RibbonGenerator>();

		//check if we have a parent ribbon, else keep querying
		if (parentRibbonGenerator) {
            if (parentRibbonGenerator.ribbonRenderer) {
                myMaterial = parentRibbonGenerator.ribbonRenderer.material;

            }
			myStem = parentRibbonGenerator.myStem;
			//smoothing out to prevent jitter;

			switch (myStem) {

			case RibbonGenerator.musicStem.Bass:
				normalizedEnergy = analyzer.GetEnergyFrequencyRange(0f,8000f) * 0.1f;
				break;
			case RibbonGenerator.musicStem.Drum:
				energyGate = 0.15f;
				float newEnergy = analyzer.GetEnergyFrequencyRange (0f, 8000f) * 0.1f;
				//newEnergy += analyzer.GetEnergyFrequencyRange (4000f, 8000f) * 0.1f;
				if (newEnergy >= energyGate) {
					normalizedEnergy = newEnergy;
				}
				break;
			case RibbonGenerator.musicStem.Harmony:
				normalizedEnergy = analyzer.GetEnergyFrequencyRange (0, 8000f) * 0.3f;
				break;
			case RibbonGenerator.musicStem.Melody:
				normalizedEnergy = analyzer.GetEnergyFrequencyRange (0, 8000f) * 0.3f;
				break;
			}

			//Debug.Log ("NormEnergy: " + myStem.ToString() + " " + normalizedEnergy);
			//normalizedEnergy = analyzer.GetWholeEnergy()*0.1f;
			smoothedEnergy = Mathf.Lerp (prevEnergy, normalizedEnergy, smoothing);
			prevEnergy = smoothedEnergy;

		}


		//check to see if we have a material, then start adjusting shader values
		if (myMaterial) {
			myMaterial.SetFloat ("_AudioInput", smoothedEnergy);

			//Debug.Log (myMaterial.GetFloat ("_AudioInput"));
			myMaterial.SetVector ("_AudioPosition", 
				new Vector4 (transform.position.x, transform.position.y, transform.position.z, 1.0f));
            myMaterial.SetTexture("_MainTex", aTexture.AudioTexture);
		}
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}
}
