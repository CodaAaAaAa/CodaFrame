using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using Coda.Tools;

namespace Coda.Editor
{
    [CustomEditor(typeof(PoolManager), true)]
    public class PoolManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _DrawPoolObject();
        }

        private void _DrawPoolObject()
        {
            PoolManager.ForEditorData data = ((PoolManager)target)._GetEditorData();

            if (data.classPool.Count == 0) return;

            EditorTools.StartChildContents();
            EditorTools.StartBlueContents();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Pooled object");
            EditorGUILayout.LabelField("Number of object");
            EditorGUILayout.EndHorizontal();

            foreach (KeyValuePair<Type, int> kv in data.classPool)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kv.Key.Name);
                EditorGUILayout.LabelField(kv.Value.ToString());
                EditorGUILayout.EndHorizontal();
            }

            EditorTools.EndBlueContents();
            EditorTools.EndChildContents();
        }
    }
}