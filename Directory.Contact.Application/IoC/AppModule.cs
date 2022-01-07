using System.Reflection;
using Autofac;
using Directory.Contact.Application.DataAccess;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MicroServices.Infrastructure.Repository;
using Module = Autofac.Module;

namespace Directory.Contact.Application.IoC
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterMediatR(assembly);

            // handler
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .AsClosedTypesOf(typeof(IRequest<>));
            
            // repository
            // builder.RegisterType<ContactRepository>().SingleInstance();
            builder.RegisterType<GenericRepository<Domain.Entities.Contact>>()
                .As<IRepository<Domain.Entities.Contact>>().SingleInstance();
        }
    }
}