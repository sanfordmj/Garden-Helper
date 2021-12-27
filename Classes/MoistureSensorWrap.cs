using CliWrap;
using Newtonsoft.Json;
using GardenHelper.Models;
using System.Text;
public class MoistureSensorWrap
{

    public MoistureSensorWrap()
    {

    }

    public async Task GetReadings()
    {


        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();

        var result = await Cli.Wrap("/usr/bin/python")
                    .WithArguments(new[] { "4026.py" })
                    .WithWorkingDirectory("/4026")
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                    .ExecuteAsync();

        var stdOut = stdOutBuffer.ToString();
        var stdErr = stdErrBuffer.ToString();

        if (!String.IsNullOrEmpty(stdOut))
        {
            MoistureReading? moisturevalue = JsonConvert.DeserializeObject<MoistureReading>(stdOut);
            if (moisturevalue != null)
                Console.WriteLine($"Value: {moisturevalue.moisture}");
        }
        if (!String.IsNullOrEmpty(stdErr))
        {
            Console.WriteLine($"Error: {stdErr}");
        }

        //Console.WriteLine($"Out: {stdOut}");
        //Console.WriteLine($"Error: {stdErr}");
    }

}