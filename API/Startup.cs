using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Queries;
using GraphiQl;
using GraphQl.AspNetCore;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Repository;
using Repository.EF;
using Schema;
using Schema.Types;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;


namespace API
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
            //Context
            services.AddSingleton<ChallengerContext>();

            //Data Repository
            services.AddTransient<ITournamentRepository, EFTournamentRepository>();
            services.AddTransient<IBracketRepository, EFBracketRepository>();
            services.AddTransient<IScoreRepository, EFScoreRepository>();
            services.AddTransient<ITeamRepository, EFTeamRepository>();

            //GraphQL Document
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<ISchema, ChallengerSchema>();

            //GraphQL Query
            services.AddScoped<ChallengerQuery>();
            services.AddScoped<ChallengerMutation>();

            //GraphQL Types
            services.AddSingleton<TournamentType>();
            services.AddSingleton<BracketType>();
            services.AddSingleton<ScoreType>();
            services.AddSingleton<TeamType>();
            services.AddSingleton<TournamentInputType>();
            services.AddSingleton<TeamInputType>();

            services.AddScoped<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});

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
