using System;

namespace Example.BLE.Model
{
    [Serializable]
    public class RotationModel
    {
        public RotationModel(float angle, float time, int direction)
        {
            Angle = angle;
            Time = time;
            Direction = direction;
        }

        public float Angle { get; }
        public float Time { get; }
        public int Direction { get; }
    }
}
