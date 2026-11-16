namespace Pos.Domain;

public class Evaluation
{
    public Guid Id { get; }
    public Guid StudentId { get; }
    public Guid CriterionId { get; }
    public Score Score { get; }
    public string Comment { get; }
    public DateTime Date { get; }

    internal Evaluation(Guid studentId, Guid criterionId, Score score, string comment)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        CriterionId = criterionId;
        Score = score;
        Comment = comment;
        Date = DateTime.Now;
    }
}
