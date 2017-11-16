using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPaint : MonoBehaviour
{

    public MeshRenderer[] m_carPaintMeshRenderes;
    public string[] m_excludeMatNames;

    public void ColorCar(Color color)
    {
        foreach (MeshRenderer mr in m_carPaintMeshRenderes)
        {
            foreach (Material mat in mr.materials)
            {
                if (!m_excludeMatNames.Contains(mat.name))
                {
                    mat.color = color;
                }
            }
        }
    }
}
