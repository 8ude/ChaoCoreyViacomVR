﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EraseRibbonAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EraseRibbon(Vector3[] vertices, float timeToErase) {

		Sequence eraseSequence = DOTween.Sequence();

		float totalDistance = 0f;

		for (int i = 1; i < vertices.Length; i ++ ) {

			totalDistance += Vector3.Distance(vertices[i-1], vertices[i]);
				
		}

		float timePerSegment =  timeToErase / ((float) vertices.Length - 1f);

		//Debug.Log (vertices.Length - 1);
		//Debug.Log ("segment time " + timePerSegment);

		for (int i = 1; i < vertices.Length; i++) {
			if (i < vertices.Length - 1) {
				eraseSequence.Append (transform.DOMove (vertices [i], timePerSegment));

			} else eraseSequence.Append(transform.DOMove(vertices[i], timePerSegment).OnComplete(DestroySelf));
		}

		eraseSequence.Play ();

	}

	void DestroySelf() {
		Color currentColor = gameObject.GetComponent<Renderer> ().material.GetColor ("_TintColor");
		currentColor.a = 0.0f;
		gameObject.GetComponent<Renderer> ().material.DOVector (currentColor, "_TintColor", 1.0f);
		Destroy (gameObject, 3f);
	}
}