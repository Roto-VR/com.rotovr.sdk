using System.Collections;
using UnityEngine;

namespace com.rotovr.sdk.editor
{
    class Visualization : MonoBehaviour
    {
        RotoBehaviour m_Behaviour;
        Coroutine m_RotationRoutine;
        float m_TargetAngle;
        RotoDataModel m_RotoModel;

        void Awake()
        {
            m_Behaviour = FindObjectOfType<RotoBehaviour>();
            if (m_Behaviour == null)
                return;

            m_RotoModel = new RotoDataModel();
            m_Behaviour.OnConnectionStatusChanged += OnConnectionHandler;
            m_Behaviour.OnDataChanged += OnDataHandler;
        }

        void OnDestroy()
        {
            if (m_Behaviour == null)
                return;
            m_Behaviour.OnConnectionStatusChanged -= OnConnectionHandler;
            m_Behaviour.OnDataChanged -= OnDataHandler;
        }

        void OnDataHandler(RotoDataModel model)
        {
            m_RotoModel = model;
        }

        void OnConnectionHandler(ConnectionStatus status)
        {
            switch (status)
            {
                case ConnectionStatus.Connected:
                    if (m_RotationRoutine == null)
                        m_RotationRoutine = StartCoroutine(RotationRoutine());
                    break;
                case ConnectionStatus.Disconnected:
                    if (m_RotationRoutine != null)
                    {
                        StopCoroutine(m_RotationRoutine);
                        m_RotationRoutine = null;
                    }

                    break;
            }
        }

        IEnumerator RotationRoutine()
        {
            while (true)
            {
                yield return null;
                var desiredRotQ = Quaternion.Euler(transform.localEulerAngles.x, m_RotoModel.Angle,
                    transform.localEulerAngles.z);
                transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotQ, Time.deltaTime * 10f);
            }
        }
    }
}