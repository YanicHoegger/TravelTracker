using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationTests.TestStartups
{
    public class SqlliteDbContextWithFakeSignInStartup : SqlliteDbContextStartup
    {
        public SqlliteDbContextWithFakeSignInStartup(IHostingEnvironment env) : base(env)
        {
        }

        protected override void ConfigureMiddleware(IApplicationBuilder app)
        {
            app.Use(next => async context =>
            {
                await FakeSignInHelper.SignInIntegrationTestUser(context);
                await next.Invoke(context);
            });
        }
    }
}
