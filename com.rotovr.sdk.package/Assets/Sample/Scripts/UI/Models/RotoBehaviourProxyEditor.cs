

using UnityEditor;
using UnityEngine;

namespace com.rotovr.sdk.sample
{

    [CustomEditor(typeof(RotoBehaviourProxy))]
    public class RotoBehaviourProxyEditor : Editor
    {
        RotoBehaviourProxy Roto => (RotoBehaviourProxy)target;

        public override void OnInspectorGUI()
        {
            
            Roto.ConnectionType = (ConnectionType)EditorGUILayout.EnumPopup("Connection Type", Roto.ConnectionType);
            Roto.Mode = (RotoModeType)EditorGUILayout.EnumPopup("Mode", Roto.Mode);
            Roto.DeviceName = EditorGUILayout.TextField("Device Name", Roto.DeviceName);
            
            if (Roto.Mode == RotoModeType.FollowObject ||
                Roto.Mode == RotoModeType.HeadTrack)
            {
                EditorGUILayout.Space();
                if (Roto.Mode == RotoModeType.FollowObject)
                {
                    EditorGUILayout.HelpBox(new GUIContent("Please provide target transform to observe rotation."));
                }
                else
                {
                    EditorGUILayout.HelpBox(new GUIContent("Please provide headset gameobject reference."));
                }

                Roto.Target = (Transform) EditorGUILayout.ObjectField(Roto.Target, typeof(Transform), true);
            }

        }
    }
}
