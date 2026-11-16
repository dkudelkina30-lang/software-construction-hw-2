namespace Pos.Domain;

public interface ICriterionRepository
{
    void Add(Criterion criterion);
    Criterion? GetById(Guid id);
    List<Criterion> GetAll();
}
