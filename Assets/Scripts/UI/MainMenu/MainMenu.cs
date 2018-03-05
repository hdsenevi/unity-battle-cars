using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool ArGame = false;
    public void LoadGame(int mode)
    {
        if (ArGame)
        {
            SceneManager.LoadScene("Testbed_Network_Lobby");
        }
        else
        {
            GameMode gameMode = (GameMode)mode;
            MapManager.GetInstance().SetGameMode(gameMode);

            SceneManager.LoadScene("MapSelect");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
