using UnityEditor;
using UnityEngine;

namespace TanksArmageddon.Core.CompositionRoot.Editor
{
    [CustomEditor(typeof(BaseCompositionRoot), true)]
    public class CompositionRootEditor : UnityEditor.Editor
    {
        private const string OtherGroupsFieldName = "_otherGroups";
        private const string ScriptFieldName = "m_Script";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ScriptFieldName));
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Special Groups", EditorStyles.boldLabel);
            DrawChildFields();
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(OtherGroupsFieldName));

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawChildFields()
        {
            SerializedProperty serializedProperty = serializedObject.GetIterator();
            bool childFieldsLeft = true;

            while (serializedProperty.NextVisible(childFieldsLeft))
            {
                if (serializedProperty.propertyPath != ScriptFieldName 
                    && serializedProperty.propertyPath != OtherGroupsFieldName)
                {
                    EditorGUILayout.PropertyField(serializedProperty, true);
                }

                childFieldsLeft = false;
            }
        }
    }
}
