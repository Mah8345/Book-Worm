namespace BookWorm.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {

        }

        public EntityNotFoundException(string entityName, object key)
            : base($"{entityName} with key:{key} was not found")
        {

        }
    }
}
