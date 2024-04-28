namespace RotoVR.Common.Model;

public struct CompensationModel
{
    public CompensationModel()
    {
        X = 0;
        Y = 0;
    }

    public CompensationModel(decimal x, decimal y)
    {
        X = x;
        Y = y;
    }

    public decimal X { get; }
    public decimal Y { get; }
}