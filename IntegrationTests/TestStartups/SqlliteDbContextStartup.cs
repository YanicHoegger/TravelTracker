using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.TestStartups
{
    public class SqlliteDbContextStartup :  TestStartupBase
    {
        readonly TestFolderCreator _testfolderCreator;

        public SqlliteDbContextStartup(IHostingEnvironment env) : base(env)
        {
            _testfolderCreator = new TestFolderCreator();
        }

        protected override void SetUpDataBase(IServiceCollection services)
        {
            _testfolderCreator.CreateFolder();

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlite($"Data Source={_testfolderCreator.FolderName}/users.sqlite",
                    optionsBuilder => optionsBuilder.MigrationsAssembly("TravelTracker")));
        }

        public override void Dispose()
        {
            _testfolderCreator.Dispose();
        }
    }
}
