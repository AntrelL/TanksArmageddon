using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.Scripts.Realese.CustomPlugins
{
    public class FindUnassignedFields : EditorWindow
    {
        [MenuItem("Tools/Find Unassigned Fields")]
        public static void ShowWindow()
        {
            GetWindow<FindUnassignedFields>("Find Unassigned Fields");
        }

        private string _results = "";
        private readonly List<Object> _objectsToPing = new List<Object>();

        private void OnGUI()
        {
            if (GUILayout.Button("Check Scene"))
            {
                _results = "";
                _objectsToPing.Clear();
                CheckSceneForUnassignedFields();
            }

            GUILayout.Label("Results:");
            EditorGUILayout.BeginVertical("Box");

            for (int i = 0; i < _objectsToPing.Count; i++)
            {
                if (GUILayout.Button(_results.Split('\n')[i]))
                {
                    EditorGUIUtility.PingObject(_objectsToPing[i]);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void CheckSceneForUnassignedFields()
        {
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (var obj in allObjects)
            {
                Component[] components = obj.GetComponents<Component>();

                foreach (var component in components)
                {
                    if (component == null) continue;
                
                    if (!IsCustomScript(component.GetType()))
                        continue;

                    var fields = component.GetType().GetFields(
                        System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Public);

                    foreach (var field in fields)
                    {
                        if (Attribute.IsDefined(field, typeof(SerializeField)))
                        {
                            var value = field.GetValue(component);

                            if (value == null || (value is UnityEngine.Object unityObject && unityObject == null))
                            {
                                string message = $"Unassigned field '{field.Name}' in component '{component.GetType().Name}' on object '{obj.name}'";
                                _results += message + "\n";
                                _objectsToPing.Add(obj);
                            }
                        }
                    }
                }
            }

            _results += "Check completed.\n";
        }

        private bool IsCustomScript(System.Type type)
        {
            if (!typeof(UnityEngine.Object).IsAssignableFrom(type))
                return false;
        
            string[] scriptPaths = AssetDatabase.FindAssets($"t:Script {type.Name}");
        
            foreach (string guid in scriptPaths)
            {
                string scriptPath = AssetDatabase.GUIDToAssetPath(guid);
            
                if (scriptPath.Contains("/Realese/"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}