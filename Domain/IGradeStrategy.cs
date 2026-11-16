namespace Pos.Domain;

public interface IGradeStrategy
{
    string Name { get; }
    double Calculate(List<(double Part, double Weight)> parts);
}
