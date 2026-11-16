namespace Pos.Domain;

public interface IStudentRepository
{
    void Add(Student student);
    Student? GetById(Guid id);
    List<Student> GetAll();
}
