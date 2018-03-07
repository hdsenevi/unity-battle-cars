﻿using UnityEngine;
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
    private Vector3 m_cameraOffset;
    private Transform m_mainCamera;
    private ARManager m_arManager;

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


    private void Start()
    {
        // If this is not the local player
        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CameraRight = Quaternion.Euler(0f, -30f, 0f) * Vector3.right;
        m_CameraForward = Quaternion.Euler(0f, -30f, 0f) * Vector3.forward;

        m_arManager = GameObject.FindObjectOfType<ARManager>();
        if (!m_arManager)
            Debug.LogError("Ar Manager not found. This is a problem");

        m_cameraOffset = new Vector3(0f, cameraHeight, -cameraDisance);
        m_mainCamera = Camera.main.transform;

        m_MovementAxisNameX = "LHorizontal" + m_PlayerNumber;
        m_MovementAxisNameY = "LVertical" + m_PlayerNumber;
        m_TurnAxisNameX = "RHorizontal" + m_PlayerNumber;
        m_TurnAxisNameY = "RVertical" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;

        MoveCamera();
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
        // Move and turn the car.
        float turnAmount = CrossPlatformInputManager.GetAxis("Horizontal");
        float moveAmount = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 deltaTranslation = transform.position + transform.forward * (m_Speed / (10f * m_arManager.gameboardScaleCoef)) * moveAmount * Time.deltaTime;
        m_Rigidbody.MovePosition(deltaTranslation);

        // Quaternion deltaRotation = Quaternion.Euler(m_TurnSpeed * new Vector3(0, turnAmount, 0) * Time.deltaTime);
        // m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);

        // MoveCamera();
    }

    private void MoveCamera()
    {
        m_mainCamera.position = transform.position;
        m_mainCamera.rotation = transform.rotation;
        m_mainCamera.Translate(m_cameraOffset);
        m_mainCamera.LookAt(transform);
    }
}