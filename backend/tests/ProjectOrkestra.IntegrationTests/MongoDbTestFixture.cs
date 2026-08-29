using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using ProjectOrkestra.Infrastructure.Data;
using ProjectOrkestra.Infrastructure.Mappings;

namespace ProjectOrkestra.IntegrationTests;

public class MongoDbTestFixture {
    public IMongoDbContext Context { get; }
    private static MongoDbSettings? _sharedSettings;
    private static readonly object _configLock = new object();

    static MongoDbTestFixture()
    {
        // Static constructor ensures this runs only once per test run
        lock (_configLock)
        {
            // Check if any classmap is already registered
            if (!BsonClassMap.IsClassMapRegistered(typeof(Domain.Entities.Tenant)))
            {
                MongoSerializationConfig.Configure();
                TenantMap.Configure();
                OrganizationMap.Configure();
                BusinessUnitMap.Configure();
                EmployeeMap.Configure();
            }
        }
    }

    public MongoDbTestFixture()
    {
        lock (_configLock)
        {
            if (_sharedSettings == null)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.test.json")
                    .Build();

                _sharedSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()!;
            }
        }

        Context = new MongoDbContext(Options.Create(_sharedSettings));
    }
}