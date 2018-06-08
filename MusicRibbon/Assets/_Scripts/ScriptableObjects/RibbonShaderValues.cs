using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RibbonShaderValues : ScriptableObject {

    public string instrumentName;
    

    public float PosTurbulence = 0.0f;

    public float WaveShudder = 0.1f;

    public float OverallTurbulence = 0.0f;

    public float TurbulenceSpeed = 1f;

    public float Spikiness = 1f;

    public float ColorShift = 0.2f;
}
