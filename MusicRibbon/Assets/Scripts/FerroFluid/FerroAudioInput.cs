using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerroAudioInput : MonoBehaviour {

    float[] hanningData;
    public AudioClip sampleClip;

    Hv_testGrain_AudioLib pdPlugIn;

	// Use this for initialization
	void Start () {
        pdPlugIn = GetComponent<Hv_testGrain_AudioLib>();
        hanningData = new float[256];
        for (int i = 0; i < hanningData.Length; i ++) {
            hanningData[i] = 0.5f + (0.5f * Mathf.Cos((2f * Mathf.PI * i / 256) - Mathf.PI));
            Debug.Log(hanningData[255]);
        }
        pdPlugIn.FillTableWithFloatBuffer("hanning_sample-0", hanningData);
        pdPlugIn.FillTableWithMonoAudioClip("mono_sample-0", sampleClip);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
