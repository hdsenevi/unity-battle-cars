using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	private static MapManager m_Instance = null;
	private GameMode m_GameMode = GameMode.SINGLE_PLAYER;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);
		m_Instance = this;
	}

	public static MapManager GetInstance() {
		if (m_Instance == null) {
			Debug.LogError("MapManager.GetInstance() : cannot get valid instance before initialization is done in Awake method");
		}

		return m_Instance;
	}

	public void SetGameMode (GameMode gameMode) {
		m_GameMode = gameMode;
	}

	public GameMode GetGameMode() {
		return m_GameMode;
	}
}
