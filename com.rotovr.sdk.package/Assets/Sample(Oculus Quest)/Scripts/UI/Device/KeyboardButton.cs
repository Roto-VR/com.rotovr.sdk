using System;
using UnityEngine;
using UnityEngine.UI;


public class KeyboardButton : MonoBehaviour
{
    [SerializeField] string m_Value;
    Button m_Button;
    public static event Action<string> OnClick;

    void Start()
    {
        m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(() => { OnClick?.Invoke(m_Value); });
    }
}