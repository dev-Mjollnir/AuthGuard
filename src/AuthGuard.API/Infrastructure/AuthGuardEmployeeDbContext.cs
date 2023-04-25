using AuthGuard.API.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AuthGuard.API.Infrastructure
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        DbSet<Employee> Employees { get; set; }

    }

    public class EmployeContextDesignFactory : IDesignTimeDbContextFactory<EmployeeContext>
    {
        public EmployeContextDesignFactory()
        {
        }

        public EmployeeContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration["ConnectionStrings:SQLServer"];
            var optionsBuilder = new DbContextOptionsBuilder<EmployeeContext>()
                .UseSqlServer(connectionString);
            return new EmployeeContext(optionsBuilder.Options);
        }
    }
}
