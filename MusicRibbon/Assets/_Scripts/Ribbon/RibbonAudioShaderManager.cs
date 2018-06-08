using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls synaesthesia values sent to ribbon mesh components
/// Sets values in scriptable objects that are read by the shader
/// </summary>
public class RibbonAudioShaderManager : MonoBehaviour {

    public static RibbonAudioShaderManager Instance = null;


    //TODO - make all these getter/setters; adjust scriptable object values on set
    //later TODO - completely modularize ribbons, i.e. all sounds, shader values, colors, etc live 
    //on objects + prefabs with a parent reference in Resources; then at Runtime, read available ribbons from
    //a list, update GUI and instantiate accordingly - kill the enums!
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

    public RibbonShaderValues bassShaderValues;

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

    public RibbonShaderValues drumShaderValues;

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

    public RibbonShaderValues harmonyShaderValues;

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

    public RibbonShaderValues melodyShaderValues;

    //Enforce Singleton Pattern
    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(gameObject); 
        }

        bassShaderValues.OverallTurbulence = BassOverallTurbulence;
        bassShaderValues.PosTurbulence = BassPosTurbulence;
        bassShaderValues.WaveShudder = BassWaveShudder;
        bassShaderValues.TurbulenceSpeed = BassTurbulenceSpeed;
        bassShaderValues.Spikiness = BassSpikiness;
        bassShaderValues.ColorShift = BassColorShift;

        drumShaderValues.OverallTurbulence = DrumOverallTurbulence;
        drumShaderValues.PosTurbulence = DrumPosTurbulence;
        drumShaderValues.WaveShudder = DrumWaveShudder;
        drumShaderValues.TurbulenceSpeed = DrumTurbulenceSpeed;
        drumShaderValues.Spikiness = DrumSpikiness;
        drumShaderValues.ColorShift = DrumColorShift;

        melodyShaderValues.OverallTurbulence = MelodyOverallTurbulence;
        melodyShaderValues.PosTurbulence = MelodyPosTurbulence;
        melodyShaderValues.WaveShudder = MelodyWaveShudder;
        melodyShaderValues.TurbulenceSpeed = MelodyTurbulenceSpeed;
        melodyShaderValues.Spikiness = MelodySpikiness;
        melodyShaderValues.ColorShift = MelodyColorShift;

        harmonyShaderValues.OverallTurbulence = HarmonyOverallTurbulence;
        harmonyShaderValues.PosTurbulence = HarmonyPosTurbulence;
        harmonyShaderValues.WaveShudder = HarmonyWaveShudder;
        harmonyShaderValues.TurbulenceSpeed = HarmonyTurbulenceSpeed;
        harmonyShaderValues.Spikiness = HarmonySpikiness;
        harmonyShaderValues.ColorShift = HarmonyColorShift;
        //Attempting to move this data to scriptable objects, rather than have singleton pattern
        //Right now, this is _NOT_ saving any time, and is resulting in more lines of code, but there are fewer dependencies
    }

}
