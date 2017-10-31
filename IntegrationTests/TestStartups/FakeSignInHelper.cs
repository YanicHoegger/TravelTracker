using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestStartups
{
    public static class FakeSignInHelper
    {
        public static async Task SignInIntegrationTestUser(HttpContext context)
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

                if(integrationTestUserHeader.Count > 1)
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
