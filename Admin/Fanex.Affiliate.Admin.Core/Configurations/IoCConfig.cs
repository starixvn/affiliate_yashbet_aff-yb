using Autofac;
using Autofac.Integration.Mvc;
using Fanex.Affiliate.Admin.Core.Apis;
using Fanex.Affiliate.User.Core.Infrastructure;
using Fanex.Caching;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.NETFramework;
using ONEbook.UM.Transport;
using Refit;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;

namespace Fanex.Affiliate.Admin.Core.Configurations
{
    public static class IoCConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            //Register repository
            var asselmblyList = GetExecutingAssembly();

            builder.RegisterAssemblyTypes(asselmblyList).AsImplementedInterfaces();

            builder.RegisterDependencies();
            builder.RegisterMemberSiteApi();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(asselmblyList);


            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // Set the dependency resolver to be Autofac.

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static Assembly[] GetExecutingAssembly()
        {
            Assembly[] assemblies = null;
            try
            {
                assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToArray();
            }
            catch
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic).ToArray();
            }

            return assemblies.Where(x => x.FullName.StartsWith("Fanex.Affiliate", StringComparison.InvariantCulture)).ToArray();
        }

        private static void RegisterMemberSiteApi(this ContainerBuilder builder)
        {
            var appSetting = AppSetting.Instance;
            var handler = new AuthenticatedHttpClientHandler(appSetting.MemberSiteApiAuthenticationKey)
            {
                Proxy = appSetting.IsUseMemberSiteProxy
                            ? new WebProxy(new Uri(appSetting.MemberSiteProxyUrl))
                            : null

            };
            builder.RegisterInstance(RestService.For<ILicenseeApi>(
                new HttpClient(handler)
                {
                    BaseAddress = new Uri(appSetting.MemberSiteApiUrl)
                }));
        }

        private static void RegisterDependencies(this ContainerBuilder builder)
        {
            builder.RegisterInstance(new UMClientConfiguration().Initialize());
            builder.RegisterInstance<ICacheService>(new CacheService());

            builder.RegisterType<UMCacheService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<IPHelperImpl>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<UMRestClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HttpContextHelper>().AsImplementedInterfaces().SingleInstance();
        }
    }
}