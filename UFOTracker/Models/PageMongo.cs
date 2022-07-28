namespace UFOTracker.Models
{
    public class PageMongo
    {
        public int Count { get; set; }

        public int Page { get; set; }

        public int Size { get; set; }

        public IEnumerable<Ufo> Ufos { get; set; }
    }
}
