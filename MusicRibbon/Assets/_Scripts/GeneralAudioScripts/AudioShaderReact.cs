using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioShaderReact : MonoBehaviour {

    SpectrumAnalysis analyzer;
	 
	public float smoothing = 0.75f;
    float energyScaleFactor = 60f;

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
	void FixedUpdate () {
		


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
                normalizedEnergy = analyzer.GetWholeEnergy() * energyScaleFactor;
				break;
			case RibbonGenerator.musicStem.Drum:
                normalizedEnergy = analyzer.GetWholeEnergy() * energyScaleFactor;
                break;
            case RibbonGenerator.musicStem.Harmony:
                normalizedEnergy = analyzer.GetWholeEnergy() * energyScaleFactor;
                break;
            case RibbonGenerator.musicStem.Melody:
                normalizedEnergy = analyzer.GetWholeEnergy() * energyScaleFactor;
                break;
            }

			//Debug.Log ("NormEnergy: " + myStem.ToString() + " " + normalizedEnergy);
			//normalizedEnergy = analyzer.GetWholeEnergy()*0.1f;
			smoothedEnergy = Mathf.Lerp (prevEnergy, normalizedEnergy, smoothing);
			prevEnergy = smoothedEnergy;

		}


		//check to see if we have a material, then start adjusting shader values
		if (myMaterial) {

            //These values are consistent among ribbons
            myMaterial.SetFloat("_AudioInput", smoothedEnergy);
            myMaterial.SetVector("_AudioPosition",
                new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f));
            myMaterial.SetTexture("_MainTex", aTexture.AudioTexture);

            switch (myStem)
            {
                case RibbonGenerator.musicStem.Bass:
                    

                    myMaterial.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.BassPosTurbulence);
                    myMaterial.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.BassWaveShudder);
                    myMaterial.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.BassOverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.BassTurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.BassSpikiness);
                    myMaterial.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.BassColorShift);

                    break;
                case RibbonGenerator.musicStem.Drum:


                    myMaterial.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.DrumPosTurbulence);
                    myMaterial.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.DrumWaveShudder);
                    myMaterial.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.DrumOverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.DrumTurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.DrumSpikiness);
                    myMaterial.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.DrumColorShift);
                    break;
                case RibbonGenerator.musicStem.Melody:

                    myMaterial.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.MelodyPosTurbulence);
                    myMaterial.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.MelodyWaveShudder);
                    myMaterial.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.MelodyOverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.MelodyTurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.MelodySpikiness);
                    myMaterial.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.MelodyColorShift);


                    break;
                case RibbonGenerator.musicStem.Harmony:
                    
                    myMaterial.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.HarmonyPosTurbulence);
                    myMaterial.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.HarmonyWaveShudder);
                    myMaterial.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.HarmonyOverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.HarmonyTurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.HarmonySpikiness);
                    myMaterial.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.HarmonyColorShift);

                    break;
            }


        }
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}

    public void WandInteract (Transform wand) {

        myMaterial.SetVector("_WandPos", wand.position);

    }
}
