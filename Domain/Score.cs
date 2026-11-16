namespace Pos.Domain;

public record Score
{
    public double Value { get; }

    public Score(double value)
    {
        if (!double.IsFinite(value) || value < 0)
        {
            throw new DomainException("Балл должен быть неотрицательным числом");
        }

        Value = value;
    }
}
