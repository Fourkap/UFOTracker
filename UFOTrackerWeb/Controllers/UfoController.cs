using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UFOTrackerWeb.Models;
using UFOTracker.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace UFOTrackerWeb.Controllers
{
    public class UfoController : Controller

    {
        static HttpClient client = new HttpClient();
        // GET: UfoController
        public async Task<ActionResult> IndexAsync(int id=1)
        {  
            HttpResponseMessage response = await client.GetAsync("https://localhost:7053/Ufo?page="+id+"&pageSize=10");
            response.EnsureSuccessStatusCode();
            var EmpResponse = response.Content.ReadAsStringAsync().Result;
            Newtonsoft.Json.Linq.JObject json = JObject.Parse(EmpResponse);
            var EmpInfo = JsonConvert.DeserializeObject<PageMongo>(json.ToString());
            return View(EmpInfo);
        }

        // GET: UfoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UfoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UfoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Ufo ufo = new Ufo();
                ufo.City = collection["City"];
                ufo.Country = collection["Country"];
                ufo.State = collection["State"];
                ufo.Shape = collection["Shape"];
                ufo.Comments = collection["Comments"];
                ufo.DateAndTime = Convert.ToDateTime(collection["DateandTime"]);
                ufo.Latitude = collection["Latitude"];
                ufo.Longitude = collection["Longitude"];
                ufo.DatePosted = DateTime.Now;

                var myContent = JsonConvert.SerializeObject(ufo);

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                client.PostAsync("https://localhost:7053/Ufo", byteContent);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UfoController/Edit/5
        public async Task<ActionResult> EditAsync(string id)
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7053/Ufo/" + id);
            response.EnsureSuccessStatusCode();
            var EmpResponse = response.Content.ReadAsStringAsync().Result;
            Newtonsoft.Json.Linq.JObject json = JObject.Parse(EmpResponse);
            var EmpInfo = JsonConvert.DeserializeObject<Ufo>(json.ToString());
            ViewBag.id = id;
            return View(EmpInfo);
        }

        // POST: UfoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                Ufo ufo = new Ufo();
                ufo.Id = collection["Id"];
                ufo.City = collection["City"];
                ufo.Country = collection["Country"];
                ufo.State = collection["State"];
                ufo.Shape = collection["Shape"];
                ufo.Comments = collection["Comments"];
                ufo.DateAndTime = Convert.ToDateTime(collection["DateandTime"]);
                ufo.Latitude = collection["Latitude"];
                ufo.Longitude = collection["Longitude"];
                ufo.DatePosted = Convert.ToDateTime(collection["Dateposted"]);

                var myContent = JsonConvert.SerializeObject(ufo);

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var test = client.PutAsync("https://localhost:7053/Ufo/" + ufo.Id, byteContent);
                var result = test.Result;

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        // GET: UfoController/Delete/5
        public ActionResult Delete(string id)
        {
            client.DeleteAsync("https://localhost:7053/Ufo/" + id);
            return RedirectToAction("Index");
        }

    }
}
