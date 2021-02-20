﻿using AspNetCoreIdentity.Areas.Identity.Data;
using AspNetCoreIdentity.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreConfiguration.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies (this IServiceCollection services)
        {
            //Register the class by Interface
            services.AddSingleton<IAuthorizationHandler, NecessessaryPermissionHandler>(); //Claims

            return services;
        }

        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Add EF to Asp.Net Context  
            services.AddDbContext<AspNetCoreIdentityContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("AspNetCoreIdentityContextConnection")));

            //Configuring the Identity on the application            
            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>() //This key suport is apply when we implement Authorization
                .AddDefaultUI(Microsoft.AspNetCore.Identity.UI.UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<AspNetCoreIdentityContext>();

            return services;
        }
    }
}
