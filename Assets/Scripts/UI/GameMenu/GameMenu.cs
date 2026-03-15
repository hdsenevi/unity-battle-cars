using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public InputActionAsset m_InputActions;
    public GameObject m_pauseMenuPanel;
    public GameObject m_messageText;

    private bool pauseMenuShowing = false;
    private InputAction pauseAction;

    void Start()
    {
        pauseMenuShowing = false;
        Time.timeScale = 1f;

        var uiMap = m_InputActions.FindActionMap("UI");
        pauseAction = uiMap.FindAction("Pause");
        pauseAction.Enable();
    }

    void Update()
    {
        if (pauseAction.WasReleasedThisFrame())
        {
            pauseMenuShowing = !pauseMenuShowing;

            m_pauseMenuPanel.SetActive(pauseMenuShowing);
            m_messageText.SetActive(!pauseMenuShowing);

            Time.timeScale = pauseMenuShowing ? 0f : 1f;
        }
    }

    private void OnDestroy()
    {
        pauseAction?.Disable();
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
