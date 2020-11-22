using OpenWeatherAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.ViewModels;

namespace WeatherApp.Services
{
    public class OpenWeatherService : ITemperatureService
    {
        OpenWeatherProcessor owp;

        public OpenWeatherService(string apiKey)
        {
            owp = OpenWeatherProcessor.Instance;
            owp.ApiKey = apiKey;
        }
        
        public async Task<TemperatureModel> GetTempAsync()
        {
            var temp = await owp.GetCurrentWeatherAsync();
            var result = new TemperatureModel();
            if (temp.Cod==400||temp.Cod==404)
            {
                result.Temperature = -400;
                return result;
            }else if (temp.Cod == 401)
            {
                result.Temperature = -401;
                return result;
            }
           else
            {
                result.DateTime = DateTime.UnixEpoch.AddSeconds(temp.DateTime);
                result.Temperature = temp.Main.Temperature;
                return result;
            };

          
        }

        public void SetLocation(string location)
        {
            owp.City = location;
        }

        public void SetApiKey(string apikey)
        {
            owp.ApiKey = apikey;
        }
    }
}
