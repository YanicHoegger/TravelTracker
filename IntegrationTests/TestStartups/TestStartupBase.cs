using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelTracker;

namespace IntegrationTests.TestStartups
{
    public abstract class TestStartupBase : Startup, IDisposable
    {
        protected TestStartupBase(IHostingEnvironment env) : base(env)
        {
        }

        public abstract void Dispose();

        protected override void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.Use(next => async context =>
            {
                await SignInIntegrationTestUser(context);
                await next.Invoke(context);
            });
        }

        async Task SignInIntegrationTestUser(HttpContext context)
        {
            var integrationTestUserHeader = context.Request.Headers["IntegrationTestLogin"];
            if (integrationTestUserHeader.Count > 0)
            {
                var userName = integrationTestUserHeader[0];
                var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return;
                }
                var signInManger = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
                var userIdentity = await signInManger.CreateUserPrincipalAsync(user);
                context.User = userIdentity;
            }
        }
    }
}
