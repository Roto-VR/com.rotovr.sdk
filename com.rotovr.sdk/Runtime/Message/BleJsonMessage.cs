using System;
using System.Collections.Generic;

namespace com.rotovr.sdk
{
    [Serializable] 
    class BleJsonMessage
    {
        public BleJsonMessage(string command, string data)
        {
            Command = command;
            Data = data;
        }
        
        public BleJsonMessage(string json)
        {
            var dict = Json.Deserialize(json) as Dictionary<string, object>;

            Command = dict["Command"].ToString();
            Data = dict["Data"].ToString();
        }

        public string Command { get; }
        public string Data { get; }
        
        public string ToJson()
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Command", Command);
            dict.Add("Data", Data);
            
            return Json.Serialize(dict);
        }
    }
}
