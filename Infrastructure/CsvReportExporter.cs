using System.Globalization;
using System.Text;
using Pos.Application;

namespace Pos.Infrastructure;

public class CsvReportExporter : IReportExporter
{
    public void Export(List<StudentResult> results, string path)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Студент: Итог");
        
        for (int idx = 0; idx < results.Count; idx++)
        {
            builder.AppendLine($"{results[idx].StudentName}: {results[idx].Grade.ToString(CultureInfo.InvariantCulture)}");
        }

        File.WriteAllText(path, builder.ToString(), Encoding.UTF8);
    }
}
