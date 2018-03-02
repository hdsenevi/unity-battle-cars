using UnityEngine;
using UnityEngine.Networking;
public class GameNetworkManager : NetworkManager
{
    [Header("Scene camera properties")]
    [SerializeField]
    Transform sceneCamera;
    [SerializeField]
    float cameraRotationRadius = 24f;
    [SerializeField]
    float cameraRoataitonSpeed = 3f;
    [SerializeField]
    bool canRotate = true;

    float m_ratation;

    public override void OnStartClient(NetworkClient client)
    {
        canRotate = false;
    }

    public override void OnStartHost()
    {
        canRotate = false;
    }

    public override void OnStopClient()
    {
        canRotate = true;
    }

    public override void OnStopHost()
    {
        canRotate = true;
    }

    void Update()
    {
        if (!canRotate)
            return;

        m_ratation += cameraRoataitonSpeed * Time.deltaTime;
        if (m_ratation >= 360f)
            m_ratation -= 360f;

        sceneCamera.position = Vector3.zero;
        sceneCamera.rotation = Quaternion.Euler(0f, m_ratation, 0f);
        sceneCamera.Translate(0f, cameraRotationRadius, -cameraRotationRadius);
        sceneCamera.LookAt(Vector3.zero);
    }
}
