using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public InputActionAsset m_InputActions;
    public GameObject m_pauseMenuPanel;
    public GameObject m_messageText;

    private bool m_pauseMenuShowing = false;
    private InputAction m_PauseAction;

    void Start()
    {
        m_pauseMenuShowing = false;
        Time.timeScale = 1f;

        var uiMap = m_InputActions.FindActionMap("UI");
        m_PauseAction = uiMap.FindAction("Pause");
        m_PauseAction.Enable();
    }

    void Update()
    {
        if (m_PauseAction.WasReleasedThisFrame())
        {
            m_pauseMenuShowing = !m_pauseMenuShowing;

            m_pauseMenuPanel.SetActive(m_pauseMenuShowing);
            m_messageText.SetActive(!m_pauseMenuShowing);

            Time.timeScale = m_pauseMenuShowing ? 0f : 1f;
        }
    }

    private void OnDestroy()
    {
        m_PauseAction?.Disable();
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
