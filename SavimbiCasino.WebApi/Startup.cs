using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SavimbiCasino.WebApi.Consts;
using SavimbiCasino.WebApi.Dtos;
using SavimbiCasino.WebApi.FluentValidation;
using SavimbiCasino.WebApi.Services;

namespace SavimbiCasino.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(hostEnvironment.ContentRootPath)
                .AddConfiguration(configuration)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();

            _configuration = configurationBuilder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SavimbiCasinoDbContext>(builder =>
                builder.UseNpgsql(_configuration.GetConnectionString(DbContextConsts.ConnectionString)));

            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IValidator<CredentialsDto>, CredentialsDtoValidator>();

            services.AddControllersWithViews()
                .AddFluentValidation();

            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}