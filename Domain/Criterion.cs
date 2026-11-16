namespace Pos.Domain;

public class Criterion
{
    public Guid Id { get; }
    public string Name { get; }
    public double MaxScore { get; }
    public double Weight { get; }

    public Criterion(string name, double maxScore, double weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Название критерия не может быть пустым");
        }

        if (!double.IsFinite(maxScore) || maxScore <= 0)
        {
            throw new DomainException("Максимальный балл должен быть больше нуля");
        }

        if (!double.IsFinite(weight) || weight <= 0)
        {
            throw new DomainException("Вес критерия должен быть больше нуля");
        }

        Id = Guid.NewGuid();
        Name = name.Trim();
        MaxScore = maxScore;
        Weight = weight;
    }

    public void CheckScore(Score score)
    {
        if (score.Value > MaxScore)
        {
            throw new DomainException($"Балл {score.Value} больше максимума {MaxScore} по критерию \"{Name}\"");
        }
    }
}
