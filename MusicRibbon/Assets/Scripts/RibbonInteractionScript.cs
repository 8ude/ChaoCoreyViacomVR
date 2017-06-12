using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class RibbonInteractionScript : MonoBehaviour {

    /// <summary>
    /// This script controls player interaction with Ribbon
    /// </summary>

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //this happens whenever a ahand is near the object
    void HandHoverUpdate( Hand hand) {

        if (hand.GetStandardInteractionButtonDown() && hand.AttachedObjects.Count == 0) {
            hand.AttachObject(gameObject);
            
        }

    }

    //This is called whenever the object is attached to the player's hand
    void OnAttachedToHand(Hand hand) {

        gameObject.BroadcastMessage("PickedUpByPlayer");


    }

    void HandAttachedUpdate(Hand hand) {

        //Here we'll do some stuff

    }

}
