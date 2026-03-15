using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public InputActionAsset m_InputActions;
    public int m_PlayerNumber = 1;

    public Vector2 MoveInput { get; private set; }
    public Vector2 TurnInput { get; private set; }
    public bool FirePressed { get; private set; }
    public bool FireHeld { get; private set; }
    public bool FireReleased { get; private set; }

    private InputActionMap m_ActionMap;
    private InputAction m_MoveAction;
    private InputAction m_TurnAction;
    private InputAction m_FireAction;

    public void Initialize(int playerNumber)
    {
        m_PlayerNumber = playerNumber;
        SetupActions();
    }

    private void SetupActions()
    {
        if (m_InputActions == null)
            return;

        m_ActionMap = m_InputActions.FindActionMap("Player" + m_PlayerNumber);
        if (m_ActionMap == null)
        {
            Debug.LogError($"Action map 'Player{m_PlayerNumber}' not found in {m_InputActions.name}");
            return;
        }

        m_MoveAction = m_ActionMap.FindAction("Move");
        m_TurnAction = m_ActionMap.FindAction("Turn");
        m_FireAction = m_ActionMap.FindAction("Fire");
    }

    private void OnEnable()
    {
        if (m_ActionMap == null)
            SetupActions();

        m_ActionMap?.Enable();
    }

    private void OnDisable()
    {
        m_ActionMap?.Disable();
        MoveInput = Vector2.zero;
        TurnInput = Vector2.zero;
        FirePressed = false;
        FireHeld = false;
        FireReleased = false;
    }

    private void Update()
    {
        if (m_ActionMap == null)
            return;

        MoveInput = m_MoveAction.ReadValue<Vector2>();
        TurnInput = m_TurnAction.ReadValue<Vector2>();

        FirePressed = m_FireAction.WasPressedThisFrame();
        FireHeld = m_FireAction.IsPressed();
        FireReleased = m_FireAction.WasReleasedThisFrame();
    }
}
