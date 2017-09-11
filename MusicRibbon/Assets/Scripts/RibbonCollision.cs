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
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    void OnTriggerEnter(Collider other) {

        //Debug.Log("collision occured");

        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                //Debug.Log("wand-ribbon collision occured");

                //set the audio clip in accordance with the collision audio in the game manager

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                //if we hit the melody stem, we want a different kind of interaction

                if (collidedStemType != RibbonGenerator.musicStem.Melody) {

                    GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset), Quaternion.identity);
                    AudioSource aSource = newParticles.GetComponent<AudioSource>();
                    switch (collidedStemType) {
                        case RibbonGenerator.musicStem.Bass:
                            aSource.clip = RibbonGameManager.instance.bassCollisionClips[clipIndex % RibbonGameManager.instance.bassCollisionClips.Length];
                            break;
                        case RibbonGenerator.musicStem.Drum:
                            aSource.clip = RibbonGameManager.instance.drumCollisionClips[clipIndex % RibbonGameManager.instance.drumCollisionClips.Length];
                            break;
                        case RibbonGenerator.musicStem.Harmony:
                            aSource.clip = RibbonGameManager.instance.harmonyCollisionClips[clipIndex % RibbonGameManager.instance.harmonyCollisionClips.Length];
                            break;

                    }
                    aSource.Play();
                    ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
                    var main = ps.main;

                    //Particle color will be somewhere between white and the ribbon color
                    main.startColor = new ParticleSystem.MinMaxGradient(other.gameObject.GetComponent<Renderer>().material.color, Color.white);

                    Destroy(newParticles, 2.0f);
                    clipIndex++;
                }
                else {
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
                    melodyClip = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>().mySource.clip;
                    other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>().mySource.outputAudioMixerGroup = mutedGroup;
                    mySource.clip = MicroClipMaker.MakeMicroClip(melodyClip, markers.Length, closestMarkerIndex, Mathf.Clamp(1f / closestMarkerDistance, 0.2f, 1f));
                    mySource.Play();
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

                //if we hit the melody stem, we want a different kind of interaction


                if (collidedStemType == RibbonGenerator.musicStem.Melody && !mySource.isPlaying) {
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

                    melodyClip = other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>().mySource.clip;
                    mySource.clip = MicroClipMaker.MakeMicroClip(melodyClip, markers.Length, closestMarkerIndex, Mathf.Clamp(1f / closestMarkerDistance, 0.2f, 1f));
                    mySource.Play();
                }
            }
        }
        
    }
    void OnTriggerExit(Collider other) {
        if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                RibbonGenerator collidedGenerator = other.transform.root.GetComponent<RibbonGenerator>();
                RibbonGenerator.musicStem collidedStemType = collidedGenerator.myStem;

                melodyClip = null;
                mySource.Stop();
                mySource.clip = null;
                if (collidedStemType == RibbonGenerator.musicStem.Melody) {
                    other.transform.parent.parent.GetComponentInChildren<DrawRibbonSound>().mySource.DOFade(0.3f, 0.3f);

                }

            }
        }
        
    }
}
