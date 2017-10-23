using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelTracker.Authorization;
using TravelTracker.User;

namespace TravelTracker
{
    public class Startup : IStartup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserLogedIn",
                                  policy => policy.Requirements.Add(new UserIsLogedInRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, UserIsLogedInHandler>();

            SetUpDataBase(services);

            var identityOptionsProvider = new IdentityOptionsProvider(Configuration);             services.AddSingleton<IIdentityOptionsProvider>(identityOptionsProvider);

            services.AddIdentity<IdentityUser, IdentityRole>(identityOptionsProvider.SetOptions)
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            return services.BuildServiceProvider();
        }

        protected virtual void SetUpDataBase(IServiceCollection services)
        {
			services.AddDbContext<IdentityDbContext>(options => options.UseSqlite("Data Source=users.sqlite",
					optionsBuilder => optionsBuilder.MigrationsAssembly("TravelTracker")));
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            var env = app.ApplicationServices.GetService<IHostingEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

                var dbContext = app.ApplicationServices.GetService<IdentityDbContext>();
                EnsureDatabaseCreated(dbContext);
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentity();
            app.UseStaticFiles();


            //TODO: Unit Test for routing
            app.UseMvc(routes =>
            {
				routes.MapRoute(
					"users",
                    "traveller/{username}/{action?}",
					new { controller = "User", action = "Index" });

                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}");
            });
        }

        protected virtual void EnsureDatabaseCreated(IdentityDbContext dbContext)
        {
            dbContext.Database.Migrate(); //this will generate the db if it does not exist
		}
    }
}