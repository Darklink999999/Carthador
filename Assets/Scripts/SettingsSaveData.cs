using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsSaveData
{

    [HideInInspector] public int [] resolution;
    [HideInInspector] public int quality;
    [HideInInspector] public bool fullscreen;
    [HideInInspector] public float volume;
    [HideInInspector] public float cameraSensitivity;



    public SettingsSaveData(Settings s)

    {
        this.resolution = new  int [3];
        this.resolution[0] = s.resolutions[s.resolutionDropdown.value].width;
        this.resolution[1] = s.resolutions[s.resolutionDropdown.value].height;
        this.resolution[2] = s.resolutions[s.resolutionDropdown.value].refreshRate;

        this.quality = s.qualityDropdown.value;

        this.fullscreen = s.fullscreen;

        this.volume = s.audioSlider.value;

        this.cameraSensitivity = s.cameraSlider.value;
    }



}
