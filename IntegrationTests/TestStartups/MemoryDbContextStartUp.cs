﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelTracker;

namespace IntegrationTests.TestStartups
{
    public class MemoryDbContextStartUp : Startup, IDisposable
    {
        public MemoryDbContextStartUp(IHostingEnvironment env) : base(env)
        {
        }

        protected override void SetUpDataBase(IServiceCollection services)
		{
            services.AddDbContext<IdentityDbContext>(options => options.UseInMemoryDatabase("Test"));
		}

        protected override void EnsureDatabaseCreated(IdentityDbContext dbContext)
        {
            //Nothing to do here, because of memory db
        }

        public void Dispose()
        {
            //Nothing to do here, because of memory db
        }
    }
}