using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts
{
    /// <summary>
    /// Used by design-time-tools to instance and use DataContext
    /// </summary>
    public class DataContextDesignFactory : IDesignTimeDbContextFactory<DataContext>
    {
        private const string EnvCnxVar = "Viv2DesignTimeConnectionString";
        
        public DataContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable(EnvCnxVar);
            
            if (args.Length > 0)
                connectionString = args[0];
            
            if (connectionString == null)
            {
                throw new Exception(
                    $"Please specify a suitable connection string, either as the only argument after --, or by setting the environment variable: '{EnvCnxVar}'");
            }
            
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}