using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private bool fullscreen = false;

    private Dropdown resolutionDropdown;
    private Toggle fullscreenToggle;

    private Resolution [] resolutions;
    private int currentResolutionIndex;

    private int qualityIndex;
    private Dropdown qualityDropdown;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenToggle = GameObject.Find("Fullscreen").GetComponent<Toggle> ();

        qualityDropdown = GameObject.Find("Quality").GetComponent<Dropdown>();


        //////////////////////////////////////////// RESOLUTION ///////////////////////////////////////////////
        resolutionDropdown = GameObject.Find("Resolution").GetComponent<Dropdown>();
        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;
        
        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            
            string option = resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        /////////////////////////////////////////////////////// QUALITY SETTINGS /////////////////////////////////////
        qualityIndex = QualitySettings.GetQualityLevel();
        qualityDropdown.value = qualityIndex;
        qualityDropdown.RefreshShownValue();



        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        currentResolutionIndex = resolutionDropdown.value;
        this.fullscreen = fullscreenToggle.isOn;
        qualityIndex = qualityDropdown.value;

    }



    public void applyChanges ()
    {

        if (resolutions[resolutionDropdown.value].width != Screen.width && resolutions[resolutionDropdown.value].height != Screen.height)
        {

            Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, this.fullscreen);
        
        }


        if (this.fullscreen != Screen.fullScreen)
        {

            Screen.SetResolution(Screen.width, Screen.height, fullscreen);
        }

        if (qualityIndex != QualitySettings.GetQualityLevel ()) {

            QualitySettings.SetQualityLevel(qualityIndex);
        }




    }
}
