using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroClipMaker : MonoBehaviour {

    int fadeSamples = 100;

    public static AudioClip MakeMicroClip (AudioClip origClip, int totalSegments, int segmentIndex, float sizeAdjust) {
        

        AudioClip newClip = AudioClip.Create("MicroClip", Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * sizeAdjust),
                                             origClip.channels, origClip.frequency, false);
        float[] newClipData = new float[Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * sizeAdjust)];
        float[] origClipData = new float[Mathf.RoundToInt(origClip.length * origClip.frequency * origClip.channels)];
        origClip.GetData(origClipData, 0);

        int clipDataStart = Mathf.RoundToInt((origClip.length / totalSegments) * origClip.frequency * origClip.channels * segmentIndex);

        for (int i = 0; i < newClipData.Length; i++) {
            newClipData[i] = origClipData[clipDataStart + i];
        }

        return newClip;

    }
    
}
