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


    }
}
