using System;
using IntegrationTests.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelTracker;

namespace IntegrationTests.TestStartups
{
    public class SqlliteDbContextStartup : StartupBase
    {
        readonly TestFolderCreator _testfolderCreator;

        public SqlliteDbContextStartup(IHostingEnvironment env) : base(env)
        {
            _testfolderCreator = new TestFolderCreator();
        }

        protected override void SetUpDataBase(IServiceCollection services)
        {
            _testfolderCreator.CreateFolder();

            services.AddDbContext<IdentityDbContext>(options => options.UseSqlite(DbConnectionString,
                    optionsBuilder => optionsBuilder.MigrationsAssembly("TravelTracker")));
        }

        public SqliteConnection GetDbConnection()
        {
            return new SqliteConnection(DbConnectionString);
        }

        public override void Dispose()
        {
            _testfolderCreator.Dispose();
        }

        string DbConnectionString 
        {
            get
            {
                return $"Data Source={_testfolderCreator.FolderName}/users.sqlite";
            }
        }
    }
}
