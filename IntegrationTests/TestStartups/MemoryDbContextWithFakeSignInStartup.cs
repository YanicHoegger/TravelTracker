﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationTests.TestStartups
{
    public class MemoryDbContextWithFakeSignInStartup : MemoryDbContextStartUp
    {
        public MemoryDbContextWithFakeSignInStartup(IHostingEnvironment env) : base(env)
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
