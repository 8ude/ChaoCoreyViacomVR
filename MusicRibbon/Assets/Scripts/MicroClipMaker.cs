﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroClipMaker : MonoBehaviour {

    int fadeSamples = 100;

    public static AudioClip MakeMicroClip (AudioClip origClip, int totalSegments, int segmentIndex, float sizeAdjust) {
        

        AudioClip newClip = AudioClip.Create("MicroClip", Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * sizeAdjust),
                                             origClip.channels, origClip.frequency, false);
        float[] newClipData = new float[Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * sizeAdjust)];
        float[] origClipData = new float[Mathf.RoundToInt(origClip.samples * origClip.channels)];
        origClip.GetData(origClipData, 0);

        int clipDataStart = Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * segmentIndex);
        //Debug.Log("microclip start index: " + clipDataStart);
        //Debug.Log("microclip total samples: " + origClipData.Length);

        int tenthLengthSamples = Mathf.RoundToInt(newClipData.Length / 10f);

        for (int i = 0; i < newClipData.Length; i++) {
            newClipData[i] = origClipData[clipDataStart + i];

        }
        for (int i = 0; i < tenthLengthSamples; i++) {
            newClipData[i] *= Mathf.Lerp(0, 1, (float)i / tenthLengthSamples);
            newClipData[newClipData.Length - i] *= Mathf.Lerp(0, 1, (float)i / tenthLengthSamples);
        }
        newClip.SetData(newClipData, 0);
        return newClip;

    }
    
}
