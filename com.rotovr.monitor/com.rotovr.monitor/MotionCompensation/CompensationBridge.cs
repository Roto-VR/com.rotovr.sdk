using RotoVR.Common.Model;
using Microsoft.Win32;

namespace RotoVR.MotionCompensation;

public class CompensationBridge : ICompensationBridge
{
    private CompensationModel m_compensationModel = new();
    private const string m_keyName = "MotionCompensation";
    private const string m_xValue = "X_Value";
    private const string m_yValue = "Y_Value";
    private RegistryKey m_registryKey;

    public void Init()
    {
        /// Load OpenVR API
        ///
        /// 

        var names = Registry.CurrentUser.GetSubKeyNames();
        if (!names.Contains(m_keyName))
        {
            m_registryKey = Registry.CurrentUser.CreateSubKey(m_keyName);
            m_registryKey.Close();
        }
        else
        {
            m_registryKey = Registry.CurrentUser.OpenSubKey(m_keyName);
            decimal x = Convert.ToDecimal(m_registryKey.GetValue(m_xValue));
            decimal y = Convert.ToDecimal(m_registryKey.GetValue(m_yValue));
            m_registryKey.Close();
            m_compensationModel = new CompensationModel(x, y);
        }
    }

    public CompensationModel GetCompensationModel()
    {
        return m_compensationModel;
    }

    public void SetCompensationValue(CompensationModel model)
    {
        m_compensationModel = model;
        m_registryKey = Registry.CurrentUser.OpenSubKey(m_keyName, true);
        m_registryKey.SetValue(m_xValue, m_compensationModel.X);
        m_registryKey.SetValue(m_yValue, m_compensationModel.Y);
        m_registryKey.Close();
    }

    public void Start()
    {
    }

    public void Stop()
    {
    }
}