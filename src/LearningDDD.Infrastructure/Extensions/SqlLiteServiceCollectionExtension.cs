using LearningDDD.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace LearningDDD.Infrastructure.Extensions
{
    public static class SqlLiteServiceCollectionExtension
    {
        public static IServiceCollection AddSqlLite(this IServiceCollection services)
        {
            // Register the DbContext with SQLite provider
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=../LearningDDD.Infrastructure/app.db"));
            return services;
        }
    }
}