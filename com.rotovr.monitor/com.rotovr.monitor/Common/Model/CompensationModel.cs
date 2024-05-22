namespace RotoVR.Common.Model;

public struct CompensationModel
{
    public CompensationModel()
    {
        X = 0;
        Y = 0;
    }

    public CompensationModel(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; }
    public double Y { get; }
}