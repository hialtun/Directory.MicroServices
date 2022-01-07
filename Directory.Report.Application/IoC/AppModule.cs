using Autofac;
using Directory.Report.Application.DataAccess;
using MicroServices.Infrastructure.Repository;

namespace Directory.Report.Application.IoC
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // repository
            builder.RegisterType<GenericRepository<Domain.Entities.Report>>()
                .As<IRepository<Domain.Entities.Report>>().SingleInstance();
        }
    }
}