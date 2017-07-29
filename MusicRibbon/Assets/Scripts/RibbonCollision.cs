using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Attach to Wand object to spawn particles upon collision with ribbon
/// </summary>

public class RibbonCollision : MonoBehaviour {

    public GameObject particlePrefab;
    
    //the distance along the wand where the particles will spawn
	public float yOffset;

    // use DrawRibbon.cs to tell if we are erasing or not
	DrawRibbon drawRibbonScript;

    //using this to make sure we cycle through the collision audio clips
    int clipIndex;

	// Use this for initialization
	void Start () {

		drawRibbonScript = transform.root.GetComponentInChildren<DrawRibbon> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {

        Debug.Log("collision occured");

		if (other.transform.parent != null && !drawRibbonScript.eraseRibbon.isErasing) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                Debug.Log("wand-ribbon collision occured");

				GameObject newParticles = Instantiate(particlePrefab, transform.position + (transform.up * yOffset) , Quaternion.identity);
                
                //set the audio clip in accordance with the collision audio in the game manager
                AudioSource aSource = newParticles.GetComponent<AudioSource>();

                ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
                var main = ps.main;

                //Particle color will be somewhere between white and the ribbon color
                main.startColor = new ParticleSystem.MinMaxGradient( other.gameObject.GetComponent<Renderer>().material.color, Color.white);

                Destroy(newParticles, 3.0f);

            }
        }

    }
}
