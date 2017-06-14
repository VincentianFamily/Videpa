using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using SimpleInjector.Integration.WebApi;
using Videpa.Identity.Api.DependencyContainer;
using Videpa.Identity.Api.ExceptionHandling;

namespace Videpa.Identity.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            #region - Dependency Resolver -

            var dependencyContainer = new DependencyResolver().GetContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(dependencyContainer);

            #endregion


            #region - Message Handlers -

            // GlobalConfiguration.Configuration.MessageHandlers.Add(new XmlHeaderHandler());

            //GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new AcceptHeaderHandler());
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new LanguageMessageHandler());
            //GlobalConfiguration.Configuration.MessageHandlers.Add(new DelegatingHandlerProxy<LogHandler>(dependencyContainer));

            GlobalConfiguration.Configuration.Services.Replace(typeof(IExceptionHandler), new DelegatingExceptionHandlerProxy<RequestExceptionHandler>(dependencyContainer));
            GlobalConfiguration.Configuration.Services.Add(typeof(IExceptionLogger), new DelegatingExceptionLoggerProxy<RequestExceptionLogger>(dependencyContainer));

            #endregion

            #region - Web api routing -

            GlobalConfiguration.Configure(WebApiConfig.Register);

            #endregion
        }
    }
}
