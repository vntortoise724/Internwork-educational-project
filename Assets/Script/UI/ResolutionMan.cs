using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;

public class ResolutionMan : MonoBehaviour
{
    [SerializeField]
    private TMP_Dropdown resolution;

    private Resolution[] ress;
    private List<Resolution> resolutionList;

    private float currentRefresh;
    private int currentRes = 0;

    void Start()
    {
        ress = Screen.resolutions;
        resolutionList = new List<Resolution>();

        resolution.ClearOptions();
        currentRefresh = Screen.currentResolution.refreshRateRatio.numerator;

        for (int i = 0; i < ress.Length; i++)
        {
            if (ress[i].refreshRateRatio.numerator ==  currentRefresh)
            {
                resolutionList.Add(ress[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0;i < resolutionList.Count;i++)
        {
            string resOption = resolutionList[i].width + "x" + resolutionList[i].height + " " + resolutionList[i].refreshRateRatio.value.ToString("#") + "Hz";
            options.Add(resOption);
            if (resolutionList[i].width == Screen.width && resolutionList[i].height == Screen.height)
            {
                currentRes = i;
            }
        }

        resolution.AddOptions(options);
        resolution.value = currentRes;
        resolution.RefreshShownValue();

    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = resolutionList[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
