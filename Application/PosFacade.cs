using Pos.Domain;

namespace Pos.Application;

public class PosFacade
{
    private readonly IStudentRepository _students;
    private readonly ICriterionRepository _criteria;
    private readonly IEvaluationRepository _evaluations;
    private readonly EvaluationFactory _factory;
    private readonly FinalGradeService _gradeService;
    private readonly List<IGradeStrategy> _strategies;
    private readonly IReportExporter _exporter;

    public PosFacade(
        IStudentRepository students,
        ICriterionRepository criteria,
        IEvaluationRepository evaluations,
        EvaluationFactory factory,
        FinalGradeService gradeService,
        IEnumerable<IGradeStrategy> strategies,
        IReportExporter exporter)
    {
        _students = students;
        _criteria = criteria;
        _evaluations = evaluations;
        _factory = factory;
        _gradeService = gradeService;
        _strategies = strategies.ToList();
        _exporter = exporter;
    }

    public Student AddStudent(string name)
    {
        var student = new Student(name);
        _students.Add(student);
        return student;
    }

    public List<Student> GetStudents()
    {
        return _students.GetAll();
    }

    public Criterion AddCriterion(string name, double maxScore, double weight)
    {
        var criterion = new Criterion(name, maxScore, weight);
        _criteria.Add(criterion);
        return criterion;
    }

    public List<Criterion> GetCriteria()
    {
        return _criteria.GetAll();
    }

    public void AddEvaluation(Guid studentId, Guid criterionId, double score, string comment)
    {
        var evaluation = _factory.Create(studentId, criterionId, score, comment);
        _evaluations.Add(evaluation);
    }

    public List<EvaluationView> GetEvaluations()
    {
        var result = new List<EvaluationView>();
        var all = _evaluations.GetAll();
        
        for (int idx = 0; idx < all.Count; idx++)
        {
            var evaluation = all[idx];
            var student = _students.GetById(evaluation.StudentId);
            var criterion = _criteria.GetById(evaluation.CriterionId);

            if (student == null || criterion == null)
            {
                continue;
            }

            result.Add(new EvaluationView(evaluation.Id, student.Name, criterion.Name, evaluation.Score.Value,
                criterion.MaxScore, evaluation.Comment, evaluation.Date));
        }

        return result;
    }

    public void RemoveEvaluation(Guid evaluationId)
    {
        if (_evaluations.GetById(evaluationId) == null)
        {
            throw new DomainException("Оценка не найдена");
        }

        _evaluations.Remove(evaluationId);
    }

    public List<StudentResult> GetResults()
    {
        var results = new List<StudentResult>();
        var students = _students.GetAll();
        var criteria = _criteria.GetAll();
        var evaluations = _evaluations.GetAll();
        
        for (int idx = 0; idx < students.Count; idx++)
        {
            double grade = _gradeService.Calculate(students[idx], criteria, evaluations);
            results.Add(new StudentResult(students[idx].Name, grade));
        }

        return results;
    }

    public void ExportResults(string path)
    {
        _exporter.Export(GetResults(), path);
    }

    public List<string> GetStrategyNames()
    {
        return _strategies.Select(s => s.Name).ToList();
    }

    public string GetCurrentStrategyName()
    {
        return _gradeService.StrategyName;
    }

    public void SetStrategy(int number)
    {
        if (number < 0 || number >= _strategies.Count)
        {
            throw new DomainException("Нет такого правила подсчёта");
        }

        _gradeService.SetStrategy(_strategies[number]);
    }
}
