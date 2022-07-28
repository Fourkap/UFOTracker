using UFOTracker.Models;
using UFOTracker.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace UFOTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UfoController
    {
        private readonly UfoService _ufoService;

        public UfoController(UfoService ufoService) =>
            _ufoService = ufoService;

        [HttpGet]
        public async Task<ActionResult<PageMongo>> GetUfos(int page, int pageSize)
        {
            var ufo = await _ufoService.GetPageMongoResultAsync(page, pageSize);

            if (ufo is null)
            {
                throw new Exception("not found");
            }

            return ufo;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Ufo>> Get(string id)
        {
            var ufo = await _ufoService.GetAsync(id);

            if (ufo is null)
            {
                throw new Exception("not found");
            }

            return ufo;
        }

        [HttpPost]
        public async Task<ActionResult<Ufo>> Post(Ufo ufo)
        {
            await _ufoService.CreateAsync(ufo);

            return ufo;
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<Ufo>> Update(string id, Ufo ufo)
        {
            var ufoFromDb = await _ufoService.GetAsync(id);

            if (ufoFromDb is null)
            {
                throw new Exception("not found");
            }

            ufo.Id = ufoFromDb.Id;

            await _ufoService.UpdateAsync(id, ufo);

            return ufo;
        }

        [HttpDelete("{id:length(24)}")]
        public async void Delete(string id)
        {
            var ufo = await _ufoService.GetAsync(id);

            if (ufo is null)
            {
                throw new Exception("not found");
            }

            await _ufoService.RemoveAsync(id);
        }
    }
}
