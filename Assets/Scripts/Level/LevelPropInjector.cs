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
                if (i < gameManager.m_Tanks.Length)
                {
                    gameManager.m_Tanks[i].m_SpawnPoint = m_SpawnPoints[i];
                }
            }

            gameManager.Init();
        }

        // TODO / REFACTOR : get rid of seperate game managers and use on manager system
        GameMgrSurvivalMode gameManagerSurvival = GameObject.FindObjectOfType(typeof(GameMgrSurvivalMode)) as GameMgrSurvivalMode;

        if (gameManagerSurvival)
        {
            gameManagerSurvival.m_CameraControl = m_CameraControl;
            foreach (Transform trans in m_Waypoints)
            {
                gameManagerSurvival.wayPointsForAI.Add(trans);
            }

            for (int i = 0; i < m_SpawnPoints.Length; i++)
            {
                if (i < gameManagerSurvival.m_Cars.Length)
                {
                    gameManagerSurvival.m_Cars[i].m_SpawnPoint = m_SpawnPoints[i];
                }
            }

            for (int i = 0; i < gameManagerSurvival.m_EnemyCars.Length; i++)
            {
                gameManagerSurvival.m_EnemyCars[i].m_SpawnPoint = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length - 1)];
            }

            gameManagerSurvival.m_SpawnPoints = m_SpawnPoints;


            gameManagerSurvival.Init();
        }
    }
}
