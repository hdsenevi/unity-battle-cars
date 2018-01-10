using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectMenu : MonoBehaviour
{
    public void LoadMap(int mapId)
    {
        GameMode gameMode = MapManager.GetInstance().GetGameMode();

        switch (gameMode)
        {
            case GameMode.LOCAL_MULTIPLAYER:
                SceneManager.LoadScene(SceneNames.MultiPlayer);
                break;
            case GameMode.SINGLE_PLAYER:
                SceneManager.LoadScene(SceneNames.SinglePlayer);
                break;
            case GameMode.SURVIVAL_MODE:
                SceneManager.LoadScene(SceneNames.Survival);
                break;
            default:
                SceneManager.LoadScene(SceneNames.SinglePlayer);
                break;
        }

        switch (mapId)
        {
            case 0:
                SceneManager.LoadSceneAsync(SceneNames.LevelSnow, LoadSceneMode.Additive);
                break;
            case 1:
                SceneManager.LoadSceneAsync(SceneNames.LevelDesert, LoadSceneMode.Additive);
                break;
            default:
                SceneManager.LoadSceneAsync(SceneNames.LevelSnow, LoadSceneMode.Additive);
                break;
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
