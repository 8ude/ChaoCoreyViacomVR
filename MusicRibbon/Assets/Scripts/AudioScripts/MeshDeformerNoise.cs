using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerNoise : MonoBehaviour {

    public double frequency = 220;
    public double modFrequency = 660;
    public double LFOGain = 3;
    public double gain = 0.05;
    public double currentGain;
    double LFOAmount = 0.01; 

    double mainOSCIncrement, modOSCIncrement, LFOIncrement;
    double mainPhase, modPhase, LFOPhase;
    double sampling_frequency = 44100;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

    //public void SetGrainSize(float sizeFactor) {
    //    grainTime = GrainClip.length * sizeFactor;
    //}

    private void OnAudioFilterRead(float[] data, int channels) {
        mainOSCIncrement = frequency * 2 * Mathf.PI / sampling_frequency;
        modOSCIncrement = modFrequency * 2 * Mathf.PI / sampling_frequency;
        LFOIncrement = LFOGain * 2 * Mathf.PI / sampling_frequency;


        for (int i = 0; i < data.Length; i ++) {
            mainPhase = mainPhase + mainOSCIncrement;
            modPhase = modPhase + modOSCIncrement;
            LFOPhase = LFOPhase + LFOIncrement;

            currentGain = gain + LFOAmount * Mathf.Sin((float)LFOPhase);
            data[i] = (float)currentGain * Mathf.Sin((float)mainPhase);
            //data[i] += Mathf.Sin(mainPhase * frequency / sampling_frequency);

        }
        //data[i] = 
        //signal += wave_function(note_phase * note_frequency / sample_rate fm_index * sin(note_phase * fm_frequency * pi / sample_rate)) * note_amplitude
    }
}
