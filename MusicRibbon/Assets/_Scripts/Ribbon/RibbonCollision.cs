using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

/// <summary>
/// Attach to Wand object to spawn particles upon collision with ribbon
/// </summary>

public class RibbonCollision : MonoBehaviour {

    public GameObject particlePrefab;
    public AudioSource mySource;
    AudioClip melodyClip;
    public AudioMixerGroup mutedGroup, origGroup;

    public AudioMixer audioMixer;

    public float mixerLPFreq;

    public Transform wandTip;

    //rate at which melody ribbons trigger - ideally the closer the ribbon,
    // the more they overlap
    public float TriggerCooldown;
    float TriggerTimer;
 
    
    //the distance along the wand where the particles will spawn
    public float yOffset;

    // use DrawRibbon.cs to tell if we are erasing or not
    DrawRibbon drawRibbonScript;

    bool playingMicroSample = false;

    //using this to make sure we cycle through the collision audio clips
    int clipIndex;

    // Use this for initialization
    void Start () {
        
        drawRibbonScript = transform.root.GetComponentInChildren<DrawRibbon> ();
        melodyClip = null;
        mySource = GetComponent<AudioSource>();
        audioMixer = mySource.outputAudioMixerGroup.audioMixer;
        playingMicroSample = false;
        TriggerTimer = 0f;
        TriggerCooldown = 0f;
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void OnTriggerEnter(Collider other) {



        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                //set the audio clip in accordance with the collision audio in the game manager

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                //now different collision types for each instrument

                switch (collidedStemType) {
                    case RibbonGenerator.musicStem.Melody:
                        MelodyRibbonCollisionEnter(other);
                        break;
                    case RibbonGenerator.musicStem.Bass:
						BassRibbonCollisionEnter(other);
                        break;
                    case RibbonGenerator.musicStem.Drum:
						DrumRibbonCollisionEnter(other);
                        break;
                    case RibbonGenerator.musicStem.Harmony:
						HarmonyRibbonCollisionEnter(other);
                        break;
                }
                
            }
        }
    }
    void OnTriggerStay(Collider other) {


        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

            

                //set the audio clip in accordance with the collision audio in the game manager

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                //if trigger is in the melody or bass stem, we want a different kind of interaction
                TriggerTimer += Time.fixedDeltaTime;
                    
				if (TriggerTimer >= TriggerCooldown) 
				{
                    RibbonCollisionStay(other);
                } 
            }
        }
        
    }
    void OnTriggerExit(Collider other) {
        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();

				switch (collidedStemType) {
					case RibbonGenerator.musicStem.Melody:
						
                        ribbonSound.autoMoveSound = true;
                        ribbonSound.myHighSource.outputAudioMixerGroup = origGroup;
						break;
					case RibbonGenerator.musicStem.Bass:
						
						ribbonSound.autoMoveSound = true;
						break;
					case RibbonGenerator.musicStem.Drum:

						break;
					case RibbonGenerator.musicStem.Harmony:
                        
						break;
				}


                melodyClip = null;
                mySource.Stop();
                mySource.clip = null;
                if (collidedStemType == RibbonGenerator.musicStem.Melody) {
                    other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>().myHighSource.outputAudioMixerGroup = origGroup;

                }
                playingMicroSample = false;
                audioMixer.SetFloat("GlobalLPFreq", 22000);
            } 
        }
        
    }

    void OnAudioFilterRead(float[] data, int channels) {
        if (!playingMicroSample)
            return;
    }


    public void RibbonCollisionStay(Collider other) {
        
		//melody ribbon - touching ribbon will create microsampled fragment
		
        playingMicroSample = true;
		MarkerObjectBehavior[] markers = other.transform.root.GetComponentsInChildren<MarkerObjectBehavior>();
		
        //find closest marker
		float[] markerDistances = new float[markers.Length];
		int closestMarkerIndex = 0;
		float closestMarkerDistance = 3f;
		for (int i = 0; i < markers.Length; i++) {
			markerDistances[i] = Vector3.Distance(markers[i].gameObject.transform.position, transform.position);
			if (markerDistances[i] < closestMarkerDistance) {
				closestMarkerIndex = i;
				closestMarkerDistance = markerDistances[i];
			}
		}
		
        //adjust the size of the clip based on wand distance from the marker
        float sizeAdjust = Mathf.Clamp(closestMarkerDistance, 0.1f, 1f);


		//Debug.Log("Closest Marker Distance: " + closestMarkerDistance);
		//Debug.Log("size adjust: " + sizeAdjust);


        DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();

        //trying to get the mesh displacement to occur where the collision is occuring
        ribbonSound.autoMoveSound = false;
        ribbonSound.transform.position = transform.position;

        //mute the main stem while we do the micro thingy
        ribbonSound.myHighSource.outputAudioMixerGroup = mutedGroup;
        melodyClip = ribbonSound.myHighSource.clip;
		
        AudioClip microClip = MicroClipMaker.MakeMicroClip(melodyClip, markers.Length, closestMarkerIndex, Mathf.Clamp(sizeAdjust, 0.5f, 1f));
        //reset the triggerCooldown and play the clip
        TriggerCooldown = 0.8f * microClip.length;
        mySource.PlayOneShot(microClip);
        
    }

