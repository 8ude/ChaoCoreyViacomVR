using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

	public float gameFadeTime;



    public void OnTriggerEnter(Collider other) {

        if(other.transform.parent.parent.tag == "Controller") {
			FadeToWhite ();
			Invoke("ResetGame", gameFadeTime);
        }


    }
		

	void FadeToWhite() {
		SteamVR_Fade.Start(Color.white, gameFadeTime);
	}

	void ResetGame() {

		SceneManager.LoadScene("Echo");
	}

    

}
	
