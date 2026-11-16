namespace Pos.Domain;

public class FinalGradeService
{
    private IGradeStrategy _strategy;

    public FinalGradeService(IGradeStrategy strategy)
    {
        _strategy = strategy;
    }

    public string StrategyName => _strategy.Name;

    public void SetStrategy(IGradeStrategy strategy)
    {
        _strategy = strategy;
    }

    public double Calculate(Student student, List<Criterion> criteria, List<Evaluation> evaluations)
    {
        var parts = new List<(double Part, double Weight)>();
        
        for (int idx = 0; idx < criteria.Count; idx++)
        {
            var criterion = criteria[idx];
            var evaluation = evaluations.FirstOrDefault(e => e.StudentId == student.Id && e.CriterionId == criterion.Id);
            double part = evaluation == null ? 0 : evaluation.Score.Value / criterion.MaxScore;
            parts.Add((part, criterion.Weight));
        }

        return Math.Round(_strategy.Calculate(parts), 2);
    }
}
