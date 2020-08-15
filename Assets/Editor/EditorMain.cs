using UnityEditor;
using UnityEngine;

public class EditorMain : EditorWindow
{
    [MenuItem("Window/Custom Editor")]
    public static void ShowGUI()
    {
        GetWindow<EditorMain>("Custom Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("BIER");
    }
}
