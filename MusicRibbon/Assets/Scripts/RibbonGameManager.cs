using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonGameManager : MonoBehaviour {

    //We don't need this quite yet, but probably will in the near future

	public List<GameObject> RibbonObjects;
	public int maxRibbons = 4;

	public bool LimitRibbonAmount = false;

    //public Material ribbonOffMaterial;

    public static RibbonGameManager instance = null;

    private void Awake() {

        if (instance == null) {
            instance = this;

        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (LimitRibbonAmount) {
			CapRibbons ();
		}


		
	}

	public float RemapRange(float value, float oldMin, float oldMax, float newMin, float newMax) {
		return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
	}

	public void CapRibbons() {

		if (RibbonObjects.Count > maxRibbons) {
			GameObject ribbonToDestroy = RibbonObjects [0];
			RibbonObjects.RemoveAt (0);
			Destroy (ribbonToDestroy);
		}

	}

}
