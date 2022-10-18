using Microsoft.Extensions.DependencyInjection;
using ThriftBox.Infrastructure.Courses.Abstractions;
using ThriftBox.Infrastructure.Courses.Handler;

namespace ThriftBox.Infrastructure.Courses
{
    public static class ServiceCollection
    {
        public static void AddInfrastructureSymbolsQuery(this IServiceCollection services)
        {
            services.AddSingleton<IParseSymbolsHandler, ParseSymbolsHandler>();
        }
    }
}
