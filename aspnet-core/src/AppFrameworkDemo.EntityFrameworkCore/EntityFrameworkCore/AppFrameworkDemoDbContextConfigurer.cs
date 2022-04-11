using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AppFrameworkDemo.EntityFrameworkCore
{
    public static class AppFrameworkDemoDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AppFrameworkDemoDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AppFrameworkDemoDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}