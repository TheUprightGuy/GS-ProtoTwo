using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThirdPersonCamera))]
public class THCEditor : Editor
{
    float minAng = -90.0f;
    float maxAng = 90.0f;

    float minZoom = 0.0f;
    float maxZoom = 150.0f;
    public override void OnInspectorGUI()
    {
        ThirdPersonCamera thc = (ThirdPersonCamera)target;

        base.OnInspectorGUI();

       
        EditorGUILayout.MinMaxSlider("Vertical Angle Range" ,ref thc.minYAngle, ref thc.maxYAngle, minAng, maxAng);
        EditorGUILayout.MinMaxSlider("Zoom Distance Range", ref thc.minZoomDist, ref thc.maxZoomDist, minZoom, maxZoom);
    }
}
