namespace Map.Client.Interfaces
{
    public interface IRepository<in T>
    where T : IEntity
    {
        void SaveToDatabase(T entity);

    }
}
