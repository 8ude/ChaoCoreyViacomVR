using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxColorChange : MonoBehaviour {

    public Material SkyboxMaterial;
    public new List<Color> Colors;
    public GameObject RibbonGameManager;

    public GameObject[] drumRibbons;
    public GameObject[] bassRibbons;
    public GameObject[] melodyRibbons;
    public GameObject[] harmonyRibbons;

    public int drumRibbonsNum;
    public int bassRibbonsNum;
    public int melodyRibbonsNum;
    public int harmonyRibbonsNum;



    // Use this for initialization
    void Start () {

        drumRibbonsNum = 0;
        bassRibbonsNum = 0;
        melodyRibbonsNum = 0;
        harmonyRibbonsNum = 0;

        SkyboxMaterial.SetColor("_SkyColor1", Colors[4]);
        SkyboxMaterial.SetColor("_SkyColor2", Colors[5]);
        SkyboxMaterial.SetColor("_SkyColor3", Colors[6]);
    }
	
	// Update is called once per frame
	void Update () {

        drumRibbons = RibbonGameManager.gameObject.GetComponent<RibbonGameManager>().drumRibbons;
        bassRibbons = RibbonGameManager.gameObject.GetComponent<RibbonGameManager>().bassRibbons;
        melodyRibbons = RibbonGameManager.gameObject.GetComponent<RibbonGameManager>().melodyRibbons;
        harmonyRibbons =  RibbonGameManager.gameObject.GetComponent<RibbonGameManager>().harmonyRibbons;

        drumRibbonsNum = drumRibbons.Length;
        bassRibbonsNum = bassRibbons.Length;
        melodyRibbonsNum = melodyRibbons.Length;
        harmonyRibbonsNum = harmonyRibbons.Length;

        TopColorChange();
        HorizonColorChange();
        //BottomColorChange();


    }
    void TopColorChange()
    {
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

    }

    void HorizonColorChange()
    {
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
