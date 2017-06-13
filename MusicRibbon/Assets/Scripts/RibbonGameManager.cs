using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonGameManager : MonoBehaviour {

    //We don't need this quite yet, but probably will in the near future

    public GameObject[] RibbonObjects;

    public Material ribbonOffMaterial;

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
		
	}

    public float RemapRange(float value, float oldMin, float oldMax, float newMin, float newMax) {
        return newMin + (value - oldMin) * (newMax - newMin) / (oldMax - oldMin);
    }
}
