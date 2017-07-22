using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to Wand object to spawn particles upon collision with ribbon
/// </summary>

public class RibbonCollision : MonoBehaviour {

    public GameObject particlePrefab;
    public Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other) {

        Debug.Log("collision occured");

        if (other.transform.parent != null) {
            if (other.transform.parent.parent.gameObject.tag == "MarkerParent") {

                Debug.Log("wand-ribbon collision occured");

                GameObject newParticles = Instantiate(particlePrefab, transform.position + offset, Quaternion.identity);

                ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
                var main = ps.main;

                main.startColor = other.gameObject.GetComponent<Renderer>().material.color;

                Destroy(newParticles, 3.0f);

            }
        }

    }
}
