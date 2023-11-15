using System.Collections;
using UnityEngine;

namespace RotoVR.SDK.Components
{
    public class MotionCompensation : MonoBehaviour
    {
        [SerializeField] private Transform m_motionTarget;
        Coroutine m_Routine;
        Vector3 m_StartDeltaPosition;
        Vector3 m_StartDeltaAngle;

        void Awake()
        {
            if (m_motionTarget == null)
                return;

            m_StartDeltaPosition = m_motionTarget.position - transform.position;
            m_StartDeltaAngle = m_motionTarget.eulerAngles - transform.eulerAngles;
            m_Routine = StartCoroutine(CompensationRoutine());
        }

        void OnDestroy()
        {
            if (m_Routine != null)
            {
                StopCoroutine(m_Routine);
                m_Routine = null;
            }
        }

        IEnumerator CompensationRoutine()
        {
            while (true)
            {
                transform.position = m_motionTarget.position - m_StartDeltaPosition;
                transform.eulerAngles = m_motionTarget.eulerAngles - m_StartDeltaAngle;
                yield return null;
            }
        }
    }
}