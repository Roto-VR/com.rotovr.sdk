using System;

namespace com.rotovr.sdk
{
    [Serializable]
    public class RotateToAngleModel
    {
        public RotateToAngleModel(int angle, int power, string direction)
        {
            Angle = angle;
            Power = power;
            Direction = direction;
        }
        public int Angle { get; }
        public int Power { get; }
        public string Direction { get; }
    }
}
