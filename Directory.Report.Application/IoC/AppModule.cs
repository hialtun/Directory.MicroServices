using Autofac;
using Directory.Report.Application.DataAccess;

namespace Directory.Report.Application.IoC
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // repository
            builder.RegisterType<ReportRepository>().SingleInstance();
        }
    }
}