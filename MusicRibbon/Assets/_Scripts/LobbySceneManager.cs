using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnTriggerEnter(Collider other) {

		if(other.transform.parent.parent.tag == "Controller") {

			if (this.gameObject.name == "Scene1") {
				SceneManager.LoadScene (1);
			}
			else if (this.gameObject.name == "Scene2") {
				SceneManager.LoadScene (2);
			}
			else if (this.gameObject.name == "Scene3") {
				SceneManager.LoadScene (3);
			}
			else if (this.gameObject.name == "Scene4") {
				SceneManager.LoadScene (4);
			}
		}

	}
}
