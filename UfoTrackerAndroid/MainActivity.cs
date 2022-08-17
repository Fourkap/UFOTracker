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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        HttpClient client;
        PageMongo mongo;
        private int currentPage = 1;
        private Button nextBtn, prevBtn;
        String uri = "https://10.0.2.2:7053/";
        int totalitems;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            //var uri = "https://opendata.paris.fr/api/records/1.0/search/?dataset=velib-disponibilite-en-temps-reel&q=&facet=name&facet=is_installed&facet=is_renting&facet=is_returning&facet=nom_arrondissement_communes";


            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            prevBtn = FindViewById<Button>(Resource.Id.prevBtn);
            //BUTTON CLICKS
            nextBtn.Click += nextBtn_Click;
            prevBtn.Click += prevBtn_Click;
            prevBtn.Enabled = false;

            //textMessage = FindViewById<TextView>(Resource.Id.message);
            uri += "Ufo?page=" + currentPage + "&pageSize=10";
            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);
            
            recupAPI(uri, currentPage);
            
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public async void recupAPI(string uri, int currentPage)
        {
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(content);
                    var finaltest = json.ToString();

                    //foreach (var e in test)
                    //{
                    //    Console.WriteLine(e);
                    //}.
                    TextView compteur = FindViewById<TextView>(Resource.Id.compteur);
                    compteur.Text = currentPage.ToString();

                    ListView lst1 = FindViewById<ListView>(Resource.Id.listView1);
                    mongo = JsonConvert.DeserializeObject<PageMongo>(finaltest);
                    //set totalpages;
                    totalitems = mongo.Count +1;
                    TextView total = FindViewById<TextView>(Resource.Id.total);
                    total.Text = "/" + totalitems.ToString();
                    List<string> items = new List<String>();
                    foreach (var en in mongo.Ufos)
                    {
                        items.Add(en.City);
                    }
                    var ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
                    lst1.SetAdapter(ListAdapter);
                    lst1.ItemClick += lst1_ItemClick;

                    Button maps = FindViewById<Button>(Resource.Id.maps);
                    maps.Click += (object sender, EventArgs e) =>
                    {
                        showMaps(finaltest);
                    };
                   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( "l'api ne fnctionne pas", e);
            }

        }

        public void showMaps(string finaltest)
        {
            Intent mapsActivity = new Intent(this, typeof(Maps));
            mapsActivity.PutExtra("ufos", finaltest);
            StartActivity(mapsActivity);
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
                    Intent addActivity = new Intent(this, typeof(AddUfo));
                    StartActivity(addActivity);
                    return true;
            }
            return false;
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

        void prevBtn_Click(object sender, System.EventArgs e)
        {
            currentPage -= 1;
            uri = "https://10.0.2.2:7053/Ufo?page=" + currentPage + "&pageSize=10";
            toggleButtons();
            recupAPI(uri, currentPage);
            //toggleButtons();

        }

        /*
         * NEXT BUTTON CLICKED
         */
        void nextBtn_Click(object sender, System.EventArgs e)
        {
            currentPage += 1;
            uri = "https://10.0.2.2:7053/Ufo?page=" + currentPage + "&pageSize=10";
            toggleButtons();
            recupAPI(uri, currentPage);
            //toggleButtons();
        }

        /*
         * TOGGLE BUTTON STATES
         */
        private void toggleButtons()
        {
            int totalPages = totalitems;
            if (currentPage == totalPages)
            {
                nextBtn.Enabled = false;
                prevBtn.Enabled = true;
            }
            else
                if (currentPage == 1)
            {
                prevBtn.Enabled = false;
                nextBtn.Enabled = true;
            }
            else
            {
                prevBtn.Enabled = true;
                nextBtn.Enabled = true;
            }
        }
        
        private void lst1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            List<Ufo> listUfos = (List<Ufo>)mongo.Ufos;
            var position = e.Position;
            var item = listUfos[position] as Ufo;
            var id = item.Id;

            Intent nextActivity = new Intent(this, typeof(UfoDetails));
            nextActivity.PutExtra("id", id);
            StartActivity(nextActivity);
        }


    }
}

