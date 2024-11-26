using UnityEditor;
using UnityEngine;

namespace com.rotovr.sdk.editor
{
#if !NO_UNITY
    [CustomEditor(typeof(RotoBehaviour))]
    public class RotoBehaviourEditor : Editor
    {
        SerializedProperty m_ConnectionType;
        SerializedProperty m_ModeType;
        SerializedProperty m_Target;

        void OnEnable()
        {
            // link serialized properties to the target's fields
            // more efficient doing this only once
            m_ConnectionType = serializedObject.FindProperty("m_ConnectionType");
            m_ModeType = serializedObject.FindProperty("m_ModeType");
            m_Target = serializedObject.FindProperty("m_Target");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_ConnectionType);
            EditorGUILayout.PropertyField(m_ModeType);

            if (m_ModeType.intValue == (int)RotoModeType.FollowObject ||
                m_ModeType.intValue == (int)RotoModeType.HeadTrack)
            {
                EditorGUILayout.Space();
                if (m_ModeType.intValue == (int)RotoModeType.FollowObject)
                {
                    EditorGUILayout.HelpBox(new GUIContent("Please provide target transform to observe rotation."));
                }
                else
                {
                    EditorGUILayout.HelpBox(new GUIContent("Please provide headset gameobject reference."));
                }


                EditorGUILayout.PropertyField(m_Target);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}