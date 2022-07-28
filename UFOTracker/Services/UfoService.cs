using UFOTracker.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace UFOTracker.Services
{
    public class UfoService
    {
        private readonly IMongoCollection<Ufo> _ufosCollection;

        public UfoService(
        IOptions<UfoStoreDatabaseSettings> ufoStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                ufoStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                ufoStoreDatabaseSettings.Value.DatabaseName);

            _ufosCollection = mongoDatabase.GetCollection<Ufo>(
                ufoStoreDatabaseSettings.Value.UfosCollectionName);
        }

        public async Task<List<Ufo>> GetAsync() =>
            await _ufosCollection.Find(_ => true).ToListAsync();

        public async Task<Ufo?> GetAsync(string id) =>
            await _ufosCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Ufo newUfo) =>
            await _ufosCollection.InsertOneAsync(newUfo);

        public async Task UpdateAsync(string id, Ufo updatedUfo) =>
            await _ufosCollection.ReplaceOneAsync(x => x.Id == id, updatedUfo);

        public async Task RemoveAsync(string id) =>
            await _ufosCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<PageMongo> GetPageMongoResultAsync(int page, int pageSize)
        {
            var collection = _ufosCollection;
            // count facet, aggregation stage of count
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<Ufo, AggregateCountResult>.Create(new[]
                {
                PipelineStageDefinitionBuilder.Count<Ufo>()
                }));

            // data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<Ufo, Ufo>.Create(new[]
                {
                //PipelineStageDefinitionBuilder.Sort(Builders<Ufo>.Sort.Ascending(x => x.Surname)),
                PipelineStageDefinitionBuilder.Skip<Ufo>((page - 1) * pageSize),
                PipelineStageDefinitionBuilder.Limit<Ufo>(pageSize),
                }));

            var filter = Builders<Ufo>.Filter.Empty;
            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<Ufo>();

            return new PageMongo
            {
                Count = (int)count / pageSize,
                Size = pageSize,
                Page = page,
                Ufos = data
            };
        }
    }
}

