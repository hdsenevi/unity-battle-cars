using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private InputHandler inputHandler;
    private Rigidbody rigidbody;
    private float movementInputValue;
    private float turnInputValue;
    private float originalPitch;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
    }


    private void OnEnable ()
    {
        rigidbody.isKinematic = false;
        movementInputValue = 0f;
        turnInputValue = 0f;
    }


    private void OnDisable ()
    {
        rigidbody.isKinematic = true;
    }


    private void Start()
    {
        originalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        Vector2 moveInput = inputHandler.MoveInput;
        movementInputValue = -moveInput.y;
        turnInputValue = moveInput.x;

        EngineAudio();
    }


    private void EngineAudio()
    {
        if (Mathf.Abs(movementInputValue) < 0.1f && Mathf.Abs(turnInputValue) < 0.1f)
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
        // Move();
        // Turn();
    }


    private void Move()
    {
        Vector3 movement = transform.forward * movementInputValue * m_Speed * Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + movement);
    }


    private void Turn()
    {
        float turn = turnInputValue * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }
}
