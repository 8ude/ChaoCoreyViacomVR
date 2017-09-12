using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    public void OnTriggerEnter(Collider other) {

        if(other.transform.parent.parent.tag == "Controller") {
            SceneManager.LoadScene("Echo");
        }


    }


	public void Scenechange(){

		SceneManager.LoadScene ("Echo");
	}

    

}
	
