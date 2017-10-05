using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpike : MonoBehaviour {

    Vector3 OriginalPosition;

    public float soundSpikeScale = 2;

    AudioSource mySource;

    public AudioClip[] myClips;

    public float pitchJitter = 0.1f;

    public Vector3 myVertex;

	// Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
        OriginalPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.magnitude/OriginalPosition.magnitude > soundSpikeScale && !mySource.isPlaying ) {
            PlaySoundSpike();
        }

	}

    public void PlaySoundSpike() {

        mySource.clip = myClips[Random.Range(0, myClips.Length)];
        mySource.pitch = 1f + pitchJitter * Random.value; 
        mySource.Play();

    }
}
