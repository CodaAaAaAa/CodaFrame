
using UnityEngine;
using UnityEditor;
using Coda.Tools;

namespace Coda.Editor
{
    [CustomEditor(typeof(AdaptiveResolution3D))]
    public class AdaptiveResolution3DInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SerializedObject so = new SerializedObject(target);

            SerializedProperty minAspect = so.FindProperty("_minAspect");
            SerializedProperty maxAspect = so.FindProperty("_maxAspect");

            SerializedProperty minResolution = so.FindProperty("_minResolution");
            SerializedProperty maxResolution = so.FindProperty("_maxResolution");

            SerializedProperty fixType = so.FindProperty("_fixType");
            SerializedProperty fieldOfView = so.FindProperty("_fieldOfView");

            bool isLegal = true;

            EditorGUILayout.Space();
            _DrawResolution("Min Resolution", "Min Aspect", minResolution, minAspect);
            _DrawResolution("Max Resolution", "Max Aspect", maxResolution, maxAspect);

            EditorGUILayout.PropertyField(fixType);
            if (fixType.intValue != 0)
                EditorGUILayout.PropertyField(fieldOfView);

            so.ApplyModifiedProperties();

            GUILayout.Space(10f);

            if (float.IsNaN(minAspect.floatValue) || float.IsNaN(maxAspect.floatValue) || float.IsInfinity(minAspect.floatValue) || float.IsInfinity(maxAspect.floatValue))
            {
                EditorGUILayout.HelpBox("Please input correct resolution.", MessageType.Error);
                isLegal = false;
            }

            if (minAspect.floatValue > maxAspect.floatValue)
            {
                EditorGUILayout.HelpBox("Min aspect can't not larger than max aspect.", MessageType.Error);
                isLegal = false;
            }

            if (((MonoBehaviour)target).GetComponent<Camera>().orthographic)
            {
                EditorGUILayout.HelpBox("Only perspective camera can use this script.", MessageType.Error);
                isLegal = false;
            }

            GUIStyle style = new GUIStyle(EditorStyles.toolbarButton);
            style.fontSize = 15;

            if (isLegal && GUILayout.Button("Execute", style))
                ((AdaptiveResolution3D)target).ReAdaptResolution();

            GUILayout.Space(10f);
        }

        private void _DrawResolution(string title, string aspectName, SerializedProperty resolution, SerializedProperty aspect)
        {
            EditorGUILayout.LabelField(title);
            EditorGUILayout.BeginHorizontal();
            Vector2 r = new Vector2();
            r.x = EditorGUILayout.FloatField("Weight:", resolution.vector2Value.x);
            r.y = EditorGUILayout.FloatField("Height:", resolution.vector2Value.y);
            resolution.vector2Value = r;
            EditorGUILayout.EndHorizontal();

            EditorTools.StartBlueContents();
            aspect.floatValue = resolution.vector2Value.x / resolution.vector2Value.y;
            EditorGUILayout.LabelField(aspectName + " : " + aspect.floatValue);
            EditorTools.EndBlueContents();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}