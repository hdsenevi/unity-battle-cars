﻿using UnityEngine;
using UnityEngine.Networking;

public class NetworkShellExplosion : NetworkBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_MaxDamage = 100f;
    public float m_ExplosionForce = 1000f;
    public float m_MaxLifeTime = 2f;
    public float m_ExplosionRadius = 5f;

    [ServerCallback]
    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.

        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targerRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targerRigidbody)
                continue;

            targerRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            NetworkTankHealth targetHealth = targerRigidbody.GetComponent<NetworkTankHealth>();

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targerRigidbody.position);
            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.duration);
        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;
        float explosionDistance = explosionToTarget.magnitude;
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
        float damage = relativeDistance * m_MaxDamage;
        damage = Mathf.Max(0f, damage);
        return damage;
    }
}