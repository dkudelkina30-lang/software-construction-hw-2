namespace Pos.Domain;

public class Student
{
    public Guid Id { get; }
    public string Name { get; }

    public Student(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Имя студента не может быть пустым");
        }

        Id = Guid.NewGuid();
        Name = name.Trim();
    }
}
