using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARManager : MonoBehaviour
{
    public Transform arRoot;
    public Transform levelArt;
    public void FitToSurface()
    {
        GameObject[] debugPlanes = GameObject.FindGameObjectsWithTag("DebugPlane") as GameObject[];

        if (debugPlanes.Length > 0)
        {
            GameObject go = debugPlanes[Random.Range(0, debugPlanes.Length - 1)];
            Debug.Log(go.transform.position);
            arRoot.position = go.transform.position;
            // levelArRoot.rotation = go.transform.rotation;
            // levelArRoot.localScale = go.transform.localScale;
            float size = Mathf.Max(go.transform.localScale.x, go.transform.localScale.z);
            size *= 2;
            arRoot.localScale = new Vector3(size, size, size);

            foreach (GameObject debugGo in debugPlanes)
            {
                debugGo.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    public void DisplayDebugPlanes()
    {
        GameObject[] debugPlanes = GameObject.FindGameObjectsWithTag("DebugPlane") as GameObject[];
        foreach (GameObject go in debugPlanes)
        {
            go.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
