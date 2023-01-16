using HelloApp2.Views;
using Mapsui;
using Mapsui.Projection;

using Mapsui.UI.Forms;
using Mapsui.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace HelloApp2.MapsKarta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Karta : ContentPage
    {
        

        public Karta()
        {
            InitializeComponent();
           

            var map = new Mapsui.Map
            {
                CRS = "EPSG:3857",
                Transformation = new MinimalTransformation()
                
                
            };

            var tileLayer = OpenStreetMap.CreateTileLayer();
            
            map.Layers.Add(tileLayer);
            
            map.Widgets.Add(new Mapsui.Widgets.ScaleBar.ScaleBarWidget(map) { TextAlignment = Mapsui.Widgets.Alignment.Center, HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left, VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Bottom });
            mapView.Map = map;
           
           

        }
        

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var location = await Geolocation.GetLastKnownLocationAsync();

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                
            }
            
            mapView.MyLocationLayer.UpdateMyLocation(new Position(location.Latitude, location.Longitude), true);
           
            
        }

       








        protected internal void DisplayStack()
        {
            NavigationPage navPage = (NavigationPage)App.Current.MainPage;
        }

        
    }
}