using CliWrap;
using Newtonsoft.Json;
using GardenHelper.Models;
using System.Text;
public class CO2SensorWrap
{

    public CO2SensorWrap()
    {

    }

    public async Task GetReadings()
    {

        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();
        //-c'import code; code.setBase(15); code.runToBase()'
        var result = await Cli.Wrap("/usr/bin/python")
                    .WithArguments(new[] { "sgp30.py" })
                    .WithWorkingDirectory("/sgp30")
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                    .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString();
        var stdErr = stdErrBuffer.ToString();


        if (!String.IsNullOrEmpty(stdOut))
        {
            CO2Reading? lightvalue = JsonConvert.DeserializeObject<CO2Reading>(stdOut);
            if (lightvalue != null)
                Console.WriteLine($"Value: {lightvalue.eCO2}");
        }
        if (!String.IsNullOrEmpty(stdErr))
        {
            Console.WriteLine($"Error: {stdErr}");
        }

        Console.WriteLine($"Out: {stdOut}");
        Console.WriteLine($"Error: {stdErr}");
    }

}