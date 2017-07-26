using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderParticles : MonoBehaviour {
	public ListenerSpectrumAnalysis listenerSpectrum;
	ParticleSystem myParticleSystem;

	public float baseEmissionRate = 100f;
	public float startEmissionRate = 0f;
	float emissionRate;
	public float velocityMultiplier = 10f;

	// Use this for initialization
	void Start () {
		myParticleSystem = GetComponent<ParticleSystem> ();
		var emission = myParticleSystem.emission;
		emission.rateOverTime = startEmissionRate;

	}
	
	// Update is called once per frame
	void Update () {
		var emission = myParticleSystem.emission;
		emission.rateOverTime = listenerSpectrum.GetWholeEnergy () * baseEmissionRate;

		var velocityOverLifetime = myParticleSystem.velocityOverLifetime;
		velocityOverLifetime.zMultiplier = listenerSpectrum.GetWholeEnergy () * velocityMultiplier;

		if (listenerSpectrum.GetWholeEnergy () > 0.5f) {
			emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0.0f, 25, 
				(short)(listenerSpectrum.GetWholeEnergy () * baseEmissionRate)
			)});
		}
	}
}
