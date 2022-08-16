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
using UFOTracker.Models;
using Xamarin.Essentials;

namespace UfoTrackerAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class AddUfo : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        HttpClient client;
        Ufo ufo;
        private Button Add;
        String uri = "https://10.0.2.2:7053/Ufo";
        int totalitems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.add_ufo);

            Add = FindViewById<Button>(Resource.Id.Add);

            Add.Click += addUfoClick;

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

        }

        private void addUfoClick(object sender, EventArgs e)
        {

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);

            AddUfoAPI(uri);

        }


        public async void AddUfoAPI(string uri)
        {
            try
            {

                EditText city = FindViewById<EditText>(Resource.Id.City);
                EditText state = FindViewById<EditText>(Resource.Id.State);
                EditText country = FindViewById<EditText>(Resource.Id.Country);
                EditText shape = FindViewById<EditText>(Resource.Id.Shape);
                EditText comments = FindViewById<EditText>(Resource.Id.Comments);
                EditText latitude = FindViewById<EditText>(Resource.Id.Latitude);
                EditText longitude = FindViewById<EditText>(Resource.Id.Longitude);



                Ufo ufoToAdd = new Ufo
                {
                    City = (string)city.Text,
                    State = (string)state.Text,
                    Country = (string)country.Text,
                    DateAndTime = DateTime.Now,
                    DatePosted = DateTime.Now,
                    Shape = (string)shape.Text,
                    Comments = (string)comments.Text,
                    Latitude = (string)latitude.Text,
                    Longitude = (string)longitude.Text,
                };

                var json = JsonConvert.SerializeObject(ufoToAdd);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resp = await client.PostAsync(new Uri(uri), httpContent);
                if (resp.IsSuccessStatusCode)
                {
                    Intent nextActivity = new Intent(this, typeof(MainActivity));
                    StartActivity(nextActivity);
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine("l'api ne fonctionne pas", e);
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
