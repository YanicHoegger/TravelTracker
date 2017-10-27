using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestStartups
{
    public class MemoryDbContextStartUp : TestStartupBase
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

        public override void Dispose()
        {
            //Nothing to do here, because of memory db
        }
    }
}
