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
	string test = "";
    public void OnGUI()
    {
        test = EditorGUILayout.TextField ("Text Field", test );
    }
}
