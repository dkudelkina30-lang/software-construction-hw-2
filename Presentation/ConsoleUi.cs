using System.Globalization;
using Pos.Application;
using Pos.Domain;

namespace Pos.Presentation;

public class ConsoleUi
{
    private readonly PosFacade _facade;

    public ConsoleUi(PosFacade facade)
    {
        _facade = facade;
    }

    public void Run()
    {
        while (true)
        {
            PrintMenu();
            string choice = ReadText("Выберите пункт: ");

            if (choice == "0")
            {
                return;
            }

            try
            {
                HandleChoice(choice);
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка записи файла: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа к файлу: {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    private void PrintMenu()
    {
        Console.WriteLine("===== ПОС =====");
        Console.WriteLine("1. Добавить студента");
        Console.WriteLine("2. Список студентов");
        Console.WriteLine("3. Добавить критерий");
        Console.WriteLine("4. Рубрикатор");
        Console.WriteLine("5. Выставить оценку");
        Console.WriteLine("6. Список оценок");
        Console.WriteLine("7. Отменить оценку");
        Console.WriteLine("8. Итоги");
        Console.WriteLine("9. Сменить правило подсчёта итога");
        Console.WriteLine("10. Экспорт итогов в CSV");
        Console.WriteLine("11. Демо-сценарий");
        Console.WriteLine("0. Выход");
    }

    private void HandleChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                AddStudent();
                break;
            case "2":
                PrintStudents();
                break;
            case "3":
                AddCriterion();
                break;
            case "4":
                PrintCriteria();
                break;
            case "5":
                AddEvaluation();
                break;
            case "6":
                PrintEvaluations();
                break;
            case "7":
                RemoveEvaluation();
                break;
            case "8":
                PrintResults();
                break;
            case "9":
                ChangeStrategy();
                break;
            case "10":
                Export();
                break;
            case "11":
                RunDemo();
                break;
            default:
                Console.WriteLine("Нет такого пункта");
                break;
        }
    }

    private void AddStudent()
    {
        string name = ReadText("Имя студента: ");
        var student = _facade.AddStudent(name);
        Console.WriteLine($"Студент {student.Name} добавлен");
    }

    private void PrintStudents()
    {
        var students = _facade.GetStudents();
        if (students.Count == 0)
        {
            Console.WriteLine("Студентов пока нет");
            return;
        }

        for (int idx = 0; idx < students.Count; idx++)
        {
            Console.WriteLine($"{idx + 1}. {students[idx].Name}");
        }
    }

    private void AddCriterion()
    {
        string name = ReadText("Название критерия: ");
        double maxScore = ReadNumber("Максимальный балл: ");
        double weight = ReadNumber("Вес: ");
        var criterion = _facade.AddCriterion(name, maxScore, weight);
        Console.WriteLine($"Критерий {criterion.Name} добавлен");
    }

    private void PrintCriteria()
    {
        var criteria = _facade.GetCriteria();

        if (criteria.Count == 0)
        {
            Console.WriteLine("Рубрикатор пуст");
            return;
        }

        for (int idx = 0; idx < criteria.Count; idx++)
        {
            Console.WriteLine($"{idx + 1}. {criteria[idx].Name} (макс. {criteria[idx].MaxScore}, вес {criteria[idx].Weight})");
        }
    }

    private void AddEvaluation()
    {
        var students = _facade.GetStudents();
        var criteria = _facade.GetCriteria();

        if (students.Count == 0 || criteria.Count == 0)
        {
            Console.WriteLine("Сначала добавьте студентов и критерии");
            return;
        }

        PrintStudents();
        int studentNumber = ReadIndex("Номер студента: ", students.Count);
        PrintCriteria();
        int criterionNumber = ReadIndex("Номер критерия: ", criteria.Count);
        double score = ReadNumber("Балл: ");
        string comment = ReadText("Комментарий (можно пустой): ");
        _facade.AddEvaluation(students[studentNumber].Id, criteria[criterionNumber].Id, score, comment);
        Console.WriteLine("Оценка выставлена");
    }

    private List<EvaluationView> PrintEvaluations()
    {
        var evaluations = _facade.GetEvaluations();

        if (evaluations.Count == 0)
        {
            Console.WriteLine("Оценок пока нет");
            return evaluations;
        }

        for (int idx = 0; idx < evaluations.Count; idx++)
        {
            var e = evaluations[idx];
            Console.WriteLine($"{idx + 1}. {e.StudentName} | {e.CriterionName} | {e.Score}/{e.MaxScore} | {e.Date:dd.MM.yyyy HH:mm} | {e.Comment}");
        }

        return evaluations;
    }

    private void RemoveEvaluation()
    {
        var evaluations = PrintEvaluations();

        if (evaluations.Count == 0)
        {
            return;
        }

        int number = ReadIndex("Номер оценки: ", evaluations.Count);
        _facade.RemoveEvaluation(evaluations[number].Id);
        Console.WriteLine("Оценка отменена");
    }

    private void PrintResults()
    {
        var results = _facade.GetResults();

        if (results.Count == 0)
        {
            Console.WriteLine("Студентов пока нет");
            return;
        }

        Console.WriteLine($"Правило подсчёта: {_facade.GetCurrentStrategyName()}");
        
        for (int idx = 0; idx < results.Count; idx++)
        {
            Console.WriteLine($"{results[idx].StudentName}: {results[idx].Grade} из 10");
        }
    }

    private void ChangeStrategy()
    {
        var names = _facade.GetStrategyNames();

        for (int idx = 0; idx < names.Count; idx++)
        {
            Console.WriteLine($"{idx + 1}. {names[idx]}");
        }

        int number = ReadIndex("Номер правила: ", names.Count);
        _facade.SetStrategy(number);
        Console.WriteLine($"Теперь итог считается так: {_facade.GetCurrentStrategyName()}");
    }

    private void Export()
    {
        string path = ReadText("Имя файла (по умолчанию results.csv): ");
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "results.csv";
        }

        _facade.ExportResults(path);
        Console.WriteLine($"Итоги сохранены в {Path.GetFullPath(path)}");
    }

    private void RunDemo()
    {
        Console.WriteLine("--- Заводим студентов и критерии ---");
        var ivanov = _facade.AddStudent("Иванов Иван");
        var petrova = _facade.AddStudent("Петрова Анна");
        var homework = _facade.AddCriterion("Домашнее задание", 10, 0.5);
        var test = _facade.AddCriterion("Контрольная", 5, 0.3);
        var activity = _facade.AddCriterion("Активность", 3, 0.2);
        PrintStudents();
        PrintCriteria();

        Console.WriteLine("--- Выставляем оценки ---");
        _facade.AddEvaluation(ivanov.Id, homework.Id, 8, "хорошо");
        _facade.AddEvaluation(ivanov.Id, test.Id, 4, "");
        _facade.AddEvaluation(ivanov.Id, activity.Id, 3, "активен на семинарах");
        _facade.AddEvaluation(petrova.Id, homework.Id, 10, "отлично");
        _facade.AddEvaluation(petrova.Id, test.Id, 3, "");

        Console.WriteLine("Пробуем поставить 12 баллов по критерию с максимумом 3:");
        try
        {
            _facade.AddEvaluation(petrova.Id, activity.Id, 12, "");
        }
        catch (DomainException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        PrintEvaluations();

        Console.WriteLine("--- Итоги ---");
        PrintResults();

        Console.WriteLine("--- Экспорт ---");
        _facade.ExportResults("demo_results.csv");
        Console.WriteLine($"Итоги сохранены в {Path.GetFullPath("demo_results.csv")}");
    }

    private string ReadText(string message)
    {
        Console.Write(message);
        string? line = Console.ReadLine();
        
        if (line == null)
        {
            Environment.Exit(0);
        }

        return line;
    }

    private double ReadNumber(string message)
    {
        while (true)
        {
            string text = ReadText(message).Replace(',', '.');
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }

            Console.WriteLine("Нужно ввести число");
        }
    }

    private int ReadIndex(string message, int count)
    {
        while (true)
        {
            string text = ReadText(message);
            if (int.TryParse(text, out int number) && number >= 1 && number <= count)
            {
                return number - 1;
            }

            Console.WriteLine($"Нужно ввести число от 1 до {count}");
        }
    }
}
