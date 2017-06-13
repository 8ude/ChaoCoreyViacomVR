using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class RibbonInteractionScript : MonoBehaviour {

    /// <summary>
    /// This script controls player interaction with Ribbon
    /// </summary>
  
    public bool isCarriedByPlayer;

    //We want to store this value for when we want to manipulate the volume
    public float StemVolume = 1.0f;

    Material initSphereMaterial;

	// Use this for initialization
	void Start () {

        isCarriedByPlayer = false;
        initSphereMaterial = GetComponent<Renderer>().material;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //this happens whenever a ahand is near the object
    void HandHoverUpdate( Hand hand) {


        if (hand.GetStandardInteractionButtonDown() && hand.AttachedObjects.Count == 1) {
            if (!isCarriedByPlayer) {
                //condition when the player picks up an object
                hand.AttachObject(gameObject);
                gameObject.GetComponent<RibbonMovement>().PickedUpByPlayer();
                isCarriedByPlayer = true;
            } else if (isCarriedByPlayer) {
                //condition when the player alread has the object picked up, and uses the trigger from the other controller
                ToggleAudioOn();

            }

        }
    }

    //This is called whenever the object is attached to the player's hand
    void OnAttachedToHand(Hand hand) {

        gameObject.BroadcastMessage("PickedUpByPlayer");
        isCarriedByPlayer = true;
        GetComponent<AudioHighPassFilter>().enabled = true;

    }

    void HandAttachedUpdate(Hand hand) {
        
        //
        if(hand.GetStandardInteractionButtonDown()) {

            hand.DetachObject(gameObject);

        }

 
        GetComponent<AudioHighPassFilter>().cutoffFrequency = RibbonGameManager.instance.RemapRange(transform.position.y, 0f, 3f, 10f, 1000f);
        
    }

    void OnDetachedFromHand(Hand hand) {

        gameObject.BroadcastMessage("ReleasedByPlayer");
        isCarriedByPlayer = false;
        GetComponent<AudioHighPassFilter>().enabled = false;

    }

    public void ToggleAudioOn() {

        AudioSource myAudio = GetComponent<AudioSource>();

        if (myAudio.volume != 0.0f) {

            myAudio.volume = 0.0f;

            gameObject.GetComponent<Renderer>().material = RibbonGameManager.instance.ribbonOffMaterial;

            gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;

        } else {
            myAudio.volume = StemVolume;
            gameObject.GetComponent<Renderer>().material = initSphereMaterial;
            gameObject.GetComponentInChildren<TrailRenderer>().enabled = true;
        }

        
    }

}
