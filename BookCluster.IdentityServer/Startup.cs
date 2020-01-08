using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BookCluster.IdentityServer.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookCluster.IdentityServer
{
    public class Startup
    {
        //private readonly IConfiguration configuration;
        //public Startup(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}

        private IConfiguration configuration;
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Get certificate password (temporary)
            var config = configuration.GetSection("Data").Get<Option>();
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Configure Identity Server
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(@"..\bookcluster.pfx", config.CertificatePass))
                .AddTestUsers(Config.Users) // temporary
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("BookCluster"),
                    sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("BookCluster"),
                    sql => sql.MigrationsAssembly(assembly));
                });

            services.AddMvc(option => option.EnableEndpointRouting = false); // temporary
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // TRemprorary for seeding
            MigrateInMemoryDataToSqlServer(app);

            // Add Identity Server middleware to pipeline
            app.UseIdentityServer();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }

        public void MigrateInMemoryDataToSqlServer(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                context.Database.Migrate();

                if(!context.Clients.Any())
                {
                    foreach(var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }

                    context.SaveChanges();
                }

                if(!context.IdentityResources.Any())
                {
                    foreach(var id in Config.Identity)
                    {
                        context.IdentityResources.Add(id.ToEntity());
                    }

                    context.SaveChanges();
                }

                if(!context.ApiResources.Any())
                {
                    foreach(var api in Config.Apis)
                    {
                        context.ApiResources.Add(api.ToEntity());
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
