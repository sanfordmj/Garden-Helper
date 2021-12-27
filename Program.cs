using System;
using System.Device.I2c;

using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Threading;
using CliWrap;
using Newtonsoft.Json;
using System.Text;

using GardenHelper.Models;
using GardenHelper.Classes;


public partial class Program
{

    static int pin = 10;
    static int lightTime = 3000;
    static int buttonPin = 26;
    public static HttpClient client = new HttpClient();

    const int PinInterrupt = 4;

    static void Main(string[] args)
    {
        HttpTransmitter transmitter = new HttpTransmitter();

        var assembly = typeof(GpioDriver).Assembly;
        var driverType = assembly.GetType("System.Device.Gpio.Drivers.RaspberryPi3LinuxDriver");
        var ctor = driverType.GetConstructor(new Type[] { });
        var driver = ctor.Invoke(null) as GpioDriver;

        GpioController controller = new GpioController(PinNumberingScheme.Board, driver);
        controller.OpenPin(pin, PinMode.Output);
        controller.OpenPin(buttonPin, PinMode.InputPullUp);

        client.DefaultRequestHeaders.Add("Authorization", "JWT eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJeFVzZXIiOiIwIiwibmJmIjoxNjM5ODYyOTM1LCJleHAiOjE2NzEzOTg5MzUsImlhdCI6MTYzOTg2MjkzNSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdC9nYXJkZW5zZXJ2aWNlLnNpZ25pdC8iLCJhdWQiOiJodHRwOi8vZG9ja2VyZ2FyZGVuc2VydmljZS5jb20vIn0.K86nKP7RI76Y8RSpHr2Ur3zIHHJ-KDhi0FtPtXoa2sg");
        client.BaseAddress = new Uri("http://OWNER-MOBILE:5000");

        Random rand = new Random();

        try
        {

            LightSensorWrap lightSensor = new LightSensorWrap();
            CO2SensorWrap cO2Sensor = new CO2SensorWrap();
            MoistureSensorWrap moistureSensor = new MoistureSensorWrap();

            Task.Run(async () => await cO2Sensor.GetReadings());


            while (true)
            {


                Console.WriteLine($"After wait");

                Task.Run(async () => await lightSensor.GetReadings());
                Task.Run(async () => await moistureSensor.GetReadings());
                Thread.Sleep(5000);

                //Console.WriteLine($"Temperature: {tempValue.DegreesCelsius:0.#}\u00B0C");
                //Console.WriteLine($"Pressure: {preValue.Hectopascals:#.##} hPa");
                //Console.WriteLine($"Relative humidity: {humValue.Percent:#.##}%");
                //Console.WriteLine($"Estimated altitude: {altValue.Meters:#} m");

                if (controller.Read(buttonPin) == false)
                {
                    var value = rand.Next(0, 200);
                    var Reading = new SensorReading { Value = value, EnteredDate = DateTime.Now, IX_Sensor = 1 };

                    transmitter.SendData(Reading);
                    controller.Write(pin, PinValue.High);

                    Thread.Sleep(lightTime);
                }
                else
                {
                    controller.Write(pin, PinValue.Low);
                }


            }

        }
        finally
        {
            controller.ClosePin(pin);
            controller.ClosePin(buttonPin);
            Console.WriteLine($"Finally Hit");
        }
    }
}