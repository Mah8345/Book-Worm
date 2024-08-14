namespace BookWorm.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public Type EntityType { get;}

        public EntityNotFoundException(Type entityType, string entityName, object key)
            : base($"entity with type: {entityType.Name}, name: {entityName}, key: {key} was not found")
        {
            EntityType = entityType;
        }
    }
}
