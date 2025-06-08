namespace LearningDDD.Domain.SeedWork
{
    public abstract class Entity<T> where T : IComparable<T>
    {
        public T Id { get; protected set; }

        protected Entity(T id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id), "Entity ID cannot be null."); ;
        }

        protected Entity() { }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            Entity<T> other = (Entity<T>)obj;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // Optional: Overload operators for convenience
        public static bool operator ==(Entity<T> left, Entity<T> right)
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }
            return left.Equals(right);
        }

        public static bool operator !=(Entity<T> left, Entity<T> right)
        {
            return !(left == right);
        }
    }
}
