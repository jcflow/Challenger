using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Repository;
using Repository.EF;
using Schema;
using Schema.Types;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Net.WebSockets;
using System.Threading;
using System.Timers;
using System.Text;
using Hub;

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
            services.AddTransient<IUserRepository, EFUserRepository>();
            services.AddTransient<ITournamentCategoryRepository, EFTournamentCategoryRepository>();
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
            services.AddSingleton<UserType>();
            services.AddSingleton<TournamentCategoryType>();
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
            ConnectionHub.Init();
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
            app.UseDefaultFiles();
            app.UseFileServer(enableDirectoryBrowsing: true);
            app.UseWebSockets(); // Only for Kestrel

            app.Map("/ws", builder =>
            {
                builder.Use(async (context, next) =>
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        await ConnectionHub.Echo(webSocket);
                        return;
                    }
                    await next();
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
