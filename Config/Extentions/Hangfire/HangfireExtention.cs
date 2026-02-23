using Hangfire;
using Hangfire.SqlServer;

namespace salian_api.Config.Extentions.Hangfire
{
    public static class HangfireExtention
    {
        public static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection services,
            string databaseConnection
        )
        {
            // Add Hangfire services.
            services.AddHangfire(configuration =>
                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(
                        databaseConnection,
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true,
                        }
                    )
            );

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            return services;
        }
    }
}
