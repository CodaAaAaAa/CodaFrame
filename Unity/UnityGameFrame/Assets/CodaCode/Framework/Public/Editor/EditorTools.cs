
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

namespace Coda.Editor
{
    public static class EditorTools
    {
        public static void DrawTitle(string text, int fontSize = 20)
        {
            GUILayout.BeginVertical();
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = fontSize;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField(text, style);

                GUILayout.Space(10);

                DrawPartLine();
            }
            GUILayout.EndVertical();
        }

        public static void DrawFooter(string text, int fontSize = 10)
        {
            GUILayout.BeginVertical();
            {
                DrawPartLine();

                GUIStyle style = new GUIStyle();
                style.fontSize = fontSize;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleRight;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField(text, style);

                GUILayout.Space(10);
            }
            GUILayout.EndVertical();
        }

        public static void DrawPartLine()
        {
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MaxHeight(10f));
            EditorGUILayout.EndHorizontal();
        }

        public static bool DrawHeader(string text)
        {
            string key = text;
            bool state = EditorPrefs.GetBool(key, true);

            GUILayout.Space(3f);
            if (!state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUILayout.Space(8f);
            GUI.changed = false;

            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
            {
                state = !state;
            }

            if (GUI.changed) { EditorPrefs.SetBool(key, state); }

            GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!state) GUILayout.Space(3f);
            return state;
        }

        public static void StartChildContents()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndChildContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
            GUILayout.Space(3f);
        }

        public static void StartBlueContents()
        {
            GUI.backgroundColor = new Color32(150, 200, 255, 255);
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
            GUILayout.BeginVertical();
            GUILayout.Space(2f);
        }

        public static void EndBlueContents()
        {
            GUILayout.Space(3f);
            GUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3f);
            GUI.backgroundColor = Color.white;
        }

        [MenuItem("Assets/GetReference")]
        private static void GetReference()
        {
            string target = "";
            if (Selection.activeObject != null)
                target = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(target))
                return;
            string[] files = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);
            string[] scene = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);

            List<Object> filelst = new List<Object>();
            for (int i = 0; i < files.Length; i++)
            {
                string[] source = AssetDatabase.GetDependencies(new string[] { files[i].Replace(Application.dataPath, "Assets") });
                for (int j = 0; j < source.Length; j++)
                {
                    if (source[j] == target)
                        filelst.Add(AssetDatabase.LoadMainAssetAtPath(files[i].Replace(Application.dataPath, "Assets")));
                }
            }
            for (int i = 0; i < scene.Length; i++)
            {
                string[] source = AssetDatabase.GetDependencies(new string[] { scene[i].Replace(Application.dataPath, "Assets") });
                for (int j = 0; j < source.Length; j++)
                {
                    if (source[j] == target)
                        filelst.Add(AssetDatabase.LoadMainAssetAtPath(scene[i].Replace(Application.dataPath, "Assets")));
                }
            }
            Selection.objects = filelst.ToArray();
        }
    }
}