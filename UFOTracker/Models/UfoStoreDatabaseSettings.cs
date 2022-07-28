namespace UFOTracker.Models
{
    public class UfoStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UfosCollectionName { get; set; } = null!;
    }
}
