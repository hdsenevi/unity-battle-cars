using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;

    private InputHandler inputHandler;
    private float currentLaunchForce;
    private float chargeSpeed;
    private bool fired;
    private float nextFireTime;


    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    private void OnEnable()
    {
        currentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        chargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        m_AimSlider.value = m_MinLaunchForce;

        if (currentLaunchForce >= m_MaxLaunchForce && !fired)
        {
            currentLaunchForce = m_MaxLaunchForce;
            Fire(currentLaunchForce, 1);
        }
        else if (inputHandler.FirePressed)
        {
            fired = false;
            currentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (inputHandler.FireHeld && !fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            m_AimSlider.value = currentLaunchForce;
        }
        else if (inputHandler.FireReleased && !fired)
        {
            Fire(currentLaunchForce, 1);
        }
    }


    public void Fire(float launchForce, float fireRate)
    {
        if (Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            fired = true;

            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            shellInstance.linearVelocity = currentLaunchForce * m_FireTransform.forward;

            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            currentLaunchForce = m_MinLaunchForce;
        }

    }
}
