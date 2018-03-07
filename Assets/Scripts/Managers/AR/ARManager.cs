using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public Transform arRoot;
    public Transform levelArt;
    public ArUiManager arUiManager;
    [HideInInspector]
    public float gameboardScaleCoef = 1f;

    private GameObject m_debugPlaneGoRef;

    public void FitToSurface()
    {
        GameObject[] debugPlanes = GameObject.FindGameObjectsWithTag("DebugPlane") as GameObject[];

        if (debugPlanes.Length > 0)
        {
            m_debugPlaneGoRef = debugPlanes[Random.Range(0, debugPlanes.Length - 1)];
            Debug.Log(m_debugPlaneGoRef.transform.position);
            arRoot.position = m_debugPlaneGoRef.transform.position;
            arRoot.rotation = m_debugPlaneGoRef.transform.rotation;
            float size = Mathf.Max(m_debugPlaneGoRef.transform.localScale.x, m_debugPlaneGoRef.transform.localScale.z);
            gameboardScaleCoef = size * arUiManager.gameboardScaleSetting;
            arRoot.localScale = new Vector3(size, size, size);

            foreach (GameObject debugGo in debugPlanes)
            {
                debugGo.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    public void AdjustGameboard(float scaleMultiplier, float rotation)
    {
        if (!m_debugPlaneGoRef)
            return;

        float size = Mathf.Max(m_debugPlaneGoRef.transform.localScale.x, m_debugPlaneGoRef.transform.localScale.z);
        gameboardScaleCoef = size * scaleMultiplier;
        arRoot.localScale = new Vector3(gameboardScaleCoef, gameboardScaleCoef, gameboardScaleCoef);

        if (!Mathf.Approximately(rotation, 0f))
            arRoot.Rotate(Vector3.up, rotation);
    }

    public void DisplayDebugPlanes()
    {
        if (!m_debugPlaneGoRef)
            return;

        GameObject[] debugPlanes = GameObject.FindGameObjectsWithTag("DebugPlane") as GameObject[];
        foreach (GameObject go in debugPlanes)
        {
            go.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
