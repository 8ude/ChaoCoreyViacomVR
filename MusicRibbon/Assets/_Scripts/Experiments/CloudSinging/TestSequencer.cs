using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmfLite;

public class TestSequencer : MonoBehaviour
{
    public TextAsset sourceFile;
    MidiFileContainer song;
    MidiTrackSequencer sequencer;
    public AudioSource aSource;

    public Hv_vocoderMod_AudioLib vocoder;

    public Hv_vocoderMod_AudioLib[] vocoders;

    int currentXIndex;
    public float xScale = 1f;

    //public GameObject level;

    //public GameObject blockPrefab;

    public bool looping = true;

    void ResetAndPlay ()
    {
        //aSource.Play ();
        sequencer = new MidiTrackSequencer (song.tracks [0], song.division, 131.0f);
        ApplyMessages (sequencer.Start ());
    }

    IEnumerator Start ()
    {
        song = MidiFileLoader.Load (sourceFile.bytes);
        yield return new WaitForSeconds (1.0f);
        ResetAndPlay ();
    }
    
    void Update ()
    {


        if (sequencer != null && sequencer.Playing) {
            ApplyMessages (sequencer.Advance (Time.deltaTime));
		} else if (sequencer != null && !sequencer.Playing) {
			ResetAndPlay();
		}
         
        //Debug.Log(sequencer.Playing);
        //Debug.Log(currentXIndex);

    }

    void ApplyMessages (List<MidiEvent> messages)
    {
        if (messages != null) {
            currentXIndex++;
            foreach (var m in messages) {
                //0x80 is hex for "note off"
                //0x90 is hex for "note on"
                //but what kind of weird bit-wise fuckery is this....?
                if ((m.status & 0xf0) == 0x90) {
                    vocoder.SetFloatParameter(Hv_vocoderMod_AudioLib.Parameter.Ramp_note, System.Int32.Parse(m.data1.ToString()));
					
                }
            }
        } 

    }

}