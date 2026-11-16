using Pos.Domain;

namespace Pos.Infrastructure;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();

    public void Add(Student student)
    {
        _students.Add(student);
    }

    public Student? GetById(Guid id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }

    public List<Student> GetAll()
    {
        return _students.ToList();
    }
}
