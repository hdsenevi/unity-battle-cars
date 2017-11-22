using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneMaterialSwapper : EditorWindow {

	[MenuItem("Tools/SceneMaterialSwapper")]
	public static void Window()
    {
        EditorWindow.GetWindow(typeof(SceneMaterialSwapper));

        Debug.Log("It should have appeared!");
    }
	
	// Update is called once per frame
	static string tagStr = "";
	public Material matFind = null;
	public Material matReplace = null;
    void OnGUI()
    {
		EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Swap materials for selection"))
        {
            SwapMaterials();
        }
		EditorGUILayout.EndVertical();

        Rect lastRect = GUILayoutUtility.GetLastRect();

        matFind = (Material)EditorGUI.ObjectField(new Rect(3, lastRect.y + lastRect.height + 3, position.width - 6, 20), "Find material", matFind, typeof(Material), true);
        matReplace = (Material)EditorGUI.ObjectField(new Rect(3, lastRect.y + lastRect.height + 23, position.width - 6, 20), "Replace material", matReplace, typeof(Material), true);
    }

	void OnInspectorUpdate()	
    {
        Repaint();
    }

    void SwapMaterials()
    {
        if (matFind == null || matReplace == null) {
            Debug.LogError("Either 'Find Material' or 'Replace Material' not assigned.");
            return;
        }

        foreach (GameObject go in Selection.gameObjects)
        {
            MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];
            foreach (MeshRenderer mr in mrs) {
                Material[] mats = mr.sharedMaterials;
                for(int i = 0; i < mats.Length; i++) {
                    if (mats[i].name == matFind.name) {
                        Debug.Log("material name : " + mats[i].name);
                        mats[i] = matReplace;
                        mr.materials = mats;
                    }
                }
            }
        }
    }
}
