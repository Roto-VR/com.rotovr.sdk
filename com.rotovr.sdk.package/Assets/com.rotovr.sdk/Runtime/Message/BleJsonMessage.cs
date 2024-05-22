using System;

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

        public string Command { get; }
        public string Data { get; }
    }
}
