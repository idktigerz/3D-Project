using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

//[RequireComponent(typeof(Glo))]
public class NightVisionController : MonoBehaviour
{
    [SerializeField] private Color defaultLightColor;
    [SerializeField] private Color boostedLightColor;

    public bool isEnabled = false;
    //public PostProcessVolume volume;

    public TimeController timeController;
    public Volume volume;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        RenderSettings.ambientLight = defaultLightColor;
        volume = gameObject.GetComponent<Volume>();
        volume.weight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ToggleNightVision();
        }

        if(timeController.canToggleNightVision == false)
        {
            volume.weight = 0;
        }

        if (timeController.canToggleNightVision && isEnabled == false)
        {
            RenderSettings.ambientIntensity = 0.1f;
        }
    }

    private void ToggleNightVision()
    {

        if (isEnabled == false && timeController.canToggleNightVision && playerController.cameraON)
        {
            RenderSettings.ambientLight = boostedLightColor;
            RenderSettings.ambientIntensity = 2f;
            volume.weight = 1;
            Debug.Log("Nightvision enabled");
            isEnabled = true;
        }
        else
        {
            RenderSettings.ambientIntensity = 0.1f;
            RenderSettings.ambientLight = defaultLightColor;
            volume.weight = 0;
            Debug.Log("Nightvision disabled");
            isEnabled = false;
        }
    }
}
