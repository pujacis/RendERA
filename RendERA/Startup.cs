using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http.Features;

namespace RendERA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            //SESSION
            services.AddDistributedMemoryCache();
            services.AddSession(options => {  options.IdleTimeout = TimeSpan.FromMinutes(10);   });
            services.AddHttpContextAccessor();
            // if < .NET Core 2.2 use this
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            /////////////////////////////////////////


            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = 20009715200;
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                       
            //DB
            services.AddDbContext<DB.Models.RendERAContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("RendERADBContext")));
            //SERVICES
            services.AddTransient<Infrastructure.IRepositories.IUnitOfWork, Infrastructure.Repositories.UnitOfWork>();
            services.AddTransient<ServiceManager.IServices.IAccountSrv, ServiceManager.Services.AccountSrv>();
            services.AddTransient<ServiceManager.IServices.IMessageReplySrv, ServiceManager.Services.MessageReplySrv>();
            services.AddTransient<ServiceManager.IServices.IMessagesSrv, ServiceManager.Services.MessagesSrv>();
            services.AddTransient<ServiceManager.IServices.IDocumentSrv, ServiceManager.Services.DocumentSrv>();
            services.AddTransient<ServiceManager.IServices.IParameterSrv, ServiceManager.Services.ParameterSrv>();
            services.AddTransient<ServiceManager.IServices.ITeamSrv, ServiceManager.Services.TeamSrv>();
            services.AddTransient<ServiceManager.IServices.IProjectSrv, ServiceManager.Services.ProjectSrv>();
            services.AddTransient<ServiceManager.IServices.IServerListSrv, ServiceManager.Services.ServerListSrv>();
            services.AddTransient<ServiceManager.IServices.IPaymentTransactionSrv, ServiceManager.Services.PaymentTransactionSrv>();
            services.AddTransient<ServiceManager.IServices.IConfigurationSettingsSrv, ServiceManager.Services.ConfigurationSettingsSrv>();
            services.AddTransient<ServiceManager.IServices.IJobLogSrv, ServiceManager.Services.JobLogSrv>();
            services.AddTransient<ServiceManager.IServices.IMasterCategorySrv, ServiceManager.Services.MasterCategorySrv>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
