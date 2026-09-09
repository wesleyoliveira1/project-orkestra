using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using ProjectOrkestra.Domain.Entities;

namespace ProjectOrkestra.Infrastructure.Mappings;

public static class EmployeeMap
{
    public static void Configure()
    {
        if (BsonClassMap.IsClassMapRegistered(typeof(Employee)))
            return;

        BsonClassMap.RegisterClassMap<Employee>(map =>
        {
            map.AutoMap();

            map.MapIdMember(x => x.Id).SetIdGenerator(CombGuidGenerator.Instance);
        });
    }
}
