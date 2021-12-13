using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
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
#endif
