using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to Wand object to spawn particles upon collision with ribbon
/// </summary>

public class RibbonCollision : MonoBehaviour {

    public GameObject particlePrefab;
    public Vector3 offset;

	public float yOffset;

	DrawRibbon drawRibbonScript;

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

				GameObject newParticles = Instantiate(particlePrefab, transform.position + (Vector3.up * yOffset) , Quaternion.identity);

                ParticleSystem ps = newParticles.GetComponent<ParticleSystem>();
                var main = ps.main;

                main.startColor = new ParticleSystem.MinMaxGradient( other.gameObject.GetComponent<Renderer>().material.color, Color.white);

                Destroy(newParticles, 3.0f);

            }
        }

    }
}
