using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls synaesthesia values sent to ribbon mesh components
/// </summary>
public class AudioShaderManager : MonoBehaviour {

    public static AudioShaderManager Instance = null;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject); 
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
