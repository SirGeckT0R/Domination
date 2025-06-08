#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder; // For PolyShape
using System.Linq;

public class BakePolyShapePoints : EditorWindow
{
    [MenuItem("Tools/Bake All BorderDrawer PolyShapes")]
    public static void BakeAllBorderDrawers()
    {
        var drawers = GameObject.FindObjectsOfType<BordersVisualization>();

        int bakedCount = 0;

        foreach (var drawer in drawers)
        {
            var polyShape = drawer.GetComponent<PolyShape>();
            if (polyShape == null) continue;

            var bakedPoints = polyShape.controlPoints.ToArray();

            Undo.RecordObject(drawer, "Bake BorderDrawer Points");

            SerializedObject so = new SerializedObject(drawer);
            SerializedProperty pointsProp = so.FindProperty("savedPoints");

            pointsProp.ClearArray();
            for (int i = 0; i < bakedPoints.Length; i++)
            {
                pointsProp.InsertArrayElementAtIndex(i);
                pointsProp.GetArrayElementAtIndex(i).vector3Value = bakedPoints[i];
            }

            so.ApplyModifiedProperties();
            EditorUtility.SetDirty(drawer);
            bakedCount++;
        }

    }
}
#endif