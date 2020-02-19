using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BookCluster.Domain.Interfaces;
using BookCluster.IdentityServer.Configuration;
using BookCluster.IdentityServer.Models;
using BookCluster.Repository;
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

        public void ConfigureServices(IServiceCollection services)
        {
            // Get certificate password (temporary)
            var config = configuration.GetSection("Data").Get<Option>();
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Configure Identity Server
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddTransient<IUserRepository>(isp => 
            new UserRepository(configuration.GetConnectionString("BookCluster"))); // First type should be an interface

            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(@"..\bookcluster.pfx", config.CertificatePass))
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("BookClusterIdentity"),
                    sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("BookClusterIdentity"),
                    sql => sql.MigrationsAssembly(assembly));
                });

            services.AddMvc(option => option.EnableEndpointRouting = false); // temporary
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add Identity Server middleware to pipeline
            app.UseIdentityServer();
                
            app.UseRouting();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
