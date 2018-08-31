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
                    

                    myMaterial.SetFloat("_PosTurb", parentRibbonGenerator.BassValues.PosTurbulence);
                    myMaterial.SetFloat("_WaveShud", parentRibbonGenerator.BassValues.WaveShudder);
                    myMaterial.SetFloat("_Turbulence", parentRibbonGenerator.BassValues.OverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", parentRibbonGenerator.BassValues.TurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", parentRibbonGenerator.BassValues.Spikiness);
                    myMaterial.SetFloat("_ColorShift", parentRibbonGenerator.BassValues.ColorShift);

                    break;
                case RibbonGenerator.musicStem.Drum:


                    myMaterial.SetFloat("_PosTurb", parentRibbonGenerator.DrumValues.PosTurbulence);
                    myMaterial.SetFloat("_WaveShud", parentRibbonGenerator.DrumValues.WaveShudder);
                    myMaterial.SetFloat("_Turbulence", parentRibbonGenerator.DrumValues.OverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", parentRibbonGenerator.DrumValues.TurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", parentRibbonGenerator.DrumValues.Spikiness);
                    myMaterial.SetFloat("_ColorShift", parentRibbonGenerator.DrumValues.ColorShift);
                    break;

                case RibbonGenerator.musicStem.Melody:

                    myMaterial.SetFloat("_PosTurb", parentRibbonGenerator.MelodyValues.PosTurbulence);
                    myMaterial.SetFloat("_WaveShud", parentRibbonGenerator.MelodyValues.WaveShudder);
                    myMaterial.SetFloat("_Turbulence", parentRibbonGenerator.MelodyValues.OverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", parentRibbonGenerator.MelodyValues.TurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", parentRibbonGenerator.MelodyValues.Spikiness);
                    myMaterial.SetFloat("_ColorShift", parentRibbonGenerator.MelodyValues.ColorShift);
                    break;

                case RibbonGenerator.musicStem.Harmony:

                    myMaterial.SetFloat("_PosTurb", parentRibbonGenerator.HarmonyValues.PosTurbulence);
                    myMaterial.SetFloat("_WaveShud", parentRibbonGenerator.HarmonyValues.WaveShudder);
                    myMaterial.SetFloat("_Turbulence", parentRibbonGenerator.HarmonyValues.OverallTurbulence);
                    myMaterial.SetFloat("_TurbulenceSpeed", parentRibbonGenerator.HarmonyValues.TurbulenceSpeed);
                    myMaterial.SetFloat("_Spikiness", parentRibbonGenerator.HarmonyValues.Spikiness);
                    myMaterial.SetFloat("_ColorShift", parentRibbonGenerator.HarmonyValues.ColorShift);
                    break;

            }


        }
		//Shader.SetGlobalFloat ("_AudioInput", smoothedEnergy);
        

	}

    public void WandInteract (Transform wand) {

        myMaterial.SetVector("_WandPos", wand.position);

    }
}
