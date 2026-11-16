namespace Pos.Application;

public record EvaluationView(Guid Id, string StudentName, string CriterionName, double Score, double MaxScore, string Comment, DateTime Date);
