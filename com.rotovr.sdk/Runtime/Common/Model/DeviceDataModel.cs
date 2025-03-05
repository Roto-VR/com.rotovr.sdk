using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Represents a connected Roto VR device, including its name and MAC address.
    /// </summary>
    [Serializable]
    public class DeviceDataModel
    {

        /// <summary>
        /// Initializes a new instance of <see cref="DeviceDataModel"/> from a JSON string.
        /// </summary>
        /// <param name="json">A JSON-formatted string containing the device name and address.</param>
        public DeviceDataModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            Name = dict["Name"].ToString();
            Address = dict["Address"].ToString();
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="DeviceDataModel"/> with the specified name and address.
        /// </summary>
        /// <param name="name">The name of the device.</param>
        /// <param name="address">The MAC address of the device.</param>
        public DeviceDataModel(string name, string address)
        {
            Name = name;
            Address = address;
        }

        /// <summary>
        /// Gets the name of the connected device.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the MAC address of the connected device.
        /// </summary>
        public string Address { get; }

        /// <summary>
        /// Converts the current <see cref="DeviceDataModel"/> instance into a JSON string.
        /// </summary>
        /// <returns>A JSON-formatted string representing the device data.</returns>
        public string ToJson()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Name", Name);
            dict.Add("Address", Address);

            return Json.Serialize(dict);
        }
    }
}