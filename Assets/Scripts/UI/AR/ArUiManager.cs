using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArUiManager : MonoBehaviour
{
    public GameObject MenuBtn;
    public GameObject OptionsPanel;
    public GameObject TouchControls;
    public float gameboardScaleSetting;
    public float gameboardRotateSetting;
    public ARManager aRManager;

    public void MenuBtnPressed()
    {
        OptionsPanel.SetActive(true);
        MenuBtn.SetActive(false);
        TouchControls.SetActive(false);
    }

    public void OptionsPanelBtnPressed()
    {
        OptionsPanel.SetActive(false);
        MenuBtn.SetActive(true);
        // TouchControls.SetActive(true);
    }

    public void ScaleGameBoard(float sliderVlaue)
    {
        Debug.Log("ScaleGameBoard :" + sliderVlaue);
        gameboardScaleSetting = sliderVlaue;
        aRManager.AdjustGameboard(gameboardScaleSetting, gameboardRotateSetting);
    }

    public void RotateGameBoard(float sliderVlaue)
    {
        Debug.Log("RotateGameBoard :" + sliderVlaue);
        gameboardRotateSetting = sliderVlaue;
        aRManager.AdjustGameboard(gameboardScaleSetting, gameboardRotateSetting);
    }
}
