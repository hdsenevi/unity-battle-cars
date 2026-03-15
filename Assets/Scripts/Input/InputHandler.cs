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

    private InputActionMap actionMap;
    private InputAction moveAction;
    private InputAction turnAction;
    private InputAction fireAction;
    private bool isCloned;

    public void Initialize(int playerNumber)
    {
        m_PlayerNumber = playerNumber;
        SetupActions();
    }

    private void SetupActions()
    {
        if (m_InputActions == null)
            return;

        if (!isCloned)
        {
            m_InputActions = Instantiate(m_InputActions);
            isCloned = true;
        }
        actionMap = m_InputActions.FindActionMap("Player" + m_PlayerNumber);
        if (actionMap == null)
        {
            Debug.LogError($"Action map 'Player{m_PlayerNumber}' not found in {m_InputActions.name}");
            return;
        }

        moveAction = actionMap.FindAction("Move");
        turnAction = actionMap.FindAction("Turn");
        fireAction = actionMap.FindAction("Fire");
    }

    private void OnEnable()
    {
        if (actionMap == null)
            SetupActions();

        actionMap?.Enable();
    }

    private void OnDisable()
    {
        actionMap?.Disable();
        MoveInput = Vector2.zero;
        TurnInput = Vector2.zero;
        FirePressed = false;
        FireHeld = false;
        FireReleased = false;
    }

    private void OnDestroy()
    {
        if (isCloned && m_InputActions != null)
            Destroy(m_InputActions);
    }

    private void Update()
    {
        if (actionMap == null)
            return;

        MoveInput = moveAction.ReadValue<Vector2>();
        TurnInput = turnAction.ReadValue<Vector2>();

        FirePressed = fireAction.WasPressedThisFrame();
        FireHeld = fireAction.IsPressed();
        FireReleased = fireAction.WasReleasedThisFrame();
    }
}
