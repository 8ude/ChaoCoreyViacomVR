using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkyboxColorChange : MonoBehaviour {

    public Material SkyboxMaterial;
    public new List<Color> Colors;
	public float[] ColorWeights;


    public GameObject[] drumRibbons;
    public GameObject[] bassRibbons;
    public GameObject[] melodyRibbons;
    public GameObject[] harmonyRibbons;

    public int drumRibbonsNum;
    public int bassRibbonsNum;
    public int melodyRibbonsNum;
    public int harmonyRibbonsNum;

	public ListenerSpectrumAnalysis ListenerSpectrum;



    // Use this for initialization
    void Start () {

        drumRibbonsNum = 0;
        bassRibbonsNum = 0;
        melodyRibbonsNum = 0;
        harmonyRibbonsNum = 0;

		ColorWeights = new float[4];

        SkyboxMaterial.SetColor("_SkyColor1", Colors[4]);
        SkyboxMaterial.SetColor("_SkyColor2", Colors[5]);
        SkyboxMaterial.SetColor("_SkyColor3", Colors[6]);
    }
	
	// Update is called once per frame
	void Update () {

		drumRibbons = RibbonGameManager.instance.drumRibbons;
        bassRibbons = RibbonGameManager.instance.bassRibbons;
		melodyRibbons = RibbonGameManager.instance.melodyRibbons;
		harmonyRibbons =  RibbonGameManager.instance.harmonyRibbons;

        drumRibbonsNum = drumRibbons.Length;
        bassRibbonsNum = bassRibbons.Length;
        melodyRibbonsNum = melodyRibbons.Length;
        harmonyRibbonsNum = harmonyRibbons.Length;

		if (RibbonGameManager.instance.totalRibbons != 0) {

			ColorWeights [0] = (float)drumRibbonsNum / RibbonGameManager.instance.totalRibbons;
			ColorWeights [1] = (float)bassRibbonsNum / RibbonGameManager.instance.totalRibbons;
			ColorWeights [2] = (float)melodyRibbonsNum / RibbonGameManager.instance.totalRibbons;
			ColorWeights [3] = (float)harmonyRibbonsNum / RibbonGameManager.instance.totalRibbons;

		} else {
			for (int i = 0; i < 4; i++) {
				ColorWeights [i] = 0;
			}
		}
        TopColorChange();
        HorizonColorChange();
        //BottomColorChange();


    }
    void TopColorChange()
    {

		//ALTERNATE WAY - BLENDING ALL COLORS USING RGB VALUES

		Color prevColor = SkyboxMaterial.GetColor ("_SkyColor1");

		float newR = 0f;
		float newG = 0f;
		float newB = 0;



		for (int i = 0; i < 4; i++) {

			newR += ColorWeights [i] * Colors [i].r;
			newG += ColorWeights [i] * Colors [i].g;
			newB += ColorWeights [i] * Colors [i].b;

		}

		if (newR != 0f) {
			Debug.Log ("new R " + newR);
			Color newColor = new Color (newR, newG, newB);



			SkyboxMaterial.SetColor ("_SkyColor1", Color.Lerp(prevColor, newColor, Time.deltaTime));
		} else {
			SkyboxMaterial.SetColor ("_SkyColor1", Colors[4]);
		}
		/*

        if( drumRibbonsNum >0 &&
            drumRibbonsNum>=bassRibbonsNum &&
            drumRibbonsNum >= melodyRibbonsNum &&
            drumRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor1", Colors[0]);
            
        }

       else if (bassRibbonsNum > 0 &&
                bassRibbonsNum >= drumRibbonsNum &&
                bassRibbonsNum >= melodyRibbonsNum &&
                bassRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor1", Colors[1]);

        }

        else if (melodyRibbonsNum > 0 &&
         melodyRibbonsNum >= drumRibbonsNum &&
         melodyRibbonsNum >= bassRibbonsNum &&
         melodyRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor1", Colors[2]);

        }

        else if (harmonyRibbonsNum > 0 &&
          harmonyRibbonsNum >= drumRibbonsNum &&
          harmonyRibbonsNum >= bassRibbonsNum &&
          harmonyRibbonsNum >= melodyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor1", Colors[3]);

        }

	*/

    }

    void HorizonColorChange()
    {

		Color prevColor = SkyboxMaterial.GetColor ("_SkyColor2");

		float newR = 0f;
		float newG = 0f;
		float newB = 0;



		for (int i = 0; i < 4; i++) {

			newR += ColorWeights [i] * Colors [i].r;
			newG += ColorWeights [i] * Colors [i].g;
			newB += ColorWeights [i] * Colors [i].b;

		}

		if (newR != 0f) {

			Color newColor = new Color (newR, newG, newB);

			float hue;
			float saturation;
			float value;

			Color.RGBToHSV (newColor, out hue, out saturation, out value);

			value = 0.6f + (ListenerSpectrum.GetBandBufferEnergy () * 0.2f);

			newColor = Color.HSVToRGB (hue, saturation, value);

			SkyboxMaterial.SetColor ("_SkyColor2", Color.Lerp(newColor, prevColor, Time.deltaTime ));
		} else {
			SkyboxMaterial.SetColor ("_SkyColor2", Colors[4]);
		}
		/*
        if (drumRibbonsNum > 0 &&
                drumRibbonsNum >= bassRibbonsNum &&
                drumRibbonsNum >= melodyRibbonsNum &&
                drumRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor2", Colors[0]);

        }

        else if (bassRibbonsNum > 0 &&
                 bassRibbonsNum >= drumRibbonsNum &&
                 bassRibbonsNum >= melodyRibbonsNum &&
                 bassRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor2", Colors[1]);

        }

        else if (melodyRibbonsNum > 0 &&
         melodyRibbonsNum >= drumRibbonsNum &&
         melodyRibbonsNum >= bassRibbonsNum &&
         melodyRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor2", Colors[2]);

        }

        else if (harmonyRibbonsNum > 0 &&
          harmonyRibbonsNum >= drumRibbonsNum &&
          harmonyRibbonsNum >= bassRibbonsNum &&
          harmonyRibbonsNum >= melodyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor2", Colors[3]);

        }
        */
    }
    
    void BottomColorChange()
    {
        if (drumRibbonsNum > 0 &&
            drumRibbonsNum >= bassRibbonsNum &&
            drumRibbonsNum >= melodyRibbonsNum &&
            drumRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor3", Colors[0]);

        }

        else if (bassRibbonsNum > 0 &&
                 bassRibbonsNum >= drumRibbonsNum &&
                 bassRibbonsNum >= melodyRibbonsNum &&
                 bassRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor3", Colors[1]);

        }

        else if (melodyRibbonsNum > 0 &&
         melodyRibbonsNum >= drumRibbonsNum &&
         melodyRibbonsNum >= bassRibbonsNum &&
         melodyRibbonsNum >= harmonyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor3", Colors[2]);

        }

        else if (harmonyRibbonsNum > 0 &&
          harmonyRibbonsNum >= drumRibbonsNum &&
          harmonyRibbonsNum >= bassRibbonsNum &&
          harmonyRibbonsNum >= melodyRibbonsNum)
        {
            SkyboxMaterial.SetColor("_SkyColor3", Colors[3]);

        }
    }
}
