using UnityEngine;

public class TwinStickMovement : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private InputHandler inputHandler;
    private Rigidbody rigidbody;
    private float originalPitch;
    private Vector2 moveInputValue;
    private Vector2 turnInputValue;
    private Vector3 cameraRight;
    private Vector3 cameraForward;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        cameraRight = Quaternion.Euler(0f, -30f, 0f) * Vector3.right;
        cameraForward = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward;
    }


    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        moveInputValue = Vector2.zero;
        turnInputValue = Vector2.zero;
    }


    private void OnDisable()
    {
        rigidbody.isKinematic = true;
    }


    private void Start()
    {
        originalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        moveInputValue = inputHandler.MoveInput;
        turnInputValue = inputHandler.TurnInput;

        EngineAudio();
    }


    private void EngineAudio()
    {
        if (moveInputValue.magnitude < 0.1f && turnInputValue.magnitude < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(originalPitch - m_PitchRange, originalPitch + m_PitchRange);
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
        Vector3 movement = (cameraRight * moveInputValue.y + cameraForward * -moveInputValue.x) * m_Speed * Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Turn()
    {
        Vector3 playerDirection = (cameraRight * turnInputValue.y + cameraForward * -turnInputValue.x) * m_TurnSpeed * Time.deltaTime;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            rigidbody.MoveRotation(Quaternion.Slerp(rigidbody.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 1f));
        }
    }
}
