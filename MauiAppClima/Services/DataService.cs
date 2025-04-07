using MauiAppClima.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppClima.Services
{
    public class DataService
    {
        public static async Task<(Tempo?, HttpStatusCode)> GetPrevisaoComStatus(string cidade)
        {
            Tempo? t = null;
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;

            string chave = "824189dbc03c5c8cf9a3efb4a87d398c";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}" +
                         $"&units=metric&appid={chave}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage resp = await client.GetAsync(url);
                    statusCode = resp.StatusCode;

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                        t = new Tempo()
                        {
                            lat = (double)rascunho["coord"]["lat"],
                            lon = (double)rascunho["coord"]["lon"],
                            description = (string)rascunho["weather"][0]["description"],
                            main = (string)rascunho["weather"][0]["main"],
                            temp_min = (double)rascunho["main"]["temp_min"],
                            temp_max = (double)rascunho["main"]["temp_max"],
                            speed = (double)rascunho["wind"]["speed"],
                            visibility = (int)rascunho["visibility"],
                            sunrise = sunrise.ToString(),
                            sunset = sunset.ToString()
                        };
                    }
                }
                catch (HttpRequestException)
                {
                    throw; 
                }
            }

            return (t, statusCode);
        }
    }
}
