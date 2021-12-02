using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AdvanceMeshMerge))]
public class MeshCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AdvanceMeshMerge mm = (AdvanceMeshMerge)target;
        if (GUILayout.Button("Merge Child Meshes") )
        {
            mm.AdvancedMerge();
        }
    }
}
