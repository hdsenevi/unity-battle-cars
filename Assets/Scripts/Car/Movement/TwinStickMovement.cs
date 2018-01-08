using UnityEngine;

public class TwinStickMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private string m_MovementAxisNameX;
    private string m_MovementAxisNameY;
    private string m_TurnAxisNameX;
    private string m_TurnAxisNameY;
    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;
    private Vector3 m_MoveInputValue;
    private Vector3 m_TurnInputValue;
    private Vector3 m_CameraRight;
    private Vector3 m_CameraForward;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CameraRight = Quaternion.Euler(0f, -30f, 0f) * Vector3.right;
        m_CameraForward = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward;
    }


    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MoveInputValue = new Vector3(0f, 0f, 0f);
        m_TurnInputValue = new Vector3(0f, 0f, 0f);
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisNameX = "LHorizontal" + m_PlayerNumber;
        m_MovementAxisNameY = "LVertical" + m_PlayerNumber;
        m_TurnAxisNameX = "RHorizontal" + m_PlayerNumber;
        m_TurnAxisNameY = "RVertical" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MoveInputValue.x = Input.GetAxis(m_MovementAxisNameX);
        m_MoveInputValue.z = Input.GetAxis(m_MovementAxisNameY);

        m_TurnInputValue.x = Input.GetAxis(m_TurnAxisNameX);
        m_TurnInputValue.z = Input.GetAxis(m_TurnAxisNameY);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MoveInputValue.magnitude) < 0.1f && Mathf.Abs(m_TurnInputValue.magnitude) < 0.1f)
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
        // Move and turn the tank.
        Move();
        Turn();
    }

    private void Move()
    {
        // Adjust the position of the car based on the player's input.
        Vector3 movement = (m_CameraRight * -m_MoveInputValue.z + m_CameraForward * -m_MoveInputValue.x) * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        Vector3 playerDirection = (m_CameraRight * -m_TurnInputValue.z + m_CameraForward * -m_TurnInputValue.x) * m_TurnSpeed * Time.deltaTime;

        if (playerDirection.sqrMagnitude > 0.0f)
        {
            m_Rigidbody.MoveRotation(Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 1f));
        }

        // float turn = (m_TurnInputValue.x + m_TurnInputValue.z) * m_TurnSpeed * Time.deltaTime;
		// Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		// m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}