namespace Pos.Domain;

public class AverageGradeStrategy : IGradeStrategy
{
    public string Name => "Среднее без весов";

    public double Calculate(List<(double Part, double Weight)> parts)
    {
        if (parts.Count == 0)
        {
            return 0;
        }

        double sum = 0;
        
        for (int idx = 0; idx < parts.Count; idx++)
        {
            sum += parts[idx].Part;
        }

        return sum / parts.Count * 10;
    }
}
