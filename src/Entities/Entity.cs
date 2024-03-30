using System;

namespace AzureDevopsTracker.Entities
{
    public abstract class Entity
    {
        public string Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }

        protected Entity(string id)
        {
            Id = id ?? Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

        protected Entity()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }
    }
}