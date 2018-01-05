using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject m_pauseMenuPanel;
    public GameObject m_messageText;

    private bool m_pauseMenuShowing = false;

    // Use this for initialization
    void Start()
    {
        m_pauseMenuShowing = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_pauseMenuShowing = !m_pauseMenuShowing;

            m_pauseMenuPanel.SetActive(m_pauseMenuShowing);
            m_messageText.SetActive(!m_pauseMenuShowing);

            Time.timeScale = m_pauseMenuShowing ? 0f : 1f;
        }
    }

	public void GotoMainMenu(){
		SceneManager.LoadScene("MainMenu");
	}
}
