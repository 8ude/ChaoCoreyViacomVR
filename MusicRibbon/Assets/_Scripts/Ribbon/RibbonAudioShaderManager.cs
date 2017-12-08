using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls synaesthesia values sent to ribbon mesh components
/// </summary>
public class RibbonAudioShaderManager : MonoBehaviour {

    public static RibbonAudioShaderManager Instance = null;

    [Header("Bass Ribbon Shader")]
    [Range(0.0f, 3.0f)]
    public float BassPosTurbulence = 0.0f;
    [Range(0.0f, 1.0f)]
    public float BassWaveShudder = 0.1f;
    [Range(0.0f, 3.0f)]
    public float BassOverallTurbulence = 0.0f;
    [Range(1f, 40f)]
    public float BassTurbulenceSpeed = 1f;
    [Range(1f, 3f)]
    public float BassSpikiness = 1f;
    [Range(0.1f, 0.5f)]
    public float BassColorShift = 0.2f;

    [Header("Drum Ribbon Shader")]
    [Range(0.0f, 3.0f)]
    public float DrumPosTurbulence = 0.0f;
    [Range(0.0f, 1.0f)]
    public float DrumWaveShudder = 0.1f;
    [Range(0.0f, 3.0f)]
    public float DrumOverallTurbulence = 0.0f;
    [Range(1f, 40f)]
    public float DrumTurbulenceSpeed = 1f;
    [Range(1f, 3f)]
    public float DrumSpikiness = 1f;
    [Range(0.1f, 0.5f)]
    public float DrumColorShift = 0.2f;

    [Header("Harmony Ribbon Shader")]
    [Range(0.0f, 3.0f)]
    public float HarmonyPosTurbulence = 0.0f;
    [Range(0.0f, 1.0f)]
    public float HarmonyWaveShudder = 0.1f;
    [Range(0.0f, 3.0f)]
    public float HarmonyOverallTurbulence = 0.0f;
    [Range(1f, 40f)]
    public float HarmonyTurbulenceSpeed = 1f;
    [Range(1f, 3f)]
    public float HarmonySpikiness = 1f;
    [Range(0.1f, 0.5f)]
    public float HarmonyColorShift = 0.2f;

    [Header("Melody Ribbon Shader")]
    [Range(0.0f, 3.0f)]
    public float MelodyPosTurbulence = 0.0f;
    [Range(0.0f, 1.0f)]
    public float MelodyWaveShudder = 0.1f;
    [Range(0.0f, 3.0f)]
    public float MelodyOverallTurbulence = 0.0f;
    [Range(1f, 40f)]
    public float MelodyTurbulenceSpeed = 1f;
    [Range(1f, 3f)]
    public float MelodySpikiness = 1f;
    [Range(0.1f, 0.5f)]
    public float MelodyColorShift = 0.2f;


    //Enforce Singleton Pattern
    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject); 
        }
    }

}
