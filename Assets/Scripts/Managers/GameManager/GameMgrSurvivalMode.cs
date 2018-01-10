using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameMgrSurvivalMode : MonoBehaviour
{
    public int m_NumRoundsToWin = 5;
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject[] m_CarPrefabs;
    public TankManager[] m_Cars;
    public GameObject[] m_EnemyPrefabs;
    public TankManager[] m_EnemyCars;
    public List<Transform> wayPointsForAI;
    public GameMode m_GameMode = GameMode.SINGLE_PLAYER;
    public Transform[] m_SpawnPoints;

    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;
    private List<Transform> m_CameraTragetTrans = new List<Transform>();

    public virtual void Init()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnPlayerTanks();
        SpawnEnemyTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnPlayerTanks()
    {
        switch (m_GameMode)
        {
            case GameMode.SURVIVAL_MODE:
                // Setup player cars
                for (int i = 0; i < m_Cars.Length; i++)
                {
                    m_Cars[i].m_Instance =
                        Instantiate(m_CarPrefabs[i], m_Cars[i].m_SpawnPoint.position, m_Cars[i].m_SpawnPoint.rotation) as GameObject;
                    m_Cars[i].m_PlayerNumber = i + 1;
                    m_Cars[i].m_CarType = CarType.HUMAN;
                    m_Cars[i].SetupPlayerTank();
                }
                break;
        }
    }

    private void SpawnEnemyTanks()
    {
        switch (m_GameMode)
        {
            case GameMode.SURVIVAL_MODE:
                // Setup player cars
                for (int i = 0; i < m_EnemyCars.Length; i++)
                {
                    // Vector3 randomPosition = GetRandomLocation();
                    Vector3 randomPosition;
		            if (RandomPoint(m_SpawnPoints[Random.Range(0, m_SpawnPoints.Length - 1)].position, 10f, out randomPosition)) {
                        m_EnemyCars[i].m_Instance =
                            Instantiate(m_EnemyPrefabs[i], randomPosition, m_SpawnPoints[i].rotation) as GameObject;
                        m_EnemyCars[i].m_PlayerNumber = i + 3;
                        m_EnemyCars[i].m_CarType = CarType.NPC;
                        m_EnemyCars[i].SetupAI(wayPointsForAI);
                    }
                }
                break;
        }
    }

    private void SetCameraTargets()
    {
        Transform[] targets = new Transform[m_Cars.Length];

        for (int i = 0; i < targets.Length; i++)
        {
            targets[i] = m_Cars[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;

        m_CameraTragetTrans.Clear();
        GameObject[] humans = GameObject.FindGameObjectsWithTag("Player") as GameObject[];
        foreach (GameObject go in humans){
            m_CameraTragetTrans.Add(go.transform);
        }
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC") as GameObject[];
        foreach (GameObject go in npcs){
            m_CameraTragetTrans.Add(go.transform);
        }
        m_CameraControl.m_Targets = m_CameraTragetTrans.ToArray();
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (m_GameWinner != null)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStarting()
    {
        ResetAllTanks();
        DisableTankControl();

        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        // Resetup camera targets
        SetCameraTargets();

        yield return m_StartWait;
    }

    private IEnumerator RoundPlaying()
    {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        while (!ZeroNpcsLeft())
        {
            yield return null;
        }
    }

    private IEnumerator RoundEnding()
    {

        m_RoundWinner = null;

        m_RoundWinner = GetRoundWinner();

        if (m_RoundWinner != null)
        {
            m_RoundWinner.m_Wins++;
        }

        m_GameWinner = GetGameWinner();

        string message = EndMessage();
        m_MessageText.text = message;

        SpawnEnemyTanks();
        DisableTankControl();

        yield return m_EndWait;
    }

    private bool OneTankLeft()
    {
        int numTanksLeft = 0;

        for (int i = 0; i < m_Cars.Length; i++)
        {
            if (m_Cars[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }

    private bool ZeroNpcsLeft() {
        int numEnemiesLeft = 0;

        for (int i = 0; i < m_EnemyCars.Length; i++)
        {
            if (m_EnemyCars[i].m_Instance.activeSelf)
                numEnemiesLeft++;
        }

        return numEnemiesLeft <= 0;
    }

    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < m_Cars.Length; i++)
        {
            if (m_Cars[i].m_Instance.activeSelf)
                return m_Cars[i];
        }

        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < m_Cars.Length; i++)
        {
            if (m_Cars[i].m_Wins == m_NumRoundsToWin)
                return m_Cars[i];
        }

        return null;
    }

    private string EndMessage()
    {
        string message = "DRAW!";

        if (m_RoundWinner != null)
            message = "ROUND " + m_RoundNumber + " SURVIVED!";

        if (m_GameWinner != null)
            message = "WINS THE GAME!";

        return message;
    }

    private void ResetAllTanks()
    {
        for (int i = 0; i < m_Cars.Length; i++)
        {
            m_Cars[i].Reset();
        }
        for (int i = 0; i < m_EnemyCars.Length; i++)
        {
            m_EnemyCars[i].Reset(false);
        }
    }

    private void EnableTankControl()
    {
        for (int i = 0; i < m_Cars.Length; i++)
        {
            m_Cars[i].EnableControl();
        }
        for (int i = 0; i < m_EnemyCars.Length; i++)
        {
            m_EnemyCars[i].m_Instance.SetActive(true);
            m_EnemyCars[i].EnableControl();
        }

        // Resetup camera targets
        SetCameraTargets();
    }

    private void DisableTankControl()
    {
        for (int i = 0; i < m_Cars.Length; i++)
        {
            m_Cars[i].DisableControl();
        }
        for (int i = 0; i < m_EnemyCars.Length; i++)
        {
            m_EnemyCars[i].m_Instance.SetActive(false);
            m_EnemyCars[i].DisableControl();
        }
    }

    private Vector3 GetRandomLocation()
    {
         NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
 
         // Pick the first indice of a random triangle in the nav mesh
         int t = Random.Range(0, navMeshData.indices.Length-3);
         
         // Select a random point on it
         Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t+1]], Random.value);
         Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t+2]], Random.value);
 
         return point;
     }

     bool RandomPoint(Vector3 center, float range, out Vector3 result) {
		for (int i = 0; i < 30; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}
}