using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibbonMenuObject : MonoBehaviour {

    public enum menuState { Inactive = 0, TurningOn, Active, TurningOff };

    [SerializeField] Canvas myCanvas;

    menuState myState = menuState.Inactive;

    float triggerTimer;
    [SerializeField] float menuHoverTime = 2f;


    AudioSource mySource;


    //AudioShaderValues
    public float smoothing = 0.75f;
    float energyScaleFactor = 60f;

    float normalizedEnergy = 0f;
    float prevEnergy = 0f;
    float smoothedEnergy = 0f;

    // Use this for initialization
    void Start () {
        triggerTimer = 0f;
        mySource = GetComponent<AudioSource>();
	}

	void Update () {
		if (myState == menuState.TurningOff)
        {
            triggerTimer -= Time.deltaTime;
            if (triggerTimer <= 0f)
            {
                triggerTimer = 0f;
                DeactivateMenu();
                myState = menuState.Inactive;
            }
        }
        
        if(mySource.isPlaying) {
            
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        switch (myState)
        {
            case menuState.Inactive:
                myState = menuState.TurningOn;
                mySource.Play();
                break;
            case menuState.TurningOff:
                myState = menuState.TurningOn;
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch (myState)
        {
            case menuState.TurningOn:
                triggerTimer += Time.fixedDeltaTime;
                if (triggerTimer >= 3.0f)
                {
                    myState = menuState.Active;
                }
                break;
            default:
                break;
        }

    }

    public void DeactivateMenu()
    {
        mySource.Pause();
        myCanvas.enabled = false;
    }
}
