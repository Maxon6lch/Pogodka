using HelloApp2.Helper;
using HelloApp2.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Recognizers.Text;
using Microsoft.Win32;
using HelloApp2.MapsKarta;

namespace HelloApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();
            GetWeatherInfo();
            
        }
        private string Location = "Ликино-Дулево, RU";
       
          

        private async void GetWeatherInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/weather?q={Location}&appid=74e42de11c888057454b691dca750b92&units=metric";
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo>(result.Response);
                    descriptionTxt.Text = weatherInfo.weather[0].description.ToUpper();
                    iconImg.Source = $"w{weatherInfo.weather[0].icon}";
                    cityTxt.Text = weatherInfo.name.ToUpper();
                    temperatureTxt.Text = weatherInfo.main.temp.ToString("0");
                    humidityTxt.Text = $"{weatherInfo.main.humidity}%";
                    pressureTxt.Text = $"{weatherInfo.main.grnd_level}ртс";
                    windTxt.Text = $"{weatherInfo.wind.speed}м/с";
                    cloudinessTxt.Text = $"{weatherInfo.clouds.all}%";
                    

                    var dtx = new DateTime().ToUniversalTime().AddSeconds(weatherInfo.dt);
                    dateTxt.Text = DateTime.Now.ToString("dddd, MMM dd");

                    var date = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.sys.sunrise);
                    sunriseTxt.Text = date.ToLocalTime().ToString("HH:mm").ToUpper();

                    var datex = DateTimeOffset.FromUnixTimeSeconds(weatherInfo.sys.sunset);
                    sunsetTxt.Text = datex.ToLocalTime().ToString("HH:mm").ToUpper();

                    Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            timeTxt.Text = DateTime.Now.ToString("HH:mm:ss");
                        });
                        return true;
                    });
                
                    GetForecastInfo();
                    
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Weather info", ex.Message, "ОК");
                }
            }
            else
            {
                await DisplayAlert("Weather info", "No weather information found", "ОК");
            }
        }
        
        private async void GetForecastInfo()
        {
            var url = $"http://api.openweathermap.org/data/2.5/forecast?q={Location}&appid=74e42de11c888057454b691dca750b92&units=metric";
            var result = await ApiCaller.Get(url);
            if (result.Successful)
            {
                try
                {
                    var forcastInfo = JsonConvert.DeserializeObject<ForecastInfo>(result.Response);
                    List<List> allList = new List<List>();
                    foreach (var list in forcastInfo.list)
                    {
                        var date = DateTime.Parse(list.dt_txt);
                        if (date > DateTime.Now && date.Hour == 0 && date.Minute == 0 && date.Second == 0)

                            allList.Add(list);
                    }

                        dayOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dddd");
                        dateOneTxt.Text = DateTime.Parse(allList[0].dt_txt).ToString("dd MMM");
                        iconOneImg.Source = $"w{allList[0].weather[0].icon}";
                        tempOneTxt.Text = allList[0].main.temp.ToString("0");

                        dayTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dddd");
                        dateTwoTxt.Text = DateTime.Parse(allList[1].dt_txt).ToString("dd MMM");
                        iconTwoImg.Source = $"w{allList[1].weather[0].icon}";
                        tempTwoTxt.Text = allList[1].main.temp.ToString("0");

                        dayThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dddd");
                        dateThreeTxt.Text = DateTime.Parse(allList[2].dt_txt).ToString("dd MMM");
                        iconThreeImg.Source = $"w{allList[2].weather[0].icon}";
                        tempThreeTxt.Text = allList[2].main.temp.ToString("0");

                        dayFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dddd");
                        dateFourTxt.Text = DateTime.Parse(allList[3].dt_txt).ToString("dd MMM");
                        iconFourImg.Source = $"w{allList[3].weather[0].icon}";
                        tempFourTxt.Text = allList[3].main.temp.ToString("0");
                    
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Weather info", ex.Message, "ОК");
                }
            }
            else
            {
                await DisplayAlert("Weather info", "No forcast information found", "ОК");
            }
        }

        
        bool loaded = false;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (loaded == false)
            {
                DisplayStack();
                loaded = true;
            }
        }
        protected internal void DisplayStack()
        {
            NavigationPage navPage = (NavigationPage)App.Current.MainPage;
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Karta page = new Karta();
            await Navigation.PushAsync(page);
            page.DisplayStack();
            
        }
    }
}