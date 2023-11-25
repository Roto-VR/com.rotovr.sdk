using RotoVR.SDK.Components;
using UnityEngine;

namespace Example.Helper
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform m_Target;
        [SerializeField] RotoBehaviour m_Roto;
        [SerializeField] float m_SmoothTime = 0.3F;
        Vector3 m_Velocity = Vector3.zero;

        private void Update()
        {
            Vector3 targetPosition = m_Target.TransformPoint(new Vector3(0, 0, 2));

            Vector3 position = Vector3.SmoothDamp(transform.position, targetPosition, ref m_Velocity, m_SmoothTime);
            transform.position = new Vector3(position.x, position.y, position.z);
            var lookAtPos = new Vector3(m_Target.position.x, m_Target.position.y, m_Target.position.z);
            transform.LookAt(lookAtPos);
        }
    }
}