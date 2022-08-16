using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UFOTracker.Models;
using Xamarin.Essentials;

namespace UfoTrackerAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class EditUfo : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        HttpClient client;
        Ufo ufo;
        private Button update;
        String uri = "https://10.0.2.2:7053/Ufo/";
        int totalitems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.edit_ufo);

            //Add = FindViewById<Button>(Resource.Id.Add);

            string id = Intent.GetStringExtra("id");
            uri += id;

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);



            update = FindViewById<Button>(Resource.Id.Update);


            update.Click += (object sender, EventArgs e) =>
            {
                UpdateUfoAsync(id);
            };

            recupAPI(uri);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

        }

        public async Task UpdateUfoAsync(string id)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var finaltest = json.ToString();

                    ufo = JsonConvert.DeserializeObject<Ufo>(finaltest);

                    EditText city = FindViewById<EditText>(Resource.Id.City);
                    EditText state = FindViewById<EditText>(Resource.Id.State);
                    EditText country = FindViewById<EditText>(Resource.Id.Country);
                    EditText shape = FindViewById<EditText>(Resource.Id.Shape);
                    EditText comments = FindViewById<EditText>(Resource.Id.Comments);
                    EditText latitude = FindViewById<EditText>(Resource.Id.Latitude);
                    EditText longitude = FindViewById<EditText>(Resource.Id.Longitude);


                    ufo.City = (string)city.Text;
                    ufo.State = (string)state.Text;
                    ufo.Country = (string)country.Text;
                    ufo.Shape = (string)shape.Text;
                    ufo.Comments = (string)comments.Text;
                    ufo.Latitude = (string)latitude.Text;
                    ufo.Longitude = (string)longitude.Text;
                   

                    var json1 = JsonConvert.SerializeObject(ufo);
                    HttpContent httpContent = new StringContent(json1);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var resp = await client.PutAsync(new Uri(uri), httpContent);
                    if (resp.IsSuccessStatusCode)
                    {
                        Intent nextActivity = new Intent(this, typeof(MainActivity));
                        StartActivity(nextActivity);
                    }

                }
            }


            catch (Exception e)
            {
                Console.WriteLine("l'api ne fonctionne pas", e);
            }
        }


        public async void recupAPI(string uri)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var finaltest = json.ToString();

                    ufo = JsonConvert.DeserializeObject<Ufo>(finaltest);
                    //set totalpages;

                    EditText city = FindViewById<EditText>(Resource.Id.City);
                    city.Text = ufo.City;

                    EditText state = FindViewById<EditText>(Resource.Id.State);
                    state.Text = ufo.State;

                    EditText country = FindViewById<EditText>(Resource.Id.Country);
                    country.Text = ufo.Country;

                    EditText shape = FindViewById<EditText>(Resource.Id.Shape);
                    shape.Text = ufo.Shape;

                    EditText comments = FindViewById<EditText>(Resource.Id.Comments);
                    comments.Text = ufo.Comments;

                    EditText latitude = FindViewById<EditText>(Resource.Id.Latitude);
                    latitude.Text = ufo.Latitude.ToString();

                    EditText longitude = FindViewById<EditText>(Resource.Id.Longitude);
                    longitude.Text = ufo.Longitude.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    Intent nextActivity = new Intent(this, typeof(MainActivity));
                    StartActivity(nextActivity);
                    return true;

                case Resource.Id.navigation_dashboard:

                    return true;
            }
            return false;
        }
    }
}
