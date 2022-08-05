using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace AppFramework.EntityFrameworkCore
{
    public static class AppFrameworkDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AppFrameworkDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AppFrameworkDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}