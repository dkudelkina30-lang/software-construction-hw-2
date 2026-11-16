namespace Pos.Domain;

public class WeightedGradeStrategy : IGradeStrategy
{
    public string Name => "Взвешенная сумма";

    public double Calculate(List<(double Part, double Weight)> parts)
    {
        double sum = 0;
        double weights = 0;
        
        for (int idx = 0; idx < parts.Count; idx++)
        {
            sum += parts[idx].Part * parts[idx].Weight;
            weights += parts[idx].Weight;
        }

        if (weights == 0)
        {
            return 0;
        }

        return sum / weights * 10;
    }
}
