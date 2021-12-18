using System;
using System.Device.I2c;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading;
using GardenHelper.Models;
using Newtonsoft.Json;
using System.Text;

int pin = 10;
int lightTime = 300;
//int dimTime = 200;
int buttonPin = 26;


var assembly = typeof(GpioDriver).Assembly;
var driverType = assembly.GetType("System.Device.Gpio.Drivers.RaspberryPi3LinuxDriver");
var ctor = driverType.GetConstructor(new Type[] { });
var driver = ctor.Invoke(null) as GpioDriver;

GpioController controller = new GpioController(PinNumberingScheme.Board, driver);


controller.OpenPin(pin, PinMode.Output);
controller.OpenPin(buttonPin, PinMode.InputPullUp);
/*
Console.WriteLine($"GPIO pin enabled: {pin}");

Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

const int busId = 1; //new

I2cConnectionSettings i2cSettings = new(busId, Bme280.DefaultI2cAddress);
using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
using Bme280 bme80 = new Bme280(i2cDevice)
{
    // set higher sampling
    TemperatureSampling = Sampling.LowPower,
    PressureSampling = Sampling.UltraHighResolution,
    HumiditySampling = Sampling.Standard,

    controller.Write(pin, PinValue.Low);
    controller.Dispose();
}
*/
HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Authorization", "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJeFVzZXIiOiIwIiwibmJmIjoxNjM5ODYyOTM1LCJleHAiOjE2NzEzOTg5MzUsImlhdCI6MTYzOTg2MjkzNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdC9nYXJkZW5zZXJ2aWNlLnNpZ25pdC8iLCJhdWQiOiJodHRwOi8vZG9ja2VyZ2FyZGVuc2VydmljZS5jb20vIn0.K86nKP7RI76Y8RSpHr2Ur3zIHHJ-KDhi0FtPtXoa2sg");
client.BaseAddress = new Uri("http://OWNER-MOBILE:5000");

Random rand = new Random();

try

{

    while (true)
    {
        if (controller.Read(buttonPin) == false)
        {
            var value = rand.Next(0, 200);

            controller.Write(pin, PinValue.High);
            var objAsJson = JsonConvert.SerializeObject(new SensorReading { Value = value, EnteredDate = DateTime.Now, IX_Sensor = 1 });
            var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("SensorReading", content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);
            Thread.Sleep(lightTime);
        }
        else
        {
            controller.Write(pin, PinValue.Low);
        }

        //Console.WriteLine($"Light for {lightTime}ms");
        //controller.Write(pin, PinValue.High);
        //Thread.Sleep(lightTime);

        //Console.WriteLine($"Dim for {dimTime}ms");
        //controller.Write(pin, PinValue.Low);
        //Thread.Sleep(lightTime);
    }
}
finally
{
    controller.ClosePin(pin);
    controller.ClosePin(buttonPin);
    Console.WriteLine($"Finally Hit");
}
