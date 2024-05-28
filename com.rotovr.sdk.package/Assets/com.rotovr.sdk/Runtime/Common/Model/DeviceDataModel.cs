using System;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Connected device model.
    /// </summary>
    [Serializable]
    public class DeviceDataModel
    {
        public DeviceDataModel(string name, string address)
        {
            Name = name;
            Address = address;
        }

        /// <summary>
        /// Device name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Device MAC Address.
        /// </summary>
        public string Address { get; }
    }
}