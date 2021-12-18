using System;
using System.Device.I2c;
using System.Device.Gpio;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.FilteringMode;
using Iot.Device.Bmxx80.PowerMode;
using Iot.Device.Common;
using System.Threading;

int pin = 17;
int lightTime = 1000;
int dimTime = 1000;

using GpioController controller = new();
controller.OpenPin(pin, PinMode.Output);

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

};


void myHandler(object sender, ConsoleCancelEventArgs args)
    {

        controller.Write(pin, PinValue.Low);
        controller.Dispose();
    }

    while (true)
    {
        
        var readResult = bme80.Read();

        Console.WriteLine($"Temperature: {readResult.Temperature?.DegreesCelsius:0.#}\u00B0C");
        Console.WriteLine($"Pressure: {readResult.Pressure?.Hectopascals:0.##}hPa");        
        Console.WriteLine($"Relative humidity: {readResult.Humidity?.Percent:0.#}%");



        Console.WriteLine($"Light for {lightTime}ms");
        controller.Write(pin, PinValue.High);
        Thread.Sleep(lightTime);

        Console.WriteLine($"Dim for {dimTime}ms");
        controller.Write(pin, PinValue.Low);
        Thread.Sleep(dimTime);
    }

