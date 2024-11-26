using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace com.rotovr.sdk
{
    /// <summary>
    /// Connected device model.
    /// </summary>
    [Serializable]
    public class DeviceDataModel
    {

        public DeviceDataModel(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;
            Name = dict["Name"].ToString();
            Address = dict["Address"].ToString();
        }
        
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

        public string ToJson()
        {
            var dict = new Dictionary<string, string>();
            dict.Add("Name", Name);
            dict.Add("Address", Address);

            return Json.Serialize(dict);
        }
    }
}