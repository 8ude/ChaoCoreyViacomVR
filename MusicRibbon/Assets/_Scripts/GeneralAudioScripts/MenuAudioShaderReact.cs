using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudioShaderReact : MonoBehaviour {

    //Basic version of the AudioShaderReact script, for use in the Menu Scene
    SpectrumAnalysis analyzer;

    public float smoothing = 0.75f;
    float energyScaleFactor = 60f;

    float normalizedEnergy = 0f;
    float prevEnergy = 0f;
    float smoothedEnergy = 0f;

    [SerializeField] Material myMaterial;


    private void Awake()
    {
        myMaterial = GetComponent<Renderer>().material;
    }

    // Use this for initialization
    void Start()
    {

        analyzer = GetComponent<SpectrumAnalysis>();
  

    }


    void FixedUpdate()
    {

        normalizedEnergy = analyzer.GetWholeEnergy() * energyScaleFactor;
        smoothedEnergy = Mathf.Lerp(prevEnergy, normalizedEnergy, smoothing);
        prevEnergy = smoothedEnergy;

        myMaterial.SetFloat("_AudioInput", smoothedEnergy);

    }

}
