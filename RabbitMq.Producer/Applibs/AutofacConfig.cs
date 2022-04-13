using System.Reflection;
using Autofac;
using RabbitMq.Domain.Model;

namespace RabbitMq.PubSubData.Applibs
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

            builder.RegisterType<CreateWallets>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            container = builder.Build();
        }
    }
}