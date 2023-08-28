using System;

namespace Example.BLE.Model
{
    [Serializable]
    public class DeviceDataModel
    {
        public DeviceDataModel(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public string Name { get; }
        public string Address { get; }
    }
}
