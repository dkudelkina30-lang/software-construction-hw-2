using Microsoft.Extensions.DependencyInjection;
using Pos.Application;
using Pos.Domain;
using Pos.Infrastructure;
using Pos.Presentation;

var services = new ServiceCollection();

services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
services.AddSingleton<ICriterionRepository, InMemoryCriterionRepository>();
services.AddSingleton<IEvaluationRepository, InMemoryEvaluationRepository>();
services.AddSingleton<IReportExporter, CsvReportExporter>();

services.AddSingleton<WeightedGradeStrategy>();
services.AddSingleton<IGradeStrategy>(sp => sp.GetRequiredService<WeightedGradeStrategy>());
services.AddSingleton<IGradeStrategy, AverageGradeStrategy>();
services.AddSingleton(sp => new FinalGradeService(sp.GetRequiredService<WeightedGradeStrategy>()));

services.AddSingleton<EvaluationFactory>();
services.AddSingleton<PosFacade>();
services.AddSingleton<ConsoleUi>();

var provider = services.BuildServiceProvider();
provider.GetRequiredService<ConsoleUi>().Run();
