using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	private static MapManager instance = null;
	private GameMode gameMode = GameMode.SINGLE_PLAYER;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(this);
		instance = this;
	}

	public static MapManager GetInstance() {
		if (instance == null) {
			Debug.LogError("MapManager.GetInstance() : cannot get valid instance before initialization is done in Awake method");
		}

		return instance;
	}

	public void SetGameMode (GameMode gameMode) {
		gameMode = gameMode;
	}

	public GameMode GetGameMode() {
		return gameMode;
	}
}
