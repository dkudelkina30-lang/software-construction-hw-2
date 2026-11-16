namespace Pos.Application;

public interface IReportExporter
{
    void Export(List<StudentResult> results, string path);
}
