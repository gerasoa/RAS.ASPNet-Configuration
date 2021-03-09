using AspNetCoreConfiguration.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using System.Text;
using System;
using System.Diagnostics;

namespace AspNetCoreIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //To create Enviroment of execution
        public Startup(IHostingEnvironment hostEnviroment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnviroment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnviroment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();


            //Create in theproject the file: secrets.json 
            if (hostEnviroment.IsProduction())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfig(Configuration);
            services.AddAuthorizationConfig();
            services.ResolveDependencies();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // app.UseKissLogMiddleware() must to be referenced after app.UseAuthentication(), app.UseSession()
            app.UseKissLogMiddleware(options => {
                ConfigureKissLog(options);
            });

            //Identity configured in the application
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void ConfigureKissLog(IOptionsBuilder options)
        {
            // optional KissLog configuration
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register logs output
            RegisterKissLogListeners(options);
        }

        private void RegisterKissLogListeners(IOptionsBuilder options)
        {
            // multiple listeners can be registered using options.Listeners.Add() method

            // register KissLog.net cloud listener
            options.Listeners.Add(new RequestLogsApiListener(new Application(
                Configuration["KissLog.OrganizationId"],    //  "eb383d1f-4aa5-4bb3-84bc-bf176aaf6d74"
                Configuration["KissLog.ApplicationId"])     //  "9d6914eb-9206-4f50-a91e-7b4b164b8cab"
            )
            {
                ApiUrl = Configuration["KissLog.ApiUrl"]    //  "https://api.kisslog.net"
            });
        }

    }
}
