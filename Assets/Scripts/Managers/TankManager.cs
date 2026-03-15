using System;
using System.Collections.Generic;
using UnityEngine;
using PluggableAI;

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int m_Wins;


    private TwinStickMovement movement;
    private TankShooting shooting;
    private InputHandler inputHandler;
    private GameObject canvasGameObject;
    private StateController stateController;

    public void SetupAI(List<Transform> wayPointList)
    {
        stateController = m_Instance.GetComponent<StateController>();
        stateController.SetupAI(true, wayPointList);

        shooting = m_Instance.GetComponent<TankShooting>();

        canvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void SetupPlayerTank()
    {
        movement = m_Instance.GetComponent<TwinStickMovement>();
        shooting = m_Instance.GetComponent<TankShooting>();
        inputHandler = m_Instance.GetComponent<InputHandler>();
        canvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        inputHandler.Initialize(m_PlayerNumber);

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void DisableControl()
    {
        if (movement != null)
            movement.enabled = false;

        if (stateController != null)
            stateController.enabled = false;

        if (inputHandler != null)
            inputHandler.enabled = false;

        shooting.enabled = false;

        canvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        if (inputHandler != null)
            inputHandler.enabled = true;

        if (movement != null)
            movement.enabled = true;

        if (stateController != null)
            stateController.enabled = true;

        shooting.enabled = true;

        canvasGameObject.SetActive(true);
    }


    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    private void PaintCar()
    {
        CarPaint carPaint = m_Instance.GetComponentInChildren<CarPaint>();
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        if (carPaint)
        {
            carPaint.ColorCar(m_PlayerColor);
        }
        else
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = m_PlayerColor;
            }
        }
    }
}
