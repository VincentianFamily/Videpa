using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Videpa.Identity.Jwt;
using Videpa.Identity.Logic.Interfaces;
using Videpa.Identity.Logic.Ports;
using Videpa.Identity.Logic.Services;
using Videpa.Identity.Persistence;

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

            container.Register<IPasswordService, PasswordService>(Lifestyle.Scoped);

            container.Register<IJwtIssuer, JwtService>(Lifestyle.Scoped);
            container.Register<IJwtAudience, JwtService>(Lifestyle.Scoped);

            container.Register<IUserProfileRepository, UserProfileInMemoryRepository>(Lifestyle.Scoped);
            // container.Register<IUserProfileRepository, UserProfileAzureRepository>(Lifestyle.Scoped);

            container.Register<IUserProfileService, UserProfileService>(Lifestyle.Scoped);

            container.Verify();

            return container;
        }
    }
}