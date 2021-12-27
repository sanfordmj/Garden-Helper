using CliWrap;
using Newtonsoft.Json;
using GardenHelper.Models;
using System.Text;
public class LightSensorWrap
{

    public LightSensorWrap()
    {

    }

    public async Task GetReadings()
    {


        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();

        var result = await Cli.Wrap("/usr/bin/python")
                    .WithArguments(new[] { "tsl2591.py" })
                    .WithWorkingDirectory("/tsl2591")
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                    .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString();
        var stdErr = stdErrBuffer.ToString();

        if (!String.IsNullOrEmpty(stdOut))
        {
            LightReading? lightvalue = JsonConvert.DeserializeObject<LightReading>(stdOut);
            if (lightvalue != null)
                Console.WriteLine($"Value: {lightvalue.lux}");
        }
        if (!String.IsNullOrEmpty(stdErr))
        {
            Console.WriteLine($"Error: {stdErr}");
        }

        //Console.WriteLine($"Out: {stdOut}");
        //Console.WriteLine($"Error: {stdErr}");
    }

}