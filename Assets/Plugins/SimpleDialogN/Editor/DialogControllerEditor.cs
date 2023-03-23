using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogController))]
public class DialogControllerEditor : Editor
{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        DialogController dialogController = (DialogController)target;
        if(GUILayout.Button("Instantiate Dialog Canvas")){
            dialogController.InstatiateDialogCanvas();
        }
        if(GUILayout.Button("Instantiate Don't Destroy on Load")){
            dialogController.SetDontDestroyOnLoad();
        }
    }
}