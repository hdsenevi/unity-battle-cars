using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARCarInit : MonoBehaviour
{
    void Start()
    {
        ARManager manager = GameObject.FindObjectOfType<ARManager>();
        if (!manager)
        {
            Debug.LogError("ARManage not found");
        }

        transform.parent = manager.levelArt;
        transform.localScale = Vector3.one;
    }
}
