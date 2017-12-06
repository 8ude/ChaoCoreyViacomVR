﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Beat;
using DG.Tweening;

/// <summary>
/// Attach to Wand object to spawn particles upon collision with ribbon
/// </summary>

public class RibbonCollision : MonoBehaviour {

    public GameObject particlePrefab;
    public GameObject DrumParticlePrefab;
    public GameObject BassParticlePrefab;
    public GameObject MelodyParticlePrefab;
    public GameObject HarmonyParticlePrefab;
    public Material AudioReactiveMaterial;
    public AudioSource mySource;
    AudioClip melodyClip;
    public AudioMixerGroup mutedGroup, origGroup;

    public AudioMixer audioMixer;

    public float mixerLPFreq;

    public Transform wandTip;

    //rate at which melody ribbons trigger - ideally the closer the ribbon,
    // the more they overlap
    public float MicroClipCooldown;
    [SerializeField] float MicroClipTimer;

    public float TriggerCoolDown;
    [SerializeField] float triggerTimer;
 
    
    
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
        MicroClipTimer = 0f;
        MicroClipCooldown = Clock.Instance.SixteenthLength();

        triggerTimer = 0f;
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    private void FixedUpdate() {
        MicroClipTimer += Time.fixedDeltaTime;
        triggerTimer += Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider other) {



        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent" && triggerTimer > TriggerCoolDown) {

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

                triggerTimer = 0f;
                
            }
        }
    }
    void OnTriggerStay(Collider other) {


        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            //check to see if this object is a child and we're not erasing

            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {
                //check the tag on the root-level parent 
                //(marker parent contains ribbon generator script as well as the 
                // spline generator)


                //set the audio clip in accordance with the collision audio in the game manager

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                //if trigger is in the melody or bass stem, we want a different kind of interaction
                MicroClipTimer += Time.fixedDeltaTime;
                    
				if (MicroClipTimer >= MicroClipCooldown) 
				{
                    RibbonCollisionStay(other);
                    if(other.gameObject.name == "Create Mesh_5_Mesh000")
                    {
                        //SAVE FOR LATER, also there's a reference to the mesh in collider.root.GetComponent<RibbonGenerator>();

                        //Debug.Log(other.gameObject.name);
                        //AudioReactiveMaterial.
                    }
                   
                }

                triggerTimer = 0f;

                other.transform.root.GetComponentInChildren<AudioShaderReact>().WandInteract(transform);

            }
        }
        
    }
    void OnTriggerExit(Collider other) {
        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                MicroClipTimer = 0f; //clear timer

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();

                ribbonSound.autoMoveSound = true;
                ribbonSound.myHighSource.outputAudioMixerGroup = origGroup;
                ribbonSound.myLowSource.outputAudioMixerGroup = origGroup;


                melodyClip = null;
                mySource.Stop();
                mySource.clip = null;

                playingMicroSample = false;

            }
            RibbonCollisionExit(other);

            triggerTimer = 0f;
            
            //other.gameObject.transform.root.GetComponentInChildren<DrawRibbonSound>().RestartClips(0);
        }
        
    }


    void OnCollisionExit(Collision collision) {
        //TODO - find nearest marker, somehow translate that to an index 

        collision.gameObject.transform.root.GetComponentInChildren<DrawRibbonSound>().RestartClips(0);
        Debug.Log("gaaaah");

    }

    public void RibbonCollisionExit(Collider other) {
        MarkerObjectBehavior[] markers = other.transform.root.GetComponentsInChildren<MarkerObjectBehavior>();

        //find closest marker
        float[] markerDistances = new float[markers.Length];
        int closestMarkerIndex = 0;
        float closestMarkerDistance = 3f;
        for (int i = 0; i < markers.Length; i++)
        {
            markerDistances[i] = Vector3.Distance(markers[i].gameObject.transform.position, transform.position);
            if (markerDistances[i] < closestMarkerDistance)
            {
                closestMarkerIndex = i;
                closestMarkerDistance = markerDistances[i];
            }
        }

        //Get the ribbon sound component for the audio sources
        DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();

        //divide the clip up into half notes (round down)
        int halfLengthOfClip = Mathf.FloorToInt(ribbonSound.myHighSource.clip.length / Clock.Instance.HalfLength());

        //find the half note index of our nearest marker (four eigth notes per half note)
        int halfNoteIndex = Mathf.FloorToInt((float)closestMarkerIndex / 4f);

        //pause the audio sources, set the time according to the index, then play at the next beat
        ribbonSound.myHighSource.Pause();
        ribbonSound.myLowSource.Pause();

        ribbonSound.myHighSource.time = halfNoteIndex * Clock.Instance.HalfLength();
        ribbonSound.myLowSource.time = halfNoteIndex * Clock.Instance.HalfLength();

        ribbonSound.myHighSource.PlayScheduled(Clock.Instance.AtNextQuarter());
        ribbonSound.myLowSource.PlayScheduled(Clock.Instance.AtNextQuarter());

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



        DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();

        //trying to get the mesh displacement to occur where the collision is occuring
        ribbonSound.autoMoveSound = false;
        ribbonSound.transform.position = transform.position;

        //mute the main stem while we do the micro thingy
        ribbonSound.myHighSource.outputAudioMixerGroup = mutedGroup;
        ribbonSound.myLowSource.outputAudioMixerGroup = mutedGroup;
        melodyClip = ribbonSound.myHighSource.clip;
		
        AudioClip microClip = MicroClipMaker.MakeMicroClip(melodyClip, markers.Length, closestMarkerIndex, 0.2f);
        //reset the triggerCooldown and play the clip
        MicroClipTimer = 0f;
        AudioSource.PlayClipAtPoint(microClip, transform.position, 0.3f);
        
    }



    public void DrumRibbonCollisionEnter(Collider other) {
		
		GameObject newParticles = Instantiate(DrumParticlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
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

		GameObject newParticles = Instantiate(BassParticlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
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

		GameObject newParticles = Instantiate(MelodyParticlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
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
		GameObject newParticles = Instantiate(HarmonyParticlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
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


    //    NO LONGER USING Bass Ribbon Low Pass Method
    //
    //    public void BassRibbonCollision(Collider other) {
    //        //TODO check on VR to see if bug is fixed (renamed LP filter to be the same as exposed parameter)
    //
    //        MarkerObjectBehavior[] markers = other.transform.root.GetComponentsInChildren<MarkerObjectBehavior>();
    //
    //      //find closest marker and second closest marker
    //      float[] markerDistances = new float[markers.Length];
    //        float[] lowPassFrequencies = new float[markers.Length];
    //      int closestMarkerIndex = 0;
    //        int secondClosestMarkerIndex = 0;
    //      float closestMarkerDistance = 1f;
    //        float secondClosestMarkerDistance = 1f;
    //      for (int i = 0; i < markers.Length; i++) {
    //            //populate an array with cutoff frequencies, corresponding to points along the ribbon
    //          markerDistances[i] = Vector3.Distance(markers[i].gameObject.transform.position, transform.position);
    //            lowPassFrequencies[i] = (float) 6000f / markers.Length * i;
    //          if (markerDistances[i] < closestMarkerDistance) {
    //                secondClosestMarkerIndex = closestMarkerIndex;
    //                secondClosestMarkerDistance = closestMarkerDistance;
    //                    
    //              closestMarkerIndex = i;
    //              closestMarkerDistance = markerDistances[i];
    //          }
    //      }
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
    //      DrawRibbonSound ribbonSound = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>();
    //
    //      //trying to get the mesh displacement to occur where the collision is occuring
    //      ribbonSound.autoMoveSound = false;
    //
    //      ribbonSound.transform.position = transform.position;
    //
    //    }

}
