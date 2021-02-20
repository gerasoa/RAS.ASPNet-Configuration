using AspNetCoreIdentity.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreConfiguration.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddAuthorizationConfig (this IServiceCollection services)
        {
            //Configuring the Claims
            services.AddAuthorization(options =>
            {
                //Handler by ASP.Net
                options.AddPolicy("AuthorizedDelete", policy => policy.RequireClaim("AuthorizedDelete"));

                //When I create a Requiriment (NecessessaryPermission) this is my responsability create a Handle
                options.AddPolicy("Write", policy => policy.Requirements.Add(new NecessessaryPermission("Write"))); //Claims 
                options.AddPolicy("Read", policy => policy.Requirements.Add(new NecessessaryPermission("Read"))); //Claims
            });

            return services;
        }
    }
}
