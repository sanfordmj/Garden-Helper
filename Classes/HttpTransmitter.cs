using Newtonsoft.Json;
using System.Text;
using GardenHelper.Models;

namespace GardenHelper.Classes
{

    public class HttpTransmitter
    {

        public async void SendData(SensorReading reading)
        {
            var objAsJson = JsonConvert.SerializeObject(reading);
            var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await Program.client.PostAsync("SensorReading", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);
        }
    }
}