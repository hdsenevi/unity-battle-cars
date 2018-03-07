using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARManager : MonoBehaviour
{
    public enum ArState
    {
        INIT,
        PLANE_DETECTED,
    }

    public Transform arRoot;
    public Transform levelArt;
    public ArUiManager arUiManager;
    [HideInInspector]
    public float gameboardScaleCoef = 1f;
    public UnityPointCloudExample pointCloudExample;
    public PointCloudParticleExample pointCloudParticleExample;
    [HideInInspector]
    public MultiplayerMovement m_multiplayerMovement;
    public ArState State;

    private GameObject m_debugPlaneGoRef;

    void Awake()
    {
        State = ArState.INIT;
    }

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

            // if (pointCloudExample)
            //     pointCloudExample.enabled = false;

            // if (pointCloudParticleExample)
            //     pointCloudParticleExample.enabled = false;

            StartCoroutine(EnablePlayer());
        }
    }

    private IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(3);
        m_multiplayerMovement.EnableRigidbody(true);

        yield return new WaitForFixedUpdate();
        State = ArState.PLANE_DETECTED;
    }

    private IEnumerator CleanUpPointCloud()
    {
        yield return new WaitForFixedUpdate();
        GameObject[] pointCloud = GameObject.FindGameObjectsWithTag("PointCloudParticle");
        foreach (GameObject pointGo in pointCloud)
        {
            DestroyImmediate(pointGo);
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

        if (pointCloudExample)
            pointCloudExample.enabled = true;

        if (pointCloudParticleExample)
            pointCloudParticleExample.enabled = true;
    }
}
