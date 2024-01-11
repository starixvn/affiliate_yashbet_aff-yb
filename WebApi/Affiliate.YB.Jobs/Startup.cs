using Affiliate.YB.Jobs.Configurations;
using Affiliate.YB.Jobs.Middlewares;
using Fanex.Logging;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Affiliate.YB.Jobs
{
    public class Startup
    {
        private AppSettings appSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            appSettings = new AppSettings();
            Configuration.GetSection("AppSettings").Bind(appSettings);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.RegisterHangfire(Configuration);
            services.RegisterServices(Configuration, appSettings);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Add This
            app.UseHangfire(appSettings);
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Site is running");
            });

            Logger.Log.Info("Affiliate YB Job started!");
        }
    }
}