//    public void BassRibbonCollision(Collider other) {
//        //TODO check on VR to see if bug is fixed (renamed LP filter to be the same as exposed parameter)
//
//        MarkerObjectBehavior[] markers = other.transform.root.GetComponentsInChildren<MarkerObjectBehavior>();
//
//		//find closest marker and second closest marker
//		float[] markerDistances = new float[markers.Length];
//        float[] lowPassFrequencies = new float[markers.Length];
//		int closestMarkerIndex = 0;
//        int secondClosestMarkerIndex = 0;
//		float closestMarkerDistance = 1f;
//        float secondClosestMarkerDistance = 1f;
//		for (int i = 0; i < markers.Length; i++) {
//            //populate an array with cutoff frequencies, corresponding to points along the ribbon
//			markerDistances[i] = Vector3.Distance(markers[i].gameObject.transform.position, transform.position);
//            lowPassFrequencies[i] = (float) 6000f / markers.Length * i;
//			if (markerDistances[i] < closestMarkerDistance) {
//                secondClosestMarkerIndex = closestMarkerIndex;
//                secondClosestMarkerDistance = closestMarkerDistance;
//                    
//				closestMarkerIndex = i;
//				closestMarkerDistance = markerDistances[i];
//			}
//		}
//
//        //Vector math to (theoretically) find wand's relative location along the ribbon
//        Vector3 markerVectorLine = markers[closestMarkerIndex].transform.position - markers[secondClosestMarkerIndex].transform.position;
//        float markerDistance = markerVectorLine.magnitude;
//        Vector3 markerALine = markers[closestMarkerIndex].transform.position - transform.position;
//        Vector3 markerBLine = markers[secondClosestMarkerIndex].transform.position - transform.position;
//        float angle = Vector3.Angle(markerVectorLine, markerBLine);
//        float projection = markerBLine.magnitude * Mathf.Cos(Mathf.Deg2Rad * angle);
//
//        //Adjust low pass filter to reflect how far wand is along the ribbon
//        audioMixer.DOSetFloat("GlobalLPFreq", lowPassFrequencies[closestMarkerIndex], 0.5f);
//
//		DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();
//
//		//trying to get the mesh displacement to occur where the collision is occuring
//		ribbonSound.autoMoveSound = false;
//
//		ribbonSound.transform.position = transform.position;
//
//    }

    public void DrumRibbonCollisionEnter(Collider other) {
		
		GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
		AudioSource aSource = newParticles.GetComponent<AudioSource>();
		aSource.clip = RibbonGameManager.instance.drumCollisionClips[clipIndex % RibbonGameManager.instance.drumCollisionClips.Length];
		aSource.Play();
		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;
		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient(other.gameObject.GetComponent<Renderer>().material.color, Color.white);

		Destroy(newParticles, 2.0f);
		clipIndex++;
        
    }

	public void BassRibbonCollisionEnter(Collider other) {

		GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
		AudioSource aSource = newParticles.GetComponent<AudioSource>();
		aSource.clip = RibbonGameManager.instance.bassCollisionClips[clipIndex % RibbonGameManager.instance.bassCollisionClips.Length];
		aSource.Play();
		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;
		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient(other.gameObject.GetComponent<Renderer>().material.color, Color.white);

		Destroy(newParticles, 2.0f);
		clipIndex++;

	}

	public void MelodyRibbonCollisionEnter(Collider other) {

		GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
		AudioSource aSource = newParticles.GetComponent<AudioSource>();
		aSource.clip = RibbonGameManager.instance.melodyCollisionClips[clipIndex % RibbonGameManager.instance.melodyCollisionClips.Length];
		aSource.Play();
		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;
		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient(other.gameObject.GetComponent<Renderer>().material.color, Color.white);

		Destroy(newParticles, 2.0f);
		clipIndex++;

	}

	public void HarmonyRibbonCollisionEnter(Collider other) {
		//not really sure what to do here ... keep the same as drum for now
		GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
		AudioSource aSource = newParticles.GetComponent<AudioSource>();
		aSource.clip = RibbonGameManager.instance.harmonyCollisionClips[clipIndex % RibbonGameManager.instance.harmonyCollisionClips.Length];
		aSource.Play();
		ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
		var main = ps.main;
		//Particle color will be somewhere between white and the ribbon color
		main.startColor = new ParticleSystem.MinMaxGradient(other.gameObject.GetComponent<Renderer>().material.color, Color.white);

		Destroy(newParticles, 2.0f);
		clipIndex++;
	}


}
