using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryEvaluationRepository : IEvaluationRepository
{
    private readonly List<Evaluation> _evaluations = new();

    public void Add(Evaluation evaluation)
    {
        _evaluations.Add(evaluation);
    }

    public void Remove(Guid id)
    {
        _evaluations.RemoveAll(e => e.Id == id);
    }

    public Evaluation? GetById(Guid id)
    {
        return _evaluations.FirstOrDefault(e => e.Id == id);
    }

    public List<Evaluation> GetAll()
    {
        return _evaluations.ToList();
    }
}
