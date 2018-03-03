using UnityEngine;
using UnityEngine.Networking;

public class NetworkCarColor : NetworkBehaviour
{
    [SyncVar]
    public Color color;

    // Use this for initialization
    void Start()
    {
        CarPaint carPaint = gameObject.GetComponent<CarPaint>() as CarPaint;
        carPaint.ColorCar(color);
    }
}
