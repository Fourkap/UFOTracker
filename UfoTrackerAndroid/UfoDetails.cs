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
using UFOTracker.Models;
using Xamarin.Essentials;

namespace UfoTrackerAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class UfoDetails : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        HttpClient client;
        Ufo ufo;
        private int currentPage = 1;
        private Button nextBtn, prevBtn;
        String uri = "https://10.0.2.2:7053/";
        int totalitems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.ufo_details);



            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);

            string id = Intent.GetStringExtra("id");
            uri += "Ufo/" + id;

            Button delete = FindViewById<Button>(Resource.Id.Delete);


            delete.Click += (object sender, EventArgs e) =>
            {
                DeleteUfo(id);
            };
            recupAPI(uri);

            //delete.Click += DeleteUfo;

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

        }

        public async void DeleteUfo(string id)
        {
            uri += "Ufo/" + id;
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Intent nextActivity = new Intent(this, typeof(MainActivity));
                    StartActivity(nextActivity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

                    TextView city = FindViewById<TextView>(Resource.Id.City);
                    city.Text = "City: " + ufo.City;

                    TextView state = FindViewById<TextView>(Resource.Id.State);
                    state.Text = "State: " + ufo.State;

                    TextView country = FindViewById<TextView>(Resource.Id.Country);
                    country.Text = "Country: " + ufo.Country;

                    TextView date = FindViewById<TextView>(Resource.Id.Date);
                    date.Text = "Date: " + ufo.DateAndTime.ToString();

                    TextView datePosted = FindViewById<TextView>(Resource.Id.PostedDate);
                    datePosted.Text = "Posted: " + ufo.DatePosted.ToString();

                    TextView shape = FindViewById<TextView>(Resource.Id.Shape);
                    shape.Text = "Shape: " + ufo.Shape;

                    TextView comments = FindViewById<TextView>(Resource.Id.Comments);
                    comments.Text = "Comments: " + ufo.Comments;

                    TextView latitude = FindViewById<TextView>(Resource.Id.Latitude);
                    latitude.Text = "Latitude: " + ufo.Latitude.ToString();

                    TextView longitude = FindViewById<TextView>(Resource.Id.Longitude);
                    longitude.Text = "Longitude: " + ufo.Longitude.ToString();
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
