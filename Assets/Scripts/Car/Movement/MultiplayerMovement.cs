using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class MultiplayerMovement : NetworkBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;
    [Header("Camera position variables")]
    [SerializeField]
    float cameraDisance = 16f;
    [SerializeField]
    float cameraHeight = 16f;

    // private string m_MovementAxisNameX;
    // private string m_MovementAxisNameY;
    // private string m_TurnAxisNameX;
    // private string m_TurnAxisNameY;
    private Rigidbody m_Rigidbody;
    private float m_OriginalPitch;
    private Vector3 m_MoveInputValue;
    private Vector3 m_TurnInputValue;
    private Vector3 m_CameraRight;
    private Vector3 m_CameraForward;
    private Vector3 m_cameraOffset;
    private Transform m_mainCamera;
    private ARManager m_arManager;
    private GameObject m_hitCube;

    private void OnEnable()
    {
        if (m_Rigidbody)
            m_Rigidbody.isKinematic = false;
        m_MoveInputValue = new Vector3(0f, 0f, 0f);
        m_TurnInputValue = new Vector3(0f, 0f, 0f);
    }


    private void OnDisable()
    {
        if (m_Rigidbody)
            m_Rigidbody.isKinematic = true;
    }

    public void EnableRigidbody(bool enable)
    {
        m_Rigidbody.detectCollisions = enable;
        m_Rigidbody.isKinematic = !enable;
        transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        // If this is not the local player
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        m_Rigidbody = GetComponent<Rigidbody>();
        EnableRigidbody(false);
        m_CameraRight = Quaternion.Euler(0f, -30f, 0f) * Vector3.right;
        m_CameraForward = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward;

        m_arManager = GameObject.FindObjectOfType<ARManager>();
        if (!m_arManager)
            Debug.LogError("Ar Manager not found. This is a problem");
        else
            m_arManager.m_multiplayerMovement = this;

        // m_hitCube = GameObject.FindGameObjectWithTag("HitCube");
        // if (!m_hitCube)
        //     Debug.LogError("Hit Cube is not found. This is a problem");

        m_cameraOffset = new Vector3(0f, cameraHeight, -cameraDisance);
        m_mainCamera = Camera.main.transform;

        // m_MovementAxisNameX = "LHorizontal" + m_PlayerNumber;
        // m_MovementAxisNameY = "LVertical" + m_PlayerNumber;
        // m_TurnAxisNameX = "RHorizontal" + m_PlayerNumber;
        // m_TurnAxisNameY = "RVertical" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;

        MoveCamera();
    }

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        // m_MoveInputValue.x = Input.GetAxis(m_MovementAxisNameX);
        // m_MoveInputValue.z = Input.GetAxis(m_MovementAxisNameY);

        // m_TurnInputValue.x = Input.GetAxis(m_TurnAxisNameX);
        // m_TurnInputValue.z = Input.GetAxis(m_TurnAxisNameY);

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
        if (!m_arManager)
            return;

        if (m_arManager.State != ARManager.ArState.PLANE_DETECTED)
            return;

        // Move and turn the car.
        // float turnAmount = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveAmount = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 deltaTranslation = transform.position + transform.forward * (m_Speed / (10f * m_arManager.gameboardScaleCoef)) * moveAmount * Time.deltaTime;
        m_Rigidbody.MovePosition(deltaTranslation);

        Turn();

        // Quaternion deltaRotation = Quaternion.Euler(m_TurnSpeed * new Vector3(0, turnAmount, 0) * Time.deltaTime);
        // m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);

        // MoveCamera();
    }

    private void Turn()
    {
        // Debug.Log("Camera x:" + m_mainCamera.rotation.eulerAngles.x + " y:" + m_mainCamera.rotation.eulerAngles.y + " z:" + m_mainCamera.rotation.eulerAngles.z);

        // Adjust the rotation of the car based on the player's AR camera movement.
        // Vector3 playerDirection = new Vector3(0f, m_mainCamera.rotation.eulerAngles.y, 0f);

        // if (playerDirection.sqrMagnitude > 0.0f)
        // {
        // m_Rigidbody.MoveRotation(Quaternion.Slerp(m_Rigidbody.rotation, Quaternion.LookRotation(playerDirection, Vector3.up), 1f));
        // }
        //m_Rigidbody.MoveRotation(Quaternion.Slerp(m_mainCamera.rotation, Quaternion.LookRotation(m_mainCamera.forward, Vector3.up), 1f));
        // Vector3 rot = transform.InverseTransformDirection(m_mainCamera.forward);

        // m_hitCube.transform.rotation = Quaternion.Euler(m_mainCamera.rotation.eulerAngles.x, m_mainCamera.rotation.eulerAngles.y, m_mainCamera.rotation.eulerAngles.z);

        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, m_mainCamera.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // float turn = m_mainCamera.rotation.eulerAngles.y * m_TurnSpeed * Time.deltaTime;

        Quaternion turnRotation = Quaternion.Euler(0f, m_mainCamera.rotation.eulerAngles.y, 0f);
        m_Rigidbody.MoveRotation(turnRotation);
    }

    private void MoveCamera()
    {
        m_mainCamera.position = transform.position;
        m_mainCamera.rotation = transform.rotation;
        m_mainCamera.Translate(m_cameraOffset);
        m_mainCamera.LookAt(transform);
    }
}