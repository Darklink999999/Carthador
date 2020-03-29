using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class Settings : MonoBehaviour
{

    private SpawnImportantObjects sIO;




    [HideInInspector] public bool fullscreen = false;

    [HideInInspector] public Dropdown resolutionDropdown;
    private Toggle fullscreenToggle;

    [HideInInspector] public Resolution [] resolutions;
    private int currentResolutionIndex;

    private int qualityIndex;
    [HideInInspector] public Dropdown qualityDropdown;

    [HideInInspector] public Slider audioSlider;
    private float globalVolume = 1f;

    [HideInInspector] public Slider cameraSlider;
    private float cameraSensitivity;

    // Start is called before the first frame update
    void Start()
    {

        sIO = GameObject.Find("SpawnImportantObjects").GetComponent<SpawnImportantObjects>();


        fullscreenToggle = GameObject.Find("Fullscreen").GetComponent<Toggle> ();

        qualityDropdown = GameObject.Find("Quality").GetComponent<Dropdown>();
        
        cameraSlider = GameObject.Find("CameraSlider").GetComponent<Slider>();



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


        //////////////////////////////////////////////////// AUDIO ////////////////////////////////////////////////////

        audioSlider = GameObject.Find("AudioSlider").GetComponent<Slider>();



        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        currentResolutionIndex = resolutionDropdown.value;
        
        this.fullscreen = fullscreenToggle.isOn;
        
        qualityIndex = qualityDropdown.value;
        
        globalVolume = audioSlider.value;

        cameraSensitivity = cameraSlider.value;

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

        if (AudioListener.volume != globalVolume)
        {

            AudioListener.volume = globalVolume;
        }

        if (cameraSensitivity != sIO.camera.GetComponent <FreeLookCam>().m_TurnSpeed)
        {

            sIO.camera.GetComponent<FreeLookCam>().m_TurnSpeed = cameraSensitivity;
        }

        SettingsSaveSystem.Save(this);

    }


    public void exit ()
    {

        this.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "StartScene")
            sIO.startMenu.SetActive(true);
        else
            Camera.main.GetComponent<Game>().mainMenu.SetActive (true);
    }
}
