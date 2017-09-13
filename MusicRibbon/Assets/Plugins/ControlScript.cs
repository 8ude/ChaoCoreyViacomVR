using UnityEngine;
using System.Collections;

public class ControlScript : MonoBehaviour {

	public Hv_Heavyproject_LibWrapper HeavyScript;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("space"))
			
		{
			HeavyScript.SendBangToReceiver ("onOff");
			//print ("space pressed");
		}
		
		else if (Input.GetKeyDown ("q"))

		{
			HeavyScript.SendBangToReceiver ("waveToggle");
			//print ("space pressed");

		}

		else if (Input.GetKey ("a"))
			
		{
			HeavyScript.freq -= 2;
			//print ("space pressed");
		}

		else if (Input.GetKey ("s"))
			
		{
			HeavyScript.freq += 2;
			//print ("space pressed");
		}



		else
			{
			return;
			}
		}

}


