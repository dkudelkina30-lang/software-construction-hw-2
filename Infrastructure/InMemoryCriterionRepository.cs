using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryCriterionRepository : ICriterionRepository
{
    private readonly List<Criterion> _criteria = new();

    public void Add(Criterion criterion)
    {
        _criteria.Add(criterion);
    }

    public Criterion? GetById(Guid id)
    {
        return _criteria.FirstOrDefault(c => c.Id == id);
    }

    public List<Criterion> GetAll()
    {
        return _criteria.ToList();
    }
}
