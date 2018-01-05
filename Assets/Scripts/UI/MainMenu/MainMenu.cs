using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame(int mode)
    {
        GameMode gameMode = (GameMode)mode;
        MapManager.GetInstance().SetGameMode(gameMode);

        SceneManager.LoadScene("MapSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
