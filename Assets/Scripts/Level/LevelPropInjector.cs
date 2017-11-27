using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPropInjector : MonoBehaviour
{
    public CameraControl m_CameraControl;
    public Transform[] m_Waypoints;
    public Transform[] m_SpawnPoints;

    // Use this for initialization
    void Awake()
    {
        GameManager gameManager = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;

        if (gameManager)
        {
            gameManager.m_CameraControl = m_CameraControl;
            foreach (Transform trans in m_Waypoints)
            {
				gameManager.wayPointsForAI.Add(trans);
            }
            
			for (int i = 0; i < m_SpawnPoints.Length; i++)
            {
				gameManager.m_Tanks[i].m_SpawnPoint = m_SpawnPoints[i];
            }

			gameManager.Init();
        }
    }
}
