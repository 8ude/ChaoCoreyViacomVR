using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

	public float gameFadeTime;

    void Update() {
        if (Input.GetKeyDown(KeyCode.W)) {
            FadeToWhite();
        }
    }
    public void OnTriggerEnter(Collider other) {

        if(other.transform.parent.parent.tag == "Controller") {

			FadeToWhite ();
			Invoke("ResetGame", gameFadeTime);
        }

    }
		

	void FadeToWhite() {
        Debug.Log("white");
		SteamVR_Fade.Start(Color.white, gameFadeTime);
	}

	void ResetGame() {
        Debug.Log("reset");
		SceneManager.LoadScene(0);
	}

    

}
	
