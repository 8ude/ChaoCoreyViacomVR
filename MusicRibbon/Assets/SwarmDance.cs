using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kvant;

public class SwarmDance : MonoBehaviour {

	public ListenerSpectrumAnalysis listenerSpectrum;
	public SkyboxColorChange skyboxChanger;

	float prevWidth;
	float prevYFlow;
	Swarm swarm;

	public float widthSmoothing = 0.01f;
	public float heightSmoothing = 0.01f;

	void Awake() {
		swarm = GetComponent<Swarm> ();
		prevWidth = 0f;
		prevYFlow = 0f;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		swarm.lineWidth += (listenerSpectrum.GetWholeEnergy () - prevWidth) * widthSmoothing;
		prevWidth = swarm.lineWidth;

		swarm.flow = new Vector3 (0f, swarm.flow.y + (4f*listenerSpectrum.GetWholeEnergy () - prevYFlow) * heightSmoothing, 0f);
		prevYFlow = swarm.flow.y;

		swarm.color = skyboxChanger.topColor;

	}
}
