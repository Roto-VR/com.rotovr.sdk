using System;
using System.Collections;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [SerializeField] float m_Angle = 100;
    [SerializeField] float m_Speed = 1;
    float m_StartAngle;
    Direction m_Direction;
    Coroutine m_RotationRoutine;

    public void StartRotation()
    {
        m_Direction = Direction.Right;
        m_RotationRoutine = StartCoroutine(Rotate());
        m_StartAngle = transform.eulerAngles.y;
    }

    public void StopRotation()
    {
        if (m_RotationRoutine != null)
        {
            StopCoroutine(m_RotationRoutine);
        }

        transform.eulerAngles = Vector3.zero;
    }

    IEnumerator Rotate()
    {
        float angle = transform.eulerAngles.y;
        float delta = 0;
        float targetAngle =
            NormalizeAngle(m_Direction == Direction.Right ? m_StartAngle + m_Angle : m_StartAngle - m_Angle);

        while (Mathf.Abs(targetAngle - NormalizeAngle(transform.eulerAngles.y)) > 5)
        {
            yield return null;
            delta += Time.deltaTime * m_Speed;
            switch (m_Direction)
            {
                case Direction.Right:
                    angle += delta;
                    break;
                case Direction.Left:
                    angle -= delta;
                    break;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z);
        }

        m_Direction = m_Direction == Direction.Right ? Direction.Left : Direction.Right;

        yield return Wait();
        m_RotationRoutine = StartCoroutine(Rotate());
    }

    float NormalizeAngle(float angle)
    {
        if (angle < 0)
            angle += 360;
        else if (angle > 360)
            angle -= 360;

        return angle;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
    }

    enum Direction
    {
        Left,
        Right,
    }
}