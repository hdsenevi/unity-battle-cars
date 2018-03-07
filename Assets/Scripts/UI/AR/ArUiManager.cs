using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArUiManager : MonoBehaviour
{
    public GameObject MenuBtn;
    public GameObject OptionsPanel;
    public GameObject TouchControls;

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
        TouchControls.SetActive(true);
    }
}
