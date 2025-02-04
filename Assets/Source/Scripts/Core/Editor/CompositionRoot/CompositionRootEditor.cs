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
            SerializedProperty iterator = serializedObject.GetIterator();
            bool childFieldsLeft = true;

            while (iterator.NextVisible(childFieldsLeft))
            {
                if (iterator.propertyPath != ScriptFieldName 
                    && iterator.propertyPath != OtherGroupsFieldName)
                {
                    EditorGUILayout.PropertyField(iterator, true);
                }

                childFieldsLeft = false;
            }
        }
    }
}
