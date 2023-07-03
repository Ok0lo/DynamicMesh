using UnityEditor;
using UnityEngine;
using DynamicMesh;

[CustomEditor(typeof(HexMeshDynamic))]
public class HexMeshDynamicEditor : UnityEditor.Editor {

    public override void OnInspectorGUI() {
        var hexMeshDynamic = (HexMeshDynamic)target;

        if (hexMeshDynamic.IsNull() == true) {
            hexMeshDynamic.GenerateMesh();
        }

        if (DrawDefaultInspector() == true) {
            hexMeshDynamic.GenerateMesh();
        }

        if (GUILayout.Button("GenerateMesh")) {
            hexMeshDynamic.GenerateMesh();
        }
    }

}
