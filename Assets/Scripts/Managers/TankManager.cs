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


    private TwinStickMovement m_Movement;
    private TankShooting m_Shooting;
    private InputHandler m_InputHandler;
    private GameObject m_CanvasGameObject;
    private StateController m_StateController;

    public void SetupAI(List<Transform> wayPointList)
    {
        m_StateController = m_Instance.GetComponent<StateController>();
        m_StateController.SetupAI(true, wayPointList);

        m_Shooting = m_Instance.GetComponent<TankShooting>();

        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void SetupPlayerTank()
    {
        m_Movement = m_Instance.GetComponent<TwinStickMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_InputHandler = m_Instance.GetComponent<InputHandler>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_InputHandler.Initialize(m_PlayerNumber);

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void DisableControl()
    {
        if (m_Movement != null)
            m_Movement.enabled = false;

        if (m_StateController != null)
            m_StateController.enabled = false;

        if (m_InputHandler != null)
            m_InputHandler.enabled = false;

        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        if (m_InputHandler != null)
            m_InputHandler.enabled = true;

        if (m_Movement != null)
            m_Movement.enabled = true;

        if (m_StateController != null)
            m_StateController.enabled = true;

        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
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
