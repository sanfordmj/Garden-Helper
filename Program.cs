using System;
using System.Device.Gpio;
using System.Threading;

int pin = 18;
int lightTime = 1000;
int dimTime = 200;

using GpioController controller = new();
controller.OpenPin(pin, PinMode.Output);

Console.WriteLine($"GPIO pin enabled: {pin}");

Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

void myHandler(object sender, ConsoleCancelEventArgs args)
{

    controller.Write(pin, PinValue.Low);
    controller.Dispose();
}

while (true)
{
    Console.WriteLine($"Light for {lightTime}ms");
    controller.Write(pin, PinValue.High);
    Thread.Sleep(lightTime);

    Console.WriteLine($"Dim for {dimTime}ms");
    controller.Write(pin, PinValue.Low);
    Thread.Sleep(dimTime);
}
