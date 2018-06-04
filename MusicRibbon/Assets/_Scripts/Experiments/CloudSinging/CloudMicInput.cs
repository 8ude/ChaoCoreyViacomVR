using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMicInput : MonoBehaviour {

    AudioSource[] audSources;
    public float loopLength;
    public float startMicTime;
    public float endMicTime;

    public AudioClip microphoneClip;
    public AudioClip newMicClip;
    public GameObject spatialSoundPrefab;

    float[] truncatedMicSamples;

    float[] triWave;

    public Hv_vocoderMod_AudioLib vocoder;

    void Awake() {
        triWave = new float[4096];


        float[] harmonicWeights = { 1f, 0f, -0.11111111f, 0f, 0.04f, 0f, -0.0204082f, 0f, 0.01234567f, 0f, -0.00826446f };

        float stepScale = 2f * Mathf.PI / 4096f;

        for (int i = 0; i < triWave.Length; i++) {

            triWave[i] = 0f;

            for (int j = 0; j < harmonicWeights.Length; j ++ ) {
                triWave[i] += Mathf.Sin(i * ((float)j + 1f) * stepScale);
            }

            triWave[i] *= 0.75f;
        }

        //vocoder.FillTableWithFloatBuffer("TriWav", triWave);
    }

	// Use this for initialization
	void Start () {
        

        audSources = GetComponents<AudioSource>();
        for (int i = 0; i < audSources.Length; i++) {
            audSources[i].clip = Microphone.Start("Built-in Microphone", true, 1, 44100);
            audSources[i].Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            RecordSpatialVoice();
        } else if (Input.GetMouseButtonUp(0)) {
            StopRecordSpatialVoice();
        }
	}

    public void PlayStreamingSound() {
		for (int i = 0; i < audSources.Length; i++) {
			audSources[i].clip = Microphone.Start("Built-in Microphone", true, 1, 44100);
			audSources[i].Play();
		}
    }

    public void RecordSpatialVoice() {
        if (Microphone.IsRecording("Built-in Microphone")) {
            Microphone.End("Built-in Microphone");
        }
        startMicTime = Time.time;
        microphoneClip = Microphone.Start("Built-in Microphone", false, 20, 44100);
       
    }

    public void StopRecordSpatialVoice() {
		if (Microphone.IsRecording("Built-in Microphone")) {
			Microphone.End("Built-in Microphone");
		}
        endMicTime = Time.time;
        if (microphoneClip.length > (endMicTime - startMicTime)) {
            
            float[] recordedMicSamples = new float [microphoneClip.samples * microphoneClip.channels];
            microphoneClip.GetData(recordedMicSamples, 0);

            truncatedMicSamples = new float[Mathf.RoundToInt(AudioSettings.outputSampleRate * (endMicTime - startMicTime))];

            for (int i = 0; i < truncatedMicSamples.Length; i ++ ) {
                truncatedMicSamples[i] = recordedMicSamples[i];
              
            }
            newMicClip = AudioClip.Create("voiceAudioClip", truncatedMicSamples.Length, 1, AudioSettings.outputSampleRate, false);
            newMicClip.SetData(truncatedMicSamples, 0);

        }

        GameObject newSoundPrefab = Instantiate(spatialSoundPrefab, transform.position, Quaternion.identity);
        newSoundPrefab.GetComponent<AudioSource>().clip = newMicClip;;
        newSoundPrefab.GetComponent<AudioSource>().loop = true;
        newSoundPrefab.GetComponent<AudioSource>().Play();
    }
}
