using System;
using System.Security.Claims;
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
    public abstract class StartupBase : Startup, IDisposable
    {
        protected StartupBase(IHostingEnvironment env) : base(env)
        {
        }

        protected override void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.Use(next => async context =>
            {
                await SignInIntegrationTestUser(context);
                await next.Invoke(context);
            });
        }

        public abstract void Dispose();

        static async Task SignInIntegrationTestUser(HttpContext context)
        {
            var integrationTestUserHeader = context.Request.Headers["IntegrationTestLogin"];
            if (integrationTestUserHeader.Count > 0)
            {
                var userName = integrationTestUserHeader[0];

                var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
                var user = await userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return; //TODO: Throw an exception here?
                }

                if (integrationTestUserHeader.Count > 1)
                {
                    var role = integrationTestUserHeader[1];
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
                }

                var signInManger = context.RequestServices.GetRequiredService<SignInManager<IdentityUser>>();
                var userIdentity = await signInManger.CreateUserPrincipalAsync(user);

                context.User = userIdentity;
            }
        }
    }
}
