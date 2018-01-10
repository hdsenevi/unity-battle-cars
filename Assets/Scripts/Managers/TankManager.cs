using System;
using System.Collections.Generic;
using UnityEngine;
using PluggableAI;

public enum CarType {
    HUMAN,
    NPC,
}

[Serializable]
public class TankManager
{
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public int m_Wins;
    [HideInInspector] public CarType m_CarType;

    private TwinStickMovement m_Movement;
    private TankShooting m_Shooting;
    private GameObject m_CanvasGameObject;
    private StateController m_StateController;				// Reference to the StateController for AI tanks

    public void SetupAI(List<Transform> wayPointList)
    {
        m_StateController = m_Instance.GetComponent<StateController>();
        m_StateController.SetupAI(true, wayPointList);

        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void SetupPlayerTank()
    {
        // Get references to the components.
        m_Movement = m_Instance.GetComponent<TwinStickMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        // Set the player numbers to be consistent across the scripts.
        if (!m_Instance.tag.Equals("NPC"))
            m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        PaintCar();
    }

    public void DisableControl()
    {
        if (m_Movement != null)
            m_Movement.enabled = false;

        if (m_StateController != null)
            m_StateController.enabled = false;

        m_Shooting.enabled = false;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl()
    {
        if (m_Movement != null)
            m_Movement.enabled = true;

        if (m_StateController != null)
            m_StateController.enabled = true;

        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset(bool resetTransform = true)
    {
        if (resetTransform) {
            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;
        }

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }

    private void PaintCar()
    {
        // Get all of the renderers of the tank.
        CarPaint carPaint = m_Instance.GetComponentInChildren<CarPaint>();
        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        if (carPaint)
        {
            carPaint.ColorCar(m_PlayerColor);
        }
        else
        {
            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }
        }
    }
}
