using System;
using System.Net;
using Newtonsoft.Json;



namespace LAB6
{

    public struct Weather
    {
        public string Country { get; set; }
        public string Name { get; set; }
        public double Temp { get; set; }
        public string Description { get; set; }

        public Weather(string Country, string Name, double Temp, string Description)
        {
            this.Country = Country;
            this.Name = Name;
            this.Temp = Temp;
            this.Description = Description;
        }
    }

    public class WeatherResponse 
    {
        public TemperatureInfo main { get; set; }
        public CountryInfo sys { get; set; }
        public string name { get; set; }
        public DescriptionInfo[] weather { get; set; }
    }

    public class DescriptionInfo 
    {
        public string description { get; set; }
    }
    public class TemperatureInfo 
    {
        public double temp { get; set; }
    }

    public class CountryInfo 
    {
        public string country { get; set; }
    }

    public class Programm
    {
        public static void Main()
        {
            string API_key = "9074a89f53780baef2f636c1099aea02";
            double latitude = 0, longitude = 0;
            List<Weather> weather_list = new List<Weather>();
            Random rnd = new Random();

            while (weather_list.Count != 50)
            {
                latitude = rnd.NextDouble() * ((90 + 90) - 90);
                longitude = rnd.NextDouble() * ((180 + 180) - 180);
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid={API_key}";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream())) 
                {
                    response = streamReader.ReadToEnd();
                }
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
                if (weatherResponse.sys.country == "" || weatherResponse.name == "") continue;
                Weather weather = new Weather(weatherResponse.sys.country, weatherResponse.name, weatherResponse.main.temp, weatherResponse.weather[0].description);
                weather_list.Add(weather);
            }

            string countryMaxTemp = weather_list.OrderByDescending(w => w.Temp).First().Country;
            Console.WriteLine($"Самая высокая температура в стране: {countryMaxTemp}");
            string countryMinTemp = weather_list.OrderBy(w => w.Temp).First().Country;
            Console.WriteLine($"Самая низкая температура в стране: {countryMinTemp}");
            double averageTemp = weather_list.Average(w => w.Temp);
            Console.WriteLine($"Средняя температура в мире: {averageTemp}");
            double country_count = weather_list.Select(w => w.Country).Distinct().Count();
            Console.WriteLine($"Количество стран: {country_count}");
            string country_clear_sky = weather_list.Where(w => w.Description == "clear sky").First().Country;
            Console.WriteLine($"Первая страна, в которой clear sky: {country_clear_sky}");
            string city_clear_sky = weather_list.Where(w => w.Description == "clear sky").First().Name;
            Console.WriteLine($"Первый город, в которой clear sky: {city_clear_sky}");
            string country_rain = weather_list.Where(w => w.Description == "light rain").First().Country;
            Console.WriteLine($"Первая страна, в которой rain: {country_rain}");
            string city_rain = weather_list.Where(w => w.Description == "light rain").First().Name;
            Console.WriteLine($"Первый город, в которой rain: {city_rain}");
            string country_few_clouds = weather_list.Where(w => w.Description == "few clouds").First().Country;
            Console.WriteLine($"Первая страна, в которой few clouds: {country_few_clouds}");
            string city_few_clouds = weather_list.Where(w => w.Description == "few clouds").First().Name;
            Console.WriteLine($"Первый город, в которой few clouds: {city_few_clouds}");
        }
    }
}