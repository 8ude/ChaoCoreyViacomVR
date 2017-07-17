using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RibbonMotion : MonoBehaviour {

	SpectrumAnalysis analyzer;
	//float prevAnalyzerValue, prevYPos;
	public float dampening = 0.1f;
	public float smoothingValue = 0.1f;

	public int frameCount = 10;
	public int frameCountDown;


	// Use this for initialization
	void Start () {
		//prevAnalyzerValue = 0f;
		//prevYPos = transform.position.y;
		analyzer = GetComponentInParent<SpectrumAnalysis> ();	
		frameCountDown = frameCount;
	}
	
	// Update is called once per frame
	void Update () {

	
		frameCountDown--;
		if (frameCountDown < 0) {
			FindNextPoint ();
			frameCountDown = frameCount;
		}

	}


	//every n frames, travel to a new point in n frames
	//may want to check if abs(analyzerValue) > threshold instead
	//blah
	void FindNextPoint() {
		float nextYPos = analyzer.GetWholeEnergy () * dampening;
		transform.DOLocalMoveY (nextYPos, (frameCount/90f));
	}
}
