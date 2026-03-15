using UnityEngine;

public class TwinStickMovement : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private InputHandler m_InputHandler;
    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;
    private Vector2 m_MoveInputValue;
    private Vector2 m_TurnInputValue;
    private Vector3 m_CameraRight;
    private Vector3 m_CameraForward;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_InputHandler = GetComponent<InputHandler>();
        m_CameraRight = Quaternion.Euler(0f, -30f, 0f) * Vector3.right;
        m_CameraForward = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward;
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MoveInputValue = Vector2.zero;
        m_TurnInputValue = Vector2.zero;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        m_MoveInputValue = m_InputHandler.MoveInput;
        m_TurnInputValue = m_InputHandler.TurnInput;

        EngineAudio();
    }


    private void EngineAudio()
    {
        if (m_MoveInputValue.magnitude < 0.1f && m_TurnInputValue.magnitude < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        Move();
        Turn();
    }

    private void Move()
    {
        Vector3 movement = (m_CameraRight * m_MoveInputValue.y + m_CameraForward * -m_MoveInputValue.x) * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        Vector3 playerDirection = (m_CameraRight * m_TurnInputValue.y + m_CameraForward * -m_TurnInputValue.x) * m_TurnSpeed * Time.deltaTime;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            m_Rigidbody.MoveRotation(Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 1f));
        }
    }
}
