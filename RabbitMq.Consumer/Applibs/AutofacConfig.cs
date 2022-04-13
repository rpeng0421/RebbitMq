using System.Reflection;
using Autofac;
using RabbitMq.Domain.Model;

namespace RabbitMq.Consumer.Applibs
{
    public class AutofacConfig
    {
        private static IContainer container;

        public static IContainer Container
        {
            get
            {
                if (container == null)
                {
                    Register();
                }

                return container;
            }
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();

            builder.RegisterType<RabbitMqFactory>()
                .WithParameter("rabbitUrl", ConfigHelper.RabbitMqUri)
                .WithParameter("rmqExpiration", ConfigHelper.RmqExpiration)
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterAssemblyTypes(asm)
                .AssignableTo<EventDispatcher>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            builder.RegisterAssemblyTypes(asm)
                .AssignableTo<IConsumerHandler<EventData>>()
                .Keyed<IConsumerHandler<EventData>>(x => x.Name.Replace("Consumer", ""))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            container = builder.Build();
        }
    }
}