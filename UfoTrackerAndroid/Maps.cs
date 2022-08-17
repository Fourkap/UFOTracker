using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using System;

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
using UfoTrackerAndroid;
using Android.Graphics;



namespace UfoTrackerAndroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class Maps : AppCompatActivity, IOnMapReadyCallback, BottomNavigationView.IOnNavigationItemSelectedListener

    {
        private GoogleMap m_map;
        private MapView m_mapView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.maps);
            m_mapView = FindViewById<MapView>(Resource.Id.mainactivity_mapView);


            m_mapView.OnCreate(savedInstanceState);
            m_mapView.GetMapAsync(this);


            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void OnMapReady(GoogleMap map)
        {
            string ufos = Intent.GetStringExtra("ufos");
            PageMongo mongo = JsonConvert.DeserializeObject<PageMongo>(ufos);

            m_map = map;
            LatLng LatLonParis = new LatLng(48.866667, 2.333333); 
            m_map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(LatLonParis, 2));

            
            foreach (var ufo in mongo.Ufos)
            {
                if ((ufo.Shape == "unknown") || (ufo.Shape == "other") || (ufo.Shape == "changing"))
                {
                    m_map.AddMarker(new MarkerOptions().SetPosition(new LatLng(Convert.ToDouble(ufo.Latitude), Convert.ToDouble(ufo.Longitude))).SetTitle(ufo.City));
                    
                }
                else
                {
                    var resourceId = (int)typeof(Resource.Drawable).GetField(ufo.Shape).GetValue(null);

                    m_map.AddMarker(new MarkerOptions().SetPosition(new LatLng(Convert.ToDouble(ufo.Latitude), Convert.ToDouble(ufo.Longitude))).SetTitle(ufo.City).SetIcon(BitmapDescriptorFactory.FromResource(resourceId)));

                }

            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            m_mapView.OnPause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_mapView.OnResume();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            m_mapView.OnSaveInstanceState(outState);
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

    }
}