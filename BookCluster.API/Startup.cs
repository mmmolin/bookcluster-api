using AutoMapper;
using BookCluster.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookCluster.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method configure services that can then be used in the project using dependency injection.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Binds Option model to data section in appsettings.json.
            services.Configure<Option>(Configuration.GetSection("Data"));
            
            // Setup for AutoMapper
            services.AddAutoMapper(typeof(Profiles.AuthorProfile), typeof(Profiles.BookProfile), typeof(Profiles.AuthorProfile));

            // Configure JWT bearer authentication handler (Identity Server 4).
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:59418";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "bookclusterapi";
                });
        }

        // This method configures the Request Processing Pipline by adding middleware. 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // Adds authentication and Authorization middleware to pipeline (identity Server 4)
            app.UseAuthentication();            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
