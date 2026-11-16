namespace Pos.Domain;

public interface IEvaluationRepository
{
    void Add(Evaluation evaluation);
    void Remove(Guid id);
    Evaluation? GetById(Guid id);
    List<Evaluation> GetAll();
}
