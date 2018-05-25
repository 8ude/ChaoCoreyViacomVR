using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyUnderware.Curvy;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using FluffyUnderware.Curvy.Controllers;
using FluffyUnderware.Curvy.Shapes;
using FluffyUnderware.DevTools;
using UnityEngine.UI;

using DG.Tweening;

/// <summary>
/// Supplements CurvyGenerator (spline mesh generator) with game-specific variables and methods
/// Stores references to spline and ribbon mesh, and confirms when they have been initialized
/// </summary>
public class RibbonGenerator : MonoBehaviour {
	
	public CurvySpline ribbonSpline;
	public MeshFilter ribbonMesh;
	public MeshRenderer ribbonRenderer;

	public CurvyGenerator curvyGenerator;
	public DrawRibbonSound drawRibbonSound;

	public enum musicStem {Bass = 0, Drum = 1, Harmony = 2, Melody = 3};
	public musicStem myStem;
	public int stemIntValue;


    public float lifeTime;
	float fadeoutTime;

	public bool fadingOut;

	public float ribbonLength;

	//float endWidth = 0.1f;
	//float endHeight = 0.5f;

	public Color myColor = new Color(1,1,1);

	public float transparency = 0f;


	// Use this for initialization
	IEnumerator Start () {
		lifeTime = 0f;
		fadingOut = false;

		fadeoutTime = RibbonGameManager.instance.autoKillFadeOutTime;


		myStem = (musicStem)stemIntValue;

		curvyGenerator = GetComponent<CurvyGenerator> ();
		ribbonRenderer = null;

		while (!ribbonSpline.IsInitialized) {
			yield return null;
		}

		drawRibbonSound = GetComponentInChildren<DrawRibbonSound> ();
		
		StartCoroutine (WaitForMeshRenderer());
		
	}
	
	// Update is called once per frame
	void Update () {

		lifeTime += Time.deltaTime;

		if (lifeTime > RibbonGameManager.instance.autoKillLifetime  && RibbonGameManager.instance.autoKillRibbons) {

			lifeTime = 0f;
			FadeOutRibbon (fadeoutTime);
            RibbonGameManager.instance.ribbonObjects.Remove(gameObject);
			Destroy (gameObject, fadeoutTime);

		}

		if (ribbonRenderer) {
			ribbonRenderer.material.SetFloat ("_InputAlpha", transparency);
		}


	}

	IEnumerator WaitForMeshRenderer() {

        //wait for Curvy to generate the mesh from spline points
        while (GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>() == null)   {
            yield return null;
        }

        if (GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>()) {
			
            ribbonRenderer = GetComponentInChildren<CreateMesh>().transform.GetComponentInChildren<MeshRenderer>();
			ribbonMesh = GetComponentInChildren<CreateMesh> ().transform.GetComponentInChildren<MeshFilter> ();
			Mesh mesh = ribbonMesh.mesh;

        
			MeshHelper.Subdivide (mesh, RibbonGameManager.instance.ribbonMeshSmoothing);
			ribbonMesh.mesh = mesh;

        } 

		while (ribbonRenderer == null) {
			yield return null;
		}


        SetMaterialValues();

		ribbonRenderer.material.color = myColor;
		DOTween.To (() => transparency, x => transparency = x, 1, 1);

        //Find the Marker objects that were used to spawn this ribbon, turn off their renderers
        MarkerObjectBehavior[] markerObjects = GetComponentsInChildren<MarkerObjectBehavior>();
        foreach(MarkerObjectBehavior markerBehavior in markerObjects) {
            markerBehavior.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        //TODO - determine why spline object is being destroyed
		CurvySpline mySpline = GetComponentInChildren<CurvySpline> ();
		Destroy (mySpline.gameObject);

        //TODO - invoke method for re-balancing audio stems
        //Better way of doing this? 
        RibbonGameManager.instance.ribbonObjects.Add(gameObject);

	}

    void SetMaterialValues() {
        //Set Mesh Material Values from RibbonAudioShaderManager
        switch (myStem)
        {
            case musicStem.Bass:
                myColor = RibbonGameManager.instance.bassColor;

                ribbonRenderer.material.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.BassPosTurbulence);
                ribbonRenderer.material.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.BassWaveShudder);
                ribbonRenderer.material.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.BassOverallTurbulence);
                ribbonRenderer.material.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.BassTurbulenceSpeed);
                ribbonRenderer.material.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.BassSpikiness);
                ribbonRenderer.material.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.BassColorShift);
                break;
            case musicStem.Drum:
                myColor = RibbonGameManager.instance.drumColor;

                ribbonRenderer.material.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.DrumPosTurbulence);
                ribbonRenderer.material.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.DrumWaveShudder);
                ribbonRenderer.material.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.DrumOverallTurbulence);
                ribbonRenderer.material.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.DrumTurbulenceSpeed);
                ribbonRenderer.material.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.DrumSpikiness);
                ribbonRenderer.material.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.DrumColorShift);
                break;
            case musicStem.Melody:
                myColor = RibbonGameManager.instance.melodyColor;

                ribbonRenderer.material.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.HarmonyPosTurbulence);
                ribbonRenderer.material.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.HarmonyWaveShudder);
                ribbonRenderer.material.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.HarmonyOverallTurbulence);
                ribbonRenderer.material.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.HarmonyTurbulenceSpeed);
                ribbonRenderer.material.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.HarmonySpikiness);
                ribbonRenderer.material.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.HarmonyColorShift);
                break;
            case musicStem.Harmony:
                myColor = RibbonGameManager.instance.harmonyColor;

                ribbonRenderer.material.SetFloat("_PosTurb", RibbonAudioShaderManager.Instance.MelodyPosTurbulence);
                ribbonRenderer.material.SetFloat("_WaveShud", RibbonAudioShaderManager.Instance.MelodyWaveShudder);
                ribbonRenderer.material.SetFloat("_Turbulence", RibbonAudioShaderManager.Instance.MelodyOverallTurbulence);
                ribbonRenderer.material.SetFloat("_TurbulenceSpeed", RibbonAudioShaderManager.Instance.MelodyTurbulenceSpeed);
                ribbonRenderer.material.SetFloat("_Spikiness", RibbonAudioShaderManager.Instance.MelodySpikiness);
                ribbonRenderer.material.SetFloat("_ColorShift", RibbonAudioShaderManager.Instance.MelodyColorShift);
                break;
        }
    }

	public void FadeOutRibbon(float time) {
		if (!fadingOut) {
			fadingOut = true;
			DOTween.To (() => transparency, x => transparency = x, 0, time);
			drawRibbonSound.myHighSource.DOFade (0.01f, time);
            Invoke("RemoveRibbon", time - 0.5f);
			Destroy (gameObject, time);
		}
	}

    public void RemoveRibbon() {
        RibbonGameManager.instance.ribbonObjects.Remove(gameObject);
    }




}
