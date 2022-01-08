using System.Reflection;
using Autofac;
using Directory.Report.Application.DataAccess;
using Directory.Report.Application.RestClients;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MicroServices.Infrastructure.MessageBroker;
using MicroServices.Infrastructure.Repository;
using Module = Autofac.Module;

namespace Directory.Report.Application.IoC
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            // repository
            builder.RegisterType<GenericRepository<Domain.Entities.Report>>()
                .As<IRepository<Domain.Entities.Report>>().SingleInstance();
            
            // rabbitmq
            builder.RegisterType<RabbitMQClient>().SingleInstance();

            // event publisher
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IPublisher<>)).InstancePerLifetimeScope();
            
            // mediatr
            builder.RegisterMediatR(assembly);

            // handler
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequest<>));
            
            // rest client
            builder.RegisterType<ContactApiClient>()
                .As<IContactApiClient>().InstancePerLifetimeScope();
        }
    }
}