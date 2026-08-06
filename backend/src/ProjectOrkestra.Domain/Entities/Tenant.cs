using System.Dynamic;

namespace ProjectOrkestra.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private Tenant()
    {
    }

    public Tenant(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }
}