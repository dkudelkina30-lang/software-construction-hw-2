namespace Pos.Domain;

public class EvaluationFactory
{
    private readonly IStudentRepository _students;
    private readonly ICriterionRepository _criteria;
    private readonly IEvaluationRepository _evaluations;

    public EvaluationFactory(IStudentRepository students, ICriterionRepository criteria, IEvaluationRepository evaluations)
    {
        _students = students;
        _criteria = criteria;
        _evaluations = evaluations;
    }

    public Evaluation Create(Guid studentId, Guid criterionId, double value, string comment)
    {
        var student = _students.GetById(studentId);

        if (student == null)
        {
            throw new DomainException("Такого студента нет");
        }

        var criterion = _criteria.GetById(criterionId);

        if (criterion == null)
        {
            throw new DomainException("Такого критерия нет в рубрикаторе");
        }

        var all = _evaluations.GetAll();
        
        for (int idx = 0; idx < all.Count; idx++)
        {
            if (all[idx].StudentId == studentId && all[idx].CriterionId == criterionId)
            {
                throw new DomainException("У студента уже есть оценка по этому критерию, сначала отмените её");
            }
        }

        var score = new Score(value);
        criterion.CheckScore(score);
        return new Evaluation(student.Id, criterion.Id, score, comment);
    }
}
