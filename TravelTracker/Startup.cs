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
    public class Startup
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
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserLogedIn",
                                  policy => policy.Requirements.Add(new UserIsLogedInRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, UserIsLogedInHandler>();

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlite("Data Source=users.sqlite",
                    optionsBuilder => optionsBuilder.MigrationsAssembly("TravelTracker")));

            var identityOptionsProvider = new IdentityOptionsProvider();
            services.AddIdentity<IdentityUser, IdentityRole>(identityOptionsProvider.SetOptions)
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IdentityDbContext dbContext)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                dbContext.Database.Migrate(); //this will generate the db if it does not exist
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
                    "landing",
                    "",
                    new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    "default",
                    "{controller}/{action}");

                routes.MapRoute(
                    "users",
                    "{*username}",
                    new { controller = "User", action = "Index" });
            });
        }
    }
}