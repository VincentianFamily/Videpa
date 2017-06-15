using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Videpa.Identity.Configuration;
using Videpa.Identity.Jwt;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Ports;
using Videpa.Identity.Logic.Services;
using Videpa.Identity.Persistence.InMemory;

namespace Videpa.Identity.Api.DependencyContainer
{
    public class DependencyResolver 
    {
        private static readonly Lazy<Container> Container = new Lazy<Container>(InitializeContainer);

        public Container GetContainer()
        {
            return Container.Value;
        }

        internal static Container InitializeContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Singletons
            container.Register<IPasswordService, PasswordService>(Lifestyle.Singleton);

            // Scoped
            container.Register<IJwtIssuer, JwtService>(Lifestyle.Scoped);
            container.Register<IJwtAudience, JwtService>(Lifestyle.Scoped);

            container.Register<IUserProfileRepository, UserProfileInMemoryRepository>(Lifestyle.Scoped);
            container.Register<IUserProfileQueries, UserProfileInMemoryRepository>(Lifestyle.Scoped);
            // container.Register<IUserProfileRepository, UserProfileAzureRepository>(Lifestyle.Scoped);

            container.Register<IUserProfileCommandHandler, UserProfileCommandHandler>(Lifestyle.Scoped);
            container.Register<IConfigurationManager, AppSettingsConfigurationManager>(Lifestyle.Scoped);

            container.Verify();

            return container;
        }
    }
}