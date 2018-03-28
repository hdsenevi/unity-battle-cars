using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArUiManager : MonoBehaviour
{
    public GameObject MenuBtn;
    public GameObject OptionsPanel;
    // [HideInInspector]
    public float gameboardScaleSetting = 1f;
    // [HideInInspector]
    public float gameboardRotateSetting = 0f;
    public ARManager aRManager;

    public void MenuBtnPressed()
    {
        OptionsPanel.SetActive(true);
        MenuBtn.SetActive(false);
    }

    public void OptionsPanelBtnPressed()
    {
        OptionsPanel.SetActive(false);
        MenuBtn.SetActive(true);
    }

    public void ScaleGameBoard(float sliderVlaue)
    {
        gameboardScaleSetting = sliderVlaue;
        Debug.Log(gameboardScaleSetting);
        aRManager.AdjustGameboard(gameboardScaleSetting, 0f);
    }

    public void RotateGameBoard(float sliderVlaue)
    {
        aRManager.AdjustGameboard(gameboardScaleSetting, gameboardRotateSetting - sliderVlaue);
        gameboardRotateSetting = sliderVlaue;
    }
